using System.Numerics;
using Microsoft.Extensions.Logging;

namespace Fibonacci.Shared;

public class CountService : ICountService
{
    
    private readonly ILogger<CountService> _logger;

    public CountService(ILogger<CountService> logger)
    {
        _logger = logger;
    }
    
    public (byte[] Previous, byte[] Current) Count(byte[] previousNumber, byte[] currentNumber)
    {
        var prev = new BigInteger(previousNumber);
        var curr = new BigInteger(currentNumber);
        Console.WriteLine($"previousNumber: {prev.ToString()}, currentNumber: {curr.ToString()}");

        var newCurrentNumber = BigInteger.Add(prev, curr);
        _logger.LogInformation($"{newCurrentNumber}");
        return (currentNumber, newCurrentNumber.ToByteArray());
    }
}