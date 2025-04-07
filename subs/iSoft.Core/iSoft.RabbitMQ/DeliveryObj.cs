using iSoft.Common.ExtensionMethods;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.RabbitMQ
{
    public class DeliveryObj
    {
        [JsonProperty("delivery_tag", NullValueHandling = NullValueHandling.Ignore)]
        public ulong DeliveryTag { get; set; }

        [JsonProperty("queue_name", NullValueHandling = NullValueHandling.Ignore)]
        public string QueueName { get; set; }

        [JsonProperty("exchange_name", NullValueHandling = NullValueHandling.Ignore)]
        public string Exchange { get; set; }

        [JsonProperty("routing_key", NullValueHandling = NullValueHandling.Ignore)]
        public string RoutingKey { get; set; }


        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public byte[] Data { get; set; }

        [JsonIgnore]
        public IModel model { get; set; }

        public T GetData<T>(ref string dataJson, ref string errMessage) where T : class
        {
            try
            {
                dataJson = Encoding.UTF8.GetString(Data);
                return JsonExtensionUtil.FromJson<T>(dataJson);
            }
            catch (Exception ex)
            {
                errMessage = ex.ToString();
                return null;
            }
        }
    }
}
