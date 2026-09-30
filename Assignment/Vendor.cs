using System;

namespace Assignment
{
    public class S3Storage : ICloudStorageProvider
    {
        public void UploadFile(string path)
        {
            Console.WriteLine($"Uploading '{path}' to Amazon S3");
        }

        public void DownloadFile(string fileId)
        {
            Console.WriteLine($"Downloading file {fileId} from Amazon S3");
        }

        public void DeleteFile(string fileId)
        {
            Console.WriteLine($"Deleting file {fileId} from Amazon S3");
        }
    }


    public class AzureStorage : ICloudStorageProvider
    {
        public void UploadFile(string path)
        {
            Console.WriteLine($"Uploading '{path}' to Azure Blob Storage");
        }

        public void DownloadFile(string fileId)
        {
            Console.WriteLine($"Downloading file {fileId} from Azure Blob Storage");
        }

        public void DeleteFile(string fileId)
        {
            Console.WriteLine($"Deleting file {fileId} from Azure Blob Storage");
        }
    }


    public class GoogleStorage : ICloudStorageProvider
    {
        public void UploadFile(string path)
        {
            Console.WriteLine($"Uploading '{path}' to Google Cloud Storage");
        }

        public void DownloadFile(string fileId)
        {
            Console.WriteLine($"Downloading file {fileId} from Google Cloud Storage");
        }

        public void DeleteFile(string fileId)
        {
            Console.WriteLine($"Deleting file {fileId} from Google Cloud Storage");
        }
    }
}