using iSoft.Common.Models.RequestModels;

namespace SourceBaseBE.MainService.Models.RequestModels
{
	public class ParameterRequestPagingModel : PagingRequestModel
	{
		public int? DeviceId { get; set; }
	}
}
