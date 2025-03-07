using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.TestHost;
using Data.DBContext;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Core.Models;
using Microsoft.AspNetCore.Http;

namespace Tests.IntegrationTests.APITests
{
    public class CustomWebApplicationFactory<Program> : WebApplicationFactory<Program> where Program : class
    {
        private ITestOutputHelper? _outputHelper;
        private readonly Guid _fakeUserId = Guid.Parse("80a87a51-d544-4653-ae91-c6395e5fd8ce");
        private readonly string _dbName;
        private readonly IConfiguration _configuration;

        public CustomWebApplicationFactory(string dbName, ITestOutputHelper outputHelper, IConfiguration configuration)
        {
            _dbName = dbName;
            _outputHelper = outputHelper;
            _configuration = configuration;

            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                var testConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Test.json");
                if (File.Exists(testConfigPath))
                {
                    config.AddJsonFile(testConfigPath, optional: true, reloadOnChange: true);
                }
                else
                {
                    throw new FileNotFoundException($"Test configuration file not found: {testConfigPath}");
                }
            });

            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_ =>
                {
                    var loggerFactory = LoggerFactory.Create(builder =>
                    {
                        builder.ClearProviders();
                        builder.AddProvider(new TestLoggerProvider(_outputHelper));
                    });
                    return loggerFactory;
                });

                var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor);
                }

                services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(_dbName));

                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();

                var fakeUser = new User("Fake name", _fakeUserId);
                db.Users.Add(fakeUser);
                db.SaveChanges();

                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(AuthenticationHandler<AuthenticationSchemeOptions>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddAuthentication("Test").AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

                services.AddAuthorization(options =>
                {
                    options.DefaultPolicy = new AuthorizationPolicyBuilder()
                        .AddAuthenticationSchemes("Test")
                        .RequireAuthenticatedUser()
                        .Build();
                });
            });

            builder.UseTestServer();
        }
    }
}
