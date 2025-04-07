using iSoft.Common.Utils;
using Microsoft.AspNetCore.Http;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Models.RequestModels
{
    public class EmployeeRequestModel : BaseCRUDRequestModel<EmployeeEntity>
    {
        public string? Name { get; set; }
        public string? FullName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? DisplayName { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeMachineCode { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public EnumEmployeeStatus? EmployeeStatus { get; set; }
        public EnumGender? Gender { get; set; }
        public string? Address { get; set; }
        public DateTime? Birdday { get; set; }
        public DateTime? JoiningDate { get; set; }
        public IFormFile? Avatar { get; set; }
        public long? DepartmentId { get; set; }
        public long? JobTitleId { get; set; }
        public EnumTypeContract? Contract { get; set; }
        public bool? IsDeleteAvatar { get; set; }

        public EmployeeEntity GetEntity(EmployeeEntity entity)
        {
            if (this.Id != null) entity.Id = (long)this.Id;
            if (this.Order != null) entity.Order = this.Order;
            if (this.Name != null) entity.Name = this.Name;
            if (this.FullName != null) entity.FullName = this.FullName;
            if (this.FirstName != null) entity.FirstName = this.FirstName;
            if (this.MiddleName != null) entity.MiddleName = this.MiddleName;
            if (this.LastName != null) entity.LastName = this.LastName;
            if (this.DisplayName != null) entity.DisplayName = this.DisplayName;
            if (this.JoiningDate != null) entity.JoiningDate = this.JoiningDate;
            if (this.EmployeeCode != null) entity.EmployeeCode = this.EmployeeCode;
            if (this.EmployeeMachineCode != null) entity.EmployeeMachineCode = this.EmployeeMachineCode;
            if (this.Email != null) entity.Email = this.Email;
            if (this.PhoneNumber != null) entity.PhoneNumber = this.PhoneNumber;
            entity.EmployeeStatus = EnumEmployeeStatus.Actived;
            if (this.Gender != null) entity.Gender = this.Gender;
            if (this.Address != null) entity.Address = this.Address;
            if (this.Birdday != null) entity.Birthday = this.Birdday;
            if (this.DepartmentId != null) entity.DepartmentId = this.DepartmentId;
            if (this.JobTitleId != null) entity.JobTitleId = this.JobTitleId;
            if (this.Contract != null) entity.Contract = this.Contract;
            if(this.IsDeleteAvatar == true) entity.Avatar = null;

            return entity;
        }

        public override Dictionary<string, (string, IFormFile)> GetFiles()
        {
            Dictionary<string, (string, IFormFile)> dicRS = new Dictionary<string, (string, IFormFile)>();
            if (this.Avatar != null)
            {
                dicRS.Add(nameof(Avatar), (ConstDatabase.ConstImageSaveAvatarPath, this.Avatar));
            }
            /*[GEN-17]*/
            return dicRS;
        }
    }
}
