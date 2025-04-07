using iSoft.Common.Enums;
using Serilog;
using static iSoft.Common.Messages;

namespace Serilog
{
  public static class ExtensionMethods
  {
    public static void LogMsg(this ILogger logger, Message message, params object?[] args)
    {
      if (logger == null || message == null)
      {
        return;
      }
      switch (message.GetMsgType())
      {
        case EnumMessageType.Info:
          logger.Information(message.GetMessage(args));
          break;
        case EnumMessageType.Warning:
          logger.Warning(message.GetMessage(args));
          break;
        case EnumMessageType.Error:
          logger.Error(message.GetMessage(args));
          break;
      }
    }
    public static void LogInformation(this ILogger logger, string message, params string[] parameters)
    {
      logger.Information(message, parameters);
    }
    public static void LogInformation(this ILogger logger, string message)
    {
      logger.Information(message);
    }
    public static void LogError(this ILogger logger, string message, params string[] parameters)
    {
      logger.Error(message, parameters);
    }
    public static void LogError(this ILogger logger, string message)
    {
      logger.Error(message);
    }
  }
}