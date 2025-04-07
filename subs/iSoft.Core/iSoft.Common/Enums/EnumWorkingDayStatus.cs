using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.Common.Enums
{
	public enum EnumWorkingDayStatus
	{
		Attended = 1,
		Absent = 2,

	}
	public enum EnumWorkingTypeCode
	{
		Unknown = 0,
		C3,
		Unpaid_Day,
		Paid_100,
		Paid_75,
		Maternity_Leave,
		Stop_Working,
		Meal_Allowance,
		OT_150,
		Nigh_shift_30,
		OT_200,
		OT_Night_270,
		OT_300,
		OT_Night_390
	}
}
