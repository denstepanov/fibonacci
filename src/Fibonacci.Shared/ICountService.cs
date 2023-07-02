using System.Numerics;

namespace Fibonacci.Shared;

public interface ICountService
{
    (byte[] Previous, byte[] Current) Count(byte[] previousNumber, byte[] currentNumber);
}