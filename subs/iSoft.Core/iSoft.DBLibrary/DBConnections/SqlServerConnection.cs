using ClickHouse.Client.ADO;
using ClickHouse.Client.Utility;
using iSoft.DBLibrary.DBConnections.Interfaces;
using iSoft.Common.Enums.DBProvider;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;
using iSoft.Common.Models.ConfigModel.Subs;

#nullable disable

namespace iSoft.DBLibrary.DBConnections
{
    public partial class SqlServerConnection : IDBConnectionWithTransaction
	{
		public EnumDBProvider dbProvider { get; set; }
		SqlConnection conn;
		DBServerConfigModel configModel;

		public SqlServerConnection(EnumDBProvider dbProvider, DBServerConfigModel configModel)
		{
			this.dbProvider = dbProvider;
			this.configModel = configModel;
    }

    public static string GetConnectionString(DBServerConfigModel configModel)
    {
      return $"Data Source = {configModel.Address},{configModel.Port}; Initial Catalog = {configModel.DatabaseName};User Id={configModel.Username};Password={configModel.Password};TrustServerCertificate=True;";
    }

    public string GetConnectionString()
		{
			return $"Data Source = {configModel.Address},{configModel.Port}; Initial Catalog = {configModel.DatabaseName};User Id={configModel.Username};Password={configModel.Password};TrustServerCertificate=True;";
		}
		public IDbConnection GetConnection()
		{
			this.conn = new SqlConnection(this.GetConnectionString());
			return this.conn;
		}
		public DbTransaction BeginTransaction()
		{
			return this.conn.BeginTransaction();
		}

		public IDbCommand AddParam(IDbCommand command, string key, object value)
		{
			((SqlCommand)command).Parameters.AddWithValue(key, value);
			return command;
		}
		public IDbCommand AddParam(IDbCommand command, string key, object value, string type = "")
		{
			((SqlCommand)command).Parameters.AddWithValue(key, value);
			return command;
		}

		public void SetOptionBuilder(ref DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(this.GetConnectionString());
    }

    public DBServerConfigModel GetDBConfig()
    {
      return this.configModel;
    }

    public EnumDBProvider GetDBProvider()
    {
      return this.dbProvider;
    }
  }
}

