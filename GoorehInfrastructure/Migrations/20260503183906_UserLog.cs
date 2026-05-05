using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoorehInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedIn",
                table: "UserLogData");

            migrationBuilder.DropColumn(
                name: "EditedIn",
                table: "UserLogData");

            migrationBuilder.DropColumn(
                name: "UpperUsername",
                table: "UserLogData");

            migrationBuilder.RenameColumn(
                name: "Guid",
                table: "UserLogData",
                newName: "LogGuid");

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "UserLogData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserLogData_AppUserId",
                table: "UserLogData",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogData_AppUsers_AppUserId",
                table: "UserLogData",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLogData_AppUsers_AppUserId",
                table: "UserLogData");

            migrationBuilder.DropIndex(
                name: "IX_UserLogData_AppUserId",
                table: "UserLogData");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "UserLogData");

            migrationBuilder.RenameColumn(
                name: "LogGuid",
                table: "UserLogData",
                newName: "Guid");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedIn",
                table: "UserLogData",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedIn",
                table: "UserLogData",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpperUsername",
                table: "UserLogData",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
