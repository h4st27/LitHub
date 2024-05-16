using HealthChecks.UI.Client;
using k8s.KubeConfigModels;
using Libra.Services.ApiClient;
using Libra.Services.Background;
using Libra.Services.DataBaseService;
using Libra.Services.DictionaryService;
using Libra.Services.HealthChecker;
using Libra.Services.Hubs;
using Libra.Services.JokesService;
using Libra.Services.RandomDataService;
using Libra.Services.SmtpEmailSender;
using Libra.Services.UserService;
using Libra.Services.WordsService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using System.Configuration;
using System.Text;

namespace Libra
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var versions = new[] { "v1.1", "v1.2", "v1.3", "v2.1" };
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<AppDbContext>(options => options
            .UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                    ));
            builder.Services.AddHttpClient();
            builder.Services.AddMemoryCache();
            builder.Services.AddSignalR();
            builder.Services
                .AddHostedService<PageAvailabilityService>()
                .AddHostedService<ExternalApiService>()
                .AddHostedService<DatabaseNotificationService>();
            builder.Services.AddCors();
            builder.Services.AddHealthChecks()
                .AddTypeActivatedCheck<UserHealthCheck>("user_health_check", args: new object[] { "Token type - JWT Bearer" })
                .AddDbContextCheck<AppDbContext>("database_health_check");
            builder.Services
                .AddSingleton<IRandomDataService, RandomDataService>()
                .AddSingleton<IUserService, UserService>()           // Сервіс додан як AddSingleton, адже сервіс повинен бути єдиним для усіх користувачів застосунку
                .AddSingleton<IApiClient, ApiClient>()               // Сервіс виступає в ролі методів для взаємодії із HttpClient, не передбачається, що методи повинні змінюватися, тому для роботи із єдиним об'єктом сервіса використовується AddSingleton
                .AddSingleton<IWordsService, WordsService>()        // Сервіс виступає в ролі зберігання єдиного списку слів та методів взаємодії із ним. Для роботи із єдиним об'єктом сервіса використовується AddSingleton
                .AddSingleton<IJokesService, JokesService>()        // Сервіс виступає в ролі зберігання єдиного списку жартів та методів взаємодії із ним. Для роботи із єдиним об'єктом сервіса використовується AddSingleton
                .AddSingleton<IEmailSender, SmtpEmailSender>()
                .AddScoped<IDictionaryService, DictionaryService>();// Сервіс використовує дані іншого сервіса, який може змінювати свій стан. Для реєстрації цих змін використовується AddScoped
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

            builder.Services.AddQuartz(
                q =>
                {
                    q.UseMicrosoftDependencyInjectionJobFactory();

                    q.AddJob<EmailSendingJob> (j => j.WithIdentity("emailSendingJob"));

                    q.AddTrigger(t => t
                        .ForJob("emailSendingJob")
                        .WithIdentity("emailSendingTrigger")
                        .WithSimpleSchedule(s => s.WithInterval(TimeSpan.FromSeconds(30)).RepeatForever()));
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
                    c.SwaggerDoc($"{version}", new OpenApiInfo { Title = "Libra API", Version = $"{version}", });
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
                        c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"Libra API {version}");
                    }
                });
            }

            app.MapHealthChecks("/user_health",
                new HealthCheckOptions
                {
                    Predicate = healthCheck => healthCheck.Name == "dictionary_health_check",
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

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationHub>("/notificationHub");
            });

            app.MapControllers();

            app.UseHealthChecksUI(opt =>
            {
                opt.UIPath = "/health";
            });

            app.Run();
        }
    }
}
