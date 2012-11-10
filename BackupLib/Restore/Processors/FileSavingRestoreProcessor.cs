using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace io.rz.Flywheel.BackupLib.Restore.Processors
{
    public class FileSavingRestoreProcessor : Processor<RestoreItem>
    {
        public string LocalFolder { get; private set; }

        public FileSavingRestoreProcessor(string localFolder)
        {
            this.LocalFolder = localFolder;
        }

        public override ResultType<RestoreItem> Process(RestoreItem item)
        {
            if (item is StreamRestoreItem)
            {
                if (!Directory.Exists(LocalFolder))
                {
                    Directory.CreateDirectory(LocalFolder);
                }
                var dir = Path.Combine(LocalFolder, Canonize(Path.GetDirectoryName(item.LocalFilePath)));
                if (!String.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                var typed = item as StreamRestoreItem;
                using (var fileStream = File.OpenWrite(Path.Combine(dir, Path.GetFileName(typed.LocalFilePath))))
                {
                    typed.Stream.CopyTo(fileStream);
                }
                return ResultType<RestoreItem>.Finished("Yey !", typed.RemoteName, typed.LocalFilePath);
            }
            throw new NotImplementedException("FileSavingRestoreProcessor processor only handles StreamRestore items");
        }
        string Canonize(string path)
        {
            return path.StartsWith("\\") ? path.Remove(0, 1) : path;
        }
    }


}
