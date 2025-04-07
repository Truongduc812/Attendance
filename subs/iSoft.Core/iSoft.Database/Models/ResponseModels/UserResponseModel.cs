using iSoft.Common.Enums;
using iSoft.Common.Utils;
using iSoft.Database.Entities;
using Microsoft.AspNetCore.Http;
using static iSoft.Common.ConstCommon;

namespace iSoft.Database.Models.ResponseModels
{
	public class UserResponseModel : BaseCRUDResponseModel<UserEntity>
	{
		public string? Username { get; set; }
		public string? Password { get; set; }
		public string? Role { get; set; }
		public string? License { get; set; }
		public string? Avatar { get; set; }
		public EnumActiveStatus? Status { get; set; }
		public List<ISoftProjectEntity>? ListISoftProject { get; set; }
		public string? AccessToken { get; set; }

		public override object SetData(UserEntity entity)
		{
			base.SetData(entity);
			this.Username = entity.Username;
			this.Password = entity.Password;
			this.Role = entity.Role;
			this.License = entity.License;
			this.Status = entity.Status;
			this.Avatar = entity.Avatar;
			if (entity.ListISoftProject != null)
			{
				this.ListISoftProject = entity.ListISoftProject.Select(x => x).ToList();
			}
			return this;
		}
		public override List<object> SetData(List<UserEntity> listEntity)
		{
			List<Object> listRS = new List<object>();
			foreach (UserEntity entity in listEntity)
			{
				listRS.Add(new UserResponseModel().SetData(entity));
			}
			return listRS;
		}

	}
}
