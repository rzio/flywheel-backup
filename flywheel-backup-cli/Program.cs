using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using io.rz.Flywheel.BackupLib.Backup;
using io.rz.Flywheel.BackupLib;
using io.rz.Flywheel.BackupLib.Backup.Processors;
using io.rz.Flywheel.BackupLib.Jobs;
using io.rz.Flywheel.BackupLib.Restore;
using io.rz.Flywheel.BackupLib.Restore.Processors;

namespace io.rz.Flywheel.BackupCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            var parser = new CommandLineParser(new CommandLineParserSettings(false, false,Console.Error));
            if (!parser.ParseArguments(args, options))
                Environment.Exit(1);

            if (options.Backup)
            {
                string connectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", options.AzureAccount, options.AzureKey);
                
                BackupProcessorChain chain = new BackupProcessorChain(
                    new List<Processor<BackupItem>>()
                {
                    new FileLoadingBackupProcessor(),
                    new GZipBackupProcessor(),
                    new SHA1NamingBackupProcessor(),
                    new AzureUploaderBackupProcessor(connectionString,"flywheel")
                });

                var executor = new JobExecutor();
                executor.SubmitBackupJob(new BackupJob(options.Folder, true, chain, options.MetadataFile));

                executor.JobProgress += executor_JobProgress;
                executor.AwaitJobDone();
            }
            else if (options.Restore)
            {
                string connectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", options.AzureAccount, options.AzureKey);

                RestoreProcessorChain chain = new RestoreProcessorChain(
                    new List<Processor<RestoreItem>>()
                {
                    new AzureDownloaderRestoreProcessor(connectionString,"flywheel"),
                    new GZipRestoreProcessor(),
                    new FileSavingRestoreProcessor(options.Folder)

                });

                var executor = new JobExecutor();
                executor.SubmitRestoreJob(new RestoreJob(options.Folder, chain, options.MetadataFile));

                executor.JobProgress += executor_JobProgress;
                executor.AwaitJobDone();
            }

        }

        static void executor_JobProgress(object sender, JobProgressEventArgs e)
        {
            Console.Out.WriteLine(string.Format("Finished {0} of {1} files",e.DoneTasks,e.TotalTasks));
        }

        public class Options : CommandLineOptionsBase
        {
            [Option("b", "backup", HelpText = "Do backup", MutuallyExclusiveSet = "backup")]
            public bool Backup { get; set; }

            [Option("f", "folder", HelpText = "The folder to backup from/restore to")]
            public string Folder { get; set; }

            [Option("a", "azure-account", HelpText = "azure account")]
            public string AzureAccount { get; set; }

            [Option("k", "azure-key", HelpText = "azure key")]
            public string AzureKey { get; set; }

            [Option("r", "restore", HelpText = "Do restore", MutuallyExclusiveSet = "restore")]
            public bool Restore { get; set; }

            [Option("m", "metadata-file", HelpText = "The path the backup metadata file", MutuallyExclusiveSet = "restore")]
            public string MetadataFile { get; set; }




            [HelpOption(HelpText = "Dispaly this help screen.")]
            public string GetUsage()
            {
                return string.Empty;
                //return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
            }

        }
    }


    
}
