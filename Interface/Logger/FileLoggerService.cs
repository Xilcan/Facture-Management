using System.IO.Abstractions;
using Microsoft.Extensions.Logging;

namespace Interface.Logger;
public class FileLoggerService : IFileLoggerService
{
    private readonly string _directoryName = "Logs";
    private readonly string _requestLogFileName = "request_logs";
    private readonly string _exceptionLogFileName = "exception_logs";
    private readonly string _logicLogFileName = "Business_logs";
    private readonly string _fileType = ".txt";
    private readonly ILogger<FileLoggerService> _logger;
    private readonly IFileSystem _fileSystem;

    public FileLoggerService(ILogger<FileLoggerService> logger, IFileSystem fileSystem)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(fileSystem);

        _logger = logger;
        _fileSystem = fileSystem;
    }

    public async Task LogRequestAsync(string logMessage)
    {
        var date = DateTime.Now.Date;
        var filePath = $"{_directoryName}\\{_requestLogFileName}_{date:dd_MM_yyyy}{_fileType}";
        await AppendLogAsync(filePath, logMessage);
    }

    public async Task LogExceptionAsync(string logMessage)
    {
        var date = DateTime.Now.Date;
        var filePath = $"{_directoryName}\\{_exceptionLogFileName}_{date:dd_MM_yyyy}{_fileType}";
        await AppendLogAsync(filePath, logMessage);
    }

    public async Task LogLogicAsync(string logMessage)
    {
        var date = DateTime.Now.Date;
        var filePath = $"{_directoryName}\\{_logicLogFileName}_{date:dd_MM_yyyy}{_fileType}";
        await AppendLogAsync(filePath, logMessage);
    }

    private async Task AppendLogAsync(string filePath, string logMessage)
    {
        try
        {
            if (!_fileSystem.Directory.Exists(_directoryName))
            {
                _fileSystem.Directory.CreateDirectory(_directoryName);
            }

            await _fileSystem.File.AppendAllTextAsync(filePath, $"{logMessage}{Environment.NewLine}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to write log to {filePath}: {ex.Message}");
        }
    }
}
