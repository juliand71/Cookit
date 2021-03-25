using Microsoft.EntityFrameworkCore.Migrations;

namespace Cookit.Migrations
{
    public partial class authorizationRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Recipe");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Recipe",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_AuthorId",
                table: "Recipe",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_AspNetUsers_AuthorId",
                table: "Recipe",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_AspNetUsers_AuthorId",
                table: "Recipe");

            migrationBuilder.DropIndex(
                name: "IX_Recipe_AuthorId",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Recipe");

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "Recipe",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
