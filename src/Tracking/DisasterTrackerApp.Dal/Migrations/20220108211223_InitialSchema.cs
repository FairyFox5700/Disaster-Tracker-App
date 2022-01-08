using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace DisasterTrackerApp.Dal.Migrations
{
    public partial class InitialSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DisasterEvent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExternalApiId = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Closed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Geometry = table.Column<Geometry>(type: "geography", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisasterEvent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoogleUsers",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserGoogleId = table.Column<string>(type: "text", nullable: false),
                    AccessToken = table.Column<string>(type: "text", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: false),
                    ExpiresIn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastVisit = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoogleUsers", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Calendars",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GoogleCalendarId = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Summary = table.Column<string>(type: "text", nullable: true),
                    Primary = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Calendars_GoogleUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "GoogleUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CalendarEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GoogleEventId = table.Column<string>(type: "text", nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: true),
                    StartedTs = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndTs = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Location = table.Column<string>(type: "text", nullable: true),
                    Coordinates = table.Column<Point>(type: "geography (point)", nullable: true),
                    CalendarId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalendarEvents_Calendars_CalendarId",
                        column: x => x.CalendarId,
                        principalTable: "Calendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEvents_CalendarId",
                table: "CalendarEvents",
                column: "CalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEvents_GoogleEventId",
                table: "CalendarEvents",
                column: "GoogleEventId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Calendars_GoogleCalendarId",
                table: "Calendars",
                column: "GoogleCalendarId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Calendars_UserId",
                table: "Calendars",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GoogleUsers_UserGoogleId",
                table: "GoogleUsers",
                column: "UserGoogleId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalendarEvents");

            migrationBuilder.DropTable(
                name: "DisasterEvent");

            migrationBuilder.DropTable(
                name: "Calendars");

            migrationBuilder.DropTable(
                name: "GoogleUsers");
        }
    }
}
