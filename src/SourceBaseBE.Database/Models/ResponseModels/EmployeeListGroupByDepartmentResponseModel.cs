using SourceBaseBE.Database.Entities;
using iSoft.Common.Enums;
using SourceBaseBE.Database.Enums;
using Newtonsoft.Json;
using PRPO.Database.Helpers;
using Microsoft.EntityFrameworkCore;
using iSoft.Common.Models.ResponseModels;
using System.Linq.Dynamic.Core;
using SourceBaseBE.Database.Models.RequestModels;
using MathNet.Numerics;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;

namespace SourceBaseBE.Database.Models.ResponseModels
{
    public class EmployeeListGroupByDepartmentResponseModel
    {
        [DisplayName("Department")]
        [JsonProperty("department")]
        public string? Department { get; set; }

        [DisplayName("DepartmentId")]   
        [JsonProperty("departmentid")]
        public long? DepartmentId { get; set; }

        [DisplayName("TotalRecord")]
        [JsonProperty("totalrecord")]
        public long? TotalRecord { get; set; }

        [DisplayName("EmployeeList")]
        [JsonProperty("employeelist")]
        public List<EmployeeDepartmentResponseModel>? EmployeeList { get; set; }

        [DisplayName("Counts")]
        [JsonProperty("counts")]
        public List<CountResponseModel>? Counts { get; set; }
    }

    public class EmployeeDepartmentResponseModel
    {
        [DisplayName("Name")]
        [JsonProperty("name")]
        public string? Name { get; set; }

        [DisplayName("Department")]
        [JsonProperty("department")]
        public string? Department { get; set; }

        [DisplayName("DepartmentId")]
        [JsonProperty("departmentId")]
        public long? DepartmentId { get; set; }

        [JsonProperty("status")]
        [DisplayName("Status")]
        public string? Status { get; set; }

        [JsonProperty("avatar")]
        [DisplayName("Avatar")]
        public string? Avatar { get; set; }

        public EmployeeDepartmentResponseModel SetData(WorkingDayEntity entity)
        {
            if (entity == null) throw new ArgumentNullException("Param Entity EmployeeDepartmentResponseModel Null");
            return this;
        }

        public static List<ColumnResponseModel> AddKeySearchFilterable(List<ColumnResponseModel> datas)
        {
            var properties = JsonPropertyHelper<EmployeeDepartmentResponseModel>.GetJsonPropertyNames();

            foreach (var data in datas)
            {
                if (properties.Contains(data.Key))
                {
                    string key = data.Key.ToLower();
                    //* add flag_filterable
                    if (key == nameof(EmployeeDepartmentResponseModel.Department).ToLower()
                    || key == nameof(EmployeeDepartmentResponseModel.Status).ToLower()
                      )
                        data.Filterable = true;
                    //* add flag_searchable
                    if (key == nameof(EmployeeDepartmentResponseModel.Name).ToLower()
                      || key == nameof(EmployeeDepartmentResponseModel.Department).ToLower()
                      )
                        data.Searchable = true;
                }
            }

            return datas;
        }

        //* prepare query list with where clause
        public static IQueryable<WorkingDayEntity> PrepareWhereQueryFilter(IQueryable<WorkingDayEntity> query, Dictionary<string, object> param)
        {
            var properties = JsonPropertyHelper<EmployeeDepartmentResponseModel>.GetJsonPropertyNames();
            properties.RemoveAll(p => p == null);

            foreach (var property in properties)
            {
                string key = property.ToLower();
                if (param.ContainsKey(key))
                {
                    if (key == "department")
                    {
                        query = query.Where(x => x.Employee.DepartmentId == long.Parse(param[key].ToString()));
                    } 
                }
            }
            return query;
        }

        public static IQueryable<WorkingDayEntity> PrepareWhereQueryFilterStateStatus(IQueryable<WorkingDayEntity> query, Dictionary<string, object> param)
        {
            var properties = JsonPropertyHelper<EmployeeDepartmentResponseModel>.GetJsonPropertyNames();
            properties.RemoveAll(p => p == null);

            foreach (var property in properties)
            {
                string key = property.ToLower();
                if (param.ContainsKey(key))
                {
                    if (key == "status")
                    {
                        var value = (EnumInOutTypeStatus)(int.Parse(param[key].ToString()));
                        query = query.Where(x => x.InOutState == value);
                    }
                }
            }
            return query;
        }

