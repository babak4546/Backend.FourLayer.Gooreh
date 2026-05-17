using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoorehInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProduct2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProduct",
                table: "UserProduct");

            migrationBuilder.RenameTable(
                name: "UserProduct",
                newName: "UserProducts");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "UserProducts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProducts",
                table: "UserProducts",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProducts",
                table: "UserProducts");

            migrationBuilder.RenameTable(
                name: "UserProducts",
                newName: "UserProduct");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "UserProduct",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProduct",
                table: "UserProduct",
                column: "Id");
        }
    }
}
