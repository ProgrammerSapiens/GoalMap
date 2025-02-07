using Core;
using Core.Interfaces;
using Core.Services;
using Data.Repositories;
using API.DTOProfiles;

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

            // Registers repositories for dependency injection.
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IToDoCategoryRepository, ToDoCategoryRepository>();
            builder.Services.AddScoped<IToDoRepository, ToDoRepository>();

            // Registers password hashing service.
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

            // Registers services for business logic.
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IToDoCategoryService, ToDoCategoryService>();
            builder.Services.AddScoped<IToDoService, ToDoService>();

            // Registers AutoMapper profiles for object mapping.
            builder.Services.AddAutoMapper(typeof(UserProfile));
            builder.Services.AddAutoMapper(typeof(CategoryProfile));
            builder.Services.AddAutoMapper(typeof(ToDoProfile));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
