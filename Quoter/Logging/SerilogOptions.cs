namespace Quoter.Logging
{
    public sealed class SerilogOptions
    {
        public bool UseConsole { get; set; } = true;
        public bool ExportLogsToOpenTelemetry { get; set; } = false;
        public string LogTemplate { get; set; } =
            "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level} - {Message:lj}{NewLine}{Exception}";
    }
}
