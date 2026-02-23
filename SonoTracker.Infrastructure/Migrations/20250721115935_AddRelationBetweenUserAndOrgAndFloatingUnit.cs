using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SonoTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationBetweenUserAndOrgAndFloatingUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FloatingUnitId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FloatingUnitId",
                table: "AspNetUsers",
                column: "FloatingUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_OrganizationId",
                table: "AspNetUsers",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_FloatingUnits_FloatingUnitId",
                table: "AspNetUsers",
                column: "FloatingUnitId",
                principalTable: "FloatingUnits",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Organizations_OrganizationId",
                table: "AspNetUsers",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_FloatingUnits_FloatingUnitId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Organizations_OrganizationId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FloatingUnitId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_OrganizationId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FloatingUnitId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "AspNetUsers");
        }
    }
}
