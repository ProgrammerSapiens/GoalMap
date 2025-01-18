using Data.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Core.Interfaces;
using Core.Services;
using Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Core.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;

namespace Tests.IntegrationTests.APITests
{
    public class TestProgram
    {
        public static IHostBuilder CreateHostBuider(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices(services =>
                {
                    services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("TestDatabase"));

                    services.AddScoped<IUserRepository, UserRepository>();
                    services.AddScoped<IToDoRepository, ToDoRepository>();
                    services.AddScoped<IToDoCategoryRepository, ToDoCategoryRepository>();

                    services.AddScoped<IUserService, UserService>();
                    services.AddScoped<IToDoService, ToDoService>();
                    services.AddScoped<IToDoCategoryService, ToDoCategoryService>();

                    services.AddControllers();
                })
                .Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });

                    using (var scope = app.ApplicationServices.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                        dbContext.Database.EnsureCreated();
                        SeedTestData(dbContext);
                    }
                });
            });

        private static void SeedTestData(AppDbContext dbContext)
        {
            var user1 = new User("TestUser1", "hashedPassword1", 0);
            var user2 = new User("TestUser2", "hashedPassword2", 0);

            dbContext.Users.AddRange(new List<User>
            {
                user1,
                user2
            });

            dbContext.ToDoCategories.AddRange(new List<ToDoCategory>
            {
                new ToDoCategory(user1.UserId,"Habbit"),
                new ToDoCategory(user1.UserId,"Other"),
                new ToDoCategory(user2.UserId,"Personal")
            });

            dbContext.ToDos.AddRange(new List<ToDo>
            {
                new ToDo("Morning exercise", TimeBlock.Day,Difficulty.Easy,DateTime.UtcNow,"Habbit",user1.UserId),
                new ToDo("Read a book",TimeBlock.Day,Difficulty.Medium,DateTime.UtcNow,"Other",user1.UserId),
                new ToDo("Finish report",TimeBlock.Day,Difficulty.Nightmare,DateTime.UtcNow,"Personal",user2.UserId)
            });

            dbContext.SaveChanges();
        }
    }
}
