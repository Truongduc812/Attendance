using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using iSoft.Common.Models.ResponseModels;
using iSoft.Database.Extensions;
using Newtonsoft.Json;
using SourceBaseBE.Database.Attribute;
namespace PRPO.Database.Helpers
{
	public static class JsonPropertyHelper<T>
	{
		public static List<string> GetJsonPropertyNames()
		{
			var props = typeof(T).GetProperties();

			var results = props.Select(p =>
			{
				var jsonProp = Attribute.GetCustomAttribute(p, typeof(JsonPropertyAttribute)) as JsonPropertyAttribute;
				var display = Attribute.GetCustomAttribute(p, typeof(DisplayNameAttribute)) as DisplayNameAttribute;
				return jsonProp != null ? jsonProp.PropertyName.ToLower() : null;
			}).ToList();

			return results;
		}
		public static List<string> GetJsonPropertyNames(bool isDisplay)
		{
			var props = typeof(T).GetProperties();

			var results = props.Select(p =>
			{
				var jsonProp = Attribute.GetCustomAttribute(p, typeof(JsonPropertyAttribute)) as JsonPropertyAttribute;
				return jsonProp != null ? jsonProp.PropertyName.ToLower() : null;
			}).ToList();

			return results;
		}
		public static string GetJsonPropertyName(string property)
		{
			var props = typeof(T).GetProperty(property);
			var attribute = Attribute.GetCustomAttribute(props, typeof(JsonPropertyAttribute)) as JsonPropertyAttribute;
			return attribute != null ? attribute.PropertyName.ToLower() : null; ;
		}
		public static List<string> GetDisplayNames()
		{
			var props = typeof(T).GetProperties();

			var results = props.Select(p =>
			{
				var attribute = Attribute.GetCustomAttribute(p, typeof(DisplayNameAttribute)) as DisplayNameAttribute;
				return attribute != null ? attribute.DisplayName : null;
			}).ToList();

			return results;
		}
		public static Dictionary<string, ColumnResponseModel> GetFilterProperties()
		{
			var props = typeof(T).GetProperties();
			var ret = new Dictionary<string, ColumnResponseModel>();
			foreach (var prop in props)
			{
				var attributeFilterable = Attribute.GetCustomAttribute(prop, typeof(FilterableAttribute)) as FilterableAttribute;
				var attributeJson = Attribute.GetCustomAttribute(prop, typeof(JsonPropertyAttribute)) as JsonPropertyAttribute;
				var attributeDisplay = Attribute.GetCustomAttribute(prop, typeof(DisplayNameAttribute)) as DisplayNameAttribute;
				if (attributeFilterable != null && attributeFilterable.KeyFilter != null)
				{
					ret.Add(attributeFilterable.KeyDisplay, new ColumnResponseModel()
					{
						Displayable = attributeFilterable.Displayable,
						DisplayName = attributeFilterable.KeyDisplay,
						Key = attributeFilterable.KeyFilter,
					});
				}
				else if (attributeJson != null)
				{
					if (ret.ContainsKey(attributeJson.PropertyName.ToLower()) || ret.ContainsKey(attributeJson.PropertyName))
						continue;
					ret.Add(attributeJson.PropertyName.ToLower(), new ColumnResponseModel()
					{
						Displayable = attributeDisplay != null,
						DisplayName = attributeDisplay?.DisplayName,
						Key = attributeJson.PropertyName.ToLower(),
					});
				}
			}

			return ret;
		}
		public static string GetFilterProperty(string name)
		{
			var prop = typeof(T).GetProperty(name);
			var attribute = Attribute.GetCustomAttribute(prop, typeof(FilterableAttribute)) as FilterableAttribute;
			return attribute?.KeyFilter;
		}
		public static string GetDisplayName(string property)
		{
			var props = typeof(T).GetProperty(property);
			var attribute = Attribute.GetCustomAttribute(props, typeof(DisplayNameAttribute)) as DisplayNameAttribute;
			return attribute != null ? attribute.DisplayName : null; ;
		}
		public static string GetFormDisplayName(string property)
		{
			var prop = typeof(T).GetProperty(property);
			var attribute = Attribute.GetCustomAttribute(prop, typeof(FormDataTypeAttribute)) as FormDataTypeAttribute;
			return attribute != null ? attribute.Title : null; ;
		}
	}
}