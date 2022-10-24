using System.Reflection;
using Fibonacci.First;
using Fibonacci.Shared;
using Microsoft.OpenApi.Models;

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
    services.AddControllers();
    services.AddHttpClient();

    services.AddSwaggerGen(opts =>
    {
        opts.SwaggerDoc("Fibonacci", new OpenApiInfo
        {
            Title = "Fibonacci"
        });
    
        var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
        opts.IncludeXmlComments(xmlCommentsFullPath);
    });

    services.AddTransient<ICountService, CountService>();
    services.AddHostedService<CountMessageHandler>();

    services.Configure<RabbitMQOptions>(configuration.GetSection("RabbitMQ"));
    services.Configure<ServiceOptions>(configuration.GetSection("SecondService"));
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(opts =>
{
    opts.SwaggerEndpoint("/swagger/Fibonacci/swagger.json", "Fibonacci");
    opts.RoutePrefix = String.Empty;
});
app.UseRouting();
app.MapControllers();
app.MapGet("/ping", () => "pong");
app.Run();