using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenTelemetry()
    .WithMetrics(opt =>
    
        opt
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Libra.GatewayAPI"))
            .AddMeter(builder.Configuration.GetValue<string>("LibraMeterName"))
            .AddAspNetCoreInstrumentation()
            .AddRuntimeInstrumentation()
            .AddProcessInstrumentation()
            .AddOtlpExporter(opts =>
            {
                opts.Endpoint = new Uri(builder.Configuration["Otel:Endpoint"]);
            })
    );   


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Action", "Adventure", "Comedy", "Drama", "Fantasy", "Horror", "Mystery", "Romance", "Sci-Fi", "Thriller"
};

app.MapGet("/genre", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new GenreData
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetGenre")
.WithOpenApi();



app.Run();

record GenreData(DateOnly Date, string? Summary);