using Newtonsoft.Json;

namespace iSoft.Common.Models.ResponseModels
{
	public class ColumnResponseModel
	{
		public ColumnResponseModel()
		{
			this.Filterable = false;
			this.Searchable = false;
			Displayable = true;
		}

		[JsonProperty("key")]
		public string? Key { get; set; }

		[JsonProperty("displayName")]
		public string? DisplayName { get; set; }

		[JsonProperty("searchable")]
		public bool? Searchable { get; set; }

		[JsonProperty("filterable")]
		public bool? Filterable { get; set; }
		[JsonProperty("displayable")]
		public bool? Displayable { get; set; }
	}
}
