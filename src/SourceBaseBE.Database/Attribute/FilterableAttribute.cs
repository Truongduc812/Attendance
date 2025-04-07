
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Security.Policy;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
namespace SourceBaseBE.Database.Attribute
{
	public class FilterableAttribute : System.Attribute
	{
		public string KeyDisplay { get; set; }
		public string KeyFilter { get; set; }
		public bool Displayable { get; set; }
		public KeyValuePair<string, string> DicDisPlayFilter => new KeyValuePair<string, string>(KeyDisplay, KeyFilter);
		//
		// Summary:
		//     Initializes a new instance of the System.ComponentModel.DisplayNameAttribute
		//     class using the display name.
		//
		// Parameters:
		//   displayName:
		//     The display name.
		public FilterableAttribute(string displayName, string filter, bool displayable)
		{
			this.KeyDisplay = displayName;
			this.KeyFilter = filter;
			Displayable = displayable;
		}
		public FilterableAttribute(string displayName, string filter)
		{
			this.KeyDisplay = displayName;
			this.KeyFilter = filter;
			Displayable = false;
		}
	}
}
