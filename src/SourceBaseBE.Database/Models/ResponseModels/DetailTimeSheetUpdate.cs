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
using System.Dynamic;
using SourceBaseBE.Database.Attribute;
using NPOI.SS.Formula.Functions;
using System.Data.Entity;
using System.Xml.Linq;
namespace SourceBaseBE.Database.Models.ResponseModels
{
    public class DetailTimeSheetUpdateDTO
    {
        [JsonProperty("timesheetupdateid")]
        public long? Id { get; set; }
        [DisplayName("Date")]
        [JsonProperty("date")]
        public DateTime? Date { get; set; }
        [DisplayName("Requester")]
        [JsonProperty("requester")]
        [FilterableAttribute("Requester", "requester", true)]
        public string? Requester { get; set; }
        [JsonProperty("recorded_time")]
        [DisplayName("Recoreded Time")]
        public string? RecorededTime { get; set; }
        [JsonProperty("origin_recorded_time")]
        public string? OriginRecordedTime { get; set; }
        [JsonProperty("origin_status")]
        public string? OriginStatus { get; set; }
        [JsonProperty("status")]
        [DisplayName("Status")]
        public string? Status { get; set; }

        [JsonProperty("action")]
        [DisplayName("Request")]
        public string? Action { get; set; }

        [JsonProperty("note")]
        [DisplayName("Note")]
        public string? Note { get; set; }
        [JsonProperty("requestat")]
        [DisplayName("Request At")]
        public DateTime? RequestAt { get; set; }
        [FilterableAttribute("Request Status", "request_status")]
        [JsonProperty("request_status")]
        public EnumApproveStatus? ApproveStatus { get; set; }
        [JsonProperty("department")]
        public string? Department { get; set; }
        [JsonProperty("jobtitle")]
        public string? JobTitle { get; set; }
        public static DetailTimeSheetUpdateDTO SetData(TimeSheetUpdateEntity timesheetUpdate)
        {
            if (timesheetUpdate == null) throw new ArgumentNullException("DetailTimeSheetUpdateDTO Input Parameter  Null");
            var dto = new DetailTimeSheetUpdateDTO();
            dto.Requester = timesheetUpdate?.UserEntity?.ItemEmployee != null ? timesheetUpdate?.UserEntity?.ItemEmployee.Name : timesheetUpdate?.UserEntity?.Displayname;
            dto.Status = timesheetUpdate?.Status.ToString();
            dto.OriginStatus = timesheetUpdate?.OriginStatus?.ToString();
            dto.RecorededTime = timesheetUpdate?.RecordedTime?.ToString("MM/dd/yyyy HH:mm:ss");
            dto.OriginRecordedTime = timesheetUpdate?.OriginRecordedTime?.ToString("MM/dd/yyyy HH:mm:ss");
            dto.Note = timesheetUpdate?.Notes;
            dto.Action = timesheetUpdate?.ActionRequest?.ToString();
            dto.Id = timesheetUpdate?.Id;
            dto.ApproveStatus = timesheetUpdate?.TimeSheetApprovalEntities.OrderByDescending(x => x.UpdatedAt).FirstOrDefault()?.Status;
            dto.RequestAt = timesheetUpdate?.CreatedAt.GetValueOrDefault();
            return dto;
        }
        public static List<ColumnResponseModel> AddKeySearchFilterable(List<ColumnResponseModel> datas)
        {
            foreach (var data in datas)
            {
                string key = data.Key.ToLower();
                //* add flag_filterable
                if ( key == nameof(DetailTimeSheetUpdateDTO.Action).ToLower()
                  || key == nameof(DetailTimeSheetUpdateDTO.Status).ToLower()
                    )
                    data.Filterable = true;
                //* add flag_searchable           (//|| key == "request_status")
                if (key == nameof(DetailTimeSheetUpdateDTO.Note).ToLower()
                || key == nameof(DetailTimeSheetUpdateDTO.Requester).ToLower()
                || key == nameof(DetailTimeSheetUpdateDTO.Action).ToLower()
                )
                    data.Searchable = true;
            }

            return datas;
        }

        //* prepare query list with where clause
        public static IQueryable<TimeSheetUpdateEntity> PrepareWhereQueryFilter(IQueryable<TimeSheetUpdateEntity> query, Dictionary<string, object> param)
        {
            var properties = JsonPropertyHelper<DetailTimeSheetUpdateDTO>.GetJsonPropertyNames();
            var predicate = LinqKit.PredicateBuilder.New<TimeSheetUpdateEntity>(true); // Sử dụng thư viện linqkit
            foreach (var property in properties)
            {
                string key = property.ToLower();
                if (!param.ContainsKey(key)) continue;
                if (key == JsonPropertyHelper<DetailTimeSheetUpdateDTO>.GetJsonPropertyName(nameof(DetailTimeSheetUpdateDTO.Requester))?.ToLower())
                {
                    predicate.And(x => x.UserEntity.EmployeeId == long.Parse(param[key].ToString()));
                }
                else if (key == JsonPropertyHelper<DetailTimeSheetUpdateDTO>.GetJsonPropertyName(nameof(DetailTimeSheetUpdateDTO.ApproveStatus))?.ToLower())
                {
                    var value = param[key].ToString()?.Split(",");
                    var predicateStatus = LinqKit.PredicateBuilder.New<TimeSheetUpdateEntity>(true); // Sử dụng thư viện linqkit
                    if (value.Length > 0)
                    {
                        foreach (var val in value)
                        {
                            predicateStatus.Or(x => x.TimeSheetApprovalEntities.FirstOrDefault().Status == (EnumApproveStatus)(int.Parse(val.ToString())));
                        }
                    }
                    predicate.And(predicateStatus);
                }
                else if (key == JsonPropertyHelper<DetailTimeSheetUpdateDTO>.GetJsonPropertyName(nameof(DetailTimeSheetUpdateDTO.Status))?.ToLower())
                {
                    var value = (EnumFaceId)(int.Parse(param[key].ToString()));
                    query = query.Where(x => x.Status == value);
                }
                else if (key == "action")
                {
                    var value = (EnumActionRequest)(int.Parse(param[key].ToString()));
                    query = query.Where(x => x.ActionRequest == value);
                }
                else if (key == "department")
                {
                    predicate.And(x => x.Employee.DepartmentId == long.Parse(param[key].ToString()));
                }
                else if (key == "jobtitle")
                {
                    predicate.And(x => x.Employee.DepartmentId == long.Parse(param[key].ToString()));
                }
            }

            return query.Where(predicate);
        }

