using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class addedLikes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LikedTextMaterials",
                columns: table => new
                {
                    LikedTextMaterialsId = table.Column<int>(type: "int", nullable: false),
                    UsersWhoLikedId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikedTextMaterials", x => new { x.LikedTextMaterialsId, x.UsersWhoLikedId });
                    table.ForeignKey(
                        name: "FK_LikedTextMaterials_AspNetUsers_UsersWhoLikedId",
                        column: x => x.UsersWhoLikedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LikedTextMaterials_TextMaterials_LikedTextMaterialsId",
                        column: x => x.LikedTextMaterialsId,
                        principalTable: "TextMaterials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LikedTextMaterials_UsersWhoLikedId",
                table: "LikedTextMaterials",
                column: "UsersWhoLikedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LikedTextMaterials");
        }
    }
}
