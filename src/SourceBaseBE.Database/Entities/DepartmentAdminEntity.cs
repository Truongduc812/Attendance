using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iSoft.Common;
using iSoft.Common.Enums;
using iSoft.Database.Entities;
using iSoft.Database.Extensions;
using Newtonsoft.Json;
using SourceBaseBE.Database.Interfaces;
using iSoft.Common.Enums;
using static iSoft.Common.ConstCommon;

namespace SourceBaseBE.Database.Entities
{
	[Table("DepartmentAdmins")]
	public class DepartmentAdminEntity : BaseCRUDEntity, IEntityUpdatedAt, IEntityUpdatedBy, IEnityCreatedAt, IEnityCreatedBy
	{
		[ForeignKey(nameof(UserEntity))]
		[FormDataType("User", EnumFormDataType.select, true)]
		public long? UserId { get; set; }
		public UserEntity? User { get; set; }

		[ForeignKey(nameof(DepartmentEntity))]
		[FormDataType("Department", EnumFormDataType.select, true)]
		public long? DepartmentId { get; set; }
		public DepartmentEntity? Department { get; set; }
		[FormDataType("Role", EnumFormDataType.selectMulti, true)]
		public EnumDepartmentAdmin? Role { get; set; }
		[StringLength(255)]
		public string? Note { get; set; }
	}
}

