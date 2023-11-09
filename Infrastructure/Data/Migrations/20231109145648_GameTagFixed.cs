using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class GameTagFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GameId",
                table: "Tag",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tag_GameId",
                table: "Tag",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Games_GameId",
                table: "Tag",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Games_GameId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_GameId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Tag");
        }
    }
}
