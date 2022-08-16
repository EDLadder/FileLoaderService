using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace service_app.Migrations
{
    public partial class AddExecutions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExecutionLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExecutionTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FileDownloaded = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionLogs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExecutionLogs");
        }
    }
}
