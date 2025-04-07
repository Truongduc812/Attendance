using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;
using SourceBaseBE.Database.Enums;
using Newtonsoft.Json;
using PRPO.Database.Helpers;
using PRPO.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using iSoft.Common.Models.ResponseModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using System.Linq.Dynamic.Core;
using MathNet.Numerics.LinearAlgebra;
using System;
using LinqKit;
using iSoft.Common.Enums;
using SourceBaseBE.Database.Models.RequestModels;
using MathNet.Numerics;
using System.ComponentModel;

namespace SourceBaseBE.Database.Models.ResponseModels
{
    public class WorkingdayUpdateDTO
    {
        [JsonProperty("employeeid")]
        public long? EmployeeId { get; set; }
        [JsonProperty("employeecode")]

        [DisplayName("Employee Code")]
        public string? EmployeeCode { get; set; }

        [DisplayName("Employee Name")]
        [JsonProperty("employeename")]
        public string? EmployeeName { get; set; }
        [DisplayName("Department")]
        [JsonProperty("department")]
        public string? Department { get; set; }
        [JsonProperty("jobtitle")]
        [DisplayName("JobTitle")]
        public string? JobTitle { get; set; }

        [JsonProperty("nor")]
        [DisplayName("Number Of Records")]
        public long? NoR { get; set; }
        [JsonProperty("approved")]
        [DisplayName("Approved")]
        public long? Aprroved { get; set; }
        [JsonProperty("denied")]
        [DisplayName("Denied")]
        public long? Denied { get; set; }
        [JsonProperty("pending")]
        [DisplayName("Pending")]
        public long? Pending { get; set; }

        public static List<WorkingdayUpdateDTO> SetData(List<WorkingDayUpdateEntity> workingDayUpdate, List<EmployeeEntity> employees)
        {
            if (workingDayUpdate == null) throw new ArgumentNullException("WorkingDayApprovalEntity Entity  Null");
            //var groupByEmp = workingDayUpdate.GroupBy(x => x.WorkingDay?.EmployeeEntityId);
            List<WorkingdayUpdateDTO> ret = new List<WorkingdayUpdateDTO>();
            foreach (var employee in employees)
            {
                var empWd = workingDayUpdate.Where(x => x.EmployeeId == employee.Id);
                if (empWd == null || empWd.Count() <= 0)
                {
                    continue;
                }
                var ele = new WorkingdayUpdateDTO()
                {
                    EmployeeId = employee.Id,
                    EmployeeName = employee?.Name,
                    EmployeeCode = employee.EmployeeCode,
                    Department = employee?.Department?.Name,
                    JobTitle = employee?.JobTitle?.Name,
                    NoR = empWd.Count(),
                    Aprroved = empWd.SelectMany(x => x.WorkingDayApprovals)?.Count(x => x != null && x.ApproveStatus == EnumApproveStatus.ACCEPT),
                    Denied = empWd.SelectMany(x => x.WorkingDayApprovals)?.Count(x => x != null && x.ApproveStatus == EnumApproveStatus.REJECT),
                    Pending = empWd.SelectMany(x => x.WorkingDayApprovals)?.Count(x => x != null && x.ApproveStatus == EnumApproveStatus.PENDING),
                };
                ret.Add(ele);
            }
            return ret;
        }
        public static List<ColumnResponseModel> AddKeySearchFilterable(List<ColumnResponseModel> datas)
        {

            foreach (var data in datas)
            {
                string key = data.Key.ToLower();
                //* add flag_filterable
                if (key == nameof(WorkingdayUpdateDTO.JobTitle).ToLower()
                  || key == nameof(WorkingdayUpdateDTO.Department).ToLower()
                    )
                    data.Filterable = true;
                //* add flag_searchable
                if (key == nameof(WorkingdayUpdateDTO.JobTitle).ToLower()
                  || key == nameof(WorkingdayUpdateDTO.Department).ToLower()
                    || key == nameof(WorkingdayUpdateDTO.EmployeeName).ToLower()
                    || key == nameof(WorkingdayUpdateDTO.EmployeeCode).ToLower()
                    )
                    data.Searchable = true;
            }

            return datas;
        }

