using iSoft.DBLibrary.Entities;

namespace iSoft.TrackDeviceService.Models
{
	public class ExcelModel : BaseEntity
	{
		public string Path { get; set; }
		public string TemplatePath { get; set; }
	}
}
