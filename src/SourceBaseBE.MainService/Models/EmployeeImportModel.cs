using iSoft.Common.Enums;
using SourceBaseBE.Database.Entities;
using System;

namespace SourceBaseBE.Database.Models.RequestModels
{
    public class EmployeeImportModel : BaseCRUDRequestModel<EmployeeEntity>
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
        public EnumGender? Gender { get; set; }
        public string? Address { get; set; }
        public DateTime? BirthDay { get; set; }
        public DateTime? JoiningDate { get; set; } 
        public string? DepartmentStr { get; set; }
        public string? JobTitleStr { get; set; }
        public EnumTypeContract? Contract { get; set; }
        public EnumEmployeeStatus? EmployeeStatus { get; set; }
        public DateTime? LeavingDay { get; set; }
        public string? KindOfTermination { get; set; }
        public string? Reason { get; set; }
        public string? Ward { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public string? Note { get; set; }
        public string? Avatar { get; set; }


        public EmployeeEntity GetEntity()
        {
            var entity = new EmployeeEntity();
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
            if (this.Email != null) entity.Email = this.Email;
            if (this.PhoneNumber != null) entity.PhoneNumber = this.PhoneNumber;
            if (this.Gender != null) entity.Gender = this.Gender;
            if (this.Address != null) entity.Address = this.Address;
            if (this.BirthDay != null) entity.Birthday = this.BirthDay;
            if (this.Avatar != null) entity.Avatar = this.Avatar;
            if (this.EmployeeMachineCode != null) entity.EmployeeMachineCode = this.EmployeeMachineCode;
            if (this.EmployeeStatus != null) entity.EmployeeStatus = this.EmployeeStatus;
            if(this.Contract != null) entity.Contract = this.Contract;
            return entity;
        }
        public EmployeeImportModel SetModel(EmployeeEntity entity)
        {
            this.Id = entity.Id;
            this.Order = entity.Order;
            this.Name = entity.Name;
            this.JoiningDate = entity.JoiningDate;
            this.DepartmentStr = entity.Department?.Name;
            this.JobTitleStr = entity.JobTitle?.Name;
            this.Contract = entity.Contract;    
            this.EmployeeCode = entity.EmployeeCode;
            this.Email = entity.Email;
            this.PhoneNumber = entity.PhoneNumber;
            this.Gender = entity.Gender;
            this.Address = entity.Address;
            this.BirthDay = entity.Birthday;
            this.Avatar = entity.Avatar;
            this.EmployeeMachineCode = entity.EmployeeMachineCode;
            this.EmployeeStatus = entity.EmployeeStatus;
            return this;
        }
    }
}
