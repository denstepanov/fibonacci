using Fibonacci.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Fibonacci.First;

[ApiController]
[Route("api/public/counts")]
public class CountController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ICountService _countService;
    private readonly ServiceOptions _serviceOptions;

    public CountController(IHttpClientFactory httpClientFactory,
        ICountService countService,
        IOptions<ServiceOptions> serviceOptions)
    {
        _httpClientFactory = httpClientFactory;
        _countService = countService;
        _serviceOptions = serviceOptions.Value;
    }

    [HttpPost("{numberOfThreads:int}")]
    public async Task<IActionResult> InitCount([FromRoute] int numberOfThreads = 1)
    {
        List<CountDto> result = new(numberOfThreads);
        Parallel.For(0, numberOfThreads, i =>
        {
            result.Add(_countService.Count(0, 1));
        });
        
        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri($"{_serviceOptions.Protocol}://{_serviceOptions.Host}:{_serviceOptions.Port}/");
        await client.PostAsJsonAsync(_serviceOptions.Uri, result);
        return Ok();
    }
}