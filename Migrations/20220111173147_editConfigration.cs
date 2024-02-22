using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharmacy.Migrations
{
    public partial class editConfigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AboutContentAr",
                table: "Configurations");

            migrationBuilder.DropColumn(
                name: "AboutContentEn",
                table: "Configurations");

            migrationBuilder.DropColumn(
                name: "ContactContentAr",
                table: "Configurations");

            migrationBuilder.DropColumn(
                name: "ContactContentEn",
                table: "Configurations");

            migrationBuilder.RenameColumn(
                name: "TermsContentEn",
                table: "Configurations",
                newName: "WhatsApp");

            migrationBuilder.RenameColumn(
                name: "TermsContentAr",
                table: "Configurations",
                newName: "Twitter");

            migrationBuilder.RenameColumn(
                name: "ReturnContentEn",
                table: "Configurations",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "ReturnContentAr",
                table: "Configurations",
                newName: "LinkedIn");

            migrationBuilder.RenameColumn(
                name: "PrivacyContentAr",
                table: "Configurations",
                newName: "Instgram");

            migrationBuilder.RenameColumn(
                name: "PrivacyContent1En",
                table: "Configurations",
                newName: "Facebook");

            migrationBuilder.RenameColumn(
                name: "FaqcontentEn",
                table: "Configurations",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "FaqcontentAr",
                table: "Configurations",
                newName: "Address");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WhatsApp",
                table: "Configurations",
                newName: "TermsContentEn");

            migrationBuilder.RenameColumn(
                name: "Twitter",
                table: "Configurations",
                newName: "TermsContentAr");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Configurations",
                newName: "ReturnContentEn");

            migrationBuilder.RenameColumn(
                name: "LinkedIn",
                table: "Configurations",
                newName: "ReturnContentAr");

            migrationBuilder.RenameColumn(
                name: "Instgram",
                table: "Configurations",
                newName: "PrivacyContentAr");

            migrationBuilder.RenameColumn(
                name: "Facebook",
                table: "Configurations",
                newName: "PrivacyContent1En");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Configurations",
                newName: "FaqcontentEn");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Configurations",
                newName: "FaqcontentAr");

            migrationBuilder.AddColumn<string>(
                name: "AboutContentAr",
                table: "Configurations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AboutContentEn",
                table: "Configurations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactContentAr",
                table: "Configurations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactContentEn",
                table: "Configurations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
