using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SonoTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAccidentAndMaintenanceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accidents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    TownId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GeoPointId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccidentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccidentTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FloatingUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attach = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Accidents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accidents_AccidentTypes_AccidentTypeId",
                        column: x => x.AccidentTypeId,
                        principalTable: "AccidentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accidents_FloatingUnits_FloatingUnitId",
                        column: x => x.FloatingUnitId,
                        principalTable: "FloatingUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accidents_GeoPoints_GeoPointId",
                        column: x => x.GeoPointId,
                        principalTable: "GeoPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accidents_Organizations_OrganizationId1",
                        column: x => x.OrganizationId1,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accidents_Towns_TownId",
                        column: x => x.TownId,
                        principalTable: "Towns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Maintenances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    MaintenanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NextMaintenanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MaintenanceTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FloatingUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaintenanceReport = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Other = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Maintenances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Maintenances_FloatingUnits_FloatingUnitId",
                        column: x => x.FloatingUnitId,
                        principalTable: "FloatingUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Maintenances_MaintenanceTypes_MaintenanceTypeId",
                        column: x => x.MaintenanceTypeId,
                        principalTable: "MaintenanceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accidents_AccidentTypeId",
                table: "Accidents",
                column: "AccidentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Accidents_FloatingUnitId",
                table: "Accidents",
                column: "FloatingUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Accidents_GeoPointId",
                table: "Accidents",
                column: "GeoPointId");

            migrationBuilder.CreateIndex(
                name: "IX_Accidents_OrganizationId1",
                table: "Accidents",
                column: "OrganizationId1");

            migrationBuilder.CreateIndex(
                name: "IX_Accidents_TownId",
                table: "Accidents",
                column: "TownId");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenances_FloatingUnitId",
                table: "Maintenances",
                column: "FloatingUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenances_MaintenanceTypeId",
                table: "Maintenances",
                column: "MaintenanceTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accidents");

            migrationBuilder.DropTable(
                name: "Maintenances");
        }
    }
}
