
using MyApp.Services.ApiClient;
using MyApp.Services.DictionaryService;
using MyApp.Services.JokesService;
using MyApp.Services.WordsService;

namespace MyApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHttpClient();
            builder.Services.AddCors();
            builder.Services
                .AddSingleton<IApiClient,ApiClient>()               // Сервіс виступає в ролі методів для взаємодії із HttpClient, не передбачається, що методи повинні змінюватися, тому для роботи із єдиним об'єктом сервіса використовується AddSingleton
                .AddSingleton<IWordsService, WordsService>()        // Сервіс виступає в ролі зберігання єдиного списку слів та методів взаємодії із ним. Для роботи із єдиним об'єктом сервіса використовується AddSingleton
                .AddSingleton<IJokesService, JokesService>()        // Сервіс виступає в ролі зберігання єдиного списку жартів та методів взаємодії із ним. Для роботи із єдиним об'єктом сервіса використовується AddSingleton
                .AddScoped<IDictionaryService, DictionaryService>();// Сервіс використовує дані іншого сервіса, який може змінювати свій стан. Для реєстрації цих змін використовується AddScoped
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
