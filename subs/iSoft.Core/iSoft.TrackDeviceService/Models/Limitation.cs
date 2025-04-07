using ConnectionCommon.Connection;
using iSoft.Common;
using iSoft.DBLibrary.Entities;
using iSoft.DBLibrary.Enums;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace iSoft.TrackDeviceService.Models
{
	public class Limitation : BaseEntity
	{
		public Limitation()
		{

		}

		[MaxLength(255)]
		[Column(TypeName = "NVARCHAR(255)")]
		public string? UpperLimit { get; set; }

		[MaxLength(255)]
		[Column(TypeName = "NVARCHAR(255)")]
		public string? TargetValue { get; set; }

		[MaxLength(255)]
		[Column(TypeName = "NVARCHAR(255)")]
		public string? LowerLimit { get; set; }
		[MaxLength(255)]
		[NotNull]
		[Column(TypeName = "NVARCHAR(255)")]
		public string Name { get; set; }
		[Column(TypeName = "NVARCHAR(255)")]
		[MaxLength(255)]
		public string SerialCode { get; set; }

		[DefaultValue(true)]
		public bool IsEnabled { get; set; }

		[ForeignKey(nameof(Param))]
		public long? ParamId { get; set; }
		public Param? Param { get; set; }
		public ulong CreatedBy { get; set; }
		public ulong UpdatedBy { get; set; }
	}
}