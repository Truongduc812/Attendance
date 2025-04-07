using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.Common.Enums
{
    public enum EnumTypeContract
    {
        FTE = 0,
        LTE = 1,
        STE = 2,
        OTHER = 3
    }
    public class EnumTypeContractStr
    {
        public static string FTE = "FLE";
        public static string LTE = "LTE";
        public static string STE = "STE";
        public static string OTHER = "OTHER";
        public static string GetFromCode(EnumTypeContract? st)
        {
            switch (st)
            {
                case EnumTypeContract.FTE:
                    return EnumTypeContractStr.FTE;
                case EnumTypeContract.LTE:
                    return EnumTypeContractStr.LTE;
                case EnumTypeContract.STE:
                    return EnumTypeContractStr.STE;
                case EnumTypeContract.OTHER:
                    return EnumTypeContractStr.OTHER;
            }
            return "";
        }
    }
}
