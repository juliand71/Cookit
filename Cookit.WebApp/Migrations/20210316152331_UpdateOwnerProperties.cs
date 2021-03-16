using Microsoft.EntityFrameworkCore.Migrations;

namespace Cookit.WebApp.Migrations
{
    public partial class UpdateOwnerProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Owner",
                table: "Recipe",
                newName: "OwnerHandle");

            migrationBuilder.AddColumn<string>(
                name: "OwnerEmail",
                table: "Recipe",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerEmail",
                table: "Recipe");

            migrationBuilder.RenameColumn(
                name: "OwnerHandle",
                table: "Recipe",
                newName: "Owner");
        }
    }
}
