using System;
using System.Collections.Generic;
using System.IO;

namespace io.rz.Flywheel.BackupLib.Backup
{
    public class DirectoryBackupTask : IEnumerable<FileBackupTask>
    {
        public string AbsolutePath { private set; get; }
        public bool Recursive { private set; get; }

        public DirectoryBackupTask(string absolutePath, bool recursive)
        {
            this.AbsolutePath = absolutePath;
            this.Recursive = recursive;
        }



        public IEnumerator<FileBackupTask> GetEnumerator()
        {
            foreach (String filename in Directory.GetFiles(AbsolutePath,"*.*",Recursive? SearchOption.AllDirectories: SearchOption.TopDirectoryOnly))
            {
                yield return new FileBackupTask(filename);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
