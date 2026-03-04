using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using SonoTracker.Common.Extensions;
using SonoTracker.Domain.Entities.Identity;
using SonoTracker.Infrastructure.Context;
using SonoTracker.Infrastructure.DataInitializer;

namespace SonoTracker.Api
{
    /// <summary>
    /// Start Point
    /// </summary>
    public class Program
    {
        private const string SystemActor = "System";
        private const string SuperAdminRoleName = "SuperAdmin";
        private const string SuperUserEmail = "super@sonotracker.com";
        private const string SuperUserPassword = "as1+me2";

        /// <summary>
        /// Configuration Properties
        /// </summary>
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
        /// <summary>
        /// Kick Off
        /// </summary>
        /// <param name="args"></param>
        public static async Task Main(string[] args)
        {
            //Log.Logger = BaseLoggerConfiguration
            //    .CreateLoggerConfiguration(Configuration["ApplicationName"])
            //    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
            //    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
            //    .WriteToSql(Configuration["LoggingDbConnectionString"])
            //    .WriteToSeq(Configuration["LoggingSeqUrl"])
            //    .CreateLogger();

            try
            {
                Log.Information("-----Starting web host at  Api------");
                var host = CreateHostBuilder(args).Build();

                await SeedIdentityAsync(host);
                await SeedNationalitiesAsync(host);

                await host.RunAsync();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Host terminated unexpectedly");
            }
            finally
            {
                await Log.CloseAndFlushAsync();
            }
        }
        /// <summary>
        /// Web Host Builder
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        /// <summary>
        /// Seed base role (SuperAdmin) and super user on startup.
        /// </summary>
        private static async Task SeedIdentityAsync(IHost host)
        {
            try
            {
                await using var scope = host.Services.CreateAsyncScope();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                var now = DateTime.UtcNow;

                // Ensure SuperAdmin role exists
                var role = await roleManager.FindByNameAsync(SuperAdminRoleName);
                if (role == null)
                {
                    role = new Role
                    {
                        Name = SuperAdminRoleName,
                        NormalizedName = SuperAdminRoleName.ToUpperInvariant(),
                        NameAr = "مدير النظام",
                        CreatedAt = now,
                        ModifiedAt = now,
                        CreatedById = SystemActor,
                        CreatedBy = SystemActor,
                        ModifiedById = SystemActor,
                        ModifiedBy = SystemActor,
                        IsDeleted = false
                    };
                    var result = await roleManager.CreateAsync(role);
                    if (result.Succeeded)
                        Log.Information("Seeded role: {RoleName}", SuperAdminRoleName);
                    else
                        Log.Warning("Failed to seed role SuperAdmin: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                // Ensure super user exists
                var user = await userManager.FindByEmailAsync(SuperUserEmail);
                if (user == null)
                {
                    user = new User
                    {
                        UserName = SuperUserEmail,
                        Email = SuperUserEmail,
                        EmailConfirmed = true,
                        FullName = "مدير النظام",
                        CreatedAt = now,
                        ModifiedAt = now,
                        CreatedById = SystemActor,
                        CreatedBy = SystemActor,
                        ModifiedById = SystemActor,
                        ModifiedBy = SystemActor,
                        IsDeleted = false
                    };
                    var createResult = await userManager.CreateAsync(user, SuperUserPassword);
                    if (createResult.Succeeded)
                    {
                        if (role != null && !await userManager.IsInRoleAsync(user, SuperAdminRoleName))
                        {
                            await userManager.AddToRoleAsync(user, SuperAdminRoleName);
                            Log.Information("Seeded super user and assigned role SuperAdmin: {Email}", SuperUserEmail);
                        }
                        else
                            Log.Information("Seeded super user: {Email}", SuperUserEmail);
                    }
                    else
                        Log.Warning("Failed to seed super user: {Errors}", string.Join(", ", createResult.Errors.Select(e => e.Description)));
                }
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Identity seed (role/user) skipped or failed");
            }
        }

        /// <summary>
        /// Seed nationalities from Nationalities.json into the database on startup.
        /// </summary>
        private static async Task SeedNationalitiesAsync(IHost host)
        {
            try
            {
                await using var scope = host.Services.CreateAsyncScope();
                var db = scope.ServiceProvider.GetRequiredService<SonoTrackerDbContext>();
                var initializer = scope.ServiceProvider.GetRequiredService<IDataInitializer>();

                var nationalities = initializer.SeedNationalitiesAsync().ToList();
                if (nationalities.Count == 0) return;

                var existingIds = await db.Nationalities.Select(x => x.Id).ToListAsync();
                var toAdd = nationalities.Where(n => !existingIds.Contains(n.Id)).ToList();
                if (toAdd.Count == 0) return;

                db.Nationalities.AddRange(toAdd);
                await db.SaveChangesAsync();
                Log.Information("Seeded {Count} nationalities from Nationalities.json", toAdd.Count);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Nationalities seed skipped or failed");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>();
                }).UseSerilog();
    }
}
