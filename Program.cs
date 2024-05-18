using LitHub.Services.ApiClient;
using LitHub.Services.RandomDataService;
using LitHub.Services.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
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
            builder.Services.AddHttpClient();
            builder.Services.AddCors();
            builder.Services
                .AddSingleton<IRandomDataService, RandomDataService>()
                .AddSingleton<IUserService, UserService>()
                .AddSingleton<IApiClient, ApiClient>();

            builder.Services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(2,1);
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

                app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
