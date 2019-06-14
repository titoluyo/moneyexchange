using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Api.Fixer.Data;

namespace Api.Fixer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // /* For real SQL Server */
            // CreateWebHostBuilder(args).Build().Run();

            // /* In Memory Database, for DEMO purposes */
            var host = BuildWebHost(args);
            using (var scope = host.Services.CreateScope())
            using (var context = scope.ServiceProvider.GetService<AppDbContext>())
            {
                context.Database.EnsureCreated();
            }
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(ConfigureLog)
                .UseStartup<Startup>()
                .Build();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(ConfigureLog)
                .UseStartup<Startup>();

        public static void ConfigureLog(WebHostBuilderContext hostingContext, ILoggingBuilder logging)
        {
            logging.ClearProviders();
            logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            logging.AddConsole();
            logging.AddDebug();
            logging.AddEventSourceLogger();
        }

    }
}
