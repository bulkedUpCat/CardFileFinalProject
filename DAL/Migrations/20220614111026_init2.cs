using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TextMaterials_AspNetUsers_AuthorId",
                table: "TextMaterials");

            migrationBuilder.DropIndex(
                name: "IX_TextMaterials_AuthorId",
                table: "TextMaterials");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "TextMaterials");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "TextMaterials",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateTable(
                name: "SavedTextMaterials",
                columns: table => new
                {
                    SavedTextMaterialsId = table.Column<int>(type: "int", nullable: false),
                    UsersWhoSavedId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedTextMaterials", x => new { x.SavedTextMaterialsId, x.UsersWhoSavedId });
                    table.ForeignKey(
                        name: "FK_SavedTextMaterials_AspNetUsers_UsersWhoSavedId",
                        column: x => x.UsersWhoSavedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedTextMaterials_TextMaterials_SavedTextMaterialsId",
                        column: x => x.SavedTextMaterialsId,
                        principalTable: "TextMaterials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SavedTextMaterials_UsersWhoSavedId",
                table: "SavedTextMaterials",
                column: "UsersWhoSavedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SavedTextMaterials");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "TextMaterials",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "TextMaterials",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TextMaterials_AuthorId",
                table: "TextMaterials",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_TextMaterials_AspNetUsers_AuthorId",
                table: "TextMaterials",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
