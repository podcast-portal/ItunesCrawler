using Serilog;
using Serilog.Events;

namespace PodcastManager.Doubles;

public class LoggerDummy : ILogger
{
    public void Write(LogEvent logEvent)
    {
    }
}