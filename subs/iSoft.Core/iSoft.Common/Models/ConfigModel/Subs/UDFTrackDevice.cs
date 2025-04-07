namespace iSoft.Common.Models.ConfigModel.Subs
{
    public enum eMessageBroker
    {
        Rabbit,
        Kafka
    }
    public class TrackDeviceConfig
    {
        public bool IsUsingRealtimeService { get; set; }
        public string? SocketServerUrl { get; set; }
        public string[]? EventSendRealtimeName { get; set; }
        public string[]? EventReceiveRealtimeName { get; set; }
        public int? CheckSocketConnectionInterval { get; set; }
        public eMessageBroker MessageBrokerType { get; set; }
        public bool IsLocalCache { get; set; }
        public long NumberOfCacheRecord { get; set; }
        public string BroadcastChannel { get; set; }
        public string EachConnChannel { get; set; }
        public string GroupConnChannel { get; set; }
    }
}