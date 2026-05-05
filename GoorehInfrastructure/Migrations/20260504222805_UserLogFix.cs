using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoorehInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserLogFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_UserLogDatas_RoleTitleId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_RoleTitleId",
                table: "Roles");

            migrationBuilder.RenameColumn(
                name: "RoleTitleId",
                table: "Roles",
                newName: "RoleTitle");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RoleTitle",
                table: "Roles",
                newName: "RoleTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RoleTitleId",
                table: "Roles",
                column: "RoleTitleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_UserLogDatas_RoleTitleId",
                table: "Roles",
                column: "RoleTitleId",
                principalTable: "UserLogDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
