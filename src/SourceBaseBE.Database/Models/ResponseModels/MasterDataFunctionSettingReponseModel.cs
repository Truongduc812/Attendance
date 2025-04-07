using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.Models.ResponseModels
{
  public class MasterDataFunctionSettingReponseModel : MasterDataSettingReponseModel
  {
    [JsonProperty("role", NullValueHandling = NullValueHandling.Ignore)]
    public List<MasterDataResponseModel>? Roles { get; set; }
  }
}
