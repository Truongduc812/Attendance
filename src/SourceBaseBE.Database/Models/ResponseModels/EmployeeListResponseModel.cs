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
    public class DashboardResponseModel
    {
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
        [DisplayName("Job Title")]
        public string? JobTitle { get; set; }
        [JsonProperty("status")]
        [DisplayName("Status")]
        public string? Status { get; set; }


        [JsonProperty("timein")]
        [DisplayName("Time-in")]
        public string? TimeIn { get; set; }

        [JsonProperty("timeout")]
        [DisplayName("Time-out")]
        public string? TimeOut { get; set; }
        public DashboardResponseModel SetData(WorkingDayEntity entity)
        {
            if (entity == null) throw new ArgumentNullException("Param Entity DashboardResponseModel Null");
            this.Name = entity?.Employee?.Name;
            this.EmployeeCode = entity?.Employee?.EmployeeCode;
            this.Phone = entity.Employee?.PhoneNumber;
            this.Status = entity.InOutState.ToString();
            this.Department = entity.Employee?.Department?.Name;
            this.JobTitle = entity.Employee?.JobTitle?.Name;
            this.TimeIn = entity.Time_In != null ? entity.Time_In.Value.ToString("MM/dd/yyyy HH:mm:ss") : null;
            this.TimeOut = entity.Time_Out != null ? entity.Time_Out.Value.ToString("MM/dd/yyyy HH:mm:ss") : null;
            //this.TimeDeviation = (entity.Time_Out == null || entity.Time_In == null) ? 0 : (entity?.Time_Out.GetValueOrDefault() - entity?.Time_In.GetValueOrDefault()).Value.TotalSeconds.Round(0);
            return this;
        }
        //public override List<object> SetData(List<EmployeeEntity> listEntity)
        //{
        //  List<Object> listRS = new List<object>();
        //  foreach (EmployeeEntity entity in listEntity)
        //  {
        //    listRS.Add(new EmployeeResponseModel().SetData(entity));
        //  }
        //  return listRS;
        //}

        public static List<ColumnResponseModel> AddKeySearchFilterable(List<ColumnResponseModel> datas)
        {
            var properties = JsonPropertyHelper<DashboardResponseModel>.GetJsonPropertyNames();

            foreach (var data in datas)
            {
                if (properties.Contains(data.Key))
                {
                    string key = data.Key.ToLower();
                    //* add flag_filterable
                    if (key == nameof(DashboardResponseModel.JobTitle).ToLower()
                      || key == nameof(DashboardResponseModel.Department).ToLower()
                      )
                        data.Filterable = true;
                    //* add flag_searchable
                    if (key == nameof(DashboardResponseModel.Name).ToLower()
                      || key == nameof(DashboardResponseModel.Department).ToLower()
                      || key == nameof(DashboardResponseModel.JobTitle).ToLower()
                      || key == nameof(EmployeeResponseModel.EmployeeCode).ToLower()
                      || key == nameof(DashboardResponseModel.Phone).ToLower()
                      )
                        data.Searchable = true;
                }
            }

            return datas;
        }

        //* prepare query list with where clause
        public static IQueryable<WorkingDayEntity> PrepareWhereQueryFilter(IQueryable<WorkingDayEntity> query, Dictionary<string, object> param)
        {
            var properties = JsonPropertyHelper<DashboardResponseModel>.GetJsonPropertyNames();
            properties.RemoveAll(p => p == null);

            foreach (var property in properties)
            {
                string key = property.ToLower();
                if (param.ContainsKey(key))
                {
                    if (key == "department")
                    {
                        //query = query.Where(x => x.Employee.DepartmentId == long.Parse(param[key].ToString()));
                        var value = param[key].ToString()?.Split(",");
                        if (value.Length > 0)
                        {
                            foreach (var val in value)
                            {
                                query.Where(x => x.Employee.DepartmentId == long.Parse(val));
                            }
                        } 
                    }
                    else if (key == "jobtitle")
                    {
                        query = query.Where(x => x.Employee.JobTitleId == long.Parse(param[key].ToString()));
                    }
                    else if (key == "status")
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
                    else if (key == "jobtitle")
                    {
                        predicate.And(x => x.JobTitleId == long.Parse(param[key].ToString()));
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
                else if (key == "jobtitle")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.Employee.JobTitle.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                }
                else if (key == "employeecode")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.Employee.EmployeeCode.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                }
                else if (key == "name")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.Employee.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                }
                else if (key == "phone")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.Employee.PhoneNumber.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                }
                //else if (key == "contract")
                //{
                //    predicate = predicate.And(x => x.Employee.Contract != null && EF.Functions.Unaccent(x.Employee.Contract.ToString().ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                //}
                if (key == "all")
                {
                    predicate = predicate.Or(x => EF.Functions.Unaccent(x.Employee.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                    predicate = predicate.Or(x => EF.Functions.Unaccent(x.Employee.EmployeeCode.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                    predicate = predicate.Or(x => EF.Functions.Unaccent(x.Employee.PhoneNumber.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                    predicate = predicate.Or(x => EF.Functions.Unaccent(x.Employee.Department.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                    predicate = predicate.Or(x => EF.Functions.Unaccent(x.Employee.JobTitle.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                    predicate = predicate.Or(x => EF.Functions.Unaccent(x.Employee.PhoneNumber.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                    //predicate = predicate.Or(x => x.Employee.Contract != null && EF.Functions.Unaccent(x.Employee.Contract.ToString().ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));

                }
            }
            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.Employee.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.Employee.EmployeeCode.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.Employee.PhoneNumber.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.Employee.Department.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.Employee.JobTitle.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.Employee.PhoneNumber.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
            }
            return query.Where(predicate);
        }
        public static IQueryable<WorkingDayEntity> PrepareQuerySort(IQueryable<WorkingDayEntity> query, Dictionary<string, long> param)
        {
            var properties = JsonPropertyHelper<DashboardResponseModel>.GetJsonPropertyNames();
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
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.InOutState) : query.OrderBy(x => x.InOutState);
                }
                else if (pa.Key == "phone")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.Employee.PhoneNumber) : query.OrderBy(x => x.Employee.PhoneNumber);
                }
                else if (pa.Key == "timein")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.Time_In) : query.OrderBy(x => x.Time_In);
                }
                else if (pa.Key == "timeout")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.Time_Out) : query.OrderBy(x => x.Time_Out);
                }
                else if (pa.Key == "contract")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.Employee.Contract) : query.OrderBy(x => x.Time_Out);
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
                else if (key == "jobtitle")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.JobTitle.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                }
                else if (key == "employeecode")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.EmployeeCode.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                }
                else if (key == "name")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                }
                else if (key == "phone")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.PhoneNumber.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                }
                else if (key == "email")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.Email.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                }
                if (key == "all")
                {
                    predicate = predicate.Or(x => EF.Functions.Unaccent(x.Department.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                    predicate = predicate.Or(x => EF.Functions.Unaccent(x.PhoneNumber.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                    predicate = predicate.Or(x => EF.Functions.Unaccent(x.EmployeeCode.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                    predicate = predicate.Or(x => EF.Functions.Unaccent(x.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                    predicate = predicate.Or(x => EF.Functions.Unaccent(x.JobTitle.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                    predicate = predicate.Or(x => EF.Functions.Unaccent(x.PhoneNumber.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                    predicate = predicate.Or(x => EF.Functions.Unaccent(x.Email.ToLower()).Contains(EF.Functions.Unaccent($"{searchValue}")));
                }
            }
            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.Department.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.PhoneNumber.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.EmployeeCode.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.JobTitle.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.PhoneNumber.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
                predicate = predicate.Or(x => EF.Functions.Unaccent(x.Email.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
            }
            return query.Where(predicate).AsQueryable();
        }
        public static IQueryable<EmployeeEntity> PrepareQuerySort(IQueryable<EmployeeEntity> query, Dictionary<string, long> param)
        {
            var properties = JsonPropertyHelper<DashboardResponseModel>.GetJsonPropertyNames();
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
                else if (pa.Key == "name")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
                }
                else if (pa.Key == "phone")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.PhoneNumber) : query.OrderBy(x => x.PhoneNumber);
                }
                else if (pa.Key == "email")
                {
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.Email) : query.OrderBy(x => x.Email);
                }
            }
            return query;
        }

    }
    public class DetailAttendanceResponse
    {
        [JsonProperty("Id")]
        public long? Id { get; set; }
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
        //[DisplayName("Working Day Highlight")]
        [JsonProperty("workingDayHighlight")]
        public EnumWorkingDayHighlight? WorkingDayHighlight { get; set; }
        [JsonProperty("status")]
        [DisplayName("Status")]
        public string? Status { get; set; }
        [DisplayName("Type")]
        [JsonProperty("type")]
        public string? Type { get; set; }
        [JsonProperty("recommendType")]
        public string? RecommendType { get; set; }
        [JsonProperty("recommendTypeId")]
        public long? recommendTypeId { get; set; }
        [DisplayName("Note")]
        [JsonProperty("notes")]
        public string? Notes { get; set; }
        [JsonProperty("employeeid")]
        public long? EmployeeId { get; set; }
        [JsonProperty("list_workingdayupdateid")]
        public List<WorkingDayItem>? ListWorkingDayUpdateId { get; set; }

        public class WorkingDayItem
        {
            public long? Id { get; set; }
            public DateTime? Date { get; set; }
        }
        public override string ToString()
        {
            return $"{this.Id}:{this.Date}:{this.Status.ToString()}:{this.RecommendType}";
        }
        public DetailAttendanceResponse SetData(WorkingDayEntity entity, Dictionary<long, WorkingTypeEntity> dicWorkingDayId2Type)
        {
            this.Id = entity.Id;
            this.Date = entity.WorkingDate != null ? entity.WorkingDate.Value.ToString("dd/MM/yyyy") : null;
            this.Status = entity.WorkingDayStatus.ToString();
            this.TimeIn = entity.Time_In != null ? entity.Time_In.Value.ToString("MM/dd/yyyy HH:mm:ss") : null;
            this.TimeOut = entity.Time_Out != null ? entity.Time_Out.Value.ToString("MM/dd/yyyy HH:mm:ss") : null;
            if (entity.TimeDeviation != null)
            {
                if (entity.Time_Out != null && entity.Time_In != null)
                {
                    this.TimeDeviation = (entity.Time_Out.GetValueOrDefault() - entity.Time_In.GetValueOrDefault() - TimeSpan.FromHours(8)).TotalSeconds.Round(0);
                }
            }
            this.Type = entity.WorkingType?.Code;
            this.RecommendType = dicWorkingDayId2Type[entity.Id].Code;
            this.Notes = entity.Notes;
            this.EmployeeId = entity.EmployeeEntityId;
            var workingDayUd = entity.WorkingDayUpdates.Where(x => x.WorkingDayApprovals.FirstOrDefault().ApproveStatus == Enums.EnumApproveStatus.PENDING)?.ToList();
            List<WorkingDayItem> ListWorkingDayUpdate = new List<WorkingDayItem>();

            if (workingDayUd != null)
            {
                foreach (var item in workingDayUd)
                {
                    WorkingDayItem workingDayItem = new WorkingDayItem();
                    workingDayItem.Id = item.Id;
                    workingDayItem.Date = item.CreatedAt;
                    ListWorkingDayUpdate.Add(workingDayItem);
                }

                this.ListWorkingDayUpdateId = ListWorkingDayUpdate;
            }
            return this;
        }
        public DetailAttendanceResponse SetData(WorkingDayEntity entity)
        {
            this.Id = entity.Id;
            this.Date = entity.WorkingDate != null ? entity.WorkingDate.Value.ToString("dd/MM/yyyy") : null;
            this.Status = entity.WorkingDayStatus.ToString();
            this.TimeIn = entity.Time_In != null ? entity.Time_In.Value.ToString("MM/dd/yyyy HH:mm:ss") : null;
            this.TimeOut = entity.Time_Out != null ? entity.Time_Out.Value.ToString("MM/dd/yyyy HH:mm:ss") : null;
            if (entity.TimeDeviation != null)
            {
                this.TimeDeviation = entity.TimeDeviation;
            }
            else
            {
                if (entity.Time_Out != null && entity.Time_In != null)
                {
                    this.TimeDeviation = (entity.Time_Out.GetValueOrDefault() - entity.Time_In.GetValueOrDefault() - TimeSpan.FromHours(8)).TotalSeconds.Round(0);
                }
            }
            this.Type = entity.WorkingType?.Code;
            //this.RecommendType = entity.RecommendType;
            this.Notes = entity.Notes;
            this.EmployeeId = entity.EmployeeEntityId;

            var workingdayUd = entity.WorkingDayUpdates
                             ?.Where(x => x.DeletedFlag != true && x.WorkingDayApprovals != null && x.WorkingDayApprovals.FirstOrDefault()?.ApproveStatus == Enums.EnumApproveStatus.PENDING)?.ToArray();

            List<WorkingDayItem> ListWorkingDayUpdate = new List<WorkingDayItem>();

            if (workingdayUd != null)
            {
                foreach (var item in workingdayUd)
                {
                    WorkingDayItem workingDayItem = new WorkingDayItem();
                    workingDayItem.Id = item.Id;
                    workingDayItem.Date = item.CreatedAt;
                    ListWorkingDayUpdate.Add(workingDayItem);
                }

                this.ListWorkingDayUpdateId = ListWorkingDayUpdate;
            }

            return this;
        }
        public DetailAttendanceResponse SetData(WorkingDayUpdateEntity entity)
        {
            this.Id = entity.WorkingDayId;
            this.Date = entity.WorkingDate != null ? entity.WorkingDate.Value.ToString("dd/MM/yyyy") : null;
            this.Status = entity.WorkingDayStatus.ToString();
            this.TimeIn = entity.Time_In != null ? entity.Time_In.Value.ToString("MM/dd/yyyy HH:mm:ss") : null;
            this.TimeOut = entity.Time_Out != null ? entity.Time_Out.Value.ToString("MM/dd/yyyy HH:mm:ss") : null;
            if (entity.Time_Deviation == null)
            {
                if (entity.Time_Out != null && entity.Time_In != null)
                {
                    this.TimeDeviation = (entity.Time_Out.GetValueOrDefault() - entity.Time_In.GetValueOrDefault()).TotalSeconds.Round(0);
                }
            }
            else
            {
                this.TimeDeviation = entity.Time_Deviation;
            }
            this.Type = entity.WorkingType?.Code;
            this.Notes = entity.Notes;
            this.EmployeeId = entity.EmployeeId;
            //this.ListWorkingDayUpdateId = new long[1] { entity.Id };
            return this;
        }
        public static List<ColumnResponseModel> AddKeySearchFilterable(List<ColumnResponseModel> datas)
        {
            var properties = JsonPropertyHelper<DetailAttendanceResponse>.GetJsonPropertyNames();

            foreach (var data in datas)
            {
                if (properties.Contains(data.Key))
                {
                    string key = data.Key.ToLower();
                    //* add flag_filterable
                    if (key == nameof(DetailAttendanceResponse.Status).ToLower()
                        || key == "type"
                        )
                        data.Filterable = true;

                }
            }

            return datas;
        }
        public static IQueryable<WorkingDayEntity> PrepareDetailReportWhereQueryFilter(IQueryable<WorkingDayEntity> query, Dictionary<string, object> param)
        {
            var properties = JsonPropertyHelper<DetailAttendanceResponse>.GetJsonPropertyNames();
            properties.RemoveAll(p => p == null);

            var predicate = LinqKit.PredicateBuilder.New<WorkingDayEntity>(true); // Sử dụng thư viện linqkit
            foreach (var property in properties)
            {
                string key = property.ToLower();
                if (param.ContainsKey(key))
                {
                    if (key == "type")
                    {
                        predicate.And(x => x.WorkingTypeEntityId == long.Parse(param[key].ToString()));
                    }
                    else if (key == "status")
                    {
                        var value = (EnumWorkingDayStatus)int.Parse(param[key].ToString());
                        predicate.And(x => x.WorkingDayStatus == value);
                    }
                }
            }
            return query.Where(predicate);
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
                if (key == "type")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.WorkingType.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}"))
                    || EF.Functions.Unaccent(x.WorkingType.Code.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}"))
                    );
                }
                else if (key == "status")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.WorkingDayStatus.ToString().ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                }
                else if (key == "notes")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.Notes.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                }
                if (key == "all")
                {
                    predicate = predicate.Or(x => x.Employee.Name.ToLower().Contains(searchVal)); // employee name
                    predicate = predicate.Or(x => x.Employee.EmployeeCode.ToLower().Contains(searchVal)); // employee code
                    predicate = predicate.Or(x => x.Employee.PhoneNumber.ToLower().Contains(searchVal)); // employee code
                    predicate = predicate.Or(x => x.Employee.Department.Name.ToLower().Contains(searchVal)); // employee code
                    predicate = predicate.Or(x => x.Employee.JobTitle.Name.ToLower().Contains(searchVal)); // employee code
                }
            }
            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                predicate = predicate.Or(x => x.Employee.Name.ToLower().Contains(searchKey)); // employee name
                predicate = predicate.Or(x => x.Employee.EmployeeCode.ToLower().Contains(searchKey)); // employee code
                predicate = predicate.Or(x => x.Employee.PhoneNumber.ToLower().Contains(searchKey)); // employee code
                predicate = predicate.Or(x => x.Employee.Department.Name.ToLower().Contains(searchKey)); // employee code
                predicate = predicate.Or(x => x.Employee.JobTitle.Name.ToLower().Contains(searchKey)); // employee code
            }
            return query.Where(predicate);
        }
        public static IQueryable<WorkingDayEntity> PrepareDetailReportQuerySort(IQueryable<WorkingDayEntity> query, Dictionary<string, long> param)
        {
            var properties = JsonPropertyHelper<DetailAttendanceResponse>.GetJsonPropertyNames();
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


    }

    //Edit 
    public class DetailEditAttendanceResponse
    {
        [JsonProperty("Id")]
        public long? Id { get; set; }
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
        //[DisplayName("Working Day Highlight")]
        [JsonProperty("workingDayHighlight")]
        public EnumWorkingDayHighlight? WorkingDayHighlight { get; set; }
        [JsonProperty("status")]
        [DisplayName("Status")]
        public string? Status { get; set; }

        [JsonProperty("action")]
        [DisplayName("Request")]
        public string? ActionRequest { get; set; }

        [DisplayName("Type")]
        [JsonProperty("type")]
        public string? Type { get; set; }
        [JsonProperty("recommendType")]
        public string? RecommendType { get; set; }
        [JsonProperty("recommendTypeId")]
        public long? recommendTypeId { get; set; }
        [DisplayName("Note")]
        [JsonProperty("notes")]
        public string? Notes { get; set; }
        [JsonProperty("employeeid")]
        public long? EmployeeId { get; set; }
        [JsonProperty("list_workingdayupdateid")]
        public WorkingDayUpdateItem? ListWorkingDayUpdateId { get; set; }

        public class WorkingDayUpdateItem
        {
            public long? Id { get; set; }
            public DateTime? Date { get; set; }
        }
        public override string ToString()
        {
            return $"{this.Id}:{this.Date}:{this.Status.ToString()}:{this.RecommendType}";
        }
        //public DetailEditAttendanceResponse SetData(WorkingDayUpdateEntity entity, Dictionary<long, WorkingTypeEntity> dicWorkingDayId2Type)
        //{
        //    this.Id = entity.Id;
        //    this.Date = entity.WorkingDate != null ? entity.WorkingDate.Value.ToString("dd/MM/yyyy") : null;
        //    this.Status = entity.WorkingDayStatus.ToString();
        //    this.TimeIn = entity.Time_In != null ? entity.Time_In.Value.ToString("MM/dd/yyyy HH:mm:ss") : null;
        //    this.TimeOut = entity.Time_Out != null ? entity.Time_Out.Value.ToString("MM/dd/yyyy HH:mm:ss") : null;
        //    this.ActionRequest = en
        //    if (entity.TimeDeviation != null)
        //    {
        //        if (entity.Time_Out != null && entity.Time_In != null)
        //        {
        //            this.TimeDeviation = (entity.Time_Out.GetValueOrDefault() - entity.Time_In.GetValueOrDefault() - TimeSpan.FromHours(8)).TotalSeconds.Round(0);
        //        }
        //    }
        //    this.Type = entity.WorkingType?.Code;
        //    this.RecommendType = dicWorkingDayId2Type[entity.Id].Code;
        //    this.Notes = entity.Notes;
        //    this.EmployeeId = entity.EmployeeEntityId;
        //    this.ListWorkingDayUpdateId = entity.WorkingDayUpdates.Where(x => x.WorkingDayApprovals.FirstOrDefault().ApproveStatus == Enums.EnumApproveStatus.PENDING)?.Select(x => x.Id).ToArray();
        //    return this;
        //}

        public DetailEditAttendanceResponse SetData(WorkingDayUpdateEntity entity)
        {
            this.Id = entity.WorkingDayId;
            this.Date = entity.WorkingDate != null ? entity.WorkingDate.Value.ToString("dd/MM/yyyy") : null;
            this.Status = entity.WorkingDayStatus.ToString();
            this.ActionRequest = entity.ActionRequest.ToString();
            this.TimeIn = entity.Time_In != null ? entity.Time_In.Value.ToString("MM/dd/yyyy HH:mm:ss") : null;
            this.TimeOut = entity.Time_Out != null ? entity.Time_Out.Value.ToString("MM/dd/yyyy HH:mm:ss") : null;
            if (entity.Time_Deviation == null)
            {
                if (entity.Time_Out != null && entity.Time_In != null)
                {
                    this.TimeDeviation = (entity.Time_Out.GetValueOrDefault() - entity.Time_In.GetValueOrDefault()).TotalSeconds.Round(0);
                }
            }
            else
            {
                this.TimeDeviation = entity.Time_Deviation;
            }
            this.Type = entity.WorkingType?.Code;
            this.Notes = entity.Notes;
            this.EmployeeId = entity.EmployeeId;
            var workingdayUd = entity?.WorkingDayApprovals?.Where(x => x.DeletedFlag != true && x.ApproveStatus == Enums.EnumApproveStatus.PENDING)?.FirstOrDefault();
            if (workingdayUd != null)
            {
                WorkingDayUpdateItem workingDayUpdateItem = new WorkingDayUpdateItem();
                workingDayUpdateItem.Id = workingdayUd.WorkingDayUpdateId;
                workingDayUpdateItem.Date = workingdayUd.CreatedAt;
                this.ListWorkingDayUpdateId = workingDayUpdateItem;
            }
            return this;
        }
        public static List<ColumnResponseModel> AddKeySearchFilterable(List<ColumnResponseModel> datas)
        {
            var properties = JsonPropertyHelper<DetailEditAttendanceResponse>.GetJsonPropertyNames();

            foreach (var data in datas)
            {
                if (properties.Contains(data.Key))
                {
                    string key = data.Key.ToLower();
                    //* add flag_filterable
                    if (key == nameof(DetailEditAttendanceResponse.Status).ToLower()
                        || key == "type"
                        || key == "action"
                        )
                        data.Filterable = true;
                }
            }

            return datas;
        }
        public static IQueryable<WorkingDayUpdateEntity> PrepareDetailReportWhereQueryFilter(IQueryable<WorkingDayUpdateEntity> query, Dictionary<string, object> param)
        {
            var properties = JsonPropertyHelper<DetailEditAttendanceResponse>.GetJsonPropertyNames();
            properties.RemoveAll(p => p == null);

            var predicate = LinqKit.PredicateBuilder.New<WorkingDayUpdateEntity>(true); // Sử dụng thư viện linqkit
            foreach (var property in properties)
            {
                string key = property.ToLower();
                if (param.ContainsKey(key))
                {
                    if (key == "type")
                    {
                        predicate.And(x => x.WorkingTypeId == long.Parse(param[key].ToString()));
                    }
                    else if (key == "status")
                    {
                        var value = (EnumWorkingDayStatus)int.Parse(param[key].ToString());
                        predicate.And(x => x.WorkingDayStatus == value);
                    }
                }
            }
            return query.Where(predicate);
        }
        public static IQueryable<WorkingDayUpdateEntity> PrepareDetailReportWhereQuerySearch(IQueryable<WorkingDayUpdateEntity> query, SearchModel searchModel)
        {
            var predicate = LinqKit.PredicateBuilder.New<WorkingDayUpdateEntity>(true); // Sử dụng thư viện linqkit
            var dicSearch = searchModel.DicSearch;
            var searchKey = searchModel.SearchStr?.ToLower()?.Trim();
            foreach (var search in dicSearch)
            {
                string key = search.Key.ToLower();
                var searchVal = dicSearch[key].ToString()?.ToLower().Trim();
                if (key == "type")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.WorkingType.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}"))
                    || EF.Functions.Unaccent(x.WorkingType.Code.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}"))
                    );
                }
                else if (key == "status")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.WorkingDayStatus.ToString().ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                }
                else if (key == "notes")
                {
                    predicate = predicate.And(x => EF.Functions.Unaccent(x.Notes.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
                }
                if (key == "all")
                {
                    predicate = predicate.Or(x => x.Employee.Name.ToLower().Contains(searchVal)); // employee name
                    predicate = predicate.Or(x => x.Employee.EmployeeCode.ToLower().Contains(searchVal)); // employee code
                    predicate = predicate.Or(x => x.Employee.PhoneNumber.ToLower().Contains(searchVal)); // employee code
                    predicate = predicate.Or(x => x.Employee.Department.Name.ToLower().Contains(searchVal)); // employee code
                    predicate = predicate.Or(x => x.Employee.JobTitle.Name.ToLower().Contains(searchVal)); // employee code
                }
            }
            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                predicate = predicate.Or(x => x.Employee.Name.ToLower().Contains(searchKey)); // employee name
                predicate = predicate.Or(x => x.Employee.EmployeeCode.ToLower().Contains(searchKey)); // employee code
                predicate = predicate.Or(x => x.Employee.PhoneNumber.ToLower().Contains(searchKey)); // employee code
                predicate = predicate.Or(x => x.Employee.Department.Name.ToLower().Contains(searchKey)); // employee code
                predicate = predicate.Or(x => x.Employee.JobTitle.Name.ToLower().Contains(searchKey)); // employee code
            }
            return query.Where(predicate);
        }
        public static IQueryable<WorkingDayUpdateEntity> PrepareDetailReportQuerySort(IQueryable<WorkingDayUpdateEntity> query, Dictionary<string, long> param)
        {
            var properties = JsonPropertyHelper<DetailEditAttendanceResponse>.GetJsonPropertyNames();
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
                    query = pa.Value == -1 ? query.OrderByDescending(x => x.Time_Deviation) : query.OrderBy(x => x.Time_Deviation);
                }
            }
            return query;
        }


    }

}
