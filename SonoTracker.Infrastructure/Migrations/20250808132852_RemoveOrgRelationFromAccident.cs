using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SonoTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOrgRelationFromAccident : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accidents_Organizations_OrganizationId1",
                table: "Accidents");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganizationId1",
                table: "Accidents",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Accidents_Organizations_OrganizationId1",
                table: "Accidents",
                column: "OrganizationId1",
                principalTable: "Organizations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accidents_Organizations_OrganizationId1",
                table: "Accidents");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganizationId1",
                table: "Accidents",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Accidents_Organizations_OrganizationId1",
                table: "Accidents",
                column: "OrganizationId1",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
