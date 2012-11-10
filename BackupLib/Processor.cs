using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.rz.Flywheel.BackupLib
{
    public abstract class Processor<ItemType>
    {
        public abstract ResultType<ItemType> Process(ItemType item);

        public Processor<ItemType> Next { get; set; }

        protected ResultType<ItemType> ProcessNext(ItemType item)
        {
            if (Next != null)
                return Next.Process(item);
            return ResultType<ItemType>.Error("Unfinished chain");
        }


    }
}
