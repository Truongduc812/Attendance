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
    public class UserDepartmentResponseModel
    {
        //[DisplayName("Id")]
        [JsonProperty("userid")]
        public long? Id { get; set; }

        [JsonProperty("departmentid")]
        public long? DepartmentId { get; set; }

        [DisplayName("User Name")]
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
        [DisplayName("JobTitle")]
        public string? JobTitle { get; set; }

        [JsonProperty("email")]
        [DisplayName("Email")]
        public string? Email { get; set; }




        public UserDepartmentResponseModel SetData(UserEntity entity)
        {
            if (entity == null) throw new ArgumentNullException("Param Entity DashboardResponseModel Null");
            this.Name = entity?.Username;
            this.EmployeeCode = entity?.ItemEmployee?.EmployeeCode;
            this.Phone = entity?.PhoneNumber;
            this.Department = entity?.ItemEmployee?.Department?.Name;
            this.JobTitle = entity?.ItemEmployee?.JobTitle?.Name;
            this.Email = entity?.Email;
            this.Id = entity?.Id;
            this.Department = entity?.DepartmentAdmins?.Where(x => x.UserId == entity.Id)?.Where(x => x.DeletedFlag != true).FirstOrDefault()?.Department?.Name;
            return this;
        }

        public static List<ColumnResponseModel> AddKeySearchFilterable(List<ColumnResponseModel> datas)
        {
            var properties = JsonPropertyHelper<UserDepartmentResponseModel>.GetJsonPropertyNames();

            foreach (var data in datas)
            {
                if (properties.Contains(data.Key))
                {
                    string key = data.Key.ToLower();
                    //* add flag_filterable
                    if (key == nameof(UserDepartmentResponseModel.JobTitle).ToLower()
                      || key == nameof(UserDepartmentResponseModel.Department).ToLower())
                        data.Filterable = true;
                    //* add flag_searchable
                    if (key == nameof(UserDepartmentResponseModel.Name).ToLower()
                      || key == nameof(UserDepartmentResponseModel.Department).ToLower()
                      || key == nameof(UserDepartmentResponseModel.JobTitle).ToLower()
                      || key == nameof(UserDepartmentResponseModel.EmployeeCode).ToLower())
                        data.Searchable = true;
                }
            }

            return datas;
        }

        //* prepare query list with where clause
        public static IQueryable<UserEntity> PrepareWhereQueryFilter(IQueryable<UserEntity> query, Dictionary<string, object> param)
        {
            var properties = JsonPropertyHelper<UserDepartmentResponseModel>.GetJsonPropertyNames();
            properties.RemoveAll(p => p == null);

            var predicate = LinqKit.PredicateBuilder.New<UserEntity>(true); // Sử dụng thư viện linqkit
            foreach (var property in properties)
            {
                string key = property.ToLower();
                if (param.ContainsKey(key))
                {
                    if (key == "jobtitle")
                    {
                        query = query.Where(x => x.ItemEmployee.JobTitleId == long.Parse(param[key].ToString()));
                    }
                }
            }
            return query.Where(predicate);
        }

        public static IQueryable<UserEntity> PrepareWhereQuerySearch(IQueryable<UserEntity> query, SearchModel searchModel)
        {
            var predicate = LinqKit.PredicateBuilder.New<UserEntity>(true); // Sử dụng thư viện linqkit
            var dicSearch = searchModel.DicSearch;
            var searchKey = searchModel.SearchStr?.ToLower().Trim();
            if (dicSearch != null && dicSearch.Count() > 0)
            {
                var properties = JsonPropertyHelper<UserDepartmentResponseModel>.GetJsonPropertyNames();
                properties.RemoveAll(p => p == null);
                foreach (var property in properties)
                {
                    string key = property.ToLower();
                    if (dicSearch.ContainsKey(key))
                    {
                        var searchVal = dicSearch[key]?.ToLower()?.Trim();
                        if (key == "name")
                        {
                            predicate = predicate.And(x => EF.Functions.Unaccent(x.ItemEmployee.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                        }
                        else if (key == "employeecode")
                        {
                            predicate = predicate.And(x => EF.Functions.Unaccent(x.ItemEmployee.EmployeeCode.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                        }
                        else if (key == "jobtitle")
                        {
                            predicate = predicate.And(x => EF.Functions.Unaccent(x.ItemEmployee.JobTitle.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                        }
                        //else if (key == "phone")
                        //{
                        //	predicate = predicate.And(x => EF.Functions.Unaccent(x.ItemEmployee.PhoneNumber.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                        //}
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                predicate = predicate.And(x => EF.Functions.Unaccent(x.ItemEmployee.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey.ToLower()}")));
                predicate = predicate.And(x => EF.Functions.Unaccent(x.ItemEmployee.EmployeeCode.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey.ToLower()}")));
                predicate = predicate.And(x => EF.Functions.Unaccent(x.ItemEmployee.JobTitle.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey.ToLower()}")));
                //predicate = predicate.And(x => EF.Functions.Unaccent(x.ItemEmployee.PhoneNumber.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey.ToLower()}")));

            }
            return query.Where(predicate).AsQueryable();
        }

        public static IQueryable<UserEntity> PrepareQuerySort(IQueryable<UserEntity> query, Dictionary<string, long> param)
        {
            var properties = JsonPropertyHelper<UserDepartmentResponseModel>.GetJsonPropertyNames();
            properties.RemoveAll(p => p == null);
            foreach (var pa in param)
            {
                if (pa.Key == "department")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.ItemEmployee.Department.Name) : query.OrderBy(x => x.ItemEmployee.Department.Name);
                }
                else if (pa.Key == "jobtitle")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.ItemEmployee.JobTitle.Name) : query.OrderBy(x => x.ItemEmployee.JobTitle.Name);
                }
                else if (pa.Key == "employeecode")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.ItemEmployee.EmployeeCode) : query.OrderBy(x => x.ItemEmployee.EmployeeCode);
                }
                else if (pa.Key == "name")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.ItemEmployee.Name) : query.OrderBy(x => x.ItemEmployee.Name);
                }
            }
            return query;
        }

    }

}
