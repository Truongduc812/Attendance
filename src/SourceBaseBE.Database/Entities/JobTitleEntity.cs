using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using iSoft.Database.Entities;
using iSoft.Database.Extensions;
using SourceBaseBE.Database.Interfaces;

namespace SourceBaseBE.Database.Entities
{
	[Table("JobTitles")]
	public class JobTitleEntity : BaseCRUDEntity
	{
		[Column("name")]
		[DisplayName("Name")]
		[FormDataType(iSoft.Common.Enums.EnumFormDataType.textbox, isRequired: true)]
		public string? Name { get; set; }
		[DisplayName("Description")]
		[Column("description")]
		[FormDataType(iSoft.Common.Enums.EnumFormDataType.textbox, isRequired: false)]
		public string? Description { get; set; }
		[FormDataType(iSoft.Common.Enums.EnumFormDataType.textbox, isRequired: false)]
		[Column("notes")]
		[DisplayName("Note")]
		public string? Notes { get; set; }
		[FormDataType(iSoft.Common.Enums.EnumFormDataType.hidden, isRequired: false)]
		public List<EmployeeEntity>? Employees { get; set; }
	}
}
