using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SavedTextMaterials");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
