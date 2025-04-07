using iSoft.Common.Enums;
using iSoft.Common.ExtensionMethods;

namespace iSoft.Common.Models.ConfigModel.Subs
{
    public class FCMConfigModel
    {
        public FMCServerModel FMCServer { get; set; }
        public FMCClientModel FMCClient { get; set; }

        public FCMConfigModel(FMCServerModel fMCServer, FMCClientModel fMCClient)
        {
            FMCServer = fMCServer;
            FMCClient = fMCClient;
        }

        public object GetLogStr()
        {
            return $"FMCClient: {FMCClient.ToJson()}";
        }
    }

    public class FMCServerModel
    {
        public string ServerKey { get; set; }
        public string MessagingSenderId { get; set; }
        public string DefaultIcon { get; set; }
        public string DefaultVibrate { get; set; }

        public FMCServerModel(string serverKey, string messagingSenderId, string defaultIcon, string defaultVibrate)
        {
            ServerKey = serverKey;
            MessagingSenderId = messagingSenderId;
            DefaultIcon = defaultIcon;
            DefaultVibrate = defaultVibrate;
        }
    }

    public class FMCClientModel
    {
        public string ApiKey { get; set; }
        public string AuthDomain { get; set; }
        public string ProjectId { get; set; }
        public string StorageBucket { get; set; }
        public string MessagingSenderId { get; set; }
        public string AppId { get; set; }

        public FMCClientModel(string apiKey, string authDomain, string projectId, string storageBucket, string messagingSenderId, string appId)
        {
            ApiKey = apiKey;
            AuthDomain = authDomain;
            ProjectId = projectId;
            StorageBucket = storageBucket;
            MessagingSenderId = messagingSenderId;
            AppId = appId;
        }
    }
}