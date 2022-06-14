using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class addedAuthorToTextMaterial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorId1",
                table: "TextMaterials",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TextMaterials_AuthorId1",
                table: "TextMaterials",
                column: "AuthorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TextMaterials_AspNetUsers_AuthorId1",
                table: "TextMaterials",
                column: "AuthorId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TextMaterials_AspNetUsers_AuthorId1",
                table: "TextMaterials");

            migrationBuilder.DropIndex(
                name: "IX_TextMaterials_AuthorId1",
                table: "TextMaterials");

            migrationBuilder.DropColumn(
                name: "AuthorId1",
                table: "TextMaterials");
        }
    }
}
