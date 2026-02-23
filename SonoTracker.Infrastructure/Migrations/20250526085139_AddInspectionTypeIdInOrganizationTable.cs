using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SonoTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInspectionTypeIdInOrganizationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "TripInformations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "InspectionTypeId",
                table: "Organizations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_InspectionTypeId",
                table: "Organizations",
                column: "InspectionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_InspectionTypes_InspectionTypeId",
                table: "Organizations",
                column: "InspectionTypeId",
                principalTable: "InspectionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_InspectionTypes_InspectionTypeId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_InspectionTypeId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "TripInformations");

            migrationBuilder.DropColumn(
                name: "InspectionTypeId",
                table: "Organizations");
        }
    }
}
