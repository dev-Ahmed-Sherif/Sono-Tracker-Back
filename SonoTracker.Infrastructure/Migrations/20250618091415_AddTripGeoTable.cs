using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SonoTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTripGeoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TouristMarinaNumber",
                table: "MarinaTrips");

            migrationBuilder.CreateTable(
                name: "GeneralInspections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    InspectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TripInformationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InspectionTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsInspected = table.Column<bool>(type: "bit", nullable: false),
                    SaftyPetroleumWaste = table.Column<bool>(type: "bit", nullable: false),
                    RightWasteDisposal = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InspectionAttachment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
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
                    table.PrimaryKey("PK_GeneralInspections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralInspections_InspectionTypes_InspectionTypeId",
                        column: x => x.InspectionTypeId,
                        principalTable: "InspectionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneralInspections_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneralInspections_TripInformations_TripInformationId",
                        column: x => x.TripInformationId,
                        principalTable: "TripInformations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralInspections_InspectionTypeId",
                table: "GeneralInspections",
                column: "InspectionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralInspections_OrganizationId",
                table: "GeneralInspections",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralInspections_TripInformationId",
                table: "GeneralInspections",
                column: "TripInformationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralInspections");

            migrationBuilder.AddColumn<int>(
                name: "TouristMarinaNumber",
                table: "MarinaTrips",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
