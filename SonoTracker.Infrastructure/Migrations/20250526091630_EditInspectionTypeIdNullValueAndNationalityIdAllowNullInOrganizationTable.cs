using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SonoTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditInspectionTypeIdNullValueAndNationalityIdAllowNullInOrganizationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_InspectionTypes_InspectionTypeId",
                table: "Organizations");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Nationalities_NationalityId",
                table: "Organizations");

            migrationBuilder.AlterColumn<Guid>(
                name: "NationalityId",
                table: "Organizations",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "InspectionTypeId",
                table: "Organizations",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_InspectionTypes_InspectionTypeId",
                table: "Organizations",
                column: "InspectionTypeId",
                principalTable: "InspectionTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Nationalities_NationalityId",
                table: "Organizations",
                column: "NationalityId",
                principalTable: "Nationalities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_InspectionTypes_InspectionTypeId",
                table: "Organizations");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Nationalities_NationalityId",
                table: "Organizations");

            migrationBuilder.AlterColumn<Guid>(
                name: "NationalityId",
                table: "Organizations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "InspectionTypeId",
                table: "Organizations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_InspectionTypes_InspectionTypeId",
                table: "Organizations",
                column: "InspectionTypeId",
                principalTable: "InspectionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Nationalities_NationalityId",
                table: "Organizations",
                column: "NationalityId",
                principalTable: "Nationalities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
