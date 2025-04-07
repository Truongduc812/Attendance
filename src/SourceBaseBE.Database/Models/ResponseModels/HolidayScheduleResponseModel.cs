using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;
using iSoft.Common.Models.ResponseModels;
using PRPO.Database.Helpers;
using SourceBaseBE.Database.Models.RequestModels;
using Newtonsoft.Json;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace SourceBaseBE.Database.Models.ResponseModels
{
	public class HolidayScheduleResponseModel : BaseCRUDResponseModel<HolidayScheduleEntity>
	{
		public string Name { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string? Description { get; set; }
		public string? Note { get; set; }
		public ICollection<HolidayWorkingTypeEntity> HolidayWorkingTypes { get; set; }

		public HolidayScheduleResponseModel SetData(HolidayScheduleEntity entity)
		{
			base.SetData(entity);
			this.Name = entity.Name;
			this.StartDate = entity.StartDate;
			this.EndDate = entity.EndDate;
			this.Description = entity.Description;
			this.Note = entity.Note;
			this.HolidayWorkingTypes = entity.HolidayWorkingTypes;

			return this;
		}

		public override List<object> SetData(List<HolidayScheduleEntity> listEntity)
		{
			List<Object> listRS = new List<object>();
			foreach (HolidayScheduleEntity entity in listEntity)
			{
				listRS.Add(new HolidayScheduleResponseModel().SetData(entity));
			}
			return listRS;
		}



		public static List<ColumnResponseModel> AddKeySearchFilterable(List<ColumnResponseModel> datas)
		{
			var properties = JsonPropertyHelper<HolidayScheduleResponseModel>.GetJsonPropertyNames();

			foreach (var data in datas)
			{
				if (properties.Contains(data.Key))
				{
					string key = data.Key.ToLower();
					//* add flag_searchable
					if (key == nameof(HolidayScheduleResponseModel.Name).ToLower()) ;
					data.Searchable = true;
				}
			}

			return datas;
		}

		//* prepare query list with where clause
		public static IQueryable<HolidayScheduleEntity> PrepareWhereQueryFilter(IQueryable<HolidayScheduleEntity> query, Dictionary<string, object> param)
		{
			var properties = JsonPropertyHelper<HolidayScheduleResponseModel>.GetJsonPropertyNames();
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


		public static IQueryable<HolidayScheduleEntity> PrepareQuerySort(IQueryable<HolidayScheduleEntity> query, Dictionary<string, long> param)
		{
			var properties = JsonPropertyHelper<HolidayScheduleResponseModel>.GetJsonPropertyNames();
			properties.RemoveAll(p => p == null);
			foreach (var pa in param)
			{
				query = pa.Value == 1 ? query.OrderBy(x => x.GetType().GetProperty(pa.Key)) : query.OrderByDescending(x => x.GetType().GetProperty(pa.Key));
			}
			return query;
		}
	}
}
