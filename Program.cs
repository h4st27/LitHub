using HealthChecks.UI.Client;
using LitHub.Services.ApiClient;
using LitHub.Services.DataBaseService;
using LitHub.Services.HealthChecker;
using LitHub.Services.RandomDataService;
using LitHub.Services.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace LitHub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var versions = new[] { "v1.1", "v1.2", "v1.3", "v2.1" };
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<DataBaseService>(options => options
            .UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                    ));
            builder.Services.AddHttpClient();
            builder.Services.AddCors();
            builder.Services.AddHealthChecks()
                .AddTypeActivatedCheck<UserHealthCheck>("user_health_check", args: new object[] { "Token type - JWT Bearer" })
                .AddDbContextCheck<DataBaseService>("database_health_check");
            builder.Services
                .AddSingleton<IRandomDataService, RandomDataService>()
                .AddSingleton<IUserService, UserService>()
                .AddSingleton<IApiClient, ApiClient>();
            builder.Services.AddControllers();
            builder.Services.AddHealthChecksUI(
                o =>
                {
                    o.AddHealthCheckEndpoint("user_health_check", "/user_health");
                    o.AddHealthCheckEndpoint("database_health_check", "/database_health");
                }).AddInMemoryStorage();
            builder.Services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(2, 1);
            })
            .AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });


            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Authentication")["Secret"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = false,
                };
            });
            builder.Services.AddAuthorization();
            builder.Services.AddSwaggerGen(c =>
            {
                foreach (var version in versions)
                {
                    c.SwaggerDoc($"{version}", new OpenApiInfo { Title = "LitHub API", Version = $"{version}", });
                }

                c.ResolveConflictingActions(a => a.First());

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter JWT Bearer token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                };
                c.AddSecurityDefinition("Bearer", securityScheme);

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                };
                c.AddSecurityRequirement(securityRequirement);
            });


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    foreach (var version in versions)
                    {
                        c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"LitHub API {version}");
                    }
                });
            }

            app.MapHealthChecks("/user_health",
                new HealthCheckOptions
                {
                    Predicate = healthCheck => healthCheck.Name == "user_health_check",
                    AllowCachingResponses = false,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

            app.MapHealthChecks("/database_health",
                new HealthCheckOptions
                {
                    Predicate = healthCheck => healthCheck.Name == "database_health_check",
                    AllowCachingResponses = false,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.UseHealthChecksUI(opt =>
            {
                opt.UIPath = "/health";
            });

            app.Run();
        }
    }
}
