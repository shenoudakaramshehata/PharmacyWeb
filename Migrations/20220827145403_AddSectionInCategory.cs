using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharmacy.Migrations
{
    public partial class AddSectionInCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SectionId",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_SectionId",
                table: "Categories",
                column: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Sections_SectionId",
                table: "Categories",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "SectionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Sections_SectionId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_SectionId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "Categories");
        }
    }
}
