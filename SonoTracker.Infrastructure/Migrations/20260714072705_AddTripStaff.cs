using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SonoTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTripStaff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TripPassengers",
                schema: "trip",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Job = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    IDType = table.Column<int>(type: "int", nullable: false),
                    Identity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NationalityId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TripInformationId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GovernorateId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripPassengers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripPassengers_Governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalSchema: "lookups",
                        principalTable: "Governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TripPassengers_Nationalities_NationalityId",
                        column: x => x.NationalityId,
                        principalSchema: "lookups",
                        principalTable: "Nationalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TripPassengers_TripInformations_TripInformationId",
                        column: x => x.TripInformationId,
                        principalSchema: "trip",
                        principalTable: "TripInformations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TripStaffs",
                schema: "trip",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Job = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    IDType = table.Column<int>(type: "int", nullable: false),
                    Identity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NationalityId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TripInformationId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GovernorateId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripStaffs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripStaffs_Governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalSchema: "lookups",
                        principalTable: "Governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TripStaffs_Nationalities_NationalityId",
                        column: x => x.NationalityId,
                        principalSchema: "lookups",
                        principalTable: "Nationalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TripStaffs_TripInformations_TripInformationId",
                        column: x => x.TripInformationId,
                        principalSchema: "trip",
                        principalTable: "TripInformations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TripPassengers_GovernorateId",
                schema: "trip",
                table: "TripPassengers",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_TripPassengers_NationalityId",
                schema: "trip",
                table: "TripPassengers",
                column: "NationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_TripPassengers_TripInformationId",
                schema: "trip",
                table: "TripPassengers",
                column: "TripInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_TripStaffs_GovernorateId",
                schema: "trip",
                table: "TripStaffs",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_TripStaffs_NationalityId",
                schema: "trip",
                table: "TripStaffs",
                column: "NationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_TripStaffs_TripInformationId",
                schema: "trip",
                table: "TripStaffs",
                column: "TripInformationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TripPassengers",
                schema: "trip");

            migrationBuilder.DropTable(
                name: "TripStaffs",
                schema: "trip");
        }
    }
}
