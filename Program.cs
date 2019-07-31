using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using System;
using System.IO;

namespace SWMNU_NET
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var hostingEnvironment = services.GetService<IHostingEnvironment>();

                if (hostingEnvironment.IsProduction())
                {
                    // Production only things go here
                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Information()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                        .Enrich.FromLogContext()
                        .WriteTo.File(
                            @"logs\site.txt",
                            fileSizeLimitBytes: 1_000_000,
                            rollOnFileSizeLimit: true,
                            shared: true,
                            flushToDiskInterval: TimeSpan.FromMinutes(5))
                        .CreateLogger();
                }
                else
                {
                    // DEBUG only things go here
                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                        .Enrich.FromLogContext()
                        .WriteTo.Console()
                        .CreateLogger();
                }

                try
                {
                    Log.Information("Starting web host");
                    host.Run();
                }
                catch (Exception ex)
                {
                    Log.Fatal("Host terminated unexpectedly!\n" + ex.Message);
                }
                finally
                {
                    Log.CloseAndFlush();
                }
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureAppConfiguration(options =>
                {
                    options.AddJsonFile("config/buttercms.json", optional: false, reloadOnChange: false);
                })
                .UseSerilog();
    }
}
