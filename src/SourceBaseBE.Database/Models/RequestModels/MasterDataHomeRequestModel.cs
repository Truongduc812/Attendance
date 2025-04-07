using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.Models.RequestModels
{
	public class MasterDataHomeRequestModel : MasterDataRequestModel
	{
		public bool? Status { get; set; }
	}
	public class MasterDataDetailRequestModel
	{
		public bool? Status { get; set; }
		public bool? Types { get; set; }
	}

}
