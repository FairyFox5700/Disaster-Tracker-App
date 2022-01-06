using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace DisasterTrackerApp.Dal.Migrations
{
    public partial class MakeGoogleEventCoordinatesNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coordiantes",
                table: "CalendarEvents");

            migrationBuilder.AddColumn<Point>(
                name: "Coordinates",
                table: "CalendarEvents",
                type: "geometry",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coordinates",
                table: "CalendarEvents");

            migrationBuilder.AddColumn<Point>(
                name: "Coordiantes",
                table: "CalendarEvents",
                type: "geometry",
                nullable: false);
        }
    }
}
