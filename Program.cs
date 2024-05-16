
using Libra.Services.ApiClient;
using Libra.Services.DictionaryService;
using Libra.Services.JokesService;
using Libra.Services.UserService;
using Libra.Services.WordsService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Libra
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHttpClient();
            builder.Services.AddCors();
            builder.Services
                .AddSingleton<IUserService,UserService>()           // Сервіс додан як AddSingleton, адже сервіс повинен бути єдиним для усіх користувачів застосунку
                .AddSingleton<IApiClient,ApiClient>()               // Сервіс виступає в ролі методів для взаємодії із HttpClient, не передбачається, що методи повинні змінюватися, тому для роботи із єдиним об'єктом сервіса використовується AddSingleton
                .AddSingleton<IWordsService, WordsService>()        // Сервіс виступає в ролі зберігання єдиного списку слів та методів взаємодії із ним. Для роботи із єдиним об'єктом сервіса використовується AddSingleton
                .AddSingleton<IJokesService, JokesService>()        // Сервіс виступає в ролі зберігання єдиного списку жартів та методів взаємодії із ним. Для роботи із єдиним об'єктом сервіса використовується AddSingleton
                .AddScoped<IDictionaryService, DictionaryService>();// Сервіс використовує дані іншого сервіса, який може змінювати свій стан. Для реєстрації цих змін використовується AddScoped
            builder.Services.AddControllers();

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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Libra API", Version = "v1" });

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
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
