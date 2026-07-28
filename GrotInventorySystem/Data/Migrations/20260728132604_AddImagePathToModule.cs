using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrotInventorySystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImagePathToModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Modules",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Modules");
        }
    }
}
