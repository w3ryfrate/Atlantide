

namespace Core;

public interface ILogger
{
    public void LogInformation(string message);
    public void LogWarning(string message);
    public void LogError(string message);
    public void LogFatal(string message, bool exit = true);
}
