using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoorehInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class chageRoleTitleType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleTitle",
                table: "Roles");

            migrationBuilder.AddColumn<int>(
                name: "RoleTitleId",
                table: "Roles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserLogData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogedIn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LoggedOut = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IpAddr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SysInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Guid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditedIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpperUsername = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogData", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RoleTitleId",
                table: "Roles",
                column: "RoleTitleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_UserLogData_RoleTitleId",
                table: "Roles",
                column: "RoleTitleId",
                principalTable: "UserLogData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_UserLogData_RoleTitleId",
                table: "Roles");

            migrationBuilder.DropTable(
                name: "UserLogData");

            migrationBuilder.DropIndex(
                name: "IX_Roles_RoleTitleId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "RoleTitleId",
                table: "Roles");

            migrationBuilder.AddColumn<string>(
                name: "RoleTitle",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
