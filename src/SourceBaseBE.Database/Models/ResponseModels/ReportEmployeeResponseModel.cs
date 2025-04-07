using iSoft.Common.Models.ResponseModels;
using Newtonsoft.Json;
using PRPO.Database.Helpers;
using SourceBaseBE.Database.Entities;
using SourceBaseBE.Database.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.Models.ResponseModels
{
    public class EmployeeListAttendanceResponseModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [DisplayName("Name")]
        [JsonProperty("name")]
        public string? Name { get; set; }
        [DisplayName("Employee Code")]

        [JsonProperty("employeecode")]
        public string? EmployeeCode { get; set; }
        [DisplayName("Employee Machine Code")]

        [JsonProperty("employeemachinecode")]
        public string? EmployeeMachineCode { get; set; }
        [DisplayName("Contract")]
        [JsonProperty("contract")]
        public string? Contract { get; set; }

        [DisplayName("Department")]

        [JsonProperty("department")]
        public string? Department { get; set; }
        [DisplayName("Job Title")]
        [JsonProperty("jobtitle")]
        public string? JobTitle { get; set; }
        [DisplayName("Phone Number")]
        [JsonProperty("phone")]
        public string? Phone { get; set; }
        [DisplayName("Email")]
        [JsonProperty("email")]
        public string? Email { get; set; }

        //* prepare query list with where clause
        public EmployeeListAttendanceResponseModel SetData(WorkingDayEntity entity)
        {
            this.Department = entity.Employee?.Department?.Name;
            this.JobTitle = entity.Employee?.JobTitle?.Name;
            this.EmployeeCode = entity.Employee.EmployeeCode;
            this.EmployeeMachineCode = entity.Employee.EmployeeMachineCode;
            this.Email = entity.Employee.Email;
            this.Name = entity.Employee.Name;
            this.Phone = entity.Employee.PhoneNumber;
            this.Id = entity.EmployeeEntityId.GetValueOrDefault();
            return this;
        }
        public EmployeeListAttendanceResponseModel SetData(EmployeeEntity entity)
        {
            this.EmployeeCode = entity.EmployeeCode;
            this.EmployeeMachineCode = entity.EmployeeMachineCode;
            this.Department = entity.Department?.Name;
            this.JobTitle = entity.JobTitle?.Name;
            this.Email = entity.Email;
            this.Name = entity.Name;
            this.Contract = entity.Contract.ToString();
            this.Phone = entity.PhoneNumber;
            this.Id = entity.Id;
            return this;
        }
        public static List<ColumnResponseModel> AddKeySearchFilterable(List<ColumnResponseModel> datas)
        {
            var properties = JsonPropertyHelper<EmployeeListAttendanceResponseModel>.GetJsonPropertyNames();

            foreach (var data in datas)
            {
                if (properties.Contains(data.Key))
                {
                    string key = data.Key.ToLower();
                    //* add flag_filterable
                    if (key == nameof(EmployeeListAttendanceResponseModel.JobTitle).ToLower()
                    || key == nameof(EmployeeListAttendanceResponseModel.Department).ToLower()
                    || key == nameof(EmployeeListAttendanceResponseModel.Contract).ToLower()
                        )
                        data.Filterable = true;
                    //* add flag_searchable
                    if (key == nameof(DashboardResponseModel.Name).ToLower()
                    || key == nameof(DashboardResponseModel.JobTitle).ToLower()
                    || key == nameof(EmployeeResponseModel.EmployeeCode).ToLower()
                    || key == nameof(EmployeeResponseModel.EmployeeMachineCode).ToLower()
                    || key == "phone"
                    || key == "email"
                    || key == nameof(DashboardResponseModel.Department).ToLower()
            )
                        data.Searchable = true;
                }
            }

            return datas;
        }
        public static IQueryable<EmployeeEntity> PrepareWhereQueryFilter(IQueryable<EmployeeEntity> query, Dictionary<string, object> param)
        {
            var properties = JsonPropertyHelper<EmployeeListAttendanceResponseModel>.GetJsonPropertyNames();
            properties.RemoveAll(p => p == null);

            var predicate = LinqKit.PredicateBuilder.New<EmployeeEntity>(true); // Sử dụng thư viện linqkit
            foreach (var property in properties)
            {
                string key = property.ToLower();

                if (param.ContainsKey(key))
                {
                    predicate.Or(x => x.GetType().GetProperty(key).GetValue(x).Equals(param[key]));
                }
            }
            return query.AsEnumerable().Where(predicate).AsQueryable();
        }

    }
}

