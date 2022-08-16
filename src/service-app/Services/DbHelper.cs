using System;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using service_app.Data;
using service_app.Models;

namespace service_app.Services
{
    public class DbHelper
    {
        private DataContext _dbContext;

        private DbContextOptions<DataContext> GetAllOptions()
        {
            DbContextOptionsBuilder<DataContext> optionsBuilder = new DbContextOptionsBuilder<DataContext>();

            optionsBuilder.UseNpgsql(AppSettings.ConnectionString);

            return optionsBuilder.Options;
        }

        public DateTime? GetLastExecutionDate() {
            using (_dbContext = new DataContext(GetAllOptions())) {
                try
                {
                    var lastExecution = _dbContext.ExecutionLogs.OrderByDescending(o => o.ExecutionTime).First();
                    if (lastExecution != null)
                        return lastExecution.ExecutionTime;
                    return null;
                }
                catch
                {
                    return null;
                }
            }
        }

        public void AddExecution(int fileCount) {
            using (_dbContext = new DataContext(GetAllOptions()))
            {
                var newExecution = new ExecutionLog() {
                    ExecutionTime = DateTime.UtcNow,
                    FileDownloaded = fileCount,
                };
                _dbContext.ExecutionLogs.Add(newExecution);

                _dbContext.SaveChanges();
            }
        }

        public void AddFile(string filePath) {
            using (_dbContext = new DataContext(GetAllOptions()))
            {
                var newFile = new ServerFile()
                {
                     FilePath = filePath,
                     CreationDate = DateTime.UtcNow,
                     Downloaded = true,
                };

                _dbContext.ServerFiles.Add(newFile);
                _dbContext.SaveChanges();
            }
        }
    }
}

