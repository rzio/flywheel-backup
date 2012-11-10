using System;
using System.IO;

namespace io.rz.Flywheel.BackupLib.Backup.Processors
{
    public class FileLoadingBackupProcessor : Processor<BackupItem>
    {

        public override ResultType<BackupItem> Process(BackupItem evt)
        {
            if (evt is FileBackupItem)
            {
                var filePath = ((FileBackupItem)evt).LocalFilePath;
                using (var fileStream = File.OpenRead(filePath))
                {
                    return ProcessNext(new StreamBackupItem(filePath, fileStream));
                }
            }
            throw new NotImplementedException("FileLoadingBackupProcessor only accepts File backup items");
        }
    }
}
