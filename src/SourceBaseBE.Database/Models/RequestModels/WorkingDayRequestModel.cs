using iSoft.Common.Utils;
using Microsoft.AspNetCore.Http;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Models.RequestModels
{
	public class WorkingDayRequestModel : BaseCRUDRequestModel<WorkingDayEntity>
	{
		public DateTime? working_date { get; set; }
		public DateTime? time_in { get; set; }
		public DateTime? time_out { get; set; }
		public double? TimeDeviation { get; set; }
		public EnumWorkingDayStatus? WorkingDayStatus { get; set; }
		public string? Notes { get; set; }
		public DateTime? OriginalWorkDate { get; set; }
		public DateTime? OriginTimeIn { get; set; }
		public DateTime? OriginTimeOut { get; set; }
		public long? OriginTimeDeviation { get; set; }
		public EnumWorkingDayStatus? OriginWorkingDayStatus { get; set; }
		public WorkingTypeEntity? OriginWorkingType { get; set; }
		public long? EmployeeEntityId { get; set; }
		public EmployeeEntity? ItemEmployeeEntity { get; set; }
        public IFormFile? FileImport { get; set; }
        public bool? IsReplace { get; set; }

        public WorkingDayEntity GetEntity()
		{
			var ret = new WorkingDayEntity();
			if (this.Id != null) ret.Id = (long)this.Id;
			if (this.Order != null) ret.Order = this.Order;
			if (this.working_date != null) ret.WorkingDate = this.working_date;
			if (this.time_in != null) ret.Time_In = this.time_in;
			if (this.time_out != null) ret.Time_Out = this.time_out;
			if (this.TimeDeviation != null) ret.TimeDeviation = this.TimeDeviation.GetValueOrDefault();
			if (this.WorkingDayStatus != null) ret.WorkingDayStatus = this.WorkingDayStatus;
			if (this.Notes != null) ret.Notes = this.Notes;
			if (this.EmployeeEntityId != null) ret.EmployeeEntityId = this.EmployeeEntityId;
			if (this.ItemEmployeeEntity != null) ret.Employee = this.ItemEmployeeEntity;
			return ret;
		}

        public Dictionary<string, (string, IFormFile)> GetFiles()
        {
            Dictionary<string, (string, IFormFile)> dicRS = new Dictionary<string, (string, IFormFile)>();
            if (this.FileImport != null)
            {
                dicRS.Add(nameof(FileImport), (Path.Combine(ConstFolderPath.Image, ConstFolderPath.Upload), this.FileImport));
            }
            /*[GEN-17]*/
            return dicRS;
        }
    }
}
