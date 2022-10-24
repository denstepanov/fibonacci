using Fibonacci.Shared;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var webHost = builder.WebHost;
webHost.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

webHost.ConfigureServices(services =>
{
    services.AddHttpLogging(logging =>
    {
        logging.LoggingFields = HttpLoggingFields.All;
    });

    services.AddControllers();

    services.AddTransient<ICountService, CountService>();
    services.Configure<RabbitMQOptions>(configuration.GetSection("RabbitMQ"));
});

var app = builder.Build();
app.UseRouting();
app.MapControllers();
app.MapGet("/ping", () => "pong");
app.Run();