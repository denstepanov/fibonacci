using EasyNetQ;
using Fibonacci.Shared;
using Microsoft.Extensions.Options;

namespace Fibonacci.First;

public class CountMessageHandler : IHostedService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ICountService _countService;
    private readonly IBus _bus;
    private readonly RabbitMQOptions _rabbitMqOptions;
    private readonly ServiceOptions _serviceOptions;
    
    public CountMessageHandler(IHttpClientFactory httpClientFactory,
        ICountService countService,
        IOptions<RabbitMQOptions> rabbitMqOptions,
        IOptions<ServiceOptions> serviceOptions)
    {
        _countService = countService;
        _httpClientFactory = httpClientFactory;
        _rabbitMqOptions = rabbitMqOptions.Value;
        _bus = RabbitHutch.CreateBus($"host={_rabbitMqOptions.Host};port={_rabbitMqOptions.Port}");
        _serviceOptions = serviceOptions.Value;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _bus.SendReceive.ReceiveAsync<CountDto>(Queues.Count, async message => await OnMessage(message));
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _bus.Dispose();
        return Task.CompletedTask;
    }

    private async Task OnMessage(CountDto dto)
    {
        var result = _countService.Count(dto.PreviousNumber, dto.CurrentNumber);
        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri($"{_serviceOptions.Protocol}://{_serviceOptions.Host}:{_serviceOptions.Port}/");
        dto = new CountDto
        {
            PreviousNumber = result.Previous,
            CurrentNumber = result.Current
        };
        await client.PostAsJsonAsync(_serviceOptions.Uri, dto);
    }
}