using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SonoTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReturnEnhace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdentityNumber",
                table: "OrganizationStaffs",
                newName: "Identity");

            migrationBuilder.RenameColumn(
                name: "IdentityNumber",
                table: "FloatingUnitStaffs",
                newName: "Identity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Identity",
                table: "OrganizationStaffs",
                newName: "IdentityNumber");

            migrationBuilder.RenameColumn(
                name: "Identity",
                table: "FloatingUnitStaffs",
                newName: "IdentityNumber");
        }
    }
}
