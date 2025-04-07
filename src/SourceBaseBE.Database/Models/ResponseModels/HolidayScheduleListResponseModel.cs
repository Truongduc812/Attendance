using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;
using iSoft.Common.Models.ResponseModels;
using PRPO.Database.Helpers;
using SourceBaseBE.Database.Models.RequestModels;
using Newtonsoft.Json;
using System.ComponentModel;
using iSoft.Common.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using SourceBaseBE.Database.Attribute;

namespace SourceBaseBE.Database.Models.ResponseModels
{
	public class HolidayScheduleListResponseModel
	{
		[DisplayName("Id")]
		[JsonProperty("id")]
		[Filterable("Id", "id", false)]
		public long? Id { get; set; }

		[DisplayName("Name")]
		[JsonProperty("name")]
		public string? Name { get; set; }

		[DisplayName("Start Date")]
		[JsonProperty("startdate")]
		public string? StartDate { get; set; }

		[DisplayName("End Date")]
		[JsonProperty("enddate")]
		public string? EndDate { get; set; }


		[DisplayName("Symbol")]
		[JsonProperty("holidaytype")]
		public string? HolidayType { get; set; }
		public string? Note { get; set; }
		public ICollection<HolidayWorkingTypeEntity>? HolidayWorkingTypes { get; set; }

		public HolidayScheduleListResponseModel SetData(HolidayScheduleEntity entity)
		{
			if (entity == null) throw new ArgumentNullException("Param Entity HolidayScheduleListResponseModel Null");
			this.Name = entity?.Name;
			this.StartDate = entity?.StartDate.ToString("dd/MM/yyyy");
			this.EndDate = entity?.EndDate.ToString("dd/MM/yyyy");
			this.Note = entity?.Note;
			this.HolidayType = entity?.HolidayType.ToString();
			this.Id = entity?.Id;
			//this.HolidayWorkingTypes = entity?.HolidayWorkingTypes;
			return this;
		}


		public static List<ColumnResponseModel> AddKeySearchFilterable(List<ColumnResponseModel> datas)
		{
			var properties = JsonPropertyHelper<HolidayScheduleListResponseModel>.GetJsonPropertyNames();

			foreach (var data in datas)
			{
				if (properties.Contains(data.Key))
				{
					string key = data.Key.ToLower();
					//* add flag_searchable
					if (key == nameof(HolidayScheduleListResponseModel.Name).ToLower())
					{
            data.Searchable = true;
          } 
				}
			}

			return datas;
		}

		//* prepare query list with where clause
		public static IQueryable<HolidayScheduleEntity> PrepareWhereQueryFilter(IQueryable<HolidayScheduleEntity> query, Dictionary<string, object> param)
		{
			var properties = JsonPropertyHelper<HolidayScheduleListResponseModel>.GetJsonPropertyNames();
			properties.RemoveAll(p => p == null);

			var predicate = LinqKit.PredicateBuilder.New<HolidayScheduleEntity>(true); // Sử dụng thư viện linqkit
			foreach (var property in properties)
			{
				string key = property.ToLower();
				if (param.ContainsKey(key))
				{
					if (key == "name")
					{
						predicate.Or(x => x.Name == param[key].ToString());
					}
				}
			}
			return query.Where(predicate);
		}


		public static IQueryable<HolidayScheduleEntity> PrepareWhereQuerySearch(IQueryable<HolidayScheduleEntity> query, SearchModel searchModel)
		{
			var predicate = LinqKit.PredicateBuilder.New<HolidayScheduleEntity>(true); // Sử dụng thư viện linqkit
			var dicSearch = searchModel.DicSearch;
			var searchKey = searchModel.SearchStr?.ToLower().Trim();
			foreach (var search in dicSearch)
			{
				string key = search.Key.ToLower();
				var searchVal = dicSearch[key]?.ToLower().Trim();
				if (key == "name")
				{
					predicate = predicate.And(x => EF.Functions.Unaccent(x.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}"))); 
        }
				if (key == "all")
				{
					predicate = predicate.Or(x => EF.Functions.Unaccent(x.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVal}")));
				}
			}
			if (!string.IsNullOrWhiteSpace(searchKey))
			{
				predicate = predicate.Or(x => EF.Functions.Unaccent(x.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey}")));
			}
			return query.Where(predicate).AsQueryable();
		}
		public static IQueryable<HolidayScheduleEntity> PrepareQuerySort(IQueryable<HolidayScheduleEntity> query, Dictionary<string, long> param)
		{
			var properties = JsonPropertyHelper<HolidayScheduleListResponseModel>.GetJsonPropertyNames();
			properties.RemoveAll(p => p == null);
			foreach (var pa in param)
			{
        if (pa.Key == "name")
        {
          query = pa.Value == -1 ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
        }
        else if (pa.Key == "startdate")
        {
          query = pa.Value == -1 ? query.OrderByDescending(x => x.StartDate) : query.OrderBy(x => x.StartDate);
        }
        else if (pa.Key == "enddate")
        {
          query = pa.Value == -1 ? query.OrderByDescending(x => x.EndDate) : query.OrderBy(x => x.EndDate);
        }
        else if (pa.Key == "holidaytype")
        {
          query = pa.Value == -1 ? query.OrderByDescending(x => x.HolidayType) : query.OrderBy(x => x.HolidayType);
        }
      }
			return query;


       
    }
  }
}
