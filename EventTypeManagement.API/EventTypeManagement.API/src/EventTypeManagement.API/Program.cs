#if !DEBUG
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EventTypeManagement.API.Data;

namespace EventTypeManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = CreateHostBuilder(args);
            builder.Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                    // Determine environment and load appropriate config
                    var env = hostingContext.HostingEnvironment;
                    if (env.IsProduction())
                    {
                        // Check if running in Azure or AWS
                        var isAzure = Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME") != null;
                        if (isAzure)
                        {
                            config.AddJsonFile("azure-settings.json", optional: true);
                        }
                        else
                        {
                            config.AddJsonFile("aws-settings.json", optional: true);
                        }
                    }

                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<MongoDbContext>();
                    services.AddHttpClient();
                });
    }
}
#endif