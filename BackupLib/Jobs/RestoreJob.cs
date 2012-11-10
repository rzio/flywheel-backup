using io.rz.Flywheel.BackupLib.Restore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace io.rz.Flywheel.BackupLib.Jobs
{
    public class RestoreJob
    {
        public string TargetFolder {get; private set;}
        public ProcessorChain<RestoreItem, FileRestoreTask> ProcessChain { get; private set; }
        public string MetadataFilePath { get; private set; }

        public RestoreJob(string targetFolder, ProcessorChain<RestoreItem, FileRestoreTask> chain, string metadataFilePath)
        {
            // TODO: Complete member initialization
            this.TargetFolder = targetFolder;
            this.ProcessChain = chain;
            this.MetadataFilePath = metadataFilePath;
        }
    }
}
