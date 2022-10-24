using Microsoft.Extensions.Logging;

namespace Fibonacci.Shared;

public class CountService : ICountService
{
    
    private readonly ILogger<CountService> _logger;

    public CountService(ILogger<CountService> logger)
    {
        _logger = logger;
    }
    
    public CountDto Count(ulong previousNumber, ulong currentNumber)
    {
        var result = new CountDto
        {
            PreviousNumber = currentNumber,
            CurrentNumber = previousNumber + currentNumber
        };
        
        _logger.LogInformation($"{result.CurrentNumber}");
        return result;
    }
}