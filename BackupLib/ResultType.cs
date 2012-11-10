
namespace io.rz.Flywheel.BackupLib
{
    public abstract class ResultType<ItemType>
    {
        public static ResultType<ItemType> Proceed(ItemType item)
        {
            return new ProceedResultType<ItemType>(item);
        }

        public static ResultType<ItemType> Error(string message)
        {
            return new ErrorResultType<ItemType>(message);
        }

        public static ResultType<ItemType> Finished(string message, string remoteFileName, string localFilePath)
        {
            return new FinishedResultType<ItemType>(message, remoteFileName, localFilePath);
        }
    }

    public class ProceedResultType<ItemType> : ResultType<ItemType> 
    {
        public ItemType Item { private set; get; }

        public ProceedResultType(ItemType item)
        {
            this.Item = item;
        }
    }

    public class ErrorResultType<ItemType> : ResultType<ItemType> 
    {
        public string ErrorMessage { private set; get; }

        public ErrorResultType(string message)
        {
            ErrorMessage = message;
        }
    }

    public class FinishedResultType<ItemType> : ResultType<ItemType> 
    {
        public string Message { private set; get; }
        public string RemoteFileName { private set; get; }
        public string LocalFilePath { private set; get; }

        public FinishedResultType(string message, string remoteFileName, string localFilePath)
        {
            Message = message;
            RemoteFileName = remoteFileName;
            LocalFilePath = localFilePath;
        }
    }


}