        //* prepare query list with where clause
        public static IQueryable<WorkingDayUpdateEntity> PrepareWhereQueryFilter(IQueryable<WorkingDayUpdateEntity> query, Dictionary<string, object> param)
        {
            var properties = JsonPropertyHelper<WorkingdayUpdateDTO>.GetJsonPropertyNames();
            properties.RemoveAll(p => p == null);

            var predicate = LinqKit.PredicateBuilder.New<WorkingDayUpdateEntity>(true); // Sử dụng thư viện linqkit
            foreach (var property in properties)
            {
                string key = property.ToLower();
                if (!param.ContainsKey(key)) continue;
                if (key == JsonPropertyHelper<WorkingdayUpdateDTO>.GetJsonPropertyName(nameof(WorkingdayUpdateDTO.JobTitle)))
                {
                    predicate.And(x => x.WorkingDay.Employee.JobTitleId == long.Parse(param[key].ToString()));
                }

                else if (key == JsonPropertyHelper<WorkingdayUpdateDTO>.GetJsonPropertyName(nameof(WorkingdayUpdateDTO.EmployeeName)))
                {
                    predicate.And(x => x.WorkingDay.EmployeeEntityId == long.Parse(param[key].ToString()));
                }
                else if (key == JsonPropertyHelper<WorkingdayUpdateDTO>.GetJsonPropertyName(nameof(WorkingdayUpdateDTO.Department)))
                {
                    predicate.And(x => x.WorkingDay.Employee.DepartmentId == long.Parse(param[key].ToString()));
                }
            }

            return query.Where(predicate);
        }

        public static IQueryable<WorkingDayUpdateEntity> PrepareWhereQuerySearch(IQueryable<WorkingDayUpdateEntity> query, SearchModel searchModel)
        {
            var predicate = LinqKit.PredicateBuilder.New<WorkingDayUpdateEntity>(true); // Sử dụng thư viện linqkit
            var dicSearch = searchModel.DicSearch;
            var searchKey = searchModel.SearchStr?.ToLower().Trim();
            if (dicSearch != null && dicSearch.Count() > 0)
            {
                var properties = JsonPropertyHelper<WorkingdayUpdateDTO>.GetJsonPropertyNames();
                properties.RemoveAll(p => p == null);
                foreach (var property in properties)
                {
                    string key = property.ToLower();
                    if (!dicSearch.ContainsKey(key)) continue;
                    if (dicSearch.ContainsKey(key))
                    {
                        var searchVal = dicSearch[key]?.ToLower()?.Trim();
                        if (key == JsonPropertyHelper<WorkingdayUpdateDTO>.GetJsonPropertyName(nameof(WorkingdayUpdateDTO.JobTitle)))
                        {
                            predicate = predicate.And(x => EF.Functions.Unaccent(x.WorkingDay.Employee.JobTitle.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                        }

                        else if (key == JsonPropertyHelper<WorkingdayUpdateDTO>.GetJsonPropertyName(nameof(WorkingdayUpdateDTO.EmployeeName)))
                        {
                            predicate = predicate.And(x => EF.Functions.Unaccent(x.WorkingDay.Employee.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                        }
                        else if (key == JsonPropertyHelper<WorkingdayUpdateDTO>.GetJsonPropertyName(nameof(WorkingdayUpdateDTO.EmployeeCode)))
                        {
                            predicate = predicate.And(x => EF.Functions.Unaccent(x.WorkingDay.Employee.EmployeeCode.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                        }
                        else if (key == JsonPropertyHelper<WorkingdayUpdateDTO>.GetJsonPropertyName(nameof(WorkingdayUpdateDTO.Department)))
                        {
                            predicate = predicate.And(x => EF.Functions.Unaccent(x.WorkingDay.Employee.Department.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));

                        }
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                predicate = predicate.And(x => EF.Functions.Unaccent(x.WorkingDay.Employee.JobTitle.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
                predicate = predicate.And(x => EF.Functions.Unaccent(x.WorkingDay.Employee.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
                predicate = predicate.And(x => EF.Functions.Unaccent(x.WorkingDay.Employee.EmployeeCode.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
                predicate = predicate.And(x => EF.Functions.Unaccent(x.WorkingDay.Employee.Department.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));

            }
            return query.Where(predicate);
        }

        public static IQueryable<WorkingDayUpdateEntity> PrepareQuerySort(IQueryable<WorkingDayUpdateEntity> query, Dictionary<string, long> param)
        {
            var properties = JsonPropertyHelper<WorkingdayUpdateDTO>.GetJsonPropertyNames();
            properties.RemoveAll(p => p == null);
            foreach (var pa in param)
            {
                query = pa.Value == 1 ? query.OrderBy(x => x.GetType().GetProperty(pa.Key)) : query.OrderByDescending(x => x.GetType().GetProperty(pa.Key));
            }
            return query;
        }

    }

}
