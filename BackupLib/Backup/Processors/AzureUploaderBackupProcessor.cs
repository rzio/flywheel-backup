using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;

namespace io.rz.Flywheel.BackupLib.Backup.Processors
{
    public class AzureUploaderBackupProcessor : Processor<BackupItem>
    {
        CloudStorageAccount storageAccount;
        CloudBlobClient blobClient;
        CloudBlobContainer container;
        HashSet<string> uploadedFiles = new HashSet<string>();
        object syncRoot = new object();

        public AzureUploaderBackupProcessor(string connectionString, string containerName)
        {
            storageAccount = CloudStorageAccount.Parse(connectionString);
            blobClient = storageAccount.CreateCloudBlobClient();
            container = blobClient.GetContainerReference(containerName);
            container.CreateIfNotExists();
        }

        public override ResultType<BackupItem> Process(BackupItem evt)
        {
            if (evt is NamedStreamBackupItem)
            {
                var typedEvent = evt as NamedStreamBackupItem;
                lock (syncRoot)
                {
                    if (uploadedFiles.Contains(typedEvent.Name))
                    {
                        // file was already uploaded lets conserve bandwidth
                        return ResultType<BackupItem>.Finished("Yey !", typedEvent.Name, typedEvent.LocalFilePath);
                    }
                    uploadedFiles.Add(typedEvent.Name);
                }
                var blobRef = container.GetBlockBlobReference(typedEvent.Name);
                blobRef.UploadFromStream(typedEvent.Stream);
                return ResultType<BackupItem>.Finished("Yey !", typedEvent.Name, typedEvent.LocalFilePath);
            }
            throw new NotImplementedException("AzureUploaderBackupProcessor processor only handles NamedStream Events");
        }


    }
}
