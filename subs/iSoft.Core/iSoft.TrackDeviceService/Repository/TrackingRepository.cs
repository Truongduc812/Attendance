using ConnectionCommon.Connection;
using iSoft.Extensions.EntityFrameworkCore.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using iSoft.DBLibrary.DBConnections.Factory;
using iSoft.DBLibrary.Enums.Provider;
using iSoft.DBLibrary.Entities;
using iSoft.Database.DBContexts;
using iSoft.TrackDeviceService.ConfigsNS;
using RemoteConfig = iSoft.TrackDeviceService.ConfigsNS.RemoteConfig;
using iSoft.TrackDeviceService.Context;

namespace iSoft.TrackDeviceService.Repository
{
	public class TrackDeviceRepository<TEntity> : GenericRepository<CommonDBContext, TEntity> where TEntity : BaseEntity
	{
		public TrackDeviceRepository() :
			base(new TrackDeviceContext(DBConnectionFactory.CreateDBConnection(EnumDBProvider.Postgres, RemoteConfig.GetConfig()?.MasterDatabaseConfig)))
		{
			Context.Database.EnsureCreated();
		}
	}
	public class ConfigRepository : TrackDeviceRepository<Config>
	{
		public async override Task<Config> GetByIdAsync(object id)
		{
			return await Context.Set<Config>().AsNoTracking().FirstOrDefaultAsync(s => s.Id == (ulong)id);
		}

	}
	public class ConnectionRepository : TrackDeviceRepository<Connection>
	{
		public async override Task<Connection> GetByIdAsync(object id)
		{
			return await Context.Set<Connection>().AsNoTracking().FirstOrDefaultAsync(s => s.Id == (ulong)id);
		}

	}
	public class ConnectionConfigRepository : TrackDeviceRepository<ConnectionConfig>
	{
		public async override Task<ConnectionConfig> GetByIdAsync(object id)
		{
			return await Context.Set<ConnectionConfig>().AsNoTracking().Include(s => s.Config).Include(s => s.Connection).FirstOrDefaultAsync(s => s.Id == (ulong)id);
		}

	}
	public class ConnectionParamRepository : TrackDeviceRepository<ConnectionParam>
	{
		public async override Task<ConnectionParam> GetByIdAsync(object id)
		{
			return await Context.Set<ConnectionParam>().AsNoTracking().Include(s => s.Param).Include(s => s.Connection).FirstOrDefaultAsync(s => s.Id == (ulong)id);
		}

	}
	public class ParamRepository : TrackDeviceRepository<Param>
	{
		public async override Task<Param> GetByIdAsync(object id)
		{
			return await Context.Set<Param>().AsNoTracking().FirstOrDefaultAsync(s => s.Id == (ulong)id);
		}

	}
}
