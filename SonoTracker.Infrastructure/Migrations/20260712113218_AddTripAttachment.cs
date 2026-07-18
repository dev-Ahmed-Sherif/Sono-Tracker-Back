using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SonoTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTripAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('TripNationalities', 'NationalityNumber') IS NOT NULL
    ALTER TABLE [TripNationalities] DROP COLUMN [NationalityNumber];
");

            // Align index names left behind by earlier table renames (needed before any future AlterColumn).
            migrationBuilder.Sql(@"
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_NationalityTrips_TripInformationId' AND object_id = OBJECT_ID(N'TripNationalities'))
    EXEC sp_rename N'TripNationalities.IX_NationalityTrips_TripInformationId', N'IX_TripNationalities_TripInformationId', N'INDEX';
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_NationalityTrips_NationalityId' AND object_id = OBJECT_ID(N'TripNationalities'))
    EXEC sp_rename N'TripNationalities.IX_NationalityTrips_NationalityId', N'IX_TripNationalities_NationalityId', N'INDEX';
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_MarinaTrips_TripInformationId' AND object_id = OBJECT_ID(N'TripMarinas'))
    EXEC sp_rename N'TripMarinas.IX_MarinaTrips_TripInformationId', N'IX_TripMarinas_TripInformationId', N'INDEX';
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_MarinaTrips_TouristMarinaId' AND object_id = OBJECT_ID(N'TripMarinas'))
    EXEC sp_rename N'TripMarinas.IX_MarinaTrips_TouristMarinaId', N'IX_TripMarinas_TouristMarinaId', N'INDEX';
");

            migrationBuilder.AlterColumn<string>(
                name: "TripInformationId",
                table: "TripNationalities",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NationalityId",
                table: "TripNationalities",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TripInformationId",
                table: "TripMarinas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TouristMarinaId",
                table: "TripMarinas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TripInformationId",
                table: "TripGeos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GeoPointId",
                table: "TripGeos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TripAttachments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AttachmentId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TripInformationId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripAttachments_Attachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TripAttachments_TripInformations_TripInformationId",
                        column: x => x.TripInformationId,
                        principalTable: "TripInformations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TripAttachments_AttachmentId",
                table: "TripAttachments",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TripAttachments_TripInformationId",
                table: "TripAttachments",
                column: "TripInformationId");

            // Preserve existing rows, then retire the old TripPassengerAttachments table.
            migrationBuilder.Sql(@"
IF OBJECT_ID(N'TripPassengerAttachments', N'U') IS NOT NULL
BEGIN
    INSERT INTO [TripAttachments] ([Id], [AttachmentId], [TripInformationId], [IsDeleted])
    SELECT [Id], [AttachmentId], [TripInformationId], [IsDeleted]
    FROM [TripPassengerAttachments] tpa
    WHERE NOT EXISTS (SELECT 1 FROM [TripAttachments] ta WHERE ta.[Id] = tpa.[Id]);
END
");

            migrationBuilder.DropTable(
                name: "TripPassengerAttachments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TripPassengerAttachments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AttachmentId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TripInformationId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripPassengerAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripPassengerAttachments_Attachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TripPassengerAttachments_TripInformations_TripInformationId",
                        column: x => x.TripInformationId,
                        principalTable: "TripInformations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql(@"
IF OBJECT_ID(N'TripAttachments', N'U') IS NOT NULL
BEGIN
    INSERT INTO [TripPassengerAttachments] ([Id], [AttachmentId], [TripInformationId], [IsDeleted])
    SELECT [Id], [AttachmentId], [TripInformationId], [IsDeleted]
    FROM [TripAttachments] ta
    WHERE NOT EXISTS (SELECT 1 FROM [TripPassengerAttachments] tpa WHERE tpa.[Id] = ta.[Id]);
END
");

            migrationBuilder.DropTable(
                name: "TripAttachments");

            migrationBuilder.AlterColumn<string>(
                name: "TripInformationId",
                table: "TripNationalities",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NationalityId",
                table: "TripNationalities",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "NationalityNumber",
                table: "TripNationalities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "TripInformationId",
                table: "TripMarinas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "TouristMarinaId",
                table: "TripMarinas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "TripInformationId",
                table: "TripGeos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "GeoPointId",
                table: "TripGeos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}