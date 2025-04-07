using iSoft.Common.Enums;

namespace iSoft.Common.Models.ConfigModel.Subs
{
    public class LimitModel
    {
        public EnumLimitType LimitType { get; set; }
        public int MaxValue { get; set; }
    }
}