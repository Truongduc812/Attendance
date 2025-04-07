using ClickHouse.Client.ADO;
using ClickHouse.Client.Utility;
using iSoft.DBLibrary.DBConnections.Interfaces;
using iSoft.Common.Enums.DBProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Data;
using System.Data.Common;
using iSoft.Common.Models.ConfigModel.Subs;

#nullable disable

namespace iSoft.DBLibrary.DBConnections
{
    public partial class PostgresDbConnection : IDBConnectionWithTransaction
	{
		public EnumDBProvider dbProvider { get; set; }
		NpgsqlConnection conn;
		DBServerConfigModel configModel;

		public PostgresDbConnection(EnumDBProvider dbProvider, DBServerConfigModel configModel)
		{
			this.dbProvider = dbProvider;
			this.configModel = configModel;
    }

    public static string GetConnectionString(DBServerConfigModel configModel)
    {
      if (configModel == null) return null;
      return $"Host={configModel.Address};" +
        $"Port={configModel.Port};" +
        $"User ID={configModel.Username};" +
        $"Password={configModel.Password};" +
        $"Database={configModel.DatabaseName};";
    }

    public string GetConnectionString()
		{
			if (configModel == null) return null;
			return $"Host={configModel.Address};" +
				$"Port={configModel.Port};" +
				$"User ID={configModel.Username};" +
				$"Password={configModel.Password};" +
				$"Database={configModel.DatabaseName};";
		}
		public IDbConnection GetConnection()
		{
			this.conn = new NpgsqlConnection(this.GetConnectionString());
			AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
			return this.conn;
		}
		public DbTransaction BeginTransaction()
		{
			return this.conn.BeginTransaction();
		}

		public IDbCommand AddParam(IDbCommand command, string key, object value)
		{
			((NpgsqlCommand)command).Parameters.AddWithValue(key, value);
			return command;
		}
		public IDbCommand AddParam(IDbCommand command, string key, object value, string type = "")
		{
			((NpgsqlCommand)command).Parameters.AddWithValue(key, value);
			return command;
		}

		public void SetOptionBuilder(ref DbContextOptionsBuilder optionsBuilder)
		{
			var cnn = this.GetConnectionString();
			optionsBuilder.UseNpgsql(this.GetConnectionString());
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

