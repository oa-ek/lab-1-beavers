using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class GameExpanded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SystemRequirements",
                table: "Games",
                newName: "ShortDescription");

            migrationBuilder.AddColumn<string>(
                name: "MainUrl",
                table: "Games",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MinSystemRequirements",
                table: "Games",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecommendedSystemRequirements",
                table: "Games",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainUrl",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "MinSystemRequirements",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "RecommendedSystemRequirements",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "ShortDescription",
                table: "Games",
                newName: "SystemRequirements");
        }
    }
}
