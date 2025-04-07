using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.Models.ResponseModels
{
    public class MasterDataEmployeeReportReponseModel
    {
        [JsonProperty("department", NullValueHandling = NullValueHandling.Ignore)]
        public List<MasterDataResponseModel>? Departments { get; set; }

        [JsonProperty("jobtitle", NullValueHandling = NullValueHandling.Ignore)]
        public List<MasterDataResponseModel>? JobTitles { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public List<MasterDataResponseModel>? Type { get; set; }
        [JsonProperty("contract", NullValueHandling = NullValueHandling.Ignore)]
        public List<MasterStatusReponseModel>? Contracts { get; set; }
    }
}
