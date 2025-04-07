using Serilog.Events;
using Serilog.Formatting;

namespace iSoft.Common.ExtensionMethods
{
  public class SerilogFormat : ITextFormatter
  {
    public void Format(LogEvent logEvent, TextWriter output)
    {
      string Timestamp = logEvent.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
      string Level = logEvent.Level.ToString();
      string SourceContext = GetPropertyValue(logEvent, "SourceContext");
      string Exception = GetPropertyValue(logEvent, "Exception");
      Exception = Exception == null ? "" : "\r\n" + Exception;
      string Message = logEvent.RenderMessage();
      string className = SourceContext == null?"": $" [{SourceContext.Substring(SourceContext.LastIndexOf('.') + 1)}]";

      SetColor(logEvent);
      output.WriteLine($"{Timestamp} [{Level}]{className} - {Message}{Exception}");
    }

    private string GetPropertyValue(LogEvent logEvent, string propertyName)
    {
      return (logEvent.Properties.ContainsKey(propertyName) ?
          ((ScalarValue)logEvent.Properties[propertyName]).Value?.ToString() : null)!;
    }

    private void SetColor(LogEvent logEvent)
    {
      ConsoleColor originalColor = Console.ForegroundColor;
      switch (logEvent.Level)
      {
        case LogEventLevel.Debug:
          Console.ForegroundColor = ConsoleColor.Gray;
          break;
        case LogEventLevel.Information:
          Console.ForegroundColor = ConsoleColor.White;
          break;
        case LogEventLevel.Warning:
          Console.ForegroundColor = ConsoleColor.Yellow;
          break;
        case LogEventLevel.Error:
          Console.ForegroundColor = ConsoleColor.Red;
          break;
        case LogEventLevel.Fatal:
          Console.ForegroundColor = ConsoleColor.DarkRed;
          break;
        default:
          Console.ForegroundColor = originalColor;
          break;
      }
    }
  }
}
