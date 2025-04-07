using ConnectionCommon.Connection;
using iSoft.TrackDeviceService.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iSoft.TrackDeviceService.Services
{
	public class TrackDeviceConfigService: BaseService
	{
		private readonly ConfigRepository configRepository;
		private readonly ConnectionParamRepository connectionParamRepository;
		private readonly ParamRepository paramRepository;
		private readonly ConnectionRepository connectionRepository;
		private readonly ConnectionConfigRepository connectionConfigRepository;
		public TrackDeviceConfigService()
		{
			this.configRepository = new ConfigRepository();
			this.connectionParamRepository = new ConnectionParamRepository();
			this.paramRepository = new ParamRepository();
			this.connectionRepository = new ConnectionRepository();
			this.connectionConfigRepository = new ConnectionConfigRepository();
		}

		#region Config
		public Task<IEnumerable<Config>> GetConnConfigs()
		{
			return configRepository.GetAllAsync();
		}
		public Task<Config> GetConnConfigById(long id)
		{
			return configRepository.GetByIdAsync(id);
		}

		#endregion

		#region ConnectionParam
		public Task<IEnumerable<ConnectionParam>> GetConnectionParams()
		{
			return connectionParamRepository.GetAllAsync();
		}
		public Task<ConnectionParam> GetConnectionParamById(long id)
		{
			return connectionParamRepository.GetByIdAsync(id);
		}

		#endregion

		#region Param
		public Task<IEnumerable<Param>> GetParams()
		{
			return paramRepository.GetAllAsync();
		}
		public Task<Param> GetParamById(long id)
		{
			return paramRepository.GetByIdAsync(id);
		}
		#endregion

		#region Connection
		public Task<IEnumerable<Connection>> GetConnections()
		{
			return connectionRepository.GetAllAsync();
		}
		public async Task<List<Connection>> GetConnectionsWithRelationByCommGroup(eComGroup comGroup)
		{
			return await connectionRepository
				.Context.Set<Connection>()
				.Include(x => x.ConnectionConfigs)
				 .ThenInclude(x => x.Config)
				.Include(x => x.ConnectionParams)
				.ThenInclude(x => x.Param)
				.AsNoTracking()
				.AsSingleQuery()
				.Where(x => x.CommGroup == comGroup)
				.ToListAsync();
		}
		public async Task<bool> SaveConfigs(List<Connection> connections)
		{
			foreach (var connection in connections)
			{
				await connectionRepository.UpsertAsync((long)connection.Id, connection);
			}
			return true;
		}
		public async Task<Connection> GetRealtimeConfig()
		{
			return await connectionRepository
				.Context.Set<Connection>()
				.Include(x => x.ConnectionConfigs)
				 .ThenInclude(x => x.Config)
				.Include(x => x.ConnectionParams)
				.ThenInclude(x => x.Param).AsNoTracking()
				.FirstOrDefaultAsync(x => x.ConnType == eConnType.SocketIO && x.CommGroup == eComGroup.Input);
		}
		public Task<Connection> GetConnectionById(long id)
		{
			return connectionRepository.GetByIdAsync(id);
		}
		#endregion

		#region ConnectionConfig
		public Task<IEnumerable<ConnectionConfig>> GetConnectionConfigs()
		{
			return connectionConfigRepository.GetAllAsync();
		}
		public Task<ConnectionConfig> GetConnectionConfigById(long id)
		{
			return connectionConfigRepository.GetByIdAsync(id);
		}
		#endregion
	}
}
