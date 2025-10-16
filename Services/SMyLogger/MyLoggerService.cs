namespace restapi_crud_practice.Services.SMyLogger
{
    public class MyLoggerService : IMyLoggerService
    {
        private readonly string logFilePath;
        private readonly string timestampFormat;
        private readonly bool writeToConsole;
        private readonly bool writeToFile;
        private readonly string logLevel;
        private readonly bool includeStackTrace;

        public MyLoggerService(IConfiguration config)
        {
            timestampFormat = config["MyLogger:TimestampFormat"] ?? "dd-MM-yyyy HH:mm:ss.fff";
            logFilePath = GetActualFilePath(config["MyLogger:LogFilePath"] ?? "Logs/myapp.log");
            writeToConsole = bool.Parse(config["MyLogger:WriteToConsole"] ?? "true");
            writeToFile = bool.Parse(config["MyLogger:WriteToFile"] ?? "true");
            logLevel = config["MyLogger:LogLevel"] ?? "Information";
            includeStackTrace = bool.Parse(config["MyLogger:IncludeStackTrace"] ?? "true");

            Initialize();
        }

        private static string GetActualFilePath(string filePath)
        {

            return filePath.Replace("{date}", DateTime.Now.ToString("dd-MM-yyyy"));
        }

        private void Initialize()
        {
            if (writeToFile)
            {
                var logDir = Path.GetDirectoryName(logFilePath);
                if (!string.IsNullOrEmpty(logDir))
                {
                    Directory.CreateDirectory(logDir);
                    Console.WriteLine($"Log directory: {logDir}");
                }
            }

            Console.WriteLine($"MyLogger initialized: Console={writeToConsole}, File={writeToFile}, Level={logLevel}");
        }

        public void LogInfo(string message)
        {
            if (ShouldLog("Information"))
                WriteLog("INFO", null, message);
        }

        public void LogError(string message)
        {
            if (ShouldLog("Error"))
                WriteLog("ERROR", null, message);
        }

        public void LogError(Exception ex, string message)
        {
            if (ShouldLog("Error"))
                WriteLog("ERROR", ex, message);
        }

        private bool ShouldLog(string level)
        {
            return level.ToUpper() switch
            {
                "INFORMATION" => logLevel is "Information" or "Debug",
                "ERROR" => logLevel is "Information" or "Debug" or "Error",
                _ => true
            };
        }

        private void WriteLog(string level, Exception exception, string message)
        {
            var timestamp = DateTime.Now.ToString(timestampFormat);
            var logEntry = $"{timestamp} [{level}] {message}";

            if (exception != null && includeStackTrace)
            {
                logEntry += $"{Environment.NewLine}Exception: {exception.Message}";
                if (exception.StackTrace != null)
                {
                    logEntry += $"{Environment.NewLine}Stack Trace: {exception.StackTrace}";
                }
            }

            if (writeToConsole)
            {
                Console.WriteLine(logEntry);
            }

            if (writeToFile)
            {
                try
                {
                    File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
                    Console.WriteLine($"Log written to: {logFilePath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to write log file: {ex.Message}");
                }
            }
        }
    }
}