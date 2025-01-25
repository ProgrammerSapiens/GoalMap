using Core;
using Core.Interfaces;
using Core.Services;
using Data.Repositories;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IToDoCategoryRepository, ToDoCategoryRepository>();
            builder.Services.AddScoped<IToDoRepository, ToDoRepository>();

            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IToDoCategoryService, ToDoCategoryService>();
            builder.Services.AddScoped<IToDoService, ToDoService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
