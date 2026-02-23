using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SonoTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Floatingunits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationAttachment_Organizations_OrganizationId",
                table: "OrganizationAttachment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationAttachment",
                table: "OrganizationAttachment");

            migrationBuilder.DropColumn(
                name: "UnitType",
                table: "FloatingUnits");

            migrationBuilder.RenameTable(
                name: "OrganizationAttachment",
                newName: "OrganizationAttachments");

            migrationBuilder.RenameIndex(
                name: "IX_OrganizationAttachment_OrganizationId",
                table: "OrganizationAttachments",
                newName: "IX_OrganizationAttachments_OrganizationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationAttachments",
                table: "OrganizationAttachments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PassengerTripAttachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    AttachmentDisplaySize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TripInformationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByEmployeeEn = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedByEmployeeAr = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ModifiedByEmployeeEn = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ModifiedByEmployeeAr = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedByEmployeeId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedByEmployeeId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassengerTripAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PassengerTripAttachments_TripInformations_TripInformationId",
                        column: x => x.TripInformationId,
                        principalTable: "TripInformations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PassengerTripAttachments_TripInformationId",
                table: "PassengerTripAttachments",
                column: "TripInformationId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationAttachments_Organizations_OrganizationId",
                table: "OrganizationAttachments",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationAttachments_Organizations_OrganizationId",
                table: "OrganizationAttachments");

            migrationBuilder.DropTable(
                name: "PassengerTripAttachments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationAttachments",
                table: "OrganizationAttachments");

            migrationBuilder.RenameTable(
                name: "OrganizationAttachments",
                newName: "OrganizationAttachment");

            migrationBuilder.RenameIndex(
                name: "IX_OrganizationAttachments_OrganizationId",
                table: "OrganizationAttachment",
                newName: "IX_OrganizationAttachment_OrganizationId");

            migrationBuilder.AddColumn<int>(
                name: "UnitType",
                table: "FloatingUnits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationAttachment",
                table: "OrganizationAttachment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationAttachment_Organizations_OrganizationId",
                table: "OrganizationAttachment",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
