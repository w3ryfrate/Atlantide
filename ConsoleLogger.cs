
using Serilog;

namespace ProjectNewWorld.Core;

public class ConsoleLogger : ILogger
{
    private readonly Serilog.ILogger _internalLogger;

    public ConsoleLogger()
    {
        _internalLogger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
    }

    public void LogFatal(string message, bool exit = true)
    {
        _internalLogger.Fatal(message);
        if (exit)
            Environment.Exit(1);
    }

    public void LogError(string message)
    {
        _internalLogger.Error(message);
    }

    public void LogInformation(string message)
    {
        _internalLogger.Information(message);
    }

    public void LogWarning(string message)
    {
        _internalLogger.Warning(message);
    }
}
