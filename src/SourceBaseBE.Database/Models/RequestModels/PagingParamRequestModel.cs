
using iSoft.Common.Utils;
using static iSoft.Common.ConstCommon;

namespace iSoft.Common.Models.RequestModels
{
	public class PagingParamRequestModel : PagingRequestModel
	{
		public string? SearchStr { get; set; }

		public string? FilterStr { get; set; }

		public DateTime? DateFrom { get; set; }

		public DateTime? DateTo { get; set; }

		public string? Language { get; set; }

		public string? SortStr { get; set; }
		public long? DepartmentId { get; set; }	

		public override string GetKeyCache()
		{
			string keyCache = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}",
			  base.GetKeyCache(),
			  this.SearchStr?.RemoveSpecialChar(),
			  this.FilterStr?.RemoveSpecialChar(),
			  this.SortStr?.RemoveSpecialChar(),
			  this.DateFrom == null ? "" : this.DateFrom.Value.ToString(ConstDateTimeFormat.YYYYMMDDTHHMMSS_FFF),
			  this.DateTo == null ? "" : this.DateTo.Value.ToString(ConstDateTimeFormat.YYYYMMDDTHHMMSS_FFF),
			  this.Language,
				this.DepartmentId);
			string rs = EncodeUtil.MD5(keyCache);
			return rs;
		}
	}
}