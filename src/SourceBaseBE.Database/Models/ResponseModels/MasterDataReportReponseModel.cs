using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.Models.ResponseModels
{
    public class MasterDataReportReponseModel
    {
        [JsonProperty("types", NullValueHandling = NullValueHandling.Ignore)]
        public List<MasterStatusReponseModel>? Types { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public List<MasterStatusReponseModel>? Statuss { get; set; }
    }
}
