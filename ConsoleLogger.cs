using Serilog;

namespace Core;

public class ConsoleLogger : ILogger
{
    private readonly Serilog.ILogger _logger;

    public ConsoleLogger()
    {
        _logger = new LoggerConfiguration()
        .WriteTo
        .Console()
        .CreateLogger();
    }

    public void LogInformation(string message)
    {
        _logger.Information(message);
    }

    public void LogWarning(string message)
    {
        _logger.Warning(message);
    }

    public void LogError(string message)
    {
        _logger.Error(message);
    }

    public void LogFatal(string message, bool exit = true)
    {
        _logger.Fatal(message);
        if (exit)
            Environment.Exit(1);
    }
}
