using System;
using Renci.SshNet.Sftp;

namespace service_app.Services
{
    public interface ISftpService
    {
        IEnumerable<SftpFile>? ListAllFiles(string remoteDirectory = ".");
        void DownloadFile(string remoteFilePath, string localFilePath);
    }
}

