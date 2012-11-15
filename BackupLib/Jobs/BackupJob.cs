using io.rz.Flywheel.BackupLib.Backup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace io.rz.Flywheel.BackupLib.Jobs
{
    public class BackupJob
    {

        public string AbsolutePath { private set; get; }
        public bool Recursive { private set; get; }

        public BackupJob(string absoutePath, bool recursive, ProcessorChain<BackupItem, FileBackupTask> chain, string metadataFilePath)
        {
            this.AbsolutePath = absoutePath;
            this.Recursive = recursive;
            this.ProcessChain = chain;
            this.MetadataFilePath = metadataFilePath;
        }

        public ProcessorChain<BackupItem, FileBackupTask> ProcessChain { get; private set; }

        public string MetadataFilePath { get; private set; }
    }
}
