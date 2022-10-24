using EasyNetQ;
using Fibonacci.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Fibonacci.Second;

[ApiController]
[Route("api/private/counts")]
public class InternalCountController : ControllerBase
{
    private readonly ICountService _countService;
    private readonly RabbitMQOptions _rabbitMqOptions;
    
    public InternalCountController(ICountService countService,
        IOptions<RabbitMQOptions> rabbitMqOptions)
    {
        _countService = countService;
        _rabbitMqOptions = rabbitMqOptions.Value;
    }

    [HttpPost]
    public async Task<IActionResult> Count(List<CountDto> request)
    {
        List<CountDto> result = new(request.Count);
        Parallel.ForEach(request, item =>
        {
            result.Add(_countService.Count(item.PreviousNumber, item.CurrentNumber));
        });
        
        using var bus = RabbitHutch.CreateBus($"host={_rabbitMqOptions.Host};port={_rabbitMqOptions.Port}");
        await bus.SendReceive.SendAsync(Queues.Count, result);
        return Ok();
    }
}