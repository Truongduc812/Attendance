using ConnectionCommon.Connection;
using iSoft.Common.ExtensionMethods;
using iSoft.ConnectionCommon.MessageBroker;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionCommon.MessageBroker
{
    public class MessageBrokerPayload
    {
        [JsonProperty("msg", NullValueHandling = NullValueHandling.Ignore)]
        public Msg Msg { get; set; }

        [JsonProperty("content", NullValueHandling = NullValueHandling.Ignore)]
        public Content Content { get; set; }
        public Guid MessageId { get; set; }
        public ulong DeliveryTag { get; set; }
        [JsonIgnore]
        public IModel Channel;
        public void AckMessage()
        {
            this.Channel.BasicAck(this.DeliveryTag, false);
        }
    }
    public partial class Content
    {
        [JsonProperty("connection")]
        public MBPayloadDTO Connection { get; set; }
        public static Content CreateContent(MBPayloadDTO cnn)
        {
            var content = new Content();
            content.Connection = cnn;
            return content;
        }
    }
    public partial class Msg
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("sender")]
        public string Sender { get; set; }

        [JsonProperty("group")]
        public string Group { get; set; }

        [JsonProperty("date")]
        public long Date { get; set; }
        public static Msg CreateMsg(string title, string sender, string group, long date)
        {
            var msg = new Msg();
            msg.Title = title;
            msg.Sender = sender;
            msg.Group = group;
            msg.Date = date;
            return msg;
        }
        public override string ToString()
        {
            return string.Format("[MessageBrokerPayload] json: {0}", JsonExtensionUtil.ToJson(this));
        }
    }
}
