

using iSoft.Common.Utils;
using static iSoft.Common.ConstCommon;

namespace iSoft.Common.Models.RequestModels
{
    public class PagingHolidayRequestModel : PagingRequestModel
    {
        public string? SearchStr { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public string? Language { get; set; }

        public string? SortStr { get; set; }
        public override string GetKeyCache()
        {
            string keyCache = string.Format("{0}|{1}|{2}|{3}|{4}|{5}",
              base.GetKeyCache(),
              this.SearchStr?.RemoveSpecialChar(),
              this.SortStr?.RemoveSpecialChar(),
              this.DateFrom == null ? "" : this.DateFrom.Value.ToString(ConstDateTimeFormat.YYYYMMDDTHHMMSS_FFF),
              this.DateTo == null ? "" : this.DateTo.Value.ToString(ConstDateTimeFormat.YYYYMMDDTHHMMSS_FFF),
              this.Language);
            string rs = EncodeUtil.MD5(keyCache);
            return rs;
        }

    }
    public class PagingHolidayReportModel
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

    }
}
