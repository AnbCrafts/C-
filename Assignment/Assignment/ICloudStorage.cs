using System;

namespace Assignment
{
    public interface ICloudStorageProvider
    {
        void UploadFile(string path);

        void DownloadFile(string fileId);

        void DeleteFile(string fileId);
    }
}