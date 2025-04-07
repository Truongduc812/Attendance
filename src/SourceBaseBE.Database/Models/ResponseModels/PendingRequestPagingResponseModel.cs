using iSoft.Common.Models.ResponseModels;
using Newtonsoft.Json;
using SourceBaseBE.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.Models.ResponseModels
{
    public class PendingRequestPagingResponseModel : PagingWithColumnsResponseModel
    {
        [JsonProperty("listData", NullValueHandling = NullValueHandling.Ignore)]
        public new List<WorkingdayUpdateDTO> ListData { get; set; }
    }
    public class TimeSheetPendingRequestPagingResponseModel : PagingWithColumnsResponseModel
    {
        [JsonProperty("listData", NullValueHandling = NullValueHandling.Ignore)]
        public new List<TimeSheetUpdateDTO> ListData { get; set; }
    }
    public class PersonalPendingRequestPagingResponseModel : PagingWithColumnsResponseModel
    {
        [JsonProperty("listData", NullValueHandling = NullValueHandling.Ignore)]
        public new List<DetailWorkingdayUpdateDTO> ListData { get; set; }
        [JsonProperty("employeeInfo", NullValueHandling = NullValueHandling.Ignore)]
        public List<Dictionary<string, object>> EmployeeInformation { get; set; }
    }
    public class HistoricalPendingRequestPagingResponseModel : PagingWithColumnsResponseModel
    {
        [JsonProperty("listData", NullValueHandling = NullValueHandling.Ignore)]
        public new List<HistoricalWorkingdayUpdateDTO> ListData { get; set; }
        [JsonProperty("employeeInfo", NullValueHandling = NullValueHandling.Ignore)]
        public List<Dictionary<string, object>> EmployeeInformation { get; set; }
    }
    public class PersonalTimeSheetPendingRequestPagingResponseModel : PagingWithColumnsResponseModel
    {
        [JsonProperty("listData", NullValueHandling = NullValueHandling.Ignore)]
        public new List<DetailTimeSheetUpdateDTO> ListData { get; set; }
    }
    public class HistoricalTimeSheetPendingRequestPagingResponseModel : PagingWithColumnsResponseModel
    {
        [JsonProperty("listData", NullValueHandling = NullValueHandling.Ignore)]
        public new List<HistoricalTimeSheetUpdate> ListData { get; set; }
    }
}
