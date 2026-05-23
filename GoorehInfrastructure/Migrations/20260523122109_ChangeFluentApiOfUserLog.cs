using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoorehInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFluentApiOfUserLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLogDatas_AppUsers_AppUserId",
                table: "UserLogDatas");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogDatas_AppUsers_AppUserId",
                table: "UserLogDatas",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLogDatas_AppUsers_AppUserId",
                table: "UserLogDatas");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogDatas_AppUsers_AppUserId",
                table: "UserLogDatas",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
