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
using SourceBaseBE.Database.Attribute;
using System.Linq;
using iSoft.Common.Utils;

namespace SourceBaseBE.Database.Models.ResponseModels
{
    public class OTListResponseModel
    {
        [JsonProperty("Id")]
        public long? Id { get; set; }
        [JsonProperty("name")]
        [Filterable("Name", "name", true)]
        public string? Name { get; set; }
        [JsonProperty("employeecode")]
        [Filterable("Employee Code", "employeecode", true)]
        public string? EmployeeCode { get; set; }
        [JsonProperty("department")]
        [Filterable("Department", "department", true)]
        public string? Department { get; set; }
        [JsonProperty("jobtitle")]
        [Filterable("Job Title", "jobtitle", true)]
        public string? JobTitle { get; set; }
        [JsonProperty("date")]
        [DisplayName("Date")]
        public string? Date { get; set; }
        [DisplayName("Time in")]
        [JsonProperty("timein")]
        public string? TimeIn { get; set; }
        [DisplayName("Time out")]
        [JsonProperty("timeout")]
        public string? TimeOut { get; set; }
        [DisplayName("Time Deviation")]
        [JsonProperty("timedeviation")]
        public double? TimeDeviation { get; set; }
        [JsonProperty("workingDayHighlight")]
        public EnumWorkingDayHighlight? WorkingDayHighlight { get; set; }
        [JsonProperty("status")]
        //[DisplayName("Status")]
        public string? Status { get; set; }
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
        public string? Notes { get; set; }
        [JsonProperty("employeeid")]
        public long? EmployeeId { get; set; }
        [JsonProperty("list_workingdayupdateid")]
        public long[] ListWorkingDayUpdateId { get; set; }
        public override string ToString()
        {
            return $"{this.Id}:{this.Date}:{this.Status.ToString()}:{this.Type}";
        }
        public OTListResponseModel SetData(WorkingDayEntity entity, List<WorkingTypeEntity> wktype)
        {
            try
            {
                this.Id = entity.Id;
                this.EmployeeCode = entity.Employee?.EmployeeCode;
                this.Name = entity.Employee?.Name;
                this.Department = entity.Employee?.Department?.Name;
                this.JobTitle = entity.Employee?.JobTitle?.Name;
                this.Date = entity.WorkingDate != null ? entity.WorkingDate.Value.ToString("dd/MM/yyyy") : null;
                this.Status = entity.WorkingDayStatus.ToString();
                this.TimeIn = entity.Time_In != null ? entity.Time_In.Value.ToString("MM/dd/yyyy HH:mm:ss") : null;
                this.TimeOut = entity.Time_Out != null ? entity.Time_Out.Value.ToString("MM/dd/yyyy HH:mm:ss") : null;
                if (entity.TimeDeviation == null)
                {
                    if (entity.Time_Out != null && entity.Time_In != null)
                    {
                        this.TimeDeviation = (entity.Time_Out.GetValueOrDefault() - entity.Time_In.GetValueOrDefault() - TimeSpan.FromHours(8)).TotalSeconds.Round(0);
                    }
                }
                else
                {
                    this.TimeDeviation = entity.TimeDeviation;
                }
                this.recommendTypeId = wktype.Where(x => x.Code == entity.RecommendType).FirstOrDefault()?.Id;
                this.RecommendType = entity.RecommendType;
                if (entity.WorkingType != null && (wktype.FirstOrDefault(x => x.Id == entity.WorkingTypeEntityId) != null))
                {
                    this.Type = entity.WorkingType?.Code;
                    this.isUsingRecommend = false;
                }
                else
                {
                    this.Type = this.RecommendType;
                    this.isUsingRecommend = true;
                }
                this.Notes = entity.Notes;
                this.EmployeeId = entity.EmployeeEntityId;
                this.ListWorkingDayUpdateId = entity.WorkingDayUpdates
                    ?.Where(x => x.DeletedFlag != true && x.WorkingDayApprovals?.FirstOrDefault().ApproveStatus == Enums.EnumApproveStatus.PENDING)?.Select(x => x.Id)?.ToArray();
                return this;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public static List<ColumnResponseModel> AddKeySearchFilterable(List<ColumnResponseModel> datas)
        {
            var properties = JsonPropertyHelper<OTListResponseModel>.GetJsonPropertyNames();

            foreach (var data in datas)
            {
                if (properties.Contains(data.Key))
                {
                    string key = data.Key.ToLower();
                    if (key == "department" || key == "jobtitle")
                    {
                        data.Filterable = true;
                        data.Searchable = true;
                    }
                    //* add flag_searchable
                    if (key == nameof(OTListResponseModel.Name).ToLower()
                      || key == nameof(OTListResponseModel.EmployeeCode).ToLower())
                        data.Searchable = true;
                }
            }
            // add key column for filter type or recommend type
            datas.Add(new ColumnResponseModel()
            {
                Key = "type",
                Filterable = true,
                Displayable = false,
                Searchable = false,
                DisplayName = "Type"
            });
            //
            return datas;
        }
        public static IQueryable<WorkingDayEntity> PrepareDetailReportWhereQueryFilter(IQueryable<WorkingDayEntity> query,
            Dictionary<string, object> param
            , List<WorkingTypeEntity> wkTOts
            )
        {
            var properties = JsonPropertyHelper<OTListResponseModel>.GetJsonPropertyNames();
            properties.RemoveAll(p => p == null);

            var predicate = LinqKit.PredicateBuilder.New<WorkingDayEntity>(true); // Sử dụng thư viện linqkit
            foreach (var property in properties)
            {
                string key = property.ToLower();
                if (param.ContainsKey(key))
                {
                    if (key == "department")
                    {
                        var value = param[key].ToString()?.Split(",");
                        var predicateDepartment = LinqKit.PredicateBuilder.New<WorkingDayEntity>(true); // Sử dụng thư viện linqkit
                        if (value.Length > 0)
                        {
                            foreach (var val in value)
                            {
                                predicateDepartment.Or(x => x.Employee.DepartmentId == long.Parse(val));
                            }
                        }
                        predicate.And(predicateDepartment);

                        //predicate = predicate.And(x => x.Employee.DepartmentId == long.Parse(param[key].ToString()));
                    }
                    else if (key == "jobtitle")
                    {
                        predicate = predicate.And(x => x.Employee.JobTitleId == long.Parse(param[key].ToString()));
                    }
                    else if (key == "type")
                    {
                        if (param[key].ToString() == "0")
                        {
                            predicate = predicate.And(x => x.WorkingType != null && wkTOts.Contains(x.WorkingType));
                        }
                        else if (param[key].ToString() == "1")
                        {
                            predicate = predicate.And(x => x.WorkingType == null && wkTOts.Select(x => x.Code).Contains(x.RecommendType));
                        }
                    }
                }
            }
            return query.Where(predicate);
        }

        public static List<WorkingDayEntity> PrepareDetailReportWhereQueryFilterV2(
                        List<WorkingDayEntity> query,
                        Dictionary<string, object> param,
                        List<WorkingTypeEntity> wkTOts)
        {
            var properties = JsonPropertyHelper<OTListResponseModel>.GetJsonPropertyNames();
            properties.RemoveAll(p => p == null);

            var predicate = LinqKit.PredicateBuilder.New<WorkingDayEntity>(true);

            foreach (var property in properties)
            {
                string key = property.ToLower();
                if (!param.TryGetValue(key, out var value) || value == null)
                    continue;

                switch (key)
                {
                    case "jobtitle":
                        if (long.TryParse(value.ToString(), out var jobTitleId))
                        {
                            predicate = predicate.And(x => x.Employee.JobTitleId == jobTitleId);
                        }
                        break;

                    case "type":
                        string typeValue = value.ToString();
                        var listWktOTCode = wkTOts?.Select(x => x.Code)?.ToList();
                        if (param[key].ToString() == "0")
                        {
                            predicate = predicate.And(x => x.WorkingType != null && listWktOTCode.Contains(x.WorkingType.Code));
                        }
                        else if (param[key].ToString() == "1")
                        {
                            predicate = predicate.And(x => x.WorkingType == null && wkTOts.Select(x => x.Code).Contains(x.RecommendType));
                        }
                        break;
                }
            }

            return query.AsQueryable().Where(predicate).ToList();
        }


        public static IQueryable<WorkingDayEntity> PrepareDetailReportWhereQuerySearch(IQueryable<WorkingDayEntity> query, SearchModel searchModel)
        {
            var predicate = LinqKit.PredicateBuilder.New<WorkingDayEntity>(true); // Sử dụng thư viện linqkit
            var dicSearch = searchModel.DicSearch;
            var searchKey = searchModel.SearchStr?.ToLower()?.Trim();
            foreach (var search in dicSearch)
            {
                string key = search.Key.ToLower();
                var searchVal = dicSearch[key].ToString()?.ToLower().Trim();
                if (key == "department")
                {
                    predicate = predicate.And(x => x.Employee.Department != null && EF.Functions.Unaccent(x.Employee.Department.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                }
                else if (key == "jobtitle")
                {
                    predicate = predicate.And(x => x.Employee.JobTitle != null &&
                    EF.Functions.Unaccent(x.Employee.JobTitle.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                }
                else if (key == "name")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.Employee.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                }
                else if (key == "employeecode")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.Employee.EmployeeCode.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                }
                if (key == "all")
                {
                    predicate = predicate.Or(x => x.Employee.Department != null && EF.Functions.Unaccent(x.Employee.Department.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                    predicate = predicate.Or(x => x.Employee.JobTitle != null && EF.Functions.Unaccent(x.Employee.JobTitle.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                    predicate = predicate.Or(x => x.Employee.Name != null && EF.Functions.Unaccent(x.Employee.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                    predicate = predicate.Or(x => x.Employee.EmployeeCode != null && EF.Functions.Unaccent(x.Employee.EmployeeCode.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                }
            }
            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                predicate = predicate.Or(x => x.Employee.Department != null && EF.Functions.Unaccent(x.Employee.Department.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
                predicate = predicate.Or(x => x.Employee.JobTitle != null && EF.Functions.Unaccent(x.Employee.JobTitle.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
                predicate = predicate.Or(x => x.Employee.Name != null && EF.Functions.Unaccent(x.Employee.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
                predicate = predicate.Or(x => x.Employee.EmployeeCode != null && EF.Functions.Unaccent(x.Employee.EmployeeCode.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
            }
            return query.Where(predicate);
        }

        public static List<WorkingDayEntity> PrepareDetailReportWhereQuerySearchV2(List<WorkingDayEntity> query, SearchModel searchModel)
        {
            var predicate = LinqKit.PredicateBuilder.New<WorkingDayEntity>(true);
            var dicSearch = searchModel.DicSearch;
            var searchKey = searchModel.SearchStr?.ToLower()?.Trim().RemoveAccents();

            foreach (var search in dicSearch)
            {
                string key = search.Key.ToLower();
                var searchVal = search.Value?.ToString()?.ToLower()?.Trim().RemoveAccents();

                if (string.IsNullOrWhiteSpace(searchVal)) continue;

                if (key == "department")
                {
                    predicate = predicate.And(x => x.Employee != null && x.Employee.Department != null &&
                        x.Employee.Department.Name.ToLower().RemoveAccents().Contains(searchVal));
                }
                else if (key == "jobtitle")
                {
                    predicate = predicate.And(x => x.Employee != null && x.Employee.JobTitle != null &&
                        x.Employee.JobTitle.Name.ToLower().RemoveAccents().Contains(searchVal));
                }
                else if (key == "name")
                {
                    predicate = predicate.And(x => x.Employee != null &&
                        x.Employee.Name.ToLower().RemoveAccents().Contains(searchVal));
                }
                else if (key == "employeecode")
                {
                    predicate = predicate.And(x => x.Employee != null &&
                        x.Employee.EmployeeCode.ToLower().RemoveAccents().Contains(searchVal));
                }
                else if (key == "all")
                {
                    predicate = predicate.Or(x => x.Employee != null && x.Employee.Department != null &&
                        x.Employee.Department.Name.ToLower().RemoveAccents().Contains(searchVal));
                    predicate = predicate.Or(x => x.Employee != null && x.Employee.JobTitle != null &&
                        x.Employee.JobTitle.Name.ToLower().RemoveAccents().Contains(searchVal));
                    predicate = predicate.Or(x => x.Employee != null &&
                        x.Employee.Name.ToLower().RemoveAccents().Contains(searchVal));
                    predicate = predicate.Or(x => x.Employee != null &&
                        x.Employee.EmployeeCode.ToLower().RemoveAccents().Contains(searchVal));
                }
            }

            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                predicate = predicate.Or(x => x.Employee != null && x.Employee.Department != null &&
                    x.Employee.Department.Name.ToLower().RemoveAccents().Contains(searchKey));
                predicate = predicate.Or(x => x.Employee != null && x.Employee.JobTitle != null &&
                    x.Employee.JobTitle.Name.ToLower().RemoveAccents().Contains(searchKey));
                predicate = predicate.Or(x => x.Employee != null &&
                    x.Employee.Name.ToLower().RemoveAccents().Contains(searchKey));
                predicate = predicate.Or(x => x.Employee != null &&
                    x.Employee.EmployeeCode.ToLower().RemoveAccents().Contains(searchKey));
            }

            return query.AsQueryable().Where(predicate).ToList();
        }



        public static IQueryable<WorkingDayEntity> PrepareDetailReportQuerySort(IQueryable<WorkingDayEntity> query, Dictionary<string, long> param)
        {
            var properties = JsonPropertyHelper<OTListResponseModel>.GetJsonPropertyNames();
            properties.RemoveAll(p => p == null);
            foreach (var pa in param)
            {
                if (pa.Key == "department")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.Employee.Department.Name) : query.OrderBy(x => x.Employee.Department.Name);
                }
                else if (pa.Key == "jobtitle")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.Employee.JobTitle.Name) : query.OrderBy(x => x.Employee.JobTitle.Name);
                }
                else if (pa.Key == "employeecode")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.Employee.EmployeeCode) : query.OrderBy(x => x.Employee.EmployeeCode);
                }
                else if (pa.Key == "name")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.Employee.Name) : query.OrderBy(x => x.Employee.Name);
                }
                else if (pa.Key == "status")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.WorkingDayStatus) : query.OrderBy(x => x.WorkingDayStatus);
                }
                else if (pa.Key == "date")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.WorkingDate) : query.OrderBy(x => x.WorkingDate);
                }
                else if (pa.Key == "timein")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.Time_In) : query.OrderBy(x => x.Time_In);
                }
                else if (pa.Key == "timeout")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.Time_Out) : query.OrderBy(x => x.Time_Out);
                }
                else if (pa.Key == "type")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.WorkingType.Name) : query.OrderBy(x => x.WorkingType.Name);
                }
                else if (pa.Key == "timedeviation")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.TimeDeviation) : query.OrderBy(x => x.TimeDeviation);
                }
            }
            return query;
        }
        public static List<WorkingDayEntity> PrepareDetailReportQuerySortV2(List<WorkingDayEntity> query, Dictionary<string, long> param)
        {
            var properties = JsonPropertyHelper<OTListResponseModel>.GetJsonPropertyNames();
            properties.RemoveAll(p => p == null);

            IOrderedEnumerable<WorkingDayEntity>? orderedQuery = null;

            foreach (var pa in param)
            {
                bool descending = pa.Value == -1;

                switch (pa.Key.ToLower())
                {
                    case "department":
                        orderedQuery = ApplySorting(orderedQuery, query, x => x.Employee?.Department?.Name, descending);
                        break;
                    case "jobtitle":
                        orderedQuery = ApplySorting(orderedQuery, query, x => x.Employee?.JobTitle?.Name, descending);
                        break;
                    case "employeecode":
                        orderedQuery = ApplySorting(orderedQuery, query, x => x.Employee?.EmployeeCode, descending);
                        break;
                    case "name":
                        orderedQuery = ApplySorting(orderedQuery, query, x => x.Employee?.Name, descending);
                        break;
                    case "status":
                        orderedQuery = ApplySorting(orderedQuery, query, x => x.WorkingDayStatus, descending);
                        break;
                    case "date":
                        orderedQuery = ApplySorting(orderedQuery, query, x => x.WorkingDate, descending);
                        break;
                    case "timein":
                        orderedQuery = ApplySorting(orderedQuery, query, x => x.Time_In, descending);
                        break;
                    case "timeout":
                        orderedQuery = ApplySorting(orderedQuery, query, x => x.Time_Out, descending);
                        break;
                    case "type":
                        orderedQuery = ApplySorting(orderedQuery, query, x => x.WorkingType?.Name, descending);
                        break;
                    case "timedeviation":
                        orderedQuery = ApplySorting(orderedQuery, query, x => x.TimeDeviation, descending);
                        break;
                }
            }

            return orderedQuery?.ToList() ?? query;
        }

        // Generic sorting helper function
        private static IOrderedEnumerable<T> ApplySorting<T, TKey>(
            IOrderedEnumerable<T>? orderedQuery,
            List<T> query,
            Func<T, TKey> keySelector,
            bool descending)
        {
            if (orderedQuery == null)
            {
                return descending ? query.OrderByDescending(keySelector) : query.OrderBy(keySelector);
            }
            return descending ? orderedQuery.ThenByDescending(keySelector) : orderedQuery.ThenBy(keySelector);
        }

    }
}
