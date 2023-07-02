namespace Fibonacci.First;

public record ServiceOptions
{
    public string Protocol { get; set; }
    public string Host { get; set; }
    public string Port { get; set; }
    public string Uri { get; set; }
}