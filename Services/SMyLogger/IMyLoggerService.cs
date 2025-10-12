namespace restapi_crud_practice.Services.SMyLogger
{
    public interface IMyLoggerService
    {
        void LogInfo(string message);
        void LogError(Exception ex, string message);
        void LogError(string message);
    }
}
