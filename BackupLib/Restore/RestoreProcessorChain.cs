using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace io.rz.Flywheel.BackupLib.Restore
{
    public class RestoreProcessorChain : ProcessorChain<RestoreItem, FileRestoreTask>
    {

        public RestoreProcessorChain(List<Processor<RestoreItem>> processors)
            : base(processors)
        {
        }
        public override ResultType<RestoreItem> Process(FileRestoreTask fileTask)
        {
            return firstProcessor.Process(new RestoreItem(fileTask.LocalPath,fileTask.RemoteName));
        }
    }
}
