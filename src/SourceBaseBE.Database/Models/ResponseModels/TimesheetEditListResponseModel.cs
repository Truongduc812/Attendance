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
using SourceBaseBE.Database.Models.SpecialModels;
using System.Net.NetworkInformation;

namespace SourceBaseBE.Database.Models.ResponseModels
{
    public class TimesheetEditListResponseModel
    {
        [JsonProperty("Id")]
        public long Id { get; set; }
        [JsonProperty("employeeid")]
        public long? EmployeeId { get; set; }
        [DisplayName("Name")]
        [JsonProperty("name")]
        public string? Name { get; set; }
        [DisplayName("Employee Code")]
        [JsonProperty("employeecode")]
        public string? EmployeeCode { get; set; }

        [JsonProperty("status")]
        [DisplayName("Status")]
        public string? Status { get; set; }

        [JsonProperty("recordtime")]
        [DisplayName("Record Time")]
        public DateTime? RecordTime { get; set; }

        [JsonProperty("action")]
        [DisplayName("Request")]
        public string? ActionRequest { get; set; }

        [JsonProperty("timesheetupdateid")]
        public TimeSheetUpdateItem? TimeSheetUpdateId { get; set; }

        public class TimeSheetUpdateItem
        {
            public long? Id { get; set; }
            public DateTime? Date { get; set; }
        }


        public TimesheetEditListResponseModel SetData(TimeSheetUpdateEntity entity)
        {
            if (entity == null) throw new ArgumentNullException("Param Entity TimeSheetEntity Null");
            this.Id = entity.Id;
            this.Name = entity?.Employee?.Name;
            this.EmployeeCode = entity?.Employee?.EmployeeCode;
            this.Status = entity?.Status.ToString();
            this.RecordTime = entity?.RecordedTime;
            this.EmployeeId = entity?.EmployeeId;
            this.ActionRequest = entity?.ActionRequest.ToString();

            var timeSheetUd = entity?.TimeSheetApprovalEntities.Where(x => x.DeletedFlag != true && x.Status == Enums.EnumApproveStatus.PENDING)?.FirstOrDefault();
            if (timeSheetUd != null)
            {
                TimeSheetUpdateItem timeSheetItem = new TimeSheetUpdateItem();
                timeSheetItem.Id = timeSheetUd.TimeSheetUpdateId;
                timeSheetItem.Date = timeSheetUd.CreatedAt;
                this.TimeSheetUpdateId = timeSheetItem;
            }
            return this;
        }

        public TimesheetEditListResponseModel SetData(TimeSheetUpdateEntity entity, ICollection<TimeSheetUpdateEntity> timeSheetUpdate)
        {
            if (entity == null) throw new ArgumentNullException("Param Entity TimeSheetEntity Null");
            this.Id = entity.Id;
            this.Name = entity?.Employee?.Name;
            this.EmployeeCode = entity?.Employee?.EmployeeCode;
            this.Status = entity.Status.ToString();
            this.RecordTime = entity.RecordedTime;
            this.EmployeeId = entity?.EmployeeId;
            this.ActionRequest = entity?.ActionRequest?.ToString();
            //this.TimeSheetUpdateId = timeSheetUpdate?.Select(x => x.Id).ToList();
            var timeSheetUd = entity?.TimeSheetApprovalEntities.Where(x => x.DeletedFlag != true && x.Status == Enums.EnumApproveStatus.PENDING)?.FirstOrDefault();
            if (timeSheetUd != null)
            {
                TimeSheetUpdateItem timeSheetItem = new TimeSheetUpdateItem();
                timeSheetItem.Id = timeSheetUd.Id;
                timeSheetItem.Date = timeSheetUd.CreatedAt;
                this.TimeSheetUpdateId = timeSheetItem;
            }
            return this;
        }
        public static List<ColumnResponseModel> AddKeySearchFilterable(List<ColumnResponseModel> datas)
        {
            var properties = JsonPropertyHelper<TimesheetEditListResponseModel>.GetJsonPropertyNames();

            foreach (var data in datas)
            {
                if (properties.Contains(data.Key))
                {
                    string key = data.Key.ToLower();
                    //* add flag_filterable
                    if (key == nameof(TimesheetEditListResponseModel.Status).ToLower() ||
                        key == "action"
                      )
                        data.Filterable = true;
                    //* add flag_searchable
                    if (key == nameof(TimesheetEditListResponseModel.Name).ToLower())
                        data.Searchable = true;
                }
            }

            return datas;
        }

