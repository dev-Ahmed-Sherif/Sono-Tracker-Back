using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SonoTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IsDelegateAndAppliedOn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DelegateAttachment",
                table: "OrganizationStaffs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelegate",
                table: "OrganizationStaffs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "AppliedOn",
                table: "Organizations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DelegateAttachment",
                table: "FloatingUnitStaffs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelegate",
                table: "FloatingUnitStaffs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DelegateAttachment",
                table: "OrganizationStaffs");

            migrationBuilder.DropColumn(
                name: "IsDelegate",
                table: "OrganizationStaffs");

            migrationBuilder.DropColumn(
                name: "AppliedOn",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "DelegateAttachment",
                table: "FloatingUnitStaffs");

            migrationBuilder.DropColumn(
                name: "IsDelegate",
                table: "FloatingUnitStaffs");
        }
    }
}
