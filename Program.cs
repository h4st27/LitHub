
using MyApp.Services.ApiClient;
using MyApp.Services.LibraryService;
using MyApp.Services.MangasService;
using MyApp.Services.BooksService;

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
                .AddSingleton<IApiClient,ApiClient>()    
                .AddSingleton<IBooksService, BooksService>()  
                .AddSingleton<IMangasService, MangasService>() 
                .AddScoped<ILibraryService, LibraryService>();
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
