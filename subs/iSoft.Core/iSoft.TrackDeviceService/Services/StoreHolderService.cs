using ConnectionCommon.Connection;
using iSoft.ConnectionCommon.Model;
using iSoft.TrackDeviceService.Context;
using iSoft.TrackDeviceService.Helpers;
using iSoft.TrackDeviceService.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iSoft.TrackDeviceService.Services
{
	public class StoreHolderService : BaseService
	{
		private readonly StoreHolder StoreHolder;
		public event StoreDataChange OnStoreDataChange;
		public StoreHolderService(StoreHolder storeHolder)
		{
			this.StoreHolder = storeHolder;
		}
		public async Task<StoreHolder> InitConnectionDatas(List<Connection> connections)
		{
			this.StoreHolder.ConnectionDatas = new List<Models.Store>();
			this.StoreHolder.ConnectionDatas.AddRange(connections.Select(x => new Models.Store()
			{
				ConnectionId = x.Id

			}));
			return this.StoreHolder;
		}
		public async Task<List<EnvData>> GetDataByConnectionId(ulong connectionId)
		{
			var data = StoreHolder.ConnectionDatas.Where(x => x.ConnectionId == connectionId).FirstOrDefault();
			return data.Data as List<EnvData>;
		}
		public async Task UpdateData(ulong connectionId, object dataSet)
		{
			var data = StoreHolder.ConnectionDatas.Where(x => x.ConnectionId == connectionId).FirstOrDefault();
			if (data == null)
				throw new System.Exception($"ConnectionId {data.ConnectionId} NOT FOUND");
			data.Data = dataSet;
			OnStoreDataChange?.Invoke(data.ConnectionId, data.Data as iMagPayloadMessage);
		}
	}
}
