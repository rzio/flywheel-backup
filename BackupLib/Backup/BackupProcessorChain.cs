using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace io.rz.Flywheel.BackupLib.Backup
{
    public class BackupProcessorChain : ProcessorChain<BackupItem, FileBackupTask>
    {
        public BackupProcessorChain(List<Processor<BackupItem>> processors)
            : base(processors)
        {
        }
        public override ResultType<BackupItem> Process(FileBackupTask fileTask)
        {
            return firstProcessor.Process(new FileBackupItem(fileTask.AbsolutePath));
        }
    }
}
