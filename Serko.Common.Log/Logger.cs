using Serilog;
using Serilog.Events;
using SeriLog = Serilog.Log;

namespace Serko.Common.Log
{
    public class Logger : ILogger
    {
        public Logger()
        {
            SeriLog.Logger = new LoggerConfiguration()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Fatal)
                        .Enrich.FromLogContext()
                        .WriteTo.Console(
                            outputTemplate: "[{Timestamp} {SourceContext} {Level:u4}] {Message} {NewLine} {Exception}"
                        )
                        .WriteTo.File(new Serilog.Formatting.Compact.CompactJsonFormatter(), "Serko.Expense.Api.log", shared:true)
                        .CreateLogger();
        }

        public void LogInformation<T>(string log)
        {
            SeriLog.ForContext<T>().Information(log);
        }

        public void LogWarning<T>(string log)
        {
            SeriLog.ForContext<T>().Warning(log);
        }

        public void LogError<T>(string log)
        {
            SeriLog.ForContext<T>().Error(log);
        }

        public void Flush()
        {
            SeriLog.CloseAndFlush();
        }
    }
}
