using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace iSoft.TrackDeviceService.Models
{
	public class WriteDevicePayload
	{
		public DateTime CreateAt { get; set; }
		public string Publisher { get; set; }
		public List<WriteDeviceData> Data { get; set; }
	}
	public class WriteDeviceData
	{
		public string ParamName { get; set; }
		public double Value { get; set; }
	}
}
