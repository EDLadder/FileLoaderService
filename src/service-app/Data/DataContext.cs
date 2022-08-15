using System;
using Microsoft.EntityFrameworkCore;
using service_app.Models;

namespace service_app.Data
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to postgres with connection string from app settings
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
        }

        // accessing and managing serverfile from db
        public DbSet<ServerFile> ServerFiles { get; set; }
    }
}

