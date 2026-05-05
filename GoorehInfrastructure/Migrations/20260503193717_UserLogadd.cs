using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoorehInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserLogadd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_UserLogData_RoleTitleId",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLogData_AppUsers_AppUserId",
                table: "UserLogData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLogData",
                table: "UserLogData");

            migrationBuilder.RenameTable(
                name: "UserLogData",
                newName: "UserLogDatas");

            migrationBuilder.RenameIndex(
                name: "IX_UserLogData_AppUserId",
                table: "UserLogDatas",
                newName: "IX_UserLogDatas_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLogDatas",
                table: "UserLogDatas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_UserLogDatas_RoleTitleId",
                table: "Roles",
                column: "RoleTitleId",
                principalTable: "UserLogDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogDatas_AppUsers_AppUserId",
                table: "UserLogDatas",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_UserLogDatas_RoleTitleId",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLogDatas_AppUsers_AppUserId",
                table: "UserLogDatas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLogDatas",
                table: "UserLogDatas");

            migrationBuilder.RenameTable(
                name: "UserLogDatas",
                newName: "UserLogData");

            migrationBuilder.RenameIndex(
                name: "IX_UserLogDatas_AppUserId",
                table: "UserLogData",
                newName: "IX_UserLogData_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLogData",
                table: "UserLogData",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_UserLogData_RoleTitleId",
                table: "Roles",
                column: "RoleTitleId",
                principalTable: "UserLogData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogData_AppUsers_AppUserId",
                table: "UserLogData",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
