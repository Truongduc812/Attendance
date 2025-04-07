using OfficeOpenXml.FormulaParsing;
using SourceBaseBE.Database.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.Models.ResponseModels
{
	public class LogActionReponse
	{
		[Filterable("Date", "date", true)]
		public string Date { get; set; }
		[Filterable("Time", "time", true)]
		public string Time { get; set; }
		[Filterable("Name", "name", true)]
		public string Name { get; set; }
		[Filterable("Action", "action", true)]
		public string Action { get; set; }
		[Filterable("Target", "target", true)]
		public string Target { get; set; }
		[Filterable("Status", "status", true)]
		public string Status { get; set; }
	}
}
