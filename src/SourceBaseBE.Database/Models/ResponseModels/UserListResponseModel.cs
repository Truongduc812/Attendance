using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SourceBaseBE.Database.Models.ResponseModels
{
	public class UserListResponseModel
	{
		public string Username { get; set; }
		public string Role { get; set; }
		public string? License { get; set; }
		public EnumActiveStatus? Status { get; set; } 
		public string? Address { get; set; }
		public string? CompanyName { get; set; }

		DateTime? _birthday { get; set; }
		 
	}
}
