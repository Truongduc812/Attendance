using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.ComponentModel;
using PRPO.Database.Helpers;
using SourceBaseBE.Database.Models.RequestModels;
using iSoft.Common.Models.ResponseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SourceBaseBE.Database.Models.ResponseModels
{
	public class WorkingTypeListReponseModel
	{
		[DisplayName("Name")]
		[JsonProperty("name")]
		public string? Name { get; set; }

		[DisplayName("Ký hiệu")]
		[JsonProperty("code")]
		public string Code { get; set; }

		[DisplayName("Meal")]
		[JsonProperty("meal")]
		public int? Normal_Meal { get; set; }

		[DisplayName("OT 150%")]
		[JsonProperty("ot_150")]
		public int? OT_150 { get; set; }

		[DisplayName("NS 30% ")]
		[JsonProperty("ot_night_30")]
		public int? OT_Night_30 { get; set; }

		[DisplayName("OT 200%")]
		[JsonProperty("ot_200")]
		public int? OT_200 { get; set; }


		[DisplayName("Weekend OT 200%")]
		[JsonProperty("weekend_ot_200")]
		public int? Weekend_OT_200 { get; set; }

		[DisplayName("OT NS 270%")]
		[JsonProperty("Weekend_Night_OT_270")]
		public int? Weekend_Night_OT_270 { get; set; }

		[DisplayName("OT 300%")]
		[JsonProperty("holiday_ot_300")]
		public int? Holiday_OT_300 { get; set; }

		[DisplayName("OT NS 390%")]
		[JsonProperty("holiday_ot_night_390")]
		public int? Holiday_OT_Night_390 { get; set; }

		public WorkingTypeListReponseModel SetData(WorkingTypeEntity entity)
		{
			if (entity == null) throw new ArgumentNullException("Param Entity workingType Null");
			this.Name = entity.Name;
			this.Code = entity.Code;
			this.Normal_Meal = entity.Normal_Meal;
			this.OT_150 = entity.OT_150;
			this.OT_Night_30 = entity.Normal_Night_30;
			this.OT_200 = entity.OT_200;
			this.Weekend_OT_200 = entity.Weekend_OT_200;
			this.Weekend_Night_OT_270 = entity.Weekend_Night_OT_270;
			this.Holiday_OT_300 = entity.Holiday_OT_300;
			this.Holiday_OT_Night_390 = entity.Holiday_OT_Night_390;

			return this;
		}

		//* prepare query list with where clause
		public static IQueryable<WorkingTypeEntity> PrepareWhereQueryFilter(IQueryable<WorkingTypeEntity> query, Dictionary<string, object> param)
		{
			var properties = JsonPropertyHelper<WorkingTypeListReponseModel>.GetJsonPropertyNames();
			properties.RemoveAll(p => p == null);

			var predicate = LinqKit.PredicateBuilder.New<WorkingTypeEntity>(true); // Sử dụng thư viện linqkit
			foreach (var property in properties)
			{
				string key = property.ToLower();
				if (param.ContainsKey(key))
				{

				}
			}
			return query.Where(predicate);
		}

		public static IQueryable<WorkingTypeEntity> PrepareWhereQuerySearch(IQueryable<WorkingTypeEntity> query, SearchModel searchModel)
		{
			var predicate = LinqKit.PredicateBuilder.New<WorkingTypeEntity>(true); // Sử dụng thư viện linqkit
			var dicSearch = searchModel.DicSearch;
			var searchKey = searchModel.SearchStr?.ToLower()?.Trim();
			foreach (var search in dicSearch)
			{
				string key = search.Key.ToLower();
				var searchVAl = dicSearch[key]?.ToLower()?.Trim();
				if (key == "name")
				{
					predicate = predicate.And(x => EF.Functions.Unaccent(x.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVAl}")));
				}
				else if (key == "code")
				{
					predicate = predicate.And(x => EF.Functions.Unaccent(x.Code.ToLower()).Contains(EF.Functions.Unaccent($"{searchVAl}")));
				}
				else if (key == "all")
				{
					predicate = predicate.Or(x => EF.Functions.Unaccent(x.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchVAl.ToLower()}")));
					predicate = predicate.Or(x => EF.Functions.Unaccent(x.Code.ToLower()).Contains(EF.Functions.Unaccent($"{searchVAl.ToLower()}")));
				}
			}
			if (!string.IsNullOrWhiteSpace(searchKey))
			{
				predicate = predicate.Or(x => EF.Functions.Unaccent(x.Name.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey.ToLower()}")));
				predicate = predicate.Or(x => EF.Functions.Unaccent(x.Code.ToLower()).Contains(EF.Functions.Unaccent($"{searchKey.ToLower()}")));

			}
			return query.Where(predicate).AsQueryable();
		}

		public static IQueryable<WorkingTypeEntity> PrepareQuerySort(IQueryable<WorkingTypeEntity> query, Dictionary<string, long> param)
		{
			var properties = JsonPropertyHelper<WorkingTypeListReponseModel>.GetJsonPropertyNames();
			properties.RemoveAll(p => p == null);
			foreach (var pa in param)
			{
				if (pa.Key == "name")
				{
					query = pa.Value == -1 ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
				}
				else if (pa.Key == "code")
				{
					query = pa.Value == -1 ? query.OrderByDescending(x => x.Code) : query.OrderBy(x => x.Code);
				}
				else if (pa.Key == "meal")
				{
					query = pa.Value == -1 ? query.OrderByDescending(x => x.Normal_Meal) : query.OrderBy(x => x.Normal_Meal);
				}
				else if (pa.Key == "ot_150")
				{
					query = pa.Value == -1 ? query.OrderByDescending(x => x.OT_150) : query.OrderBy(x => x.OT_150);
				}
				else if (pa.Key == "ot_night_30")
				{
					query = pa.Value == -1 ? query.OrderByDescending(x => x.Normal_Night_30) : query.OrderBy(x => x.Normal_Night_30);
				}
				else if (pa.Key == "ot_200")
				{
					query = pa.Value == -1 ? query.OrderByDescending(x => x.OT_200) : query.OrderBy(x => x.OT_200);
				}
				else if (pa.Key == "weekend_ot_200")
				{
					query = pa.Value == -1 ? query.OrderByDescending(x => x.Weekend_OT_200) : query.OrderBy(x => x.Weekend_OT_200);
				}
				else if (pa.Key == "Weekend_Night_OT_270")
				{
					query = pa.Value == -1 ? query.OrderByDescending(x => x.Weekend_Night_OT_270) : query.OrderBy(x => x.Weekend_Night_OT_270);
				}
				else if (pa.Key == "holiday_ot_300")
				{
					query = pa.Value == -1 ? query.OrderByDescending(x => x.Holiday_OT_300) : query.OrderBy(x => x.Holiday_OT_300);
				}
				else if (pa.Key == "holiday_ot_night_390")
				{
					query = pa.Value == -1 ? query.OrderByDescending(x => x.Holiday_OT_Night_390) : query.OrderBy(x => x.Holiday_OT_Night_390);
				}
			}
			return query;
		}

		public static List<ColumnResponseModel> AddKeySearchFilterable(List<ColumnResponseModel> datas)
		{
			var properties = JsonPropertyHelper<WorkingTypeListReponseModel>.GetJsonPropertyNames();

			foreach (var data in datas)
			{
				if (properties.Contains(data.Key))
				{
					string key = data.Key.ToLower();
					//* add flag_filterable
					//if (key == nameof(WorkingTypeListReponseModel.Name).ToLower())
					//  data.Filterable = true;
					//* add flag_searchable
					if (key == nameof(WorkingTypeListReponseModel.Name).ToLower()
					  || key == nameof(WorkingTypeListReponseModel.Code).ToLower())
						data.Searchable = true;
				}
			}

			return datas;
		}

	}
}