        public static IQueryable<TimeSheetUpdateEntity> PrepareWhereQuerySearch(IQueryable<TimeSheetUpdateEntity> query, SearchModel searchModel)
        {
            var predicate = LinqKit.PredicateBuilder.New<TimeSheetUpdateEntity>(true); // Sử dụng thư viện linqkit
            var dicSearch = searchModel.DicSearch;
            var searchKey = searchModel.SearchStr?.ToLower()?.Trim();
            foreach (var search in dicSearch)
            {
                string key = search.Key.ToLower();
                var searchVal = dicSearch[key]?.ToLower()?.Trim();
                if (key == JsonPropertyHelper<DetailTimeSheetUpdateDTO>.GetJsonPropertyName(nameof(DetailTimeSheetUpdateDTO.Note)).ToLower())
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.Notes.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                }
                //else if (key == JsonPropertyHelper<DetailTimeSheetUpdateDTO>.GetJsonPropertyName(nameof(DetailTimeSheetUpdateDTO.ApproveStatus)).ToLower())
                //{
                //    predicate = predicate.And(x => EF.Functions.Unaccent(x.TimeSheetApprovalEntities.FirstOrDefault().Status.ToString().ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                //}
                else if (key == JsonPropertyHelper<DetailTimeSheetUpdateDTO>.GetJsonPropertyName(nameof(DetailTimeSheetUpdateDTO.Requester)).ToLower())
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.UserEntity.ItemEmployee.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")) ||
                     EF.Functions.Unaccent(x.UserEntity.Displayname.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}"))
                    );
                }
                else if (key == "department")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.Employee.Department.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                }
                else if (key == "jobtitle")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.Employee.JobTitle.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                }
                if (key == "all")
                {
                    predicate = predicate.Or(x => EF.Functions.Unaccent(x.Employee.Department.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                    predicate = predicate.Or(x => EF.Functions.Unaccent(x.Employee.JobTitle.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                    predicate = predicate.Or(x => EF.Functions.Unaccent(x.UserEntity.ItemEmployee.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")) ||
                    EF.Functions.Unaccent(x.UserEntity.Displayname.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                }
            }
            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                //predicate = predicate.Or(x => EF.Functions.Unaccent(x.WorkingType.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.Employee.Department.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.Employee.JobTitle.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.UserEntity.ItemEmployee.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
            }
            return query.Where(predicate);
        }

        public static IQueryable<TimeSheetUpdateEntity> PrepareQuerySort(IQueryable<TimeSheetUpdateEntity> query, Dictionary<string, long> param)
        {
            var properties = JsonPropertyHelper<DetailTimeSheetUpdateDTO>.GetJsonPropertyNames();
            properties.RemoveAll(p => p == null);
            foreach (var pa in param)
            {
                if (pa.Key == JsonPropertyHelper<DetailTimeSheetUpdateDTO>.GetJsonPropertyName(nameof(DetailTimeSheetUpdateDTO.Note)).ToLower())
                {
                    query = pa.Value < 0 ? query.OrderByDescending(x => x.Notes) : query.OrderBy(x => x.Notes);
                }
                else if (pa.Key == JsonPropertyHelper<DetailTimeSheetUpdateDTO>.GetJsonPropertyName(nameof(DetailTimeSheetUpdateDTO.ApproveStatus)).ToLower())
                {
                    query = pa.Value < 0 ? query.OrderByDescending(x => x.TimeSheetApprovalEntities.FirstOrDefault().Status)
                        : query.OrderBy(x => x.TimeSheetApprovalEntities.FirstOrDefault().Status);
                }
                else if (pa.Key == JsonPropertyHelper<DetailTimeSheetUpdateDTO>.GetJsonPropertyName(nameof(DetailTimeSheetUpdateDTO.Requester)).ToLower())
                {
                    query = pa.Value < 0 ? query.OrderByDescending(x => x.UserEntity.Displayname) : query.OrderBy(x => x.UserEntity.Displayname);
                }
                else if (pa.Key == JsonPropertyHelper<DetailTimeSheetUpdateDTO>.GetJsonPropertyName(nameof(DetailTimeSheetUpdateDTO.RequestAt)).ToLower())
                {
                    query = pa.Value < 0 ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt);
                }
            }
            return query;
        }

    }

}
