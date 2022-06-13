using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class addedApprovalStatusEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "TextMaterials");

            migrationBuilder.AddColumn<int>(
                name: "ApprovalStatus",
                table: "TextMaterials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ApprovalStatusId",
                table: "TextMaterials",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "TextMaterials");

            migrationBuilder.DropColumn(
                name: "ApprovalStatusId",
                table: "TextMaterials");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "TextMaterials",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