        //* prepare query list with where clause
        public static IQueryable<TimeSheetUpdateEntity> PrepareWhereQueryFilter(IQueryable<TimeSheetUpdateEntity> query, Dictionary<string, object> param)
        {
            var properties = JsonPropertyHelper<TimesheetEditListResponseModel>.GetJsonPropertyNames();
            properties.RemoveAll(p => p == null);

            foreach (var property in properties)
            {
                string key = property.ToLower();
                if (param.ContainsKey(key))
                {
                    if (key == nameof(TimesheetEditListResponseModel.Status).ToLower())
                    {
                        var value = (EnumFaceId)(int.Parse(param[key].ToString()));
                        query = query.Where(x => x.Status == value);
                    }
                    if (key == nameof(TimesheetEditListResponseModel.ActionRequest).ToLower())
                    {
                        var value = (EnumActionRequest)(int.Parse(param[key].ToString()));
                        query = query.Where(x => x.ActionRequest == value);
                    }
                }
            }
            return query;
        }

        //* prepare query list with where clause
        public static IQueryable<TimeSheetUpdateEntity> PrepareWhereQuerySearch(IQueryable<TimeSheetUpdateEntity> query,
            SearchModel searchModel)
        {
            var predicate = LinqKit.PredicateBuilder.New<TimeSheetUpdateEntity>(true); // Sử dụng thư viện linqkit
            var dicSearch = searchModel.DicSearch;
            var searchKey = searchModel.SearchStr?.Trim();
            foreach (var search in dicSearch)
            {
                string key = search.Key.ToLower();
                var searchValue = dicSearch[key].ToString().Trim();
                if (key == nameof(TimesheetEditListResponseModel.Name).ToLower())
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.Employee.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                }
                else if (key == nameof(TimesheetEditListResponseModel.Status).ToLower())
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.Status.ToString().ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                }
                else if (key == "all")
                {
                    predicate = predicate.Or(x => x.Employee != null && EF.Functions.Unaccent(x.Employee.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                    //predicate = predicate.Or(x => x.Status != null && EF.Functions.Unaccent(x.Status.ToString().ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                }
            }
            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.Employee.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.Status.ToString().ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
            }
            return query.Where(predicate);
        }
        public static IQueryable<TimeSheetUpdateEntity> PrepareQuerySort(IQueryable<TimeSheetUpdateEntity> query, Dictionary<string, long> param)
        {
            var properties = JsonPropertyHelper<TimesheetEditListResponseModel>.GetJsonPropertyNames();
            properties.RemoveAll(p => p == null);
            bool sortFlag = false;
            foreach (var pa in param)
            {
                if (pa.Key == nameof(TimesheetEditListResponseModel.Name).ToLower())
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.Employee.Name).ThenByDescending(x => x.RecordedTime) : query.OrderBy(x => x.Employee.Name).ThenByDescending(x => x.RecordedTime);
                    sortFlag = true;
                }
                else if (pa.Key == nameof(TimesheetEditListResponseModel.Status).ToLower())
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.Status).ThenByDescending(x => x.RecordedTime) : query.OrderBy(x => x.Status).ThenByDescending(x => x.RecordedTime);
                    sortFlag = true;
                }
                else if (pa.Key == nameof(TimesheetEditListResponseModel.RecordTime).ToLower())
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.RecordedTime) : query.OrderBy(x => x.RecordedTime);
                    sortFlag = true;
                }
                else if (pa.Key == nameof(TimesheetEditListResponseModel.EmployeeCode).ToLower())
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.Employee.EmployeeCode) : query.OrderBy(x => x.Employee.EmployeeCode);
                    sortFlag = true;
                }
                else if (pa.Key == "action")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.ActionRequest) : query.OrderBy(x => x.ActionRequest);
                    sortFlag = true;
                }
            }
            if (!sortFlag)
            {
                query = query.OrderByDescending(x => x.RecordedTime);
            }
            return query;
        }
    }
}
