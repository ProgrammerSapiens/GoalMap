using Core.Interfaces;
using Core.Services;
using Data.Repositories;
using API.DTOProfiles;
using Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using System.IdentityModel.Tokens.Jwt;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("logs/app_log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IToDoCategoryRepository, ToDoCategoryRepository>();
            builder.Services.AddScoped<IToDoRepository, ToDoRepository>();

            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IToDoCategoryService, ToDoCategoryService>();
            builder.Services.AddScoped<IToDoService, ToDoService>();

            builder.Services.AddAutoMapper(typeof(UserProfile));
            builder.Services.AddAutoMapper(typeof(CategoryProfile));
            builder.Services.AddAutoMapper(typeof(ToDoProfile));

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async context =>
                        {
                            var userIdClaim = context.Principal?.FindFirst(JwtRegisteredClaimNames.Sub);
                            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
                            {
                                var todoService = context.HttpContext.RequestServices.GetRequiredService<IToDoService>();

                                await todoService.MoveRepeatedToDosAsync(userId);
                            }
                        }
                    };
                });

            builder.Services.AddAuthorization();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseSerilogRequestLogging();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            await app.RunAsync();
        }
    }
}
