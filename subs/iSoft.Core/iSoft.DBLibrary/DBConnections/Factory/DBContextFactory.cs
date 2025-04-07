using Microsoft.Extensions.Configuration;
using iSoft.DBLibrary.DBConnections.Interfaces;
using iSoft.Common.Enums.DBProvider;
using iSoft.Common.Models.ConfigModel.Subs;

namespace iSoft.DBLibrary.DBConnections.Factory
{
    public class DBConnectionFactory
	{
		public static IDBConnectionCustom CreateDBConnection(DBServerConfigModel configModel)
		{
			switch (configModel.DatabaseType)
			{
				case EnumDBProvider.SqlServer:
					return new SqlServerConnection(configModel.DatabaseType, configModel);
				case EnumDBProvider.Postgres:
					return new PostgresDbConnection(configModel.DatabaseType, configModel);
				//case EnumDBProvider.ClickHouse:
				//    return new ClickHouseDBConnection(provider);
				default:
					throw new InvalidOperationException("Invalid providerType");
			}
		}
	}
}
