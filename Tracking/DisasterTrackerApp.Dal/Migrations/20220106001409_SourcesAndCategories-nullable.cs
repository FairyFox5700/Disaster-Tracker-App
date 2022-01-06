using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisasterTrackerApp.Dal.Migrations
{
    public partial class SourcesAndCategoriesnullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DisasterPropertyEntityId",
                table: "Sources",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sources_DisasterPropertyEntityId",
                table: "Sources",
                column: "DisasterPropertyEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sources_DisasterPropertyEntity_DisasterPropertyEntityId",
                table: "Sources",
                column: "DisasterPropertyEntityId",
                principalTable: "DisasterPropertyEntity",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sources_DisasterPropertyEntity_DisasterPropertyEntityId",
                table: "Sources");

            migrationBuilder.DropIndex(
                name: "IX_Sources_DisasterPropertyEntityId",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "DisasterPropertyEntityId",
                table: "Sources");
        }
    }
}
