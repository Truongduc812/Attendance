using iSoft.Common.Utils;
using Microsoft.AspNetCore.Http;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Models.RequestModels
{
    public class UserRequestModel : BaseCRUDRequestModel<UserEntity>
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string? License { get; set; }
        public EnumActiveStatus? Status { get; set; }
        public string? Address { get; set; }
        public string? CompanyName { get; set; }
        [Column("display_name")]
        public string? Displayname { get; set; }
        DateTime? _birthday { get; set; }
        public List<long>? DepartmentAdmins { get; set; }
        public long DepartmentId { get; set; }
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
        public IFormFile? Avatar { get; set; }
        public DateTime? LastLogin { get; set; }
        public string? Description { get; set; }
        public long? EmployeeId { get; set; }
        public EmployeeEntity? ItemEmployee { get; set; }
        public long? AuthAccountTypeId { get; set; }
        public AuthAccountTypeEntity? ItemAuthAccountType { get; set; }
        public long? AuthTokenId { get; set; }
        public AuthTokenEntity? ItemAuthToken { get; set; }
        public List<long>? ListAuthGroup { get; set; }
        public List<long>? ListAuthPermission { get; set; }

        public override UserEntity GetEntity(UserEntity entity)
        {
            if (this.Id != null) entity.Id = (long)this.Id;
            if (this.Order != null) entity.Order = this.Order;
            if (this.Username != null) entity.Username = this.Username;
            if (this.Password != null) entity.Password = EncodeUtil.MD5(this.Password); ;
            if (this.Role != null) entity.Role = this.Role;
            if (this.License != null) entity.License = this.License;
            if (this.Status != null) entity.Status = this.Status;

            if (this.Address != null) entity.Address = this.Address;
            if (this.CompanyName != null) entity.CompanyName = this.CompanyName;
            if (this._birthday != null) entity.Birthday = this._birthday;
            if (this.Email != null) entity.Email = this.Email;
            if (this.FirstName != null) entity.FirstName = this.FirstName;
            if (this.MiddleName != null) entity.MiddleName = this.MiddleName;
            if (this.LastName != null) entity.LastName = this.LastName;
            if (this.Gender != null) entity.Gender = this.Gender;
            if (this.PhoneNumber != null) entity.PhoneNumber = this.PhoneNumber;
            if (this.LastLogin != null) entity.LastLogin = this.LastLogin;
            if (this.Description != null) entity.Description = this.Description;
            if (this.EmployeeId != null) entity.EmployeeId = this.EmployeeId;
            if (this.ItemEmployee != null) entity.ItemEmployee = this.ItemEmployee;
            if (this.AuthAccountTypeId != null) entity.AuthAccountTypeId = this.AuthAccountTypeId;
            if (this.ItemAuthAccountType != null) entity.ItemAuthAccountType = this.ItemAuthAccountType;
            if (this.AuthTokenId != null) entity.AuthTokenId = this.AuthTokenId;
            if (this.ItemAuthToken != null) entity.ItemAuthToken = this.ItemAuthToken;
            if (this.Displayname != null) entity.Displayname = this.Displayname;
            if (this.Role != null) entity.Role = this.Role.ToString();
            if (this.ListAuthGroup != null)
            {
                entity.AuthGroupIds = this.ListAuthGroup.Select(x => x).ToList();
            }

            if (this.ListAuthPermission != null)
            {
                entity.AuthPermissionIds = this.ListAuthPermission.Select(x => x).ToList();
            }
            if (this.DepartmentAdmins != null)
            {
                entity.AuthPermissionIds = this.DepartmentAdmins?.Select(x => x).ToList();
            }


            return entity;
        }

        public UserEntity GetEntityUserEdit(UserEntity entity)
        {
            if (this.Id != null) entity.Id = (long)this.Id;
            if (this.Order != null) entity.Order = this.Order;
            if (this.Status != null) entity.Status = this.Status;
            if (this.Address != null) entity.Address = this.Address;
            if (this.CompanyName != null) entity.CompanyName = this.CompanyName;
            if (this.Password != null ) entity.Password = EncodeUtil.MD5(this.Password);
            if (this._birthday != null) entity.Birthday = this._birthday;
            if (this.Email != null) entity.Email = this.Email;
            if (this.FirstName != null) entity.FirstName = this.FirstName;
            if (this.MiddleName != null) entity.MiddleName = this.MiddleName;
            if (this.LastName != null) entity.LastName = this.LastName;
            if (this.Gender != null) entity.Gender = this.Gender;
            if (this.PhoneNumber != null) entity.PhoneNumber = this.PhoneNumber;
            if (this.LastLogin != null) entity.LastLogin = this.LastLogin;
            if (this.Description != null) entity.Description = this.Description;
            if (this.ItemEmployee != null) entity.ItemEmployee = this.ItemEmployee;
            if (this.EmployeeId != null) entity.EmployeeId = this.EmployeeId;
            if (this.AuthAccountTypeId != null) entity.AuthAccountTypeId = this.AuthAccountTypeId;
            if (this.ItemAuthAccountType != null) entity.ItemAuthAccountType = this.ItemAuthAccountType;
            if (this.AuthTokenId != null) entity.AuthTokenId = this.AuthTokenId;
            if (this.ItemAuthToken != null) entity.ItemAuthToken = this.ItemAuthToken;
            if (this.Displayname != null) entity.Displayname = this.Displayname;
            if (this.Role != null) entity.Role = this.Role.ToString();
            if (this.ListAuthGroup != null)
            {
                entity.AuthGroupIds = this.ListAuthGroup.Select(x => x).ToList();
            }

            if (this.ListAuthPermission != null)
            {
                entity.AuthPermissionIds = this.ListAuthPermission.Select(x => x).ToList();
            }

            return entity;
        }


        public override Dictionary<string, (string, IFormFile)> GetFiles()
        {
            Dictionary<string, (string, IFormFile)> dicRS = new Dictionary<string, (string, IFormFile)>();
            if (this.Avatar != null)
            {
                dicRS.Add(nameof(Avatar), (Path.Combine(ConstFolderPath.Image, ConstFolderPath.Upload), this.Avatar));
            }
            /*[GEN-17]*/
            return dicRS;
        }
    }
}
