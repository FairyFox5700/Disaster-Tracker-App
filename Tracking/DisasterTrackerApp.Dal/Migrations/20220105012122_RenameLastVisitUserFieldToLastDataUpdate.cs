using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisasterTrackerApp.Dal.Migrations
{
    public partial class RenameLastVisitUserFieldToLastDataUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastVisit",
                table: "GoogleUsers",
                newName: "LastDataUpdate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastDataUpdate",
                table: "GoogleUsers",
                newName: "LastVisit");
        }
    }
}
