using System;
namespace service_app.Models
{
    public class ServerFile
    {
        public Guid Id { get; set; }
        public string FilePath { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Downloaded { get; set; }
    }
}

