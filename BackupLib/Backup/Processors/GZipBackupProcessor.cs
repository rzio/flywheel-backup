using System;
using System.IO;
using System.IO.Compression;

namespace io.rz.Flywheel.BackupLib.Backup.Processors
{
    public class GZipBackupProcessor : Processor<BackupItem>
    {

        public override ResultType<BackupItem> Process(BackupItem evt)
        {
            MemoryStream ms = new MemoryStream();
            GZipStream stream = new GZipStream(ms, CompressionMode.Compress);
            try
            {
                if (evt is StreamBackupItem)
                {
                    ((StreamBackupItem)evt).Stream.CopyTo(stream);

                    ms.Seek(0, SeekOrigin.Begin);
                    return ProcessNext(new StreamBackupItem(((StreamBackupItem)evt).LocalFilePath, ms));
                }
            }
            finally
            {
                stream.Dispose();
            }
            throw new NotImplementedException("GZip processor only handles Stream Events");
        }
    }
}
