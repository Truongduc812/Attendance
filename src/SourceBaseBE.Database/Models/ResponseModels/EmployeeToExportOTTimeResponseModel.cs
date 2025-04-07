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
using SourceBaseBE.Database.Attribute;

namespace SourceBaseBE.Database.Models.ResponseModels
{
    public class EmployeeToExportOTTimeResponseModel
    {
        [JsonProperty("Id")]
        public long? Id { get; set; }
        //[JsonProperty("name")]
        //[Filterable("Name", "name", true)]
        //public string? Name { get; set; }
        //[JsonProperty("employeecode")]
        //[Filterable("Employee Code", "employeecode", true)]
        //public string? EmployeeCode { get; set; }
        [JsonProperty("department")]
        [Filterable("Department", "department", true)]
        public string? Department { get; set; }
        [JsonProperty("jobtitle")]
        [Filterable("Job Title", "jobtitle", true)]
        public string? JobTitle { get; set; }
        //[JsonProperty("date")]
        //[DisplayName("Date")]
        //public string? Date { get; set; }
        //[DisplayName("Time in")]
        //[JsonProperty("timein")]
        //public string? TimeIn { get; set; }
        //[DisplayName("Time out")]
        //[JsonProperty("timeout")]
        //public string? TimeOut { get; set; }
        //[DisplayName("Time Deviation")]
        //[JsonProperty("timedeviation")]
        //public double? TimeDeviation { get; set; }
        //[JsonProperty("workingDayHighlight")]
        //public EnumWorkingDayHighlight? WorkingDayHighlight { get; set; }
        //[JsonProperty("status")]
        ////[DisplayName("Status")]
        //public string? Status { get; set; }
        [DisplayName("Type")]
        [JsonProperty("type")]
        public string? Type { get; set; }
        [JsonProperty("isUsingRecommend")]
        public bool isUsingRecommend { get; set; }
        [JsonProperty("RecommendType")]
        public string? RecommendType { get; set; }
        [JsonProperty("recommendTypeId")]
        public long? recommendTypeId { get; set; }
        [DisplayName("Note")]
        [JsonProperty("notes")]
        //public string? Notes { get; set; }
        //[JsonProperty("employeeid")]
        public long? EmployeeId { get; set; }
        [JsonProperty("list_workingdayupdateid")]
        public long[] ListWorkingDayUpdateId { get; set; }

        //* prepare query list with where clause
        public static IQueryable<EmployeeEntity> PrepareWhereQueryFilter(IQueryable<EmployeeEntity> query, 
                                                                         Dictionary<string, object> param, 
                                                                         List<WorkingTypeEntity> listWorkingTypeOT)
        {
            var properties = JsonPropertyHelper<EmployeeToExportOTTimeResponseModel>.GetJsonPropertyNames();
            properties.RemoveAll(p => p == null);

            var predicate = LinqKit.PredicateBuilder.New<EmployeeEntity>(true); // Sử dụng thư viện linqkit
            foreach (var property in properties)
            {
                string key = property.ToLower().Trim();
                if (param.ContainsKey(key))
                {
                    if (key == "jobtitle")
                    {
                        query = query.Where(x => x.JobTitleId == long.Parse(param[key].ToString()));
                    }
                    else if (key == "contract")
                    {
                        var value = param[key].ToString()?.Split(",");
                        var predicateStatus = LinqKit.PredicateBuilder.New<EmployeeEntity>(true); // Sử dụng thư viện linqkit
                        if (value.Length > 0)
                        {
                            foreach (var val in value)
                            {
                                predicateStatus.Or(x => x.Contract.Value == Enum.Parse<EnumTypeContract>(val));
                            }
                        }
                        predicate.And(predicateStatus);
                    }
                    else if (key == "type")
                    {
                        if (param[key].ToString() == "0")
                        {
                            query = query.Where(x => x.WorkingDayEntitys.Any(x => x.WorkingType != null && listWorkingTypeOT.Contains(x.WorkingType)));
                        }
                        else if (param[key].ToString() == "1")
                        {
                            query = query.Where(x => x.WorkingDayEntitys.Any(x => x.WorkingType == null && listWorkingTypeOT.Select(x => x.Code).Contains(x.RecommendType)));
                        }
                    }
                    //if (key == "department")
                    //{
                    //    var value = param[key].ToString()?.Split(",");
                    //    if (value.Length > 0)
                    //    {
                    //        foreach (var val in value)
                    //        {
                    //            query.Where(x => x.DepartmentId == long.Parse(val));
                    //        }
                    //    }
                    //}
                }
            }
            return query.Where(predicate);
        }

