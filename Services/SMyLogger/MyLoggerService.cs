using Microsoft.Extensions.Configuration;

namespace restapi_crud_practice.Services.SMyLogger
{
    public class MyLoggerService : IMyLoggerService
    {
        private readonly string logFilePath;
        private readonly string timestampFormat;
        private readonly string logLevelInfo;
        private readonly string logLevelError;
        private readonly bool writeToConsole;
        private readonly bool writeToFile;

        public MyLoggerService(IConfiguration config)
        {
            logFilePath = config["MyLogger:LogFilePath"] ?? "Logs/log-dd-MM-yyyy.log";
            timestampFormat = config["MyLogger:TimestampFormat"] ?? "dd-MM-yyyy HH:mm:ss.fff";
            logLevelInfo = config["MyLogger:LogLevelInfo"] ?? "Information";
            logLevelError = config["MyLogger:LogLevelError"] ?? "Error";
            writeToConsole = bool.TryParse(config["MyLogger:WriteToConsole"], out var console) && console;
            writeToFile = bool.TryParse(config["MyLogger:WriteToFile"], out var file) && file;

            if (writeToFile)
            {
                var logDir = Path.GetDirectoryName(logFilePath);
                if (!string.IsNullOrEmpty(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }
            }
        }

        public void LogInfo(string message)
        {
            WriteLog(logLevelInfo, message);
        }

        public void LogError(string message)
        {
            WriteLog(logLevelError, message);
        }

        public void LogError(Exception ex, string message)
        {
            var fullMessage = $"{message} - {ex.Message}";
            WriteLog(logLevelError, fullMessage);
        }

        private void WriteLog(string level, string message)
        {
            var timestamp = DateTime.Now.ToString(timestampFormat);
            var logEntry = $"{timestamp} [{level}] {message}";

            if (writeToConsole) 
            {
                Console.WriteLine(logEntry);
            }

            if (!writeToFile) 
            {
                try
                {
                    File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to write to log file: {ex.Message}");
                }
            }
        }
    }
}