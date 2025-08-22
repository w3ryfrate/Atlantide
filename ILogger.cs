
namespace ProjectNewWorld.Core;

public interface ILogger
{
    void LogFatal(string message, bool exit = true);
    void LogError(string message);
    void LogWarning(string message);
    void LogInformation(string message);
}
