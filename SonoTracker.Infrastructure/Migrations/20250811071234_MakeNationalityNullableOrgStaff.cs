using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SonoTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeNationalityNullableOrgStaff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationStaffs_Nationalities_NationalityId",
                table: "OrganizationStaffs");

            migrationBuilder.AlterColumn<Guid>(
                name: "NationalityId",
                table: "OrganizationStaffs",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationStaffs_Nationalities_NationalityId",
                table: "OrganizationStaffs",
                column: "NationalityId",
                principalTable: "Nationalities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationStaffs_Nationalities_NationalityId",
                table: "OrganizationStaffs");

            migrationBuilder.AlterColumn<Guid>(
                name: "NationalityId",
                table: "OrganizationStaffs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationStaffs_Nationalities_NationalityId",
                table: "OrganizationStaffs",
                column: "NationalityId",
                principalTable: "Nationalities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
