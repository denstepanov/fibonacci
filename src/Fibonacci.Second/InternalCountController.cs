using EasyNetQ;
using Fibonacci.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Fibonacci.Second;

[ApiController]
[Route("api/internal/counts")]
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
    public async Task<IActionResult> Count(CountDto dto)
    {
        var result = _countService.Count(dto.PreviousNumber, dto.CurrentNumber);
        using var bus = RabbitHutch.CreateBus($"host={_rabbitMqOptions.Host};port={_rabbitMqOptions.Port}");
        var message = new CountDto
        {
            PreviousNumber = result.Previous,
            CurrentNumber = result.Current
        };
        await bus.SendReceive.SendAsync(Queues.Count, message);
        return Ok();
    }
}