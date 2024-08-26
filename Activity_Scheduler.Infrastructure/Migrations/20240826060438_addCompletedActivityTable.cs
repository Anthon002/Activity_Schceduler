using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Activity_Scheduler.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addCompletedActivityTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
