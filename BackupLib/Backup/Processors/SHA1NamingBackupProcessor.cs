using System;
using System.Security.Cryptography;

namespace io.rz.Flywheel.BackupLib.Backup.Processors
{
    public class SHA1NamingBackupProcessor : Processor<BackupItem>
    {
        public override ResultType<BackupItem> Process(BackupItem evt)
        {

            if (evt is StreamBackupItem)
            {
                StreamBackupItem typedStream = evt as StreamBackupItem;
                SHA1 sha = new SHA1CryptoServiceProvider();
                byte[] result = sha.ComputeHash(typedStream.Stream);
                typedStream.Stream.Seek(0, System.IO.SeekOrigin.Begin);

                return ProcessNext(new NamedStreamBackupItem(typedStream.LocalFilePath, typedStream.Stream, BitConverter.ToString(result).Replace("-", string.Empty)));
            }
            throw new NotImplementedException("SHA1NamingBackupProcessor only works with Stream backup item");
        }
    }
}
