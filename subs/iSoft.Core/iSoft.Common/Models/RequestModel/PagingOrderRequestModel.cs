

using iSoft.Common.Utils;
using static iSoft.Common.ConstCommon;

namespace iSoft.Common.Models.RequestModels
{
    public class PagingOrdersRequestModel : PagingRequestModel
    {
        public List<string>? Orders { get; set; }
        public List<int>? Sorts { get; set; }
        public override string GetKeyCache()
        {
            string keyCache =
                base.GetKeyCache()
                + Orders != null ? string.Join(", ", Orders) : ""
                + Sorts != null ? string.Join(", ", Sorts) : "";

            string rs = EncodeUtil.MD5(keyCache);
            return rs;
        }
    }
}