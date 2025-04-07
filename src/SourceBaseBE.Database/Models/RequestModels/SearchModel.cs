using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.Models.RequestModels
{
	public class SearchModel
	{
		public string? SearchStr { get; set; } = null;
		public Dictionary<string, string>? DicSearch { get; set; } = new Dictionary<string, string>();
	}
}
