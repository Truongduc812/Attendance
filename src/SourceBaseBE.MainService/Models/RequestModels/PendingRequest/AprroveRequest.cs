using iSoft.Common.Enums;
using iSoft.Common.Models.RequestModels;
using iSoft.Common.Utils;
using Newtonsoft.Json;
using SourceBaseBE.Database.Entities;
using SourceBaseBE.Database.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SourceBaseBE.MainService.Models.RequestModels.Report
{
	public class AprroveRequest : PagingFilterRequestModel
	{
		[JsonProperty("departmentId")]
		public int? DepartmentId { get; set; }
        public override string GetKeyCache()
        {
            string rs = "";
            if (DepartmentId != null)
            {
                rs = string.Join(",", DepartmentId);
            }
            rs += base.GetKeyCache();

            return EncodeUtil.MD5(rs);
        }
    }
	public class PersonalPendingRequest : PagingFilterRequestModel
	{
		[JsonProperty("employeeeId")]
		public long? EmployeeId { get; set; }
        public override string GetKeyCache()
        {
            string rs = "";
            if (EmployeeId != null)
            {
                rs = string.Join(",", EmployeeId);
            }
            rs += base.GetKeyCache();

            return EncodeUtil.MD5(rs);
        }
    }
    public class HistoricalPendingRequest : PersonalPendingRequest
    {
        [Column("list_department")]
        public string? List_Department { get; set; }
        public override string GetKeyCache()
        {
            string rs = "";
            if (List_Department != null)
            {
                rs = string.Join(",", List_Department);
            }
            rs += base.GetKeyCache();

            return EncodeUtil.MD5(rs);
        }

        //public List<long>? ListDepartmentId => List_Department?.Split(",")?.Select(x => long.Parse(x))?.ToList();
    }
    public class EditPersonPendingRequest
	{
		[Column("list_workingdayupdateid")]
		public List<long>? ListWorkingDayUpdateId { get; set; }
		public long? UserId { get; set; }
		[JsonProperty("status")]
		public EnumApproveStatus? Status { get; set; }
		[JsonProperty("note")]
		public string? Note { get; set; }
		[Column("approve_reason")]
		public string? ApproveReason { get; set; }
	}
    public class EditTimeSheetPendingRequest
    {
        [Column("list_timesheetupdateid")]
        public List<long>? ListTimeSheetUpdateId { get; set; }
        public long? UserId { get; set; }
        [JsonProperty("status")]
        public EnumApproveStatus? Status { get; set; }
        [JsonProperty("note")]
        public string? Note { get; set; }
        [Column("approve_reason")]
        public string? ApproveReason { get; set; }
    }
    public class ExportEmployeePendingRequest : PagingFilterRequestModel
	{
		public long? EmployeeId { get; set; }

	}
    public class DeleteTimeSheetApprovalRequest
    {
        [Column("list_timesheet_approval_id")]
        public List<long>? ListTimeSheetApprovalId { get; set; }
    }

    public class DeleteWorkingdayApprovalRequest
    {
        [Column("list_workingday_approval_id")]
        public List<long>? ListWorkingdayApprovalId { get; set; }
    }

}
