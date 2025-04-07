using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SourceBaseBE.Database.Models.ResponseModels
{
	public class UserModel
	{
		public long Id { get; set; }
		public string Username { get; set; }
		public string Role { get; set; }
		public string? Address { get; set; }
		public string? CompanyName { get; set; }
		public string? Birthday { get; set; }
		public string? Email { get; set; }
		public string? FirstName { get; set; }
		public string? MiddleName { get; set; }
		public string? LastName { get; set; }
		public EnumGender? Gender { get; set; }
		public string? PhoneNumber { get; set; }
		[Column("display_name")]
		public string? Displayname { get; set; }
		public string? Avatar { get; set; }
		public long? EmployeeId { get; set; }
		public List<EnumDepartmentAdmin?>? Roles { get; set; }

	}
 
}
