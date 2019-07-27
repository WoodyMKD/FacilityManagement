using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FacilityManagement.IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            bool seed = false;

            var host = CreateWebHostBuilder(args).Build();

            if (seed)
            {
                var config = host.Services.GetRequiredService<IConfiguration>();
                var connectionString = config.GetConnectionString("FM_IdentityServerDatabase");
                SeedData.EnsureSeedData(connectionString);
                return;
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                    .UseStartup<Startup>();
        }
    }
}
