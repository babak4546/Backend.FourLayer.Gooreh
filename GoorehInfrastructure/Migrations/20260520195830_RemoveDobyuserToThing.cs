using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoorehInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDobyuserToThing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoByUsername",
                table: "AppUsers");

            migrationBuilder.CreateTable(
                name: "dbContextLoggers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Guid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoByUsername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditedIn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbContextLoggers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dbContextLoggers");

            migrationBuilder.AddColumn<string>(
                name: "DoByUsername",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