        //* prepare query list with where clause
        public static IQueryable<EmployeeEntity> PrepareWhereQueryFilter(IQueryable<EmployeeEntity> query, Dictionary<string, object> param)
        {
            var properties = JsonPropertyHelper<EmployeeEntity>.GetJsonPropertyNames();
            var predicate = LinqKit.PredicateBuilder.New<EmployeeEntity>(true); // Sử dụng thư viện linqkit
            foreach (var property in param)
            {
                string key = property.Key.ToLower();
                if (param.ContainsKey(key))
                {
                    if (key == "department")
                    {
                        var value = param[key].ToString()?.Split(",");
                        var predicateDepartment = LinqKit.PredicateBuilder.New<EmployeeEntity>(true); // Sử dụng thư viện linqkit
                        if (value.Length > 0)
                        {
                            foreach (var val in value)
                            {
                                predicateDepartment.Or(x => x.DepartmentId == long.Parse(val));
                            }
                        }
                        predicate.And(predicateDepartment);
                    }
                }
            }
            return query.Where(predicate).AsQueryable();
        }
        public static IQueryable<WorkingDayEntity> PrepareWhereQuerySearch(IQueryable<WorkingDayEntity> query,
            SearchModel searchModel)
        {
            var predicate = LinqKit.PredicateBuilder.New<WorkingDayEntity>(true); // Sử dụng thư viện linqkit
            var dicSearch = searchModel.DicSearch;
            var searchKey = searchModel.SearchStr?.Trim();
            foreach (var search in dicSearch)
            {
                var key = search.Key.ToLower();
                var searchValue = dicSearch[key].ToString().Trim();
                if (key == "department")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.Employee.Department.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                }
                else if (key == "name")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.Employee.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                }
                if (key == "all")
                {
                    predicate = predicate.Or(x => EF.Functions.Unaccent(x.Employee.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                    predicate = predicate.Or(x => EF.Functions.Unaccent(x.Employee.Department.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));

                }
            }
            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.Employee.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.Employee.Department.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
            }
            return query.Where(predicate);
        }
        public static IQueryable<WorkingDayEntity> PrepareQuerySort(IQueryable<WorkingDayEntity> query, Dictionary<string, long> param)
        {
            var properties = JsonPropertyHelper<EmployeeDepartmentResponseModel>.GetJsonPropertyNames();
            properties.RemoveAll(p => p == null);
            foreach (var pa in param)
            {
                if (pa.Key == "department")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.Employee.Department.Name) : query.OrderBy(x => x.Employee.Department.Name);
                }
                else if (pa.Key == "name")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.Employee.Name) : query.OrderBy(x => x.Employee.Name);
                }
                else if (pa.Key == "status")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.InOutState) : query.OrderBy(x => x.InOutState);
                }
            }
            return query;
        }

        public static IQueryable<EmployeeEntity> PrepareWhereQuerySearch(IQueryable<EmployeeEntity> query, SearchModel searchModel)
        {
            var predicate = LinqKit.PredicateBuilder.New<EmployeeEntity>(true); // Sử dụng thư viện linqkit
            var dicSearch = searchModel.DicSearch;
            var searchKey = searchModel.SearchStr?.Trim();
            foreach (var search in dicSearch)
            {
                string key = search.Key.ToLower();
                var searchValue = dicSearch[key].Trim();
                if (key == "department")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.Department.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                }
                else if (key == "name")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                }
                if (key == "all")
                {
                    predicate = predicate.Or(x => EF.Functions.Unaccent(x.Department.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                    predicate = predicate.Or(x => EF.Functions.Unaccent(x.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                }
            }
            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.Department.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
            }
            return query.Where(predicate).AsQueryable();
        }
        public static IQueryable<EmployeeEntity> PrepareQuerySort(IQueryable<EmployeeEntity> query, Dictionary<string, long> param)
        {
            var properties = JsonPropertyHelper<EmployeeDepartmentResponseModel>.GetJsonPropertyNames();
            properties.RemoveAll(p => p == null);
            foreach (var pa in param)
            {
                if (pa.Key == "department")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.Department.Name) : query.OrderBy(x => x.Department.Name);
                }
                else if (pa.Key == "name")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
                }
            }
            return query;
        }

    }
}
