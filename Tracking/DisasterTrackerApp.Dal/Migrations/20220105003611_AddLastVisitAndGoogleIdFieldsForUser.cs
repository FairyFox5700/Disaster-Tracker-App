using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisasterTrackerApp.Dal.Migrations
{
    public partial class AddLastVisitAndGoogleIdFieldsForUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastVisit",
                table: "GoogleUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserGoogleId",
                table: "GoogleUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_GoogleUsers_UserGoogleId",
                table: "GoogleUsers",
                column: "UserGoogleId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GoogleUsers_UserGoogleId",
                table: "GoogleUsers");

            migrationBuilder.DropColumn(
                name: "LastVisit",
                table: "GoogleUsers");

            migrationBuilder.DropColumn(
                name: "UserGoogleId",
                table: "GoogleUsers");
        }
    }
}
