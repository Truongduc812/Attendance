using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.Models.ResponseModels
{
	public class MasterDataPendingRequestReponseModel
	{

		[JsonProperty("department", NullValueHandling = NullValueHandling.Ignore)]
		public List<MasterDataResponseModel>? Departments { get; set; }
		[JsonProperty("jobtitle", NullValueHandling = NullValueHandling.Ignore)]
		public List<MasterDataResponseModel>? Titles { get; set; }
	}
	public class MasterDataDetailPendingRequestReponseModel
	{

		[JsonProperty("request_status", NullValueHandling = NullValueHandling.Ignore)]
		public List<MasterDataResponseModel>? Satuss { get; set; }
		[JsonProperty("requester", NullValueHandling = NullValueHandling.Ignore)]
		public List<MasterDataResponseModel>? Requester { get; set; }
	}
}
