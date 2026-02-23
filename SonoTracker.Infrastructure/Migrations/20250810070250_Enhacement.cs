using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SonoTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Enhacement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_NotificationGroups_NotificationGroupId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_NotificationGroupId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Identity",
                table: "OrganizationStaffs");

            migrationBuilder.DropColumn(
                name: "NotificationGroupId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Identity",
                table: "FloatingUnitStaffs");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "FloatingUnitStaffs");

            migrationBuilder.RenameColumn(
                name: "unitCategory",
                table: "FloatingUnits",
                newName: "UnitCategory");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "TouristMarinas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneExtension",
                table: "OrganizationStaffs",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "OrganizationStaffs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "OrganizationStaffs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Job",
                table: "OrganizationStaffs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "OrganizationStaffs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "IdentityNumber",
                table: "OrganizationStaffs",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Organizations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Inspections",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "IdentityNumber",
                table: "FloatingUnitStaffs",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentityNumber",
                table: "OrganizationStaffs");

            migrationBuilder.DropColumn(
                name: "IdentityNumber",
                table: "FloatingUnitStaffs");

            migrationBuilder.RenameColumn(
                name: "UnitCategory",
                table: "FloatingUnits",
                newName: "unitCategory");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "TouristMarinas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneExtension",
                table: "OrganizationStaffs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "OrganizationStaffs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "OrganizationStaffs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Job",
                table: "OrganizationStaffs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "OrganizationStaffs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Identity",
                table: "OrganizationStaffs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Organizations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<Guid>(
                name: "NotificationGroupId",
                table: "Messages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Inspections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Identity",
                table: "FloatingUnitStaffs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "FloatingUnitStaffs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_NotificationGroupId",
                table: "Messages",
                column: "NotificationGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_NotificationGroups_NotificationGroupId",
                table: "Messages",
                column: "NotificationGroupId",
                principalTable: "NotificationGroups",
                principalColumn: "Id");
        }
    }
}
