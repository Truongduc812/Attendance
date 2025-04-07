using iSoft.Common.Enums;
using iSoft.Common.ResponseObjectNS;
using Microsoft.Extensions.Logging;
using static iSoft.Common.Messages;
using static iSoft.Common.ResponseObjectNS.ResponseObject;

namespace Microsoft.Extensions.Logging
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
                    logger.LogInformation(message.GetMessage(args));
                    break;
                case EnumMessageType.Warning:
                    logger.LogWarning(message.GetMessage(args));
                    break;
                case EnumMessageType.Error:
                    logger.LogError(message.GetMessage(args));
                    break;
            }
        }
    }
}