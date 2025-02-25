﻿using Microsoft.AspNetCore.Mvc.Testing;
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

namespace Tests.IntegrationTests.APITests
{
    public class CustomWebApplicationFactory<Program> : WebApplicationFactory<Program> where Program : class
    {
        private ITestOutputHelper? _outputHelper;
        private readonly Guid _fakeUserId = Guid.Parse("80a87a51-d544-4653-ae91-c6395e5fd8ce");
        private readonly string _dbName;

        public CustomWebApplicationFactory(string dbName, ITestOutputHelper outputHelper)
        {
            _dbName = dbName;
            _outputHelper = outputHelper;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                var testConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "IntegrationTests/APITests/appsettings.Test.json");
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
                services.AddSingleton<ILoggerFactory>(_ =>
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

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase(_dbName);
                }, ServiceLifetime.Scoped);

                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();

                var fakeUser = new User("Fake name", _fakeUserId);
                db.Users.Add(fakeUser);
                db.SaveChanges();

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
