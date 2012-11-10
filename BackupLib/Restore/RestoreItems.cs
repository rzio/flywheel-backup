using System.IO;

namespace io.rz.Flywheel.BackupLib.Restore
{
    public class RestoreItem
    {
        public RestoreItem(string localFilePath, string remoteName)
        {
            this.LocalFilePath = localFilePath;
            this.RemoteName = remoteName;
        }
        public string LocalFilePath { get; protected set; }
        public string RemoteName { get; protected set; }
    }

   

    public class StreamRestoreItem : RestoreItem
    {
        public StreamRestoreItem(string localFilePath, string remoteName, Stream stream)
            : base(localFilePath, remoteName)
        {
            this.Stream = stream;
        }

        public Stream Stream { get; private set; }
    }


}
