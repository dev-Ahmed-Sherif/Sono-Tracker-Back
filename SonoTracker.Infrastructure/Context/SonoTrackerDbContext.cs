using SonoTracker.Domain.Entities.Lookups;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SonoTracker.Common.Services;
using SonoTracker.Domain.Entities.Attachments;
using SonoTracker.Domain.Entities.Identity;
using SonoTracker.Domain.Entities.Tracker;
using SonoTracker.Infrastructure.Configuration;
using SonoTracker.Infrastructure.DataInitializer;
using System;
using SonoTracker.Domain.Entities.TrackerNotification;

namespace SonoTracker.Infrastructure.Context
{
    public partial class SonoTrackerDbContext : IdentityDbContext<User>
    {
        private readonly IDataInitializer _dataInitializer;
        private readonly IClaimService _claimService;
        public SonoTrackerDbContext(DbContextOptions<SonoTrackerDbContext> options, IDataInitializer dataInitializer, IClaimService claimService) : base(options)
        {
            _dataInitializer = dataInitializer;
            _claimService = claimService;
        }
        public virtual DbSet<FloatingUnit> FloatingUnits { get; set; }

        public virtual DbSet<FloatingUnitOrganization> FloatingUnitOrganizations { get; set; }

        public virtual DbSet<FloatingUnitStaff> FloatingUnitStaffs { get; set; }

        public virtual DbSet<Governorate> Governorates { get; set; }

        public virtual DbSet<GovernorateGeoPoint> GovernorateGeoPoints { get; set; }

        public virtual DbSet<MarinaOrganization> MarinaOrganizations { get; set; }

        public virtual DbSet<LicenseApplication> LicenseApplications { get; set; }

        public virtual DbSet<MarinaTrip> MarinaTrips { get; set; }

        public virtual DbSet<NationalityTrip> NationalityTrips { get; set; }

        public virtual DbSet<Organization> Organizations { get; set; }

        public virtual DbSet<OrganizationAttachment> OrganizationAttachments { get; set; }

        public virtual DbSet<PassengerTripAttachment> PassengerTripAttachments { get; set; }

        public virtual DbSet<OrganizationStaff> OrganizationStaffs { get; set; }

        public virtual DbSet<TouristMarina> TouristMarinas { get; set; }

        public virtual DbSet<TripInformation> TripInformations { get; set; }
        public virtual DbSet<TripGeo> TripGeos { get; set; }

        public virtual DbSet<Inspection> Inspections { get; set; }

        public virtual DbSet<Message> Messages { get; set; }

        public virtual DbSet<Notification> Notifications { get; set; }

        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        public virtual DbSet<Maintenance> Maintenances { get; set; }
        public virtual DbSet<Accident> Accidents { get; set; }

        #region Lookup DbContext
        public virtual DbSet<AccidentType> AccidentTypes { get; set; }

        public virtual DbSet<City> Cities { get; set; }

        public virtual DbSet<GeoPoint> GeoPoints { get; set; }

        public virtual DbSet<InspectionType> InspectionTypes { get; set; }

        public virtual DbSet<MaintenanceType> MaintenanceTypes { get; set; }

        public virtual DbSet<Nationality> Nationalities { get; set; }

       

        public virtual DbSet<Route> Routes { get; set; }

        public virtual DbSet<Town> Towns { get; set; }

        public virtual DbSet<UnitType> UnitTypes { get; set; }

        public virtual DbSet<MessagingGroup> MessagingGroups { get; set; }

        public virtual DbSet<NotificationGroup> NotificationGroups { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasSequence<long>("TicketReference").StartsAt(1).IncrementsBy(1);

            modelBuilder.ApplyConfiguration(new TestConfig());
            //modelBuilder.Entity<Action>().HasData(_dataInitializer.SeedActionsAsync());
            //modelBuilder.Entity<Status>().HasData(_dataInitializer.SeedStatusesAsync());
            modelBuilder.Entity<LicenseApplication>(entity =>
            {
                modelBuilder.ApplyConfiguration(new BaseConfig<LicenseApplication,Guid>());

                entity.HasOne(c => c.FromOrganization) // Navigation Property
                       .WithMany(c => c.LicenseApplications)  // If Employee has no collection of Correspondence
                       .HasForeignKey(k => k.FromOrganizationId) // Ensure there is a FK property
                       .OnDelete(DeleteBehavior.NoAction);

                //entity.HasOne(c => c.ToOrganization) // Navigation Property
                //       .WithMany(c => c.LicenseApplications)  // If Employee has no collection of Correspondence
                //       .HasForeignKey(k => k.ToOrganizationId) // Ensure there is a FK property
                //       .OnDelete(DeleteBehavior.NoAction);
            });
            modelBuilder.Entity<OrganizationStaff>(entity =>
            {
                modelBuilder.ApplyConfiguration(new BaseConfig<OrganizationStaff,Guid>());

                entity.HasOne(c => c.Organization) // Navigation Property
                       .WithMany(c => c.OrganizationStaffs)  // If Employee has no collection of Correspondence
                       .HasForeignKey(k => k.OrganizationId) // Ensure there is a FK property
                       .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
