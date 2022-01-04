using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace DisasterTrackerApp.Dal.Migrations
{
    public partial class ExtendDisasterEventsPoints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coordiantes",
                table: "DisasterEvent");

            migrationBuilder.CreateTable(
                name: "DisasterEventGeometry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Point = table.Column<Point>(type: "geometry", nullable: false),
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    DisasterId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisasterEventGeometry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisasterEventGeometry_DisasterEvent_DisasterId",
                        column: x => x.DisasterId,
                        principalTable: "DisasterEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DisasterEventGeometry_DisasterId",
                table: "DisasterEventGeometry",
                column: "DisasterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisasterEventGeometry");

            migrationBuilder.AddColumn<Point>(
                name: "Coordiantes",
                table: "DisasterEvent",
                type: "geometry",
                nullable: false);
        }
    }
}
