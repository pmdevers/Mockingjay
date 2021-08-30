using Serilog.Core;
using Serilog.Events;
using System;

namespace MockingjayApp
{
    public class MockingjayLoggerSink : ILogEventSink
    {
        public event EventHandler NewLogHandler;

        public void Emit(LogEvent logEvent)
        {
#if DEBUG
            Console.WriteLine($"{logEvent.Timestamp}] {logEvent.MessageTemplate}");
#endif
            NewLogHandler?.Invoke(typeof(MockingjayLoggerSink), new LogEventArgs() { Log = logEvent });
        }
    }

    public class LogEventArgs : EventArgs
    {
        public LogEvent Log { get; set; }
    }
}
