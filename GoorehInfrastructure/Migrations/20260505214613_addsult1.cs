using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoorehInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addsult1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salt",
                table: "AppUsers");
        }
    }
}
