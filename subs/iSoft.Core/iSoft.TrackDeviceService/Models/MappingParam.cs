using System.ComponentModel.DataAnnotations.Schema;

namespace iSoft.TrackDeviceService.Models
{
	public class MappingParam
	{
		public string EnviromentVarName { get; set; }
		public string TableName { get; set; }
		public string DataType { get; set; }
		//public string Description { get; set; }
		//public string NOTE { get; set; }
	}
}
