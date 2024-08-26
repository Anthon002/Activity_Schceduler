using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Activity_Scheduler.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addExpiredActivityTableViewModelAndCompletedActivityTableVM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Activity",
                table: "Activity");

            migrationBuilder.RenameTable(
                name: "Activity",
                newName: "ActivityTable");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActivityTable",
                table: "ActivityTable",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CompletedActivityTable",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    reminderTime = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedActivityTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExpiredActivityTable",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    reminderTime = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpiredActivityTable", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompletedActivityTable");

            migrationBuilder.DropTable(
                name: "ExpiredActivityTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActivityTable",
                table: "ActivityTable");

            migrationBuilder.RenameTable(
                name: "ActivityTable",
                newName: "Activity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activity",
                table: "Activity",
                column: "Id");
        }
    }
}
