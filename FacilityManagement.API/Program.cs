using Microsoft.AspNetCore;
using FacilityManagement.API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;

namespace FacilityManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    Debug.WriteLine("Pocnat DBINitializer");
                    DbInitializer.Seed(context);
                    Debug.WriteLine("Zavrsen DBINitializer");
                }
                catch (Exception)
                {
                    Debug.WriteLine("Problem DBINitializer");
                }
            }

            host.Run();

            
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
