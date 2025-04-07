using SourceBaseBE.Database.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.MainService
{
	public class ConstMain
	{
		public static class ConstPayloadField
		{
			public const string PushTime = "checkTime";
			public const string EmployeeMachineCode = "employeeMachineCode";
			public const string EmployeeFullName = "employeeFullName";
			public const string InOutType = "inOutType";
			public const int MaximumOTHours = 40;
			public static DateTime StartTimeShift1 = new DateTime(1900, 1, 1, 6, 0, 0);
			public static DateTime StartTimeShift2 = new DateTime(1900, 1, 1, 14, 0, 0);
			public static DateTime StartTimeShift3 = new DateTime(1900, 1, 1, 22, 0, 0);

		}

		public static string ConstTemplateFilePath = "./wwwroot/ReportTemplate/template.xlsx";
        public static string ConstTemplateOTTimeFilePath = "./wwwroot/ReportTemplate/template_OT_time.xlsx";
        public static string ConstTemplateBKFilePath = "./wwwroot/ReportTemplate/Backup/template_{0}.xlsx";
    }
}
