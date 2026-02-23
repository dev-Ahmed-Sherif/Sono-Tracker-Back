using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SonoTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOrgIdAccident : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accidents_Organizations_OrganizationId1",
                table: "Accidents");

            migrationBuilder.DropIndex(
                name: "IX_Accidents_OrganizationId1",
                table: "Accidents");

            //migrationBuilder.DropColumn(
            //    name: "OrganizationId1",
            //    table: "Accidents");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganizationId",
                table: "Accidents",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Accidents_OrganizationId",
                table: "Accidents",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accidents_Organizations_OrganizationId",
                table: "Accidents",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accidents_Organizations_OrganizationId",
                table: "Accidents");

            migrationBuilder.DropIndex(
                name: "IX_Accidents_OrganizationId",
                table: "Accidents");

            migrationBuilder.AlterColumn<int>(
                name: "OrganizationId",
                table: "Accidents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            //migrationBuilder.AddColumn<Guid>(
            //    name: "OrganizationId1",
            //    table: "Accidents",
            //    type: "uniqueidentifier",
            //    nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accidents_OrganizationId1",
                table: "Accidents",
                column: "OrganizationId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Accidents_Organizations_OrganizationId1",
                table: "Accidents",
                column: "OrganizationId1",
                principalTable: "Organizations",
                principalColumn: "Id");
        }
    }
}
