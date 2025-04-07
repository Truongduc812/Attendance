using iSoft.Common.Enums;
using iSoft.Database.Extensions;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iSoft.Database.Entities
{
	[Table("Users")]
	public class UserEntity : BaseCRUDEntity
	{

		[Required]
		[FormDataTypeAttribute(EnumFormDataType.textbox, true)]
		[StringLength(255)]
		public string Username { get; set; }


		[Required]
		[FormDataTypeAttribute(EnumFormDataType.password, true)]
		[StringLength(255)]
		[JsonIgnore]
		[Browsable(false)]
		public string Password { get; set; }


		[Required]
		[StringLength(31)]
		public string Role { get; set; }


		[StringLength(255)]
		[JsonIgnore]
		[Browsable(false)]
		public string? License { get; set; }


		public EnumActiveStatus? Status { get; set; } = EnumActiveStatus.Actived;

		public string? Avatar { get; set; }
		[NotMapped]
		public List<long>? ISoftProjectIds { get; set; } = new List<long>();
		[ListEntityAttribute(nameof(ISoftProjectEntity), nameof(ISoftProjectIds), "")]
		public List<ISoftProjectEntity>? ListISoftProject { get; set; } = new();
	}
}