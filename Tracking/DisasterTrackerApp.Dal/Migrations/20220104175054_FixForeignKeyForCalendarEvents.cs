using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisasterTrackerApp.Dal.Migrations
{
    public partial class FixForeignKeyForCalendarEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarEvents_Calendars_CalendarId1",
                table: "CalendarEvents");

            migrationBuilder.DropIndex(
                name: "IX_CalendarEvents_CalendarId1",
                table: "CalendarEvents");

            migrationBuilder.DropColumn(
                name: "CalendarId1",
                table: "CalendarEvents");
            
            //
            migrationBuilder.DropColumn(
                name: "CalendarId",
                table: "CalendarEvents");
            
            migrationBuilder.AddColumn<Guid>(
                name: "CalendarId",
                table: "CalendarEvents",
                type: "uuid",
                nullable: false);
            
            /*
             migrationBuilder.AlterColumn<Guid>(
                name: "CalendarId",
                table: "CalendarEvents",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
             */
            //
            
            migrationBuilder.CreateIndex(
                name: "IX_CalendarEvents_CalendarId",
                table: "CalendarEvents",
                column: "CalendarId");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarEvents_Calendars_CalendarId",
                table: "CalendarEvents",
                column: "CalendarId",
                principalTable: "Calendars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarEvents_Calendars_CalendarId",
                table: "CalendarEvents");

            migrationBuilder.DropIndex(
                name: "IX_CalendarEvents_CalendarId",
                table: "CalendarEvents");

            migrationBuilder.AlterColumn<string>(
                name: "CalendarId",
                table: "CalendarEvents",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "CalendarId1",
                table: "CalendarEvents",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEvents_CalendarId1",
                table: "CalendarEvents",
                column: "CalendarId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarEvents_Calendars_CalendarId1",
                table: "CalendarEvents",
                column: "CalendarId1",
                principalTable: "Calendars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
