using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SonoTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DivideTablesIntoSchemas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "operations");

            migrationBuilder.EnsureSchema(
                name: "lookups");

            migrationBuilder.EnsureSchema(
                name: "units");

            migrationBuilder.EnsureSchema(
                name: "notification");

            migrationBuilder.EnsureSchema(
                name: "organization");

            migrationBuilder.EnsureSchema(
                name: "marina");

            migrationBuilder.EnsureSchema(
                name: "trip");

            migrationBuilder.RenameTable(
                name: "UnitTypes",
                newName: "UnitTypes",
                newSchema: "lookups");

            migrationBuilder.RenameTable(
                name: "TripNationalities",
                newName: "TripNationalities",
                newSchema: "trip");

            migrationBuilder.RenameTable(
                name: "TripMarinas",
                newName: "TripMarinas",
                newSchema: "trip");

            migrationBuilder.RenameTable(
                name: "TripInformations",
                newName: "TripInformations",
                newSchema: "trip");

            migrationBuilder.RenameTable(
                name: "TripGeos",
                newName: "TripGeos",
                newSchema: "trip");

            migrationBuilder.RenameTable(
                name: "TripAttachments",
                newName: "TripAttachments",
                newSchema: "trip");

            migrationBuilder.RenameTable(
                name: "Towns",
                newName: "Towns",
                newSchema: "lookups");

            migrationBuilder.RenameTable(
                name: "TouristMarinas",
                newName: "TouristMarinas",
                newSchema: "marina");

            migrationBuilder.RenameTable(
                name: "TouristMarinaOrganizations",
                newName: "TouristMarinaOrganizations",
                newSchema: "marina");

            migrationBuilder.RenameTable(
                name: "TouristMarinaLicenseApplications",
                newName: "TouristMarinaLicenseApplications",
                newSchema: "marina");

            migrationBuilder.RenameTable(
                name: "Routes",
                newName: "Routes",
                newSchema: "lookups");

            migrationBuilder.RenameTable(
                name: "OrganizationStaffs",
                newName: "OrganizationStaffs",
                newSchema: "organization");

            migrationBuilder.RenameTable(
                name: "Organizations",
                newName: "Organizations",
                newSchema: "organization");

            migrationBuilder.RenameTable(
                name: "OrganizationCategories",
                newName: "OrganizationCategories",
                newSchema: "lookups");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "Notifications",
                newSchema: "notification");

            migrationBuilder.RenameTable(
                name: "NotificationGroups",
                newName: "NotificationGroups",
                newSchema: "notification");

            migrationBuilder.RenameTable(
                name: "Nationalities",
                newName: "Nationalities",
                newSchema: "lookups");

            migrationBuilder.RenameTable(
                name: "MessagingGroups",
                newName: "MessagingGroups",
                newSchema: "notification");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "Messages",
                newSchema: "notification");

            migrationBuilder.RenameTable(
                name: "MaintenanceTypes",
                newName: "MaintenanceTypes",
                newSchema: "lookups");

            migrationBuilder.RenameTable(
                name: "Maintenances",
                newName: "Maintenances",
                newSchema: "operations");

            migrationBuilder.RenameTable(
                name: "MaintenanceAttachments",
                newName: "MaintenanceAttachments",
                newSchema: "operations");

            migrationBuilder.RenameTable(
                name: "InspectionTypes",
                newName: "InspectionTypes",
                newSchema: "lookups");

            migrationBuilder.RenameTable(
                name: "Inspections",
                newName: "Inspections",
                newSchema: "operations");

            migrationBuilder.RenameTable(
                name: "InspectionFloatingUnitClauses",
                newName: "InspectionFloatingUnitClauses",
                newSchema: "operations");

            migrationBuilder.RenameTable(
                name: "InspectionClauses",
                newName: "InspectionClauses",
                newSchema: "operations");

            migrationBuilder.RenameTable(
                name: "InspectionAttachments",
                newName: "InspectionAttachments",
                newSchema: "operations");

            migrationBuilder.RenameTable(
                name: "Governorates",
                newName: "Governorates",
                newSchema: "lookups");

            migrationBuilder.RenameTable(
                name: "GeoPoints",
                newName: "GeoPoints",
                newSchema: "lookups");

            migrationBuilder.RenameTable(
                name: "FloatingUnitStaffs",
                newName: "FloatingUnitStaffs",
                newSchema: "units");

            migrationBuilder.RenameTable(
                name: "FloatingUnits",
                newName: "FloatingUnits",
                newSchema: "units");

            migrationBuilder.RenameTable(
                name: "FloatingUnitOrganizations",
                newName: "FloatingUnitOrganizations",
                newSchema: "units");

            migrationBuilder.RenameTable(
                name: "Cities",
                newName: "Cities",
                newSchema: "lookups");

            migrationBuilder.RenameTable(
                name: "Attachments",
                newName: "Attachments",
                newSchema: "lookups");

            migrationBuilder.RenameTable(
                name: "AccidentTypes",
                newName: "AccidentTypes",
                newSchema: "lookups");

            migrationBuilder.RenameTable(
                name: "Accidents",
                newName: "Accidents",
                newSchema: "operations");

            migrationBuilder.RenameTable(
                name: "AccidentOrganizations",
                newName: "AccidentOrganizations",
                newSchema: "operations");

            migrationBuilder.RenameTable(
                name: "AccidentAttachments",
                newName: "AccidentAttachments",
                newSchema: "operations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "UnitTypes",
                schema: "lookups",
                newName: "UnitTypes");

            migrationBuilder.RenameTable(
                name: "TripNationalities",
                schema: "trip",
                newName: "TripNationalities");

            migrationBuilder.RenameTable(
                name: "TripMarinas",
                schema: "trip",
                newName: "TripMarinas");

            migrationBuilder.RenameTable(
                name: "TripInformations",
                schema: "trip",
                newName: "TripInformations");

            migrationBuilder.RenameTable(
                name: "TripGeos",
                schema: "trip",
                newName: "TripGeos");

            migrationBuilder.RenameTable(
                name: "TripAttachments",
                schema: "trip",
                newName: "TripAttachments");

            migrationBuilder.RenameTable(
                name: "Towns",
                schema: "lookups",
                newName: "Towns");

            migrationBuilder.RenameTable(
                name: "TouristMarinas",
                schema: "marina",
                newName: "TouristMarinas");

            migrationBuilder.RenameTable(
                name: "TouristMarinaOrganizations",
                schema: "marina",
                newName: "TouristMarinaOrganizations");

            migrationBuilder.RenameTable(
                name: "TouristMarinaLicenseApplications",
                schema: "marina",
                newName: "TouristMarinaLicenseApplications");

            migrationBuilder.RenameTable(
                name: "Routes",
                schema: "lookups",
                newName: "Routes");

            migrationBuilder.RenameTable(
                name: "OrganizationStaffs",
                schema: "organization",
                newName: "OrganizationStaffs");

            migrationBuilder.RenameTable(
                name: "Organizations",
                schema: "organization",
                newName: "Organizations");

            migrationBuilder.RenameTable(
                name: "OrganizationCategories",
                schema: "lookups",
                newName: "OrganizationCategories");

            migrationBuilder.RenameTable(
                name: "Notifications",
                schema: "notification",
                newName: "Notifications");

            migrationBuilder.RenameTable(
                name: "NotificationGroups",
                schema: "notification",
                newName: "NotificationGroups");

            migrationBuilder.RenameTable(
                name: "Nationalities",
                schema: "lookups",
                newName: "Nationalities");

            migrationBuilder.RenameTable(
                name: "MessagingGroups",
                schema: "notification",
                newName: "MessagingGroups");

            migrationBuilder.RenameTable(
                name: "Messages",
                schema: "notification",
                newName: "Messages");

            migrationBuilder.RenameTable(
                name: "MaintenanceTypes",
                schema: "lookups",
                newName: "MaintenanceTypes");

            migrationBuilder.RenameTable(
                name: "Maintenances",
                schema: "operations",
                newName: "Maintenances");

            migrationBuilder.RenameTable(
                name: "MaintenanceAttachments",
                schema: "operations",
                newName: "MaintenanceAttachments");

            migrationBuilder.RenameTable(
                name: "InspectionTypes",
                schema: "lookups",
                newName: "InspectionTypes");

            migrationBuilder.RenameTable(
                name: "Inspections",
                schema: "operations",
                newName: "Inspections");

            migrationBuilder.RenameTable(
                name: "InspectionFloatingUnitClauses",
                schema: "operations",
                newName: "InspectionFloatingUnitClauses");

            migrationBuilder.RenameTable(
                name: "InspectionClauses",
                schema: "operations",
                newName: "InspectionClauses");

            migrationBuilder.RenameTable(
                name: "InspectionAttachments",
                schema: "operations",
                newName: "InspectionAttachments");

            migrationBuilder.RenameTable(
                name: "Governorates",
                schema: "lookups",
                newName: "Governorates");

            migrationBuilder.RenameTable(
                name: "GeoPoints",
                schema: "lookups",
                newName: "GeoPoints");

            migrationBuilder.RenameTable(
                name: "FloatingUnitStaffs",
                schema: "units",
                newName: "FloatingUnitStaffs");

            migrationBuilder.RenameTable(
                name: "FloatingUnits",
                schema: "units",
                newName: "FloatingUnits");

            migrationBuilder.RenameTable(
                name: "FloatingUnitOrganizations",
                schema: "units",
                newName: "FloatingUnitOrganizations");

            migrationBuilder.RenameTable(
                name: "Cities",
                schema: "lookups",
                newName: "Cities");

            migrationBuilder.RenameTable(
                name: "Attachments",
                schema: "lookups",
                newName: "Attachments");

            migrationBuilder.RenameTable(
                name: "AccidentTypes",
                schema: "lookups",
                newName: "AccidentTypes");

            migrationBuilder.RenameTable(
                name: "Accidents",
                schema: "operations",
                newName: "Accidents");

            migrationBuilder.RenameTable(
                name: "AccidentOrganizations",
                schema: "operations",
                newName: "AccidentOrganizations");

            migrationBuilder.RenameTable(
                name: "AccidentAttachments",
                schema: "operations",
                newName: "AccidentAttachments");
        }
    }
}
