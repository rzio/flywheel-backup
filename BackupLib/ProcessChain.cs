using io.rz.Flywheel.BackupLib.Backup.Processors;
using System.Collections.Generic;

namespace io.rz.Flywheel.BackupLib
{
    public abstract class ProcessorChain<ItemType, TaskType>
    {
        protected Processor<ItemType> firstProcessor;
        public ProcessorChain(List<Processor<ItemType>> processors)
        {
            // Build the chain according to the order in the list
            firstProcessor = processors[0];
            var processor = firstProcessor;
            for (var i = 1; i < processors.Count; i++)
            {
                processor.Next = processors[i];
                processor = processor.Next;
            }
        }

        public abstract ResultType<ItemType> Process(TaskType fileTask);
        
    }
}
