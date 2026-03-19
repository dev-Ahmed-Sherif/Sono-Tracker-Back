using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SonoTracker.Domain.Entities.Tracker;
using SonoTracker.Domain.Entities.Lookups;
using SonoTracker.Domain.Entities.Identity;
using SonoTracker.Domain.Entities.Attachments;
using SonoTracker.Domain.Entities.TrackerNotification;

namespace SonoTracker.Infrastructure.Context
{
    public partial class SonoTrackerDbContext(
            DbContextOptions<SonoTrackerDbContext> options) 
        : IdentityDbContext<User, Role, string>(options)
    {

        #region Floating Unit

        public virtual DbSet<FloatingUnit> FloatingUnits { get; set; }
        public virtual DbSet<FloatingUnitOrganization> FloatingUnitOrganizations { get; set; }
        public virtual DbSet<FloatingUnitStaff> FloatingUnitStaffs { get; set; }

        #endregion

        #region Organization

        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<OrganizationStaff> OrganizationStaffs { get; set; }
        public virtual DbSet<OrganizationAttachment> OrganizationAttachments { get; set; }
        public virtual DbSet<MarinaOrganization> MarinaOrganizations { get; set; }

        #endregion

        #region Trip

        public virtual DbSet<TripInformation> TripInformations { get; set; }
        public virtual DbSet<TripGeo> TripGeos { get; set; }
        public virtual DbSet<MarinaTrip> MarinaTrips { get; set; }
        public virtual DbSet<NationalityTrip> NationalityTrips { get; set; }
        public virtual DbSet<PassengerTripAttachment> PassengerTripAttachments { get; set; }

        #endregion

        #region Marina

        public virtual DbSet<TouristMarina> TouristMarinas { get; set; }
        public virtual DbSet<LicenseApplication> LicenseApplications { get; set; }

        #endregion

        #region Inspection, Maintenance & Accident

        public virtual DbSet<Inspection> Inspections { get; set; }
        public virtual DbSet<Maintenance> Maintenances { get; set; }
        public virtual DbSet<Accident> Accidents { get; set; }

        #endregion

        #region Identity & Auth

        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        #endregion

        #region Lookups – Core

        public virtual DbSet<AccidentType> AccidentTypes { get; set; }
        public virtual DbSet<InspectionType> InspectionTypes { get; set; }
        public virtual DbSet<MaintenanceType> MaintenanceTypes { get; set; }
        public virtual DbSet<Nationality> Nationalities { get; set; }
        public virtual DbSet<OrganizationCategory> OrganizationCategories { get; set; }
        public virtual DbSet<Route> Routes { get; set; }
        public virtual DbSet<UnitType> UnitTypes { get; set; }

        #endregion

        #region Lookups – Geography

        public virtual DbSet<Governorate> Governorates { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Town> Towns { get; set; }
        public virtual DbSet<GeoPoint> GeoPoints { get; set; }

        #endregion

        #region Messaging & Notifications

        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<MessagingGroup> MessagingGroups { get; set; }
        public virtual DbSet<NotificationGroup> NotificationGroups { get; set; }

        #endregion

        #region Model & Relations

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            ConfigureFloatingUnitRelations(modelBuilder);
            ConfigureOrganizationRelations(modelBuilder);
            ConfigureTripRelations(modelBuilder);
            ConfigureMarinaRelations(modelBuilder);
            ConfigureInspectionMaintenanceAccidentRelations(modelBuilder);
            ConfigureIdentityAndNotificationRelations(modelBuilder);
            ConfigureLookupRelations(modelBuilder);
        }

        private static void ConfigureFloatingUnitRelations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FloatingUnitOrganization>()
                .HasOne(fo => fo.FloatingUnit)
                .WithMany(f => f.FloatingUnitOrganizations)
                .HasForeignKey(fo => fo.FloatingUnitId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FloatingUnitOrganization>()
                .HasOne(fo => fo.Organization)
                .WithMany(o => o.FloatingUnitOrganizations)
                .HasForeignKey(fo => fo.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FloatingUnitStaff>()
                .HasOne(s => s.FloatingUnit)
                .WithMany(f => f.FloatingUnitStaffs)
                .HasForeignKey(s => s.FloatingUnitId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private static void ConfigureOrganizationRelations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Organization>()
                .HasOne(o => o.OrganizationCategory)
                .WithMany(c => c.Organizations)
                .HasForeignKey(o => o.OrganizationCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Organization>()
                .HasOne(o => o.Nationality)
                .WithMany(n => n.Organizations)
                .HasForeignKey(o => o.NationalityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Organization>()
                .HasOne(o => o.InspectionType)
                .WithMany()
                .HasForeignKey(o => o.InspectionTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrganizationStaff>()
                .HasOne(s => s.Organization)
                .WithMany(o => o.OrganizationStaffs)
                .HasForeignKey(s => s.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrganizationStaff>()
                .Property(s => s.NationalId)
                .IsRequired()
                .HasMaxLength(14);

            modelBuilder.Entity<OrganizationAttachment>()
                .HasOne(a => a.Organization)
                .WithMany()
                .HasForeignKey(a => a.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MarinaOrganization>()
                .HasOne(mo => mo.Organization)
                .WithMany(o => o.MarinaOwners)
                .HasForeignKey(mo => mo.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private static void ConfigureTripRelations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TripInformation>()
                .HasOne(t => t.FloatingUnit)
                .WithMany(f => f.TripInformations)
                .HasForeignKey(t => t.FloatingUnitId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TripInformation>()
                .HasOne(t => t.Route)
                .WithMany(r => r.TripInformations)
                .HasForeignKey(t => t.RouteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TripGeo>()
                .HasOne(tg => tg.GeoPoint)
                .WithMany()
                .HasForeignKey(tg => tg.GeoPointId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NationalityTrip>()
                .HasOne(nt => nt.TripInformation)
                .WithMany(t => t.NationalityTrips)
                .HasForeignKey(nt => nt.TripInformationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MarinaTrip>()
                .HasOne(mt => mt.TripInformation)
                .WithMany(t => t.MarinaTrips)
                .HasForeignKey(mt => mt.TripInformationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MarinaTrip>()
                .HasOne(mt => mt.TouristMarina)
                .WithMany()
                .HasForeignKey(mt => mt.TouristMarinaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PassengerTripAttachment>()
                .HasOne(a => a.TripInformation)
                .WithMany(t => t.PassengerTripAttachments)
                .HasForeignKey(a => a.TripInformationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TripGeo>()
                .HasOne(tg => tg.TripInformation)
                .WithMany()
                .HasForeignKey(tg => tg.TripInformationId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private static void ConfigureMarinaRelations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TouristMarina>()
                .HasOne(tm => tm.Town)
                .WithMany(t => t.TouristMarinas)
                .HasForeignKey(tm => tm.TownId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TouristMarina>()
                .HasOne(tm => tm.GeoPoint)
                .WithMany(g => g.TouristMarinas)
                .HasForeignKey(tm => tm.GeoPointId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MarinaOrganization>()
                .HasOne(mo => mo.TouristMarina)
                .WithMany()
                .HasForeignKey(mo => mo.TouristMarinaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LicenseApplication>()
                .HasOne(la => la.FromOrganization)
                .WithMany(o => o.LicenseApplications)
                .HasForeignKey(la => la.FromOrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LicenseApplication>()
                .HasOne(la => la.ToOrganization)
                .WithMany()
                .HasForeignKey(la => la.ToOrganizationId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private static void ConfigureInspectionMaintenanceAccidentRelations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inspection>()
                .HasOne(i => i.TripInformation)
                .WithMany(t => t.Inspections)
                .HasForeignKey(i => i.TripInformationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Inspection>()
                .HasOne(i => i.FloatingUnit)
                .WithMany(f => f.Inspections)
                .HasForeignKey(i => i.FloatingUnitId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Inspection>()
                .HasOne(i => i.Organization)
                .WithMany(o => o.Inspections)
                .HasForeignKey(i => i.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Maintenance>()
                .HasOne(m => m.FloatingUnit)
                .WithMany(f => f.Maintenances)
                .HasForeignKey(m => m.FloatingUnitId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Accident>()
                .HasOne(a => a.FloatingUnit)
                .WithMany(f => f.Accidents)
                .HasForeignKey(a => a.FloatingUnitId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Accident>()
                .HasOne(a => a.Organization)
                .WithMany(o => o.Accidents)
                .HasForeignKey(a => a.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private static void ConfigureLookupRelations(ModelBuilder modelBuilder)
        {
            // Governorate -> City -> Town hierarchy
            modelBuilder.Entity<City>()
                .HasOne(c => c.Governorate)
                .WithMany(g => g.Cities)
                .HasForeignKey(c => c.GovernorateId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Town>()
                .HasOne(t => t.City)
                .WithMany(c => c.Towns)
                .HasForeignKey(t => t.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            // Nationality relationships (Organization configured in ConfigureOrganizationRelations)
            modelBuilder.Entity<Nationality>()
                .HasMany(n => n.FloatingUnitStaffs)
                .WithOne(s => s.Nationality)
                .HasForeignKey(s => s.NationalityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Nationality>()
                .HasMany(n => n.NationalityTrips)
                .WithOne(nt => nt.Nationality)
                .HasForeignKey(nt => nt.NationalityId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private static void ConfigureIdentityAndNotificationRelations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                        .Ignore(r => r.ConcurrencyStamp);

            modelBuilder.Entity<User>()
                        .Ignore(r => r.ConcurrencyStamp);

            // User -> RefreshTokens
            modelBuilder.Entity<User>()
                .HasMany(u => u.RefreshTokens)
                .WithOne(rt => rt.User)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Notifications
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Sender)
                .WithMany()
                .HasForeignKey(n => n.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Receiver)
                .WithMany()
                .HasForeignKey(n => n.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.NotificationGroup)
                .WithMany(g => g.Notifications)
                .HasForeignKey(n => n.NotificationGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            // Messages
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.MessagingGroup)
                .WithMany(g => g.Messages)
                .HasForeignKey(m => m.MessagingGroupId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        #endregion
    }
}
