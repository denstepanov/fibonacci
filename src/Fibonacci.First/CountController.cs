using System.Numerics;
using Fibonacci.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Fibonacci.First;

[ApiController]
[Route("api/public/counts")]
public class CountController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ICountService _service;
    private readonly ServiceOptions _serviceOptions;

    public CountController(IHttpClientFactory httpClientFactory,
        ICountService service,
        IOptions<ServiceOptions> serviceOptions)
    {
        _httpClientFactory = httpClientFactory;
        _service = service;
        _serviceOptions = serviceOptions.Value;
    }

    [HttpPost("{numberOfThreads:int}")]
    public IActionResult InitCount([FromRoute] int numberOfThreads = 1)
    {
        Parallel.For(0, numberOfThreads, async i =>
        {
            var result = _service.Count(
                new BigInteger(0).ToByteArray(), 
                new BigInteger(1).ToByteArray());
            
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri($"{_serviceOptions.Protocol}://{_serviceOptions.Host}:{_serviceOptions.Port}/");
            var dto = new CountDto
            {
                PreviousNumber = result.Previous,
                CurrentNumber = result.Current
            };
            await client.PostAsJsonAsync(_serviceOptions.Uri, dto);
        });
        return Ok();
    }
}
