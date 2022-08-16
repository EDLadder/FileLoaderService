using System;
namespace service_app.Models
{
    public class ExecutionLog
    {
        public Guid Id { get; set; }
        public DateTime ExecutionTime { get; set; }
        public int FileDownloaded { get; set; }
    }
}

