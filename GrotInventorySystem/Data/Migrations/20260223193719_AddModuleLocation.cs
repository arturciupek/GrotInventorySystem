using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrotInventorySystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddModuleLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "Modules",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modules_LocationId",
                table: "Modules",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Locations_LocationId",
                table: "Modules",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Locations_LocationId",
                table: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_Modules_LocationId",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Modules");
        }
    }
}
