using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SonoTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RoleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LicenseApplications_AspNetUsers_DelegateUserId",
                table: "LicenseApplications");

            migrationBuilder.DropIndex(
                name: "IX_LicenseApplications_DelegateUserId",
                table: "LicenseApplications");

            migrationBuilder.DropColumn(
                name: "DelegateUserId",
                table: "LicenseApplications");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetRoles",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetRoles");

            migrationBuilder.AddColumn<string>(
                name: "DelegateUserId",
                table: "LicenseApplications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseApplications_DelegateUserId",
                table: "LicenseApplications",
                column: "DelegateUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LicenseApplications_AspNetUsers_DelegateUserId",
                table: "LicenseApplications",
                column: "DelegateUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
