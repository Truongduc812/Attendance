using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.Models.RequestModels
{
	public class MasterDataTotalPendingRequestModel
	{
		public bool? Department { get; set; }
		public bool? JobTitle { get; set; }
	}
	public class MasterDataDetailPendingRequestModel
	{

		public bool? ApproveStatus { get; set; }
		public bool? Editer { get; set; }
		public long? EmployeeId { get; set; }
	}
}
