using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisasterTrackerApp.Dal.Migrations
{
    public partial class ExternalApiProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalApiId",
                table: "DisasterEvent");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalApiId",
                table: "DisasterEvent",
                type: "text",
                nullable: true);
        }
    }
}
