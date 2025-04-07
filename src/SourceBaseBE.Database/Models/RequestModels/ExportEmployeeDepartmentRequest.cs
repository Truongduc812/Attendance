using iSoft.Common.Enums;
using iSoft.Common.Models.RequestModels;
using Newtonsoft.Json;
using SourceBaseBE.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SourceBaseBE.MainService.Models.RequestModels.Report
{
	public class ExportEmployeeDepartmentRequest
  {
		[Column("list_department")]
		public string? List_Department { get; set; }

		[JsonProperty("dateFrom")]
		public DateTime DateFrom { get; set; }
		[JsonProperty("dateTo")]
		public DateTime DateTo { get; set; }
		public List<long>? ListDepartmentId => List_Department?.Split(",")?.Select(x => long.Parse(x))?.ToList();
	}
	 
}
