using Serilog;

namespace CSVReconciliation.Core.Services;

public class AppLogger
{
    private ILogger _logger;

    public AppLogger(string outputFolder)
    {
        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

        var logFile = Path.Combine(outputFolder, "reconciliation.log");

        _logger = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level:u4}] {Message:lj}{NewLine}")
            .WriteTo.File(logFile, outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level:u4}] {Message:lj}{NewLine}")
            .CreateLogger();
    }

    public void Info(string message)
    {
        _logger.Information(message);
    }

    public void Warning(string message)
    {
        _logger.Warning(message);
    }

    public void Error(string message)
    {
        _logger.Error(message);
    }
}
