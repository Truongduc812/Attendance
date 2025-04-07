using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace iSoft.DBLibrary.Entities
{
	public class BaseEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public ulong Id { get; set; }
		public virtual bool? isDelete { get; set; }
		public virtual bool? isEnable { get; set; }
		[JsonProperty("createdAt")]
		[NotNull]
		public DateTime? CreatedAt { get; set; } = DateTime.Now;
		[JsonProperty("updatedAt")]
		[NotNull]
		public DateTime? UpdatedAt { get; set; } = DateTime.Now;
	}
}