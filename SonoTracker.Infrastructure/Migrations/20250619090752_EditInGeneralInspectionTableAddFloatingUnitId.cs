using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SonoTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditInGeneralInspectionTableAddFloatingUnitId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralInspections_TripInformations_TripInformationId",
                table: "GeneralInspections");

            migrationBuilder.AlterColumn<Guid>(
                name: "TripInformationId",
                table: "GeneralInspections",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "FloatingUnitId",
                table: "GeneralInspections",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeneralInspections_FloatingUnitId",
                table: "GeneralInspections",
                column: "FloatingUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralInspections_FloatingUnits_FloatingUnitId",
                table: "GeneralInspections",
                column: "FloatingUnitId",
                principalTable: "FloatingUnits",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralInspections_TripInformations_TripInformationId",
                table: "GeneralInspections",
                column: "TripInformationId",
                principalTable: "TripInformations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralInspections_FloatingUnits_FloatingUnitId",
                table: "GeneralInspections");

            migrationBuilder.DropForeignKey(
                name: "FK_GeneralInspections_TripInformations_TripInformationId",
                table: "GeneralInspections");

            migrationBuilder.DropIndex(
                name: "IX_GeneralInspections_FloatingUnitId",
                table: "GeneralInspections");

            migrationBuilder.DropColumn(
                name: "FloatingUnitId",
                table: "GeneralInspections");

            migrationBuilder.AlterColumn<Guid>(
                name: "TripInformationId",
                table: "GeneralInspections",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralInspections_TripInformations_TripInformationId",
                table: "GeneralInspections",
                column: "TripInformationId",
                principalTable: "TripInformations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
