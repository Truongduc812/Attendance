using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Globalization;
using System;
using SourceBaseBE.Database.Enums;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Models.TrackDevice
{
	public partial class FaceIdPayload
	{
		[JsonProperty("result")]
		public string Result { get; set; }

		[JsonProperty("reason")]
		public string Reason { get; set; }

		[JsonProperty("data")]
		public DataArr[][] Data { get; set; }
	}

	public partial class DataArr
	{
		[JsonProperty("sn")]
		public string Sn { get; set; }

		[JsonProperty("PhotoImage")]
		public object PhotoImage { get; set; }

		[JsonProperty("AttDate")]
		public DateTimeOffset AttDate { get; set; }

		[JsonProperty("AttTime_View")]
		public DateTimeOffset AttTimeView { get; set; }

		[JsonProperty("maskflag")]
		public long? Maskflag { get; set; }

		[JsonProperty("EmployeeID")]
		public string EmployeeId { get; set; }

		[JsonProperty("FullName")]
		public string FullName { get; set; }

		[JsonProperty("MachineAlias")]
		public string MachineAlias { get; set; }

		[JsonProperty("MachineNo")]
		public long? MachineNo { get; set; }

		[JsonProperty("AttStateName")]
		public string AttStateName { get; set; }

		[JsonProperty("AttState")]
		public EnumFaceId AttState { get; set; }

		[JsonProperty("AttTime")]
		public DateTimeOffset AttTime { get; set; }

		[JsonProperty("temperature")]
		public long? Temperature { get; set; }

		[JsonProperty("AttType")]
		public long? AttType { get; set; }

		[JsonProperty("DepartmentName")]
		public string DepartmentName { get; set; }

		[JsonProperty("isReadOnlyRow")]
		public object? IsReadOnlyRow { get; set; }
	}


}
