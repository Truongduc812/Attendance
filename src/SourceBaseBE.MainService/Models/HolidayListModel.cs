using iSoft.Common.Enums;
using NPOI.SS.Formula.Functions;
using SourceBaseBE.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SourceBaseBE.MainService.Models
{
    public class HolidayListModel
    {
        public string Country { get; set; }
        public string Date { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public HolidayListModel SetData(HolidayScheduleEntity entity, DateTime date)
        {
            this.Country = "VNM";
            this.Date = date.ToString("MM/dd/yyyy");
            this.Name = entity.Name;
            this.Code = entity.HolidayType?.ToString();
            return this;
        }
    }

    public class EmployeeListmodel
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public string JobTitle { get; set; }
        public string Birtday { get; set; }
        public string Phone { get; set; }
        public string JoinDate { get; set; }
        public string Ward { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Address { get; set; }

        private int _stt; // Private field to store STT

        public int STT
        {
            get { return _stt; }
            set { _stt = value; }
        }
        //int count { get; set; }
        public EmployeeListmodel SetSTT(int stt)
        {
            STT = stt;
            return this;
        }

        public EmployeeListmodel SetData(EmployeeEntity entity, int count)
        {
            this.EmployeeCode = entity.EmployeeCode;
            this.EmployeeName = entity.Name?.ToString();
            this.Department = entity.Department?.Name;
            this.JobTitle = entity.JobTitle?.Name.ToString();
            this.Birtday = entity.Birthday?.ToString("MM/dd/yyyy");
            this.Phone = entity.PhoneNumber;
            this.JoinDate = entity.JoiningDate?.ToString("MM/dd/yyyy");

            string[] pairs = null;
            if (entity.Address != null)
            {
                pairs = entity.Address.Split(",");
            }
            if (pairs != null && pairs.Length >= 3)
            {

                this.Ward = pairs[pairs.Length - 3];
                this.City = pairs[pairs.Length - 2];
                this.Province = pairs[pairs.Length - 1];

            }
            this.Address = entity.Address;
            this.STT = count;
            return this;
        }

        public class DepartmentAdminListmodel
        {
            public string Department { get; set; } = "";
            public string Admin1 { get; set; } = "";
            public string Admin2 { get; set; } = "";
            public string Admin3 { get; set; } = "";

            private int _stt; // Private field to store STT

            public int STT
            {
                get { return _stt; }
                set { _stt = value; }
            }
            //int count { get; set; }
            public DepartmentAdminListmodel SetSTT(int stt)
            {
                STT = stt;
                return this;
            }

            public DepartmentAdminListmodel SetData(DepartmentAdminEntity entity, int count)
            {
                this.STT = count;
                this.Department = entity.Department?.Name;
                switch (entity.Role)
                {
                    case EnumDepartmentAdmin.Admin1:
                        this.Admin1 = entity.User == null ? "" : entity.User?.ItemEmployee?.EmployeeCode;
                        break;
                    case EnumDepartmentAdmin.Admin2:
                        this.Admin2 = entity.User == null ? "" : entity.User?.ItemEmployee?.EmployeeCode;
                        break;
                    case EnumDepartmentAdmin.Admin3:
                        this.Admin3 = entity.User == null ? "" : entity.User?.ItemEmployee?.EmployeeCode;
                        break;
                    default:
                        break;
                }
                return this;
            }
            public static List<DepartmentAdminListmodel> SetData(List<DepartmentAdminEntity> entity)
            {
                var ret = new List<DepartmentAdminListmodel>();
                int count = 1;
                var newDepartmentADmin = new List<DepartmentAdminListmodel>();
                var groupByDepartment = entity.GroupBy(x => x.Department.Id);
                foreach (var departmentAdmins in groupByDepartment)
                {
                    var admins1 = departmentAdmins.Where(x => x.Role == EnumDepartmentAdmin.Admin1 && x.UserId != null).ToList();
                    var admins2 = departmentAdmins.Where(x => x.Role == EnumDepartmentAdmin.Admin2 && x.UserId != null).ToList();
                    var admins3 = departmentAdmins.Where(x => x.Role == EnumDepartmentAdmin.Admin3 && x.UserId != null).ToList();
                    var maximumCount = admins1.Count > admins2.Count ? admins1.Count : admins2.Count > admins3.Count ? admins2.Count : admins3.Count;

                    for (int i = 0; i < maximumCount; i++)
                    {
                        newDepartmentADmin.Add(new DepartmentAdminListmodel()
                        {
                            Admin1 = admins1.Count > i
                            ? (admins1[i].User?.ItemEmployee != null ? $"Mã NV: {admins1[i].User?.ItemEmployee?.EmployeeCode}" : $"Account: {admins1[i].User?.Username}")
                            : "",
                            Admin2 = admins2.Count > i
                           ? (admins2[i].User?.ItemEmployee != null ? $"Mã NV: {admins2[i].User?.ItemEmployee?.EmployeeCode}" : $"Account: {admins2[i].User?.Username}")
                            : "",
                            Admin3 = admins3.Count > i
                            ? (admins3[i].User?.ItemEmployee != null ? $"Mã NV: {admins3[i].User?.ItemEmployee?.EmployeeCode}" : $"Account: {admins3[i].User?.Username}")
                            : "",
                            STT = count++,
                            Department = departmentAdmins.FirstOrDefault()?.Department?.Name
                        });
                    }
                }
                ret.AddRange(newDepartmentADmin);
                return ret;
            }
        }
    }
}
