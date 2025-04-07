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
    public class DepartmentAdminListResponseModel
    {
        //[DisplayName("Id")]
        [JsonProperty("id")]
        public long? Id { get; set; }

        [DisplayName("Name")]
        [JsonProperty("name")]
        public string? Name { get; set; }
        [DisplayName("Employee Code")]
        [JsonProperty("employeecode")]
        public string? EmployeeCode { get; set; }
        [DisplayName("Phone Number")]
        [JsonProperty("phone")]
        public string? Phone { get; set; }
        [DisplayName("Department")]
        [JsonProperty("department")]
        public string? Department { get; set; }

        [JsonProperty("jobtitle")]
        [DisplayName("jobTitle")]
        public string? JobTitle { get; set; }

        [JsonProperty("email")]
        [DisplayName("Email")]
        public string? Email { get; set; }

        [JsonProperty("role")]
        [DisplayName("role")]
        public string? Role { get; set; }

        [JsonProperty("userid")]
        public long? UserId { get; set; }

        [JsonProperty("departmentid")]
        public long? DepartmentId { get; set; }



        public DepartmentAdminListResponseModel SetData(DepartmentAdminEntity entity)
        {
            if (entity == null) throw new ArgumentNullException("Param Entity DashboardResponseModel Null");
            this.Name = entity?.User?.Username;
            this.EmployeeCode = entity?.User?.ItemEmployee?.EmployeeCode;
            this.Phone = entity?.User?.PhoneNumber;
            this.Department = entity?.Department?.Name;
            this.JobTitle = entity?.User?.ItemEmployee?.JobTitle?.Name;
            this.Id = entity.Id;
            this.Email = entity?.User?.Email;
            this.Role = entity?.Role.ToString();
            this.UserId = entity?.UserId;
            this.DepartmentId = entity?.DepartmentId;
            return this;
        }

        public static List<ColumnResponseModel> AddKeySearchFilterable(List<ColumnResponseModel> datas)
        {
            var properties = JsonPropertyHelper<DepartmentAdminListResponseModel>.GetJsonPropertyNames();

            foreach (var data in datas)
            {
                if (properties.Contains(data.Key))
                {
                    string key = data.Key.ToLower();
                    //* add flag_filterable
                    if (key == nameof(DepartmentAdminListResponseModel.JobTitle).ToLower()
                        || (key == nameof(DepartmentAdminListResponseModel.Role).ToLower()))

                        data.Filterable = true;
                    //* add flag_searchable
                    if (key == nameof(DepartmentAdminListResponseModel.Name).ToLower()
                      || key == nameof(DepartmentAdminListResponseModel.JobTitle).ToLower()
            || key == nameof(DepartmentAdminListResponseModel.EmployeeCode).ToLower())
                        data.Searchable = true;
                }
            }

            return datas;
        }

        //* prepare query list with where clause
        public static IQueryable<DepartmentAdminEntity> PrepareWhereQueryFilter(IQueryable<DepartmentAdminEntity> query, Dictionary<string, object> param)
        {
            var properties = JsonPropertyHelper<DepartmentAdminListResponseModel>.GetJsonPropertyNames();
            properties.RemoveAll(p => p == null);

            var predicate = LinqKit.PredicateBuilder.New<DepartmentAdminEntity>(true); // Sử dụng thư viện linqkit
            foreach (var property in properties)
            {
                string key = property.ToLower();
                if (param.ContainsKey(key))
                {
                    if (key == "jobtitle")
                    {
                        query = query.Where(x => x.User.ItemEmployee.JobTitleId == long.Parse(param[key].ToString()));
                    }
                    else if (key == "role")
                    {
                        EnumDepartmentAdmin value = (EnumDepartmentAdmin)(int.Parse(param[key].ToString()));
                        query = query.Where(x => x.Role == value);
                    }
                }
            }
            return query.Where(predicate);
        }

        public static IQueryable<DepartmentAdminEntity> PrepareWhereQuerySearch(IQueryable<DepartmentAdminEntity> query, SearchModel searchModel)
        {
            var predicate = LinqKit.PredicateBuilder.New<DepartmentAdminEntity>(true); // Sử dụng thư viện linqkit
            var dicSearch = searchModel.DicSearch;
            var searchKey = searchModel.SearchStr?.Trim()?.ToLower();
            foreach (var search in dicSearch)
            {
                string key = search.Key.ToLower();
                var searchVal = dicSearch[key].Trim().ToLower();
                if (key == "employeecode")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.User.ItemEmployee.EmployeeCode.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                }
                else if (key == "jobtitle")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.User.ItemEmployee.JobTitle.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                }
                else if (key == "name")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.User.Username.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                }

                if (key == "all")
                {
                    predicate = predicate.Or(x => EF.Functions.Unaccent(x.User.ItemEmployee.EmployeeCode.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                    predicate = predicate.Or(x => EF.Functions.Unaccent(x.User.ItemEmployee.JobTitle.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                    predicate = predicate.Or(x => EF.Functions.Unaccent(x.User.Username.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                }
            }
            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.User.ItemEmployee.JobTitle.Name).Contains(EF.Functions.Unaccent($"{searchKey}")));
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.User.Username).Contains(EF.Functions.Unaccent($"{searchKey}")));
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.User.ItemEmployee.EmployeeCode).Contains(EF.Functions.Unaccent($"{searchKey}")));
            }
            return query.Where(predicate).AsQueryable();
        }

        public static IQueryable<DepartmentAdminEntity> PrepareQuerySort(IQueryable<DepartmentAdminEntity> query, Dictionary<string, long> param)
        {
            var properties = JsonPropertyHelper<DepartmentAdminListResponseModel>.GetJsonPropertyNames();
            properties.RemoveAll(p => p == null);
            foreach (var pa in param)
            {
                if (pa.Key == "department")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.Department.Name) : query.OrderBy(x => x.Department.Name);
                }
                else if (pa.Key == "jobtitle")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.User.ItemEmployee.JobTitle.Name) : query.OrderBy(x => x.User.ItemEmployee.JobTitle.Name);
                }

                else if (pa.Key == "employeecode")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.User.ItemEmployee.EmployeeCode) : query.OrderBy(x => x.User.ItemEmployee.EmployeeCode);
                }
                else if (pa.Key == "name")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.User.Username) : query.OrderBy(x => x.User.Username);
                }
            }
            return query;
        }

    }

}
