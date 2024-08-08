namespace Interface.Logger;
public interface IFileLoggerService
{
    Task LogRequestAsync(string logMessage);

    Task LogExceptionAsync(string logMessage);

    Task LogLogicAsync(string logMessage);
}
