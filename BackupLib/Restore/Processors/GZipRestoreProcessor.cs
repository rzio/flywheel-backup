using io.rz.Flywheel.BackupLib.Restore;
using System;
using System.IO;
using System.IO.Compression;

namespace io.rz.Flywheel.BackupLib.Backup.Processors
{
    public class GZipRestoreProcessor : Processor<RestoreItem>
    {

        public override ResultType<RestoreItem> Process(RestoreItem item)
        {
            MemoryStream ms = new MemoryStream();
            GZipStream decopressed = new GZipStream(((StreamRestoreItem)item).Stream, CompressionMode.Decompress);
            try
            {
                if (item is StreamRestoreItem)
                {
                    decopressed.CopyTo(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    return ProcessNext(new StreamRestoreItem(item.LocalFilePath, item.RemoteName, ms));
                }
            }
            catch (Exception e)
            {
                var i = 10;
            }
            finally
            {
                decopressed.Dispose();
            }
            throw new NotImplementedException("GZip processor only handles Stream Events");
        }
    }
}
