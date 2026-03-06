using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrotInventorySystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToWeaponAndModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Weapons",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Modules",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Modules");
        }
    }
}
