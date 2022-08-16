using Microsoft.Extensions.Logging;
using service_app.Configuration;
using service_app.Services;

namespace service_app;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ISftpService _sftpService;

    public Worker(ILogger<Worker> logger, ISftpService sftpService)
    {
        // inject default logger and sftp service
        _logger = logger;
        _sftpService = sftpService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            DbHelper dbHelper = new DbHelper();

            // get time of last execution to compare with file modification time
            var lastExecutionTime = dbHelper.GetLastExecutionDate();
            var newWorker = lastExecutionTime is null;
            var newFiles = 0;

            // get all file from sftp service
            var serverFiles = _sftpService.ListAllFiles();
            // check if are files on server
            if (serverFiles != null)
                foreach (var item in serverFiles)
                {
                    if (newWorker || DateTime.Compare((DateTime)lastExecutionTime, item.LastWriteTime) < 0) {
                        _sftpService.DownloadFile(item.FullName, String.Format("{0}/{1}", "./files", item.FullName));
                        dbHelper.AddFile(item.FullName);
                        newFiles++;
                        _logger.LogInformation(string.Format("File {0} was succesfuly downloaded", item.FullName));
                    }
                }

            dbHelper.AddExecution(newFiles);

            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(60000, stoppingToken);
        }
    }
}

