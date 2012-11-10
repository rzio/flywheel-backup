
namespace io.rz.Flywheel.BackupLib.Backup
{
    public class FileBackupTask
    {
        public string AbsolutePath { private set; get; }

        public FileBackupTask(string absolutePath)
        {
            this.AbsolutePath = absolutePath;
        }
    }
}
