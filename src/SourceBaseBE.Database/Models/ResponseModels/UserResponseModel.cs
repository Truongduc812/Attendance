using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SourceBaseBE.Database.Models.ResponseModels
{
	public class UserResponseModel : BaseCRUDResponseModel<UserEntity>
	{
		public string Username { get; set; }
		public string Role { get; set; }
		public string? License { get; set; }
		public EnumActiveStatus? Status { get; set; }
		public List<ISoftProjectEntity>? ListISoftProject { get; set; }
		public string? Address { get; set; }
		public string? CompanyName { get; set; }

		DateTime? _birthday { get; set; }
		public string? Birthday
		{
			get
			{
				if (_birthday.HasValue)
				{
					return _birthday.Value.ToString(ConstDateTimeFormat.DDMMYYYY);
				}
				return null;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					try
					{
						_birthday = DateTimeUtil.GetDateTimeFromString(value, ConstDateTimeFormat.DDMMYYYY);
					}
					catch
					{
						_birthday = null;
					}
				}
			}
		}
		public string? Email { get; set; }
		public string? FirstName { get; set; }
		public string? MiddleName { get; set; }
		public string? LastName { get; set; }
		public EnumGender? Gender { get; set; }
		public string? PhoneNumber { get; set; }
		[Column("display_name")]
		public string? Displayname { get; set; }
		public string? Avatar { get; set; }
		public DateTime? LastLogin { get; set; }
		public string? Description { get; set; }
		public long? EmployeeId { get; set; }
		public EmployeeEntity? ItemEmployee { get; set; }
		public long? AuthAccountTypeId { get; set; }
		public AuthAccountTypeEntity? ItemAuthAccountType { get; set; }
		public long? AuthTokenId { get; set; }
		public AuthTokenEntity? ItemAuthToken { get; set; }
		public List<AuthGroupEntity>? ListAuthGroup { get; set; }
		public List<AuthPermissionEntity>? ListAuthPermission { get; set; }

		public override object SetData(UserEntity entity)
		{
			base.SetData(entity);
			this.Username = entity.Username;
			this.Role = entity.Role;
			this.License = entity.License;
			this.Status = entity.Status;

			if (entity.ListISoftProject != null)
			{
				this.ListISoftProject = entity.ListISoftProject.Select(x => x).ToList();
			}
			this.Address = entity.Address;
			this.CompanyName = entity.CompanyName;
			this._birthday = entity.Birthday;
			this.Email = entity.Email;
			this.FirstName = entity.FirstName;
			this.MiddleName = entity.MiddleName;
			this.LastName = entity.LastName;
			this.Gender = entity.Gender;
			this.PhoneNumber = entity.PhoneNumber;
			this.Avatar = entity.Avatar;
			this.LastLogin = entity.LastLogin;
			this.Description = entity.Description;
			this.EmployeeId = entity.EmployeeId;
			this.ItemEmployee = entity.ItemEmployee;
			this.AuthAccountTypeId = entity.AuthAccountTypeId;
			this.ItemAuthAccountType = entity.ItemAuthAccountType;
			this.AuthTokenId = entity.AuthTokenId;
			this.ItemAuthToken = entity.ItemAuthToken;
			this.Displayname = entity.Displayname;
			if (entity.ListAuthGroup != null)
			{
				this.ListAuthGroup = entity.ListAuthGroup.Select(x => x).ToList();
			}

			if (entity.ListAuthPermission != null)
			{
				this.ListAuthPermission = entity.ListAuthPermission.Select(x => x).ToList();
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
