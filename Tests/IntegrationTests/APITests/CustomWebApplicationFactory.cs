using Data.DBContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Tests.IntegrationTests.APITests
{
    public class CustomWebApplicationFactory<Program> : WebApplicationFactory<Program> where Program : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                var testConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "../Tests/appsettings.Test.json");
                config.AddJsonFile(testConfigPath, optional: true, reloadOnChange: true);
            });

            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor);
                }

                var dbContextInstance = services.SingleOrDefault(d => d.ServiceType == typeof(AppDbContext));
                if (dbContextInstance != null)
                {
                    services.Remove(dbContextInstance);
                }

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                db.Database.EnsureCreated();
            });
        }
    }
}