        public static IQueryable<EmployeeEntity> PrepareWhereQuerySearch(IQueryable<EmployeeEntity> query, SearchModel searchModel)
        {
            var predicate = LinqKit.PredicateBuilder.New<EmployeeEntity>(true); // Sử dụng thư viện linqkit
            var dicSearch = searchModel.DicSearch;
            var searchKey = searchModel.SearchStr?.Trim()?.ToLower();
            foreach (var search in dicSearch)
            {
                string key = search.Key.ToLower();
                var searchVal = dicSearch[key].Trim();
                if (key == "name")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                }
                else if (key == "employeecode")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.EmployeeCode.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                }
                else if (key == "employeemachinecode")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.EmployeeMachineCode.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                }
                else if (key == "jobtitle")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.JobTitle.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                }
                else if (key == "phone")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.PhoneNumber.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                }
                if (key == "all")
                {
                    predicate = predicate.Or(x => EF.Functions.Unaccent(x.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal.ToLower()}")));
                    predicate = predicate.Or(x => EF.Functions.Unaccent(x.EmployeeCode.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal.ToLower()}")));
                    predicate = predicate.Or(x => EF.Functions.Unaccent(x.EmployeeMachineCode.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal.ToLower()}")));
                    predicate = predicate.Or(x => EF.Functions.Unaccent(x.PhoneNumber.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal.ToLower()}")));
                    predicate = predicate.Or(x => EF.Functions.Unaccent(x.Department.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal.ToLower()}")));
                    predicate = predicate.Or(x => EF.Functions.Unaccent(x.JobTitle.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal.ToLower()}")));
                }
            }
            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey.ToLower()}")));
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.EmployeeCode.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey.ToLower()}")));
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.EmployeeMachineCode.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey.ToLower()}")));
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.PhoneNumber.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey.ToLower()}")));
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.Department.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey.ToLower()}")));
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.JobTitle.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey.ToLower()}")));
            }
            return query.Where(predicate).AsQueryable();
        }

        public static IQueryable<EmployeeEntity> PrepareQuerySort(IQueryable<EmployeeEntity> query, Dictionary<string, long> param)
        {
            var properties = JsonPropertyHelper<DepartmentListEmployeeResponseModel>.GetJsonPropertyNames();
            properties.RemoveAll(p => p == null);
            foreach (var pa in param)
            {
                if (pa.Key == "department")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.Department.Name) : query.OrderBy(x => x.Department.Name);
                }
                else if (pa.Key == "jobtitle")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.JobTitle.Name) : query.OrderBy(x => x.JobTitle.Name);
                }
                else if (pa.Key == "employeecode")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.EmployeeCode) : query.OrderBy(x => x.EmployeeCode);
                }
                else if (pa.Key == "employeemachinecode")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.EmployeeMachineCode) : query.OrderBy(x => x.EmployeeMachineCode);
                }
                else if (pa.Key == "name")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
                }
                else if (pa.Key == "phone")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.PhoneNumber) : query.OrderBy(x => x.PhoneNumber);
                }
                else if (pa.Key == "contract")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.Contract) : query.OrderBy(x => x.Contract);
                }
            }
            return query;
        }

    }

}
