using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SonoTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTripInformationAttachmentColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('TripInformations', 'SartDate') IS NOT NULL
   AND COL_LENGTH('TripInformations', 'StartDate') IS NULL
    EXEC sp_rename N'TripInformations.SartDate', N'StartDate', N'COLUMN';
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('TripInformations', 'PassengerAttachment') IS NULL
    ALTER TABLE [TripInformations] ADD [PassengerAttachment] nvarchar(140) NOT NULL CONSTRAINT DF_TripInformations_PassengerAttachment DEFAULT('');
IF COL_LENGTH('TripInformations', 'StaffAttachment') IS NULL
    ALTER TABLE [TripInformations] ADD [StaffAttachment] nvarchar(140) NOT NULL CONSTRAINT DF_TripInformations_StaffAttachment DEFAULT('');
");

            // Environments that already applied AddTripAttachment before it dropped the old table.
            migrationBuilder.Sql(@"
IF OBJECT_ID(N'TripPassengerAttachments', N'U') IS NOT NULL
BEGIN
    IF OBJECT_ID(N'TripAttachments', N'U') IS NOT NULL
    BEGIN
        INSERT INTO [TripAttachments] ([Id], [AttachmentId], [TripInformationId], [IsDeleted])
        SELECT [Id], [AttachmentId], [TripInformationId], [IsDeleted]
        FROM [TripPassengerAttachments] tpa
        WHERE NOT EXISTS (SELECT 1 FROM [TripAttachments] ta WHERE ta.[Id] = tpa.[Id]);
    END

    DROP TABLE [TripPassengerAttachments];
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('TripInformations', 'PassengerAttachment') IS NOT NULL
    ALTER TABLE [TripInformations] DROP COLUMN [PassengerAttachment];
IF COL_LENGTH('TripInformations', 'StaffAttachment') IS NOT NULL
    ALTER TABLE [TripInformations] DROP COLUMN [StaffAttachment];
IF COL_LENGTH('TripInformations', 'StartDate') IS NOT NULL
   AND COL_LENGTH('TripInformations', 'SartDate') IS NULL
    EXEC sp_rename N'TripInformations.StartDate', N'SartDate', N'COLUMN';
");
        }
    }
}
