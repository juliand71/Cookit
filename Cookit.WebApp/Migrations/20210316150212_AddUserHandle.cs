using Microsoft.EntityFrameworkCore.Migrations;

namespace Cookit.WebApp.Migrations
{
    public partial class AddUserHandle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageCaption",
                table: "Recipe");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "AspNetUsers",
                newName: "Handle");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Handle",
                table: "AspNetUsers",
                newName: "FullName");

            migrationBuilder.AddColumn<string>(
                name: "ImageCaption",
                table: "Recipe",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
