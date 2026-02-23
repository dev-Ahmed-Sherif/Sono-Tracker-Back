using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SonoTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLicenceApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "LicenseApplications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CivilProtection",
                table: "LicenseApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CommercialRegister",
                table: "LicenseApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Insurance",
                table: "LicenseApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Irrigation",
                table: "LicenseApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Other",
                table: "LicenseApplications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SendMail",
                table: "LicenseApplications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "StateProperty",
                table: "LicenseApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "LicenseApplications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Taxes",
                table: "LicenseApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TouristMarinaNumber",
                table: "LicenseApplications",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CivilProtection",
                table: "LicenseApplications");

            migrationBuilder.DropColumn(
                name: "CommercialRegister",
                table: "LicenseApplications");

            migrationBuilder.DropColumn(
                name: "Insurance",
                table: "LicenseApplications");

            migrationBuilder.DropColumn(
                name: "Irrigation",
                table: "LicenseApplications");

            migrationBuilder.DropColumn(
                name: "Other",
                table: "LicenseApplications");

            migrationBuilder.DropColumn(
                name: "SendMail",
                table: "LicenseApplications");

            migrationBuilder.DropColumn(
                name: "StateProperty",
                table: "LicenseApplications");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "LicenseApplications");

            migrationBuilder.DropColumn(
                name: "Taxes",
                table: "LicenseApplications");

            migrationBuilder.DropColumn(
                name: "TouristMarinaNumber",
                table: "LicenseApplications");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "LicenseApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
