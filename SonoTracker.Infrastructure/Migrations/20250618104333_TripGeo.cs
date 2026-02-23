using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SonoTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TripGeo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inspection");

            migrationBuilder.CreateTable(
                name: "TripGeos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TripInformationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GeoPointId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_TripGeos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripGeos_GeoPoints_GeoPointId",
                        column: x => x.GeoPointId,
                        principalTable: "GeoPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TripGeos_TripInformations_TripInformationId",
                        column: x => x.TripInformationId,
                        principalTable: "TripInformations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TripGeos_GeoPointId",
                table: "TripGeos",
                column: "GeoPointId");

            migrationBuilder.CreateIndex(
                name: "IX_TripGeos_TripInformationId",
                table: "TripGeos",
                column: "TripInformationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TripGeos");

            migrationBuilder.CreateTable(
                name: "Inspection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TripInformationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    CreatedByEmployeeAr = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedByEmployeeEn = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedByEmployeeId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InspectionAttachment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InspectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InspectionTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsInspected = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedByEmployeeAr = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ModifiedByEmployeeEn = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ModifiedByEmployeeId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RightWasteDisposal = table.Column<bool>(type: "bit", nullable: false),
                    SaftyPetroleumWaste = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inspection_InspectionTypes_InspectionTypeId",
                        column: x => x.InspectionTypeId,
                        principalTable: "InspectionTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Inspection_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inspection_TripInformations_TripInformationId",
                        column: x => x.TripInformationId,
                        principalTable: "TripInformations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inspection_InspectionTypeId",
                table: "Inspection",
                column: "InspectionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspection_OrganizationId",
                table: "Inspection",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspection_TripInformationId",
                table: "Inspection",
                column: "TripInformationId");
        }
    }
}
