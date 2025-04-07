namespace iSoft.TrackDeviceService.Models
{
	public enum eOrder
	{
		ASC,
		DESC
	}
	public class APIRequest
	{
		public ParamRequest[] Param { get; set; }
	}
	public class ParamRequest
	{
		public int? Page { get; set; }

		public int? Limit { get; set; }
		public eOrder? Order { get; set; }
		public string? Filter { get; set; }
		public ulong? Id { get; set; }
		public string? Name { get; set; }
	}
}
