using io.rz.Flywheel.BackupLib.Backup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace io.rz.Flywheel.BackupLib.Jobs
{
    public class BackupJob
    {
        public BackupJob(DirectoryBackupTask dirTask, ProcessorChain<BackupItem, FileBackupTask> chain, string metadataFilePath)
        {
            this.Task = dirTask;
            this.ProcessChain = chain;
            this.MetadataFilePath = metadataFilePath;
        }

        public DirectoryBackupTask Task { get; private set; }

        public ProcessorChain<BackupItem, FileBackupTask> ProcessChain { get; private set; }

        public string MetadataFilePath { get; private set; }
    }
}
