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
    public class TimesheetListResponseModel
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



        [JsonProperty("timesheetupdateid")]
        public List<TimeSheetItem>? TimeSheetUpdateId { get; set; }

        public class TimeSheetItem
        {
            public long? Id { get; set; }
            public DateTime? Date { get; set; }
        }
        public TimesheetListResponseModel SetData(TimeSheetEntity entity)
        {
            if (entity == null) throw new ArgumentNullException("Param Entity TimeSheetEntity Null");
            this.Id = entity.Id;
            this.Name = entity?.Employee?.Name;
            this.EmployeeCode = entity?.Employee?.EmployeeCode;
            this.Status = entity.Status.ToString();
            this.RecordTime = entity.RecordedTime;
            return this;
        }

        public TimesheetListResponseModel SetData(TimeSheetUpdateEntity entity)
        {
            if (entity == null) throw new ArgumentNullException("Param Entity TimeSheetEntity Null");
            this.Id = entity.Id;
            this.Name = entity?.Employee?.Name;
            this.EmployeeCode = entity?.Employee?.EmployeeCode;
            this.Status = entity.Status.ToString();
            this.RecordTime = entity.RecordedTime;
            this.EmployeeId = entity?.EmployeeId;
            return this;
        }

        public TimesheetListResponseModel SetData(TimeSheetEntity entity, ICollection<TimeSheetUpdateEntity> timeSheetUpdate)
        {
            if (entity == null) throw new ArgumentNullException("Param Entity TimeSheetEntity Null");
            this.Id = entity.Id;
            this.Name = entity?.Employee?.Name;
            this.EmployeeCode = entity?.Employee?.EmployeeCode;
            this.Status = entity.Status.ToString();
            this.RecordTime = entity.RecordedTime;
            this.EmployeeId = entity?.EmployeeId;
            //this.TimeSheetUpdateId = timeSheetUpdate?.Select(x => x.Id).ToList();
            var timeSheetUd = timeSheetUpdate?.Where(x => x.DeletedFlag != true && x.TimeSheetApprovalEntities?.FirstOrDefault()?.Status == Enums.EnumApproveStatus.PENDING)?.ToList();
           
            List<TimeSheetItem> ListTimeSheetUpdate = new List<TimeSheetItem>();
            if(timeSheetUd != null)
            {
                foreach (var item in timeSheetUd)
                {
                    TimeSheetItem timeSheetItem = new TimeSheetItem();
                    timeSheetItem.Id = item.Id;
                    timeSheetItem.Date = item.CreatedAt;
                    ListTimeSheetUpdate.Add(timeSheetItem);
                }

                this.TimeSheetUpdateId = ListTimeSheetUpdate;
            }
            
            
            return this;
        }
        public static List<ColumnResponseModel> AddKeySearchFilterable(List<ColumnResponseModel> datas)
        {
            var properties = JsonPropertyHelper<TimesheetListResponseModel>.GetJsonPropertyNames();

            foreach (var data in datas)
            {
                if (properties.Contains(data.Key))
                {
                    string key = data.Key.ToLower();
                    //* add flag_filterable
                    if (key == nameof(TimesheetListResponseModel.Status).ToLower()
                      )
                        data.Filterable = true;
                    //* add flag_searchable
                    if (key == nameof(TimesheetListResponseModel.Name).ToLower())
                        data.Searchable = true;
                }
            }

            return datas;
        }

        //* prepare query list with where clause
        public static IQueryable<TimeSheetEntity> PrepareWhereQueryFilter(IQueryable<TimeSheetEntity> query, Dictionary<string, object> param)
        {
            var properties = JsonPropertyHelper<TimesheetListResponseModel>.GetJsonPropertyNames();
            properties.RemoveAll(p => p == null);

            foreach (var property in properties)
            {
                string key = property.ToLower();
                if (param.ContainsKey(key))
                {
                    if (key == nameof(TimesheetListResponseModel.Status).ToLower())
                    {
                        var value = (EnumFaceId)(int.Parse(param[key].ToString()));
                        query = query.Where(x => x.Status == value);
                    }
                }
            }
            return query;
        }

        //* prepare query list with where clause
        public static IQueryable<TimeSheetEntity> PrepareWhereQuerySearch(IQueryable<TimeSheetEntity> query,
            SearchModel searchModel)
        {
            var predicate = LinqKit.PredicateBuilder.New<TimeSheetEntity>(true); // Sử dụng thư viện linqkit
            var dicSearch = searchModel.DicSearch;
            var searchKey = searchModel.SearchStr?.Trim();
            foreach (var search in dicSearch)
            {
                string key = search.Key.ToLower();
                var searchValue = dicSearch[key].ToString().Trim();
                if (key == nameof(TimesheetListResponseModel.Name).ToLower())
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.Employee.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                }
                else if (key == nameof(TimesheetListResponseModel.Status).ToLower())
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
        public static IQueryable<TimeSheetEntity> PrepareQuerySort(IQueryable<TimeSheetEntity> query, Dictionary<string, long> param)
        {
            var properties = JsonPropertyHelper<TimesheetListResponseModel>.GetJsonPropertyNames();
            properties.RemoveAll(p => p == null);
            bool sortFlag = false;
            foreach (var pa in param)
            {
                if (pa.Key == nameof(TimesheetListResponseModel.Name).ToLower())
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.Employee.Name).ThenByDescending(x => x.RecordedTime) : query.OrderBy(x => x.Employee.Name).ThenByDescending(x => x.RecordedTime);
                    sortFlag = true;
                }
                else if (pa.Key == nameof(TimesheetListResponseModel.Status).ToLower())
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.Status).ThenByDescending(x => x.RecordedTime) : query.OrderBy(x => x.Status).ThenByDescending(x => x.RecordedTime);
                    sortFlag = true;
                }
                else if (pa.Key == nameof(TimesheetListResponseModel.RecordTime).ToLower())
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.RecordedTime) : query.OrderBy(x => x.RecordedTime);
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
