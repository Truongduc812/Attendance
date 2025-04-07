using ConnectionCommon.Connection;
using iSoft.Database.DBContexts;
using iSoft.DBLibrary.DBConnections.Factory;
using iSoft.DBLibrary.DBConnections.Interfaces;
using iSoft.DBLibrary.Enums.Provider;
using iSoft.TrackDeviceService.ConfigsNS;
using iSoft.TrackDeviceService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Sockets;

namespace iSoft.TrackDeviceService.Context
{
	public class TrackDeviceContext : CommonDBContext
	{
		public DbSet<Config> Configs { get; set; }
		public DbSet<Param> Params { get; set; }
		public DbSet<ConnectionParam> ConnectionParams { get; set; }
		public DbSet<ConnectionConfig> ConnectionConfigs { get; set; }
		public DbSet<Connection> Connections { get; set; }
		public TrackDeviceContext() : base()
		{
		}
		public TrackDeviceContext(IDBConnectionCustom dbConnectionCustom) : base(dbConnectionCustom)
		{
			AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
		}
	}
}
