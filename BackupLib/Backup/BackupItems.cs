using System.IO;

namespace io.rz.Flywheel.BackupLib.Backup
{
    public abstract class BackupItem
    {
        public BackupItem(string localFilePath)
        {
            this.LocalFilePath = localFilePath;
        }
        public string LocalFilePath { get; protected set; }
    }

    public class FileBackupItem : BackupItem
    {
        public FileBackupItem(string localFilePath)
            : base(localFilePath)
        {

        }
    }

    public class StreamBackupItem : BackupItem
    {
        public StreamBackupItem(string localFilePath, Stream stream)
            : base(localFilePath)
        {
            this.Stream = stream;
        }

        public Stream Stream { get; private set; }
    }

    public class NamedStreamBackupItem : StreamBackupItem
    {
        public NamedStreamBackupItem(string localFilePath, Stream stream, string name)
            : base(localFilePath, stream)
        {
            this.Name = name;
        }

        public string Name { get; private set; }
    }
}
