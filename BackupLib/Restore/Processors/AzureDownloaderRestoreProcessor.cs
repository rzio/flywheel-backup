using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;
using System.IO;

namespace io.rz.Flywheel.BackupLib.Restore.Processors
{
    public class AzureDownloaderRestoreProcessor : Processor<RestoreItem>
    {
        CloudStorageAccount storageAccount;
        CloudBlobClient blobClient;
        CloudBlobContainer container;
        //HashSet<string> uploadedFiles = new HashSet<string>();
        object syncRoot = new object();

        public AzureDownloaderRestoreProcessor(string connectionString, string containerName)
        {
            storageAccount = CloudStorageAccount.Parse(connectionString);
            blobClient = storageAccount.CreateCloudBlobClient();
            container = blobClient.GetContainerReference(containerName);
            container.CreateIfNotExists();
        }

        public override ResultType<RestoreItem> Process(RestoreItem item)
        {

            //lock (syncRoot)
            //{
            //    if (uploadedFiles.Contains(item.RemoteName))
            //    {
            //        // file was already uploaded lets conserve bandwidth
            //        return ResultType<RestoreItem>.Finished("Yey !", item.RemoteName, item.LocalFilePath);
            //    }
            //    uploadedFiles.Add(item.RemoteName);
            //}
            var blobRef = container.GetBlockBlobReference(item.RemoteName);
            MemoryStream ms = new MemoryStream();
            blobRef.DownloadToStream(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return ProcessNext(new StreamRestoreItem(item.LocalFilePath,item.RemoteName,ms));
        }


    }
}
