using io.rz.Flywheel.BackupLib.Backup;
using io.rz.Flywheel.BackupLib.Restore;
using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace io.rz.Flywheel.BackupLib.Jobs
{
    public class JobExecutor : IDisposable
    {
        int totalTasks = 0, doneTasks = 0;
        object syncRoot = new Object();

        public event EventHandler<JobDoneEventArgs> JobDone;
        public event EventHandler<JobProgressEventArgs> JobProgress;

        Task jobExecution;

        public void SubmitBackupJob(BackupJob job)
        {
            jobExecution = Task.Factory.StartNew(() => DoBackupJob(job));
        }

        public void SubmitRestoreJob(RestoreJob restoreJob)
        {
            jobExecution = Task.Factory.StartNew(() => DoRestoreJob(restoreJob));
        }

        public void AwaitJobDone()
        {
            jobExecution.Wait();
        }

        private void DoBackupJob(BackupJob job)
        {

            XElement rootElement = new XElement("BackupSet"
                    , new XAttribute("folder", job.AbsolutePath)
                    , new XAttribute("date", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ")));
            List<Task> tasks = new List<Task>();

            foreach (var fileTask in enumerateFileBackupTasks(job.AbsolutePath,job.Recursive))
            {
                Task task = Task<ResultType<BackupItem>>.Factory.StartNew(() =>
                   {
                       lock (syncRoot)
                       {
                           totalTasks++;
                           fireProgressEvent();
                       }
                       var res = job.ProcessChain.Process(fileTask);
                       if (res is ErrorResultType<BackupItem>)
                       {
                           return res;
                       }
                       else if (res is FinishedResultType<BackupItem>)
                       {
                           var typedResult = res as FinishedResultType<BackupItem>;
                           lock (syncRoot)
                           {
                               rootElement.Add(new XElement("file",
                                   new XAttribute("local", typedResult.LocalFilePath.Replace(job.AbsolutePath, "")),
                                   new XAttribute("remote", typedResult.RemoteFileName)));
                           }
                           return res;
                       }
                       return null;
                   }).ContinueWith((t) =>
                   {
                       lock (syncRoot)
                       {
                           doneTasks++;
                           fireProgressEvent();
                       }
                   });

                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
            XDocument x = new XDocument(rootElement);
            x.Save(job.MetadataFilePath);
            if (JobDone != null)
            {
                JobDone(this, new JobDoneEventArgs());
            }
            return;// BackupResult.Finished("Finished backing up" + job.Task.AbsolutePath);
        }

        public IEnumerable<FileBackupTask> enumerateFileBackupTasks(string absolutePath, bool recursive)
        {
            if (Directory.Exists(absolutePath))
            {
                foreach (String filename in Directory.GetFiles(absolutePath, "*.*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
                {
                    yield return new FileBackupTask(filename);
                }
            }
            else
            {
                if (File.Exists(absolutePath))
                {
                    yield return new FileBackupTask(absolutePath);
                }
                else
                {
                    yield break;
                }
            }
        }


        private void DoRestoreJob(RestoreJob job)
        {


            List<Task> tasks = new List<Task>();
            XDocument doc = XDocument.Load(job.MetadataFilePath);
            foreach (XElement fileElement in doc.Element("BackupSet").Elements())
            {

                FileRestoreTask restoreTask = new FileRestoreTask(fileElement.Attribute("local").Value, fileElement.Attribute("remote").Value);
                Task task = Task<ResultType<RestoreItem>>.Factory.StartNew(() =>
                    {
                        lock (syncRoot)
                        {
                            totalTasks++;
                            fireProgressEvent();
                        }
                        return job.ProcessChain.Process(restoreTask);
                    }).ContinueWith((t) =>
                        {
                            lock (syncRoot)
                            {
                                doneTasks++;
                                fireProgressEvent();
                            }
                        });
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
            if (JobDone != null)
            {
                JobDone(this, new JobDoneEventArgs());
            }
            return;
        }
        public void fireProgressEvent()
        {
            if (JobProgress != null)
            {
                // lock ?
                JobProgress(this, new JobProgressEventArgs(totalTasks, doneTasks));
            }
        }

        public long RunningTasks
        {
            get
            {
                lock (syncRoot)
                {
                    return totalTasks - doneTasks;
                }
            }
        }

        public void Dispose()
        {

        }




    }

    public class JobProgressEventArgs : EventArgs
    {
        public int TotalTasks { get; private set; }
        public int DoneTasks { get; private set; }

        public JobProgressEventArgs(int totalTasks, int doneTasks)
            : base()
        {
            TotalTasks = totalTasks;
            DoneTasks = doneTasks;
        }
    }

    public class JobDoneEventArgs : EventArgs
    {
        public JobDoneEventArgs()
            : base()
        {
        }
    }
}
