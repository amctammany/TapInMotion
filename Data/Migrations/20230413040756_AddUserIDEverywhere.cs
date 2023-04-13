using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TapInMotion.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIDEverywhere : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Student",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccountType",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Administrator",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserID",
                table: "Administrator",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Administrator_UserID",
                table: "Administrator",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Administrator_AspNetUsers_UserID",
                table: "Administrator",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Administrator_AspNetUsers_UserID",
                table: "Administrator");

            migrationBuilder.DropIndex(
                name: "IX_Administrator_UserID",
                table: "Administrator");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "AccountType",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Administrator");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Administrator");
        }
    }
}
