using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class changedPostCategoryToTextMaterialCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TextMaterials_TextMaterialCategory_PostCategoryId",
                table: "TextMaterials");

            migrationBuilder.RenameColumn(
                name: "PostCategoryId",
                table: "TextMaterials",
                newName: "TextMaterialCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_TextMaterials_PostCategoryId",
                table: "TextMaterials",
                newName: "IX_TextMaterials_TextMaterialCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_TextMaterials_TextMaterialCategory_TextMaterialCategoryId",
                table: "TextMaterials",
                column: "TextMaterialCategoryId",
                principalTable: "TextMaterialCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TextMaterials_TextMaterialCategory_TextMaterialCategoryId",
                table: "TextMaterials");

            migrationBuilder.RenameColumn(
                name: "TextMaterialCategoryId",
                table: "TextMaterials",
                newName: "PostCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_TextMaterials_TextMaterialCategoryId",
                table: "TextMaterials",
                newName: "IX_TextMaterials_PostCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_TextMaterials_TextMaterialCategory_PostCategoryId",
                table: "TextMaterials",
                column: "PostCategoryId",
                principalTable: "TextMaterialCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
