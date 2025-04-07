using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.Helpers
{
	public class DateTimeHelper
	{
		public static DateTime GetStartOfDate(DateTime start)
		{
			return new DateTime(start.Year, start.Month, start.Day, 0, 0, 0);
		}
		public static DateTime GetEndOfDate(DateTime date)
		{
			return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
		}
		public static bool IsSameDay(DateTime start, DateTime date)
		{
			return (start.Year == date.Year && start.Month == date.Month && start.Day == date.Day);
		}
	}
}
