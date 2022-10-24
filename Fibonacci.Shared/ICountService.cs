namespace Fibonacci.Shared;

public interface ICountService
{
    CountDto Count(ulong previousNumber, ulong currentNumber);
}