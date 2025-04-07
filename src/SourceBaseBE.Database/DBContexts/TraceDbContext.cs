using ConnectionCommon.Connection;
using SourceBaseBE.Database.Entities;
using SourceBaseBE.Database.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using iSoft.DBLibrary.DBConnections.Interfaces;
using SourceBaseBE.Database.Entities.TraceData;
using iSoft.Common.Utils;

namespace SourceBaseBE.Database.DBContexts
{
  public class TraceDBContext : DbContext
  {
    public IDBConnectionCustom dbConnectionCustom;
    protected IConfiguration Configuration { get; set; }

    public DbSet<EnvironmentEntity> Environments { set; get; }
    public DbSet<DeviceConnectionEntity> DeviceConnections { set; get; }

    public TraceDBContext(IConfiguration configuration)
    {
      this.Configuration = configuration;
    }
    public TraceDBContext(IDBConnectionCustom dbConnectionCustom)
    {
      this.dbConnectionCustom = dbConnectionCustom;
    }

    public TraceDBContext()
    {
    }

    public IDbConnection GetConnection()
    {
      return this.dbConnectionCustom.GetConnection();
    }

    public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
          builder.AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Warning)
                     .AddFilter(DbLoggerCategory.Query.Name, LogLevel.Information)
                     .AddConsole()
                     .AddDebug();
        }
    );
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      base.OnConfiguring(optionsBuilder);
      optionsBuilder.UseLoggerFactory(loggerFactory);
      dbConnectionCustom.SetOptionBuilder(ref optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
    public static async Task<bool> CreateDatabase(IDBConnectionCustom dbConnectionCustom)
    {
      using (var dbcontext = new TraceDBContext(dbConnectionCustom))
      {
        bool result = await dbcontext.Database.EnsureCreatedAsync();
        return result;
      }
    }
  }
}
