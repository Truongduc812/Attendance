using iSoft.Database.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SourceBaseBE.Database.Interfaces;

namespace SourceBaseBE.Database.Entities
{
	public class LimitationEntity : BaseCRUDEntity, IEnityIsEnabled
	{
		public LimitationEntity()
		{

		}

		[MaxLength(255)]
		[Column(TypeName = "VARCHAR(255)")]
		public string? UpperLimit { get; set; }

		[MaxLength(255)]
		[Column(TypeName = "VARCHAR(255)")]
		public string? TargetValue { get; set; }

		[MaxLength(255)]
		[Column(TypeName = "VARCHAR(255)")]
		public string? LowerLimit { get; set; }

		[DefaultValue(true)]
		public bool IsEnabled { get; set; }

		[ForeignKey(nameof(ParameterEntity))]
		public long? ParameterId { get; set; }
		public ParameterEntity? Parameter { get; set; }
	}
}