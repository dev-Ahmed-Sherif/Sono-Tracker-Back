using Asp.Versioning.ApiExplorer;
using Hangfire;
using Serilog;
using SonoTracker.Api.Extensions;
using SonoTracker.Api.Hubs;
using SonoTracker.Api.MiddleWares;
using SonoTracker.Api.Seed;
using SonoTracker.Application.DependencyExtension;

namespace SonoTracker.Api
{
    /// <summary>
    /// Start Point
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Kick Off
        /// </summary>
        /// <param name="args"></param>
        public static async Task Main(string[] args)
        {
            try
            {
                Log.Information("-----Starting web host at  Api------");
                var host = CreateHostBuilder(args).Build();

                await DatabaseSeed.SeedIdentityAsync(host);
                await DatabaseSeed.SeedNationalitiesAsync(host);
                await DatabaseSeed.SeedAccidentTypesAsync(host);
                await DatabaseSeed.SeedGovernoratesAsync(host);
                await DatabaseSeed.SeedCitiesAsync(host);
                await DatabaseSeed.SeedNotificationGroupsAsync(host);

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
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices((context, services) =>
                    {
                        services.RegisterServices(context.Configuration);
                    });

                    webBuilder.Configure((context, app) =>
                    {
                        var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                        var shell = (Shell)Activator.CreateInstance(typeof(Shell))!;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                        shell.ConfigureHttp(app, context.HostingEnvironment);
                        Shell.Start(shell);
                        app.Configure(context.Configuration, provider);
                        app.UseHangfireDashboard("/sono-tracker-Jobs");

                        if (context.HostingEnvironment.IsDevelopment())
                        {
                            app.UseDeveloperExceptionPage();
                        }

                        app.ConfigureCustomMiddleware();
                        app.UseStaticFiles();
                        app.UseWebSockets();
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapHub<ChatHub>("/api/v1/hubs/chat");
                            endpoints.MapHub<NotificationHub>("/api/v1/hubs/notifications");
                            endpoints.MapHub<VideoChatHub>("/api/v1/hubs/video");
                            endpoints.MapHub<GeoHub>("/hubs/geo");
                            endpoints.MapControllers();
                        });
                    });
                })
                .UseSerilog();
    }
}
