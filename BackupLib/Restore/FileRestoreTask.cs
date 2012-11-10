
namespace io.rz.Flywheel.BackupLib.Restore
{
    public class FileRestoreTask
    {
        public string LocalPath { private set; get; }
        public string RemoteName { private set; get; }

        public FileRestoreTask(string localPath, string remoteName)
        {
            this.LocalPath = localPath;
            this.RemoteName = remoteName;
        }
    }
}
