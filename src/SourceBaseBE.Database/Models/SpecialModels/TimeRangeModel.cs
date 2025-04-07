using SourceBaseBE.Database.Entities;
using SourceBaseBE.Database.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.Models.SpecialModels
{
	public class TimeRangeModel
	{
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public EnumTimeRangeType TimeRangeType { get; set; } = EnumTimeRangeType.None;

		public TimeRangeModel(DateTime startTime, DateTime endTime)
		{
			StartTime = startTime;
			EndTime = endTime;
		}
		public override string ToString()
		{
			return $"{this.StartTime} -> {this.EndTime}";
		}
	}
}
