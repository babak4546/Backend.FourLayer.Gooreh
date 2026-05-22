using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoorehInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addDobyuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_dbContextLoggers",
                table: "dbContextLoggers");

            migrationBuilder.RenameTable(
                name: "dbContextLoggers",
                newName: "DbContextLoggers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DbContextLoggers",
                table: "DbContextLoggers",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DbContextLoggers",
                table: "DbContextLoggers");

            migrationBuilder.RenameTable(
                name: "DbContextLoggers",
                newName: "dbContextLoggers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_dbContextLoggers",
                table: "dbContextLoggers",
                column: "Id");
        }
    }
}
