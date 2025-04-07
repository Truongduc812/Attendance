using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using iSoft.Database.Entities;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using iSoft.Database.Extensions;
using SourceBaseBE.Database.Enums;
using iSoft.Common.Utils;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Entities
{
    [Table("Employees")]
    public class EmployeeEntity : BaseCRUDEntity
    {
        [Required]
        [Column("employee_code")]
        [FormDataType("Employee Code", iSoft.Common.Enums.EnumFormDataType.textbox, true)]
        public string? EmployeeCode { get; set; }

        [Column("employee_machine_code")]
        [Required]
        [Browsable(false)]
        [FormDataType("Employee Machine Code", iSoft.Common.Enums.EnumFormDataType.textbox, true)]
        public string EmployeeMachineCode { get; set; }

        [FormDataType("Name", iSoft.Common.Enums.EnumFormDataType.textbox, true)]
        [Column("name")]
        public string? Name { get; set; }

        [FormDataType("Contract", iSoft.Common.Enums.EnumFormDataType.select, true)]
        [Column("contract")]
        public EnumTypeContract? Contract { get; set; }

        [Column("full_name")]
        public string? FullName { get; set; }

        [Column("first_name")]
        public string? FirstName { get; set; }

        [Column("middle_name")]
        public string? MiddleName { get; set; }

        [Column("last_name")]
        public string? LastName { get; set; }

        [Column("display_name")]
        public string? DisplayName { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("phone_number")]
        [FormDataType("Phone Number", iSoft.Common.Enums.EnumFormDataType.textbox, false)]
        public string? PhoneNumber { get; set; }

        [Column("gender")]
        [FormDataType("Gender", iSoft.Common.Enums.EnumFormDataType.select, false)]
        public iSoft.Common.Enums.EnumGender? Gender { get; set; }

        [Column("address")]
        [FormDataType("Address", iSoft.Common.Enums.EnumFormDataType.textbox, false)]
        public string? Address { get; set; }
        [FormDataType("Birthday", iSoft.Common.Enums.EnumFormDataType.dateOnly, false)]

        [Column("birdthday")]
        public DateTime? Birthday { get; set; }
        [Column("joining_date")]
        [FormDataType("Joining Date", iSoft.Common.Enums.EnumFormDataType.dateOnly, false)]
        public DateTime? JoiningDate { get; set; }

        [Column("avatar")]
        [FormDataTypeAttribute(EnumFormDataType.image, false, "120px", "120px", 1.0)]
        [StringLength(255)]
        public string? Avatar { get; set; }

        [ForeignKey(nameof(DepartmentEntity))]
        [FormDataType("Department", iSoft.Common.Enums.EnumFormDataType.select, true)]
        public long? DepartmentId { get; set; }
        public DepartmentEntity? Department { get; set; }

        [ForeignKey(nameof(JobTitleEntity))]
        [FormDataType("JobTitle", iSoft.Common.Enums.EnumFormDataType.select, true)]
        public long? JobTitleId { get; set; }
        public JobTitleEntity? JobTitle { get; set; }

        [Column("employee_status")]
        public EnumEmployeeStatus? EmployeeStatus { get; set; }

        public ICollection<TimeSheetEntity>? ListTimeSheets { get; set; }
        public ICollection<WorkingDayEntity>? WorkingDayEntitys { get; set; }

        public string GetShowName()
        {
            if (!this.FullName.IsNullOrEmpty())
            {
                return this.FullName;
            }
            if (!this.Name.IsNullOrEmpty())
            {
                return this.Name;
            }
            if (!this.DisplayName.IsNullOrEmpty())
            {
                return this.DisplayName;
            }
            return this.EmployeeCode;
        }

        public override void SetFileURL(Dictionary<string, string> dicImagePath)
        {
            if (dicImagePath.ContainsKey(nameof(Avatar)))
            {
                this.Avatar = dicImagePath[nameof(Avatar)];
            }
        }
    }
}