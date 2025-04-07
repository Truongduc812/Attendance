using PRPO.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database
{
    public static class GlobalConsts
    {
        public const string DefaultKeySearch = "default";
        public const string NameSheetImportEmployeeDepartment = "";
        public const string NameSheetImportAdminDepartment = "";
        public const string NameSheetImportHoliday = "";
        public const string NameSheetImportSymbol = "";
        public const int MINIMUM_LENGTH_PASSWORD = 6;
        public const int MAXIMUM_LENGTH_PASSWORD = 40;
        public const int MAXIMUM_OT_Hour = 40;
        public const int MAXIMUM_HOUR_IN_SHIFT = 8;
    }
}
