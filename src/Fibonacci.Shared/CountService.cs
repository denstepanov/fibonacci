using System.Numerics;

namespace Fibonacci.Shared;

public class CountService : ICountService
{
    public CountService()
    {
    }
    
    public (byte[] Previous, byte[] Current) Count(byte[] previousNumber, byte[] currentNumber)
    {
        var prev = new BigInteger(previousNumber);
        var curr = new BigInteger(currentNumber);
        Console.WriteLine($"previousNumber: {prev}, currentNumber: {curr}");

        var newCurrentNumber = BigInteger.Add(prev, curr);
        return (currentNumber, newCurrentNumber.ToByteArray());
    }
}