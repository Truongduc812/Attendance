using iSoft.Common.Enums;
using iSoft.Database.Entities.Interface;
using iSoft.Database.Extensions;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iSoft.Database.Entities
{
	public class BaseCRUDEntity : IEntityFileURLEntity
	{
		[Key]
		[FormDataTypeAttribute(EnumFormDataType.hidden, false)]
		public long Id { get; set; }


		[FormDataTypeAttribute(EnumFormDataType.label, false)]
		public long? CreatedBy { get; set; }
		[NotMapped]
		public string? CreatedUsername { get; set; }


		[FormDataTypeAttribute(EnumFormDataType.label, false)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss.fff}")]
		public DateTime? CreatedAt { get; set; }


		[FormDataTypeAttribute(EnumFormDataType.label, false)]
		public long? UpdatedBy { get; set; }
		[NotMapped]
		public string? UpdatedUsername { get; set; }


		[FormDataTypeAttribute(EnumFormDataType.label, false)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss.fff}")]
		public DateTime? UpdatedAt { get; set; }


		[FormDataTypeAttribute(EnumFormDataType.label, false)]
		public bool? DeletedFlag { get; set; }


		[FormDataTypeAttribute(EnumFormDataType.integerNumber, false, 1, 1000, 1, null)]
		public long? Order { get; set; }

		public virtual void SetFileURL(Dictionary<string, string> dicImagePath)
		{
			// Do nothing
		}
	}
}