using System;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using service_app.Configuration;

namespace service_app.Services
{
    public class SftpService : ISftpService
    {
        private readonly ILogger<SftpService> _logger;
        private readonly IConfiguration _config;

        public SftpService(ILogger<SftpService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public void DownloadFile(string remoteFilePath, string localFilePath)
        {
            using var client = new SftpClient(_config["SftpCongig:Host"], Int16.Parse(_config["SftpCongig:Port"]), _config["SftpCongig:UserName"], _config["SftpCongig:Password"]);
            try
            {
                client.Connect();
                string folderPath = Path.GetDirectoryName(Path.GetFullPath(localFilePath));

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                using var s = File.Create(localFilePath);
                client.DownloadFile(remoteFilePath, s);
                _logger.LogInformation($"Finished downloading file [{localFilePath}] from [{remoteFilePath}]");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed in downloading file [{localFilePath}] from [{remoteFilePath}]");
            }
            finally
            {
                client.Disconnect();
            }
        }

        public IEnumerable<SftpFile>? ListAllFiles(string remoteDirectory = ".")
        {
            using var client = new SftpClient(_config["SftpCongig:Host"], Int16.Parse(_config["SftpCongig:Port"]), _config["SftpCongig:UserName"], _config["SftpCongig:Password"]);
            try
            {
                client.Connect();
                return GetFileFromPath(client, remoteDirectory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed in listing files under [{remoteDirectory}]");
                return null;
            }
            finally
            {
                client.Disconnect();
            }
        }

        private List<SftpFile>? GetFileFromPath(SftpClient client, string path = ".") {
            try
            {
                List<SftpFile> result = new List<SftpFile>();
                var files = client.ListDirectory(path);
                
                foreach (var file in files)
                {
                    if (file.IsDirectory && !file.FullName.Contains("/."))
                    {
                        result.AddRange(GetFileFromPath(client, file.FullName));
                    }
                    else if(!file.IsDirectory) result.Add(file);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed in listing files under [{path}]");
                return null;
            }
        }
    }
}

