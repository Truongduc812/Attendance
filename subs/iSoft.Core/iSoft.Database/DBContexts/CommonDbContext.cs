using iSoft.Database.Entities;
using iSoft.DBLibrary.DBConnections.Interfaces;
using iSoft.DBLibrary.Entities;
using iSoft.Common.Enums.DBProvider;
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
using iSoft.Common.Enums;

namespace iSoft.Database.DBContexts
{
  public class CommonDBContext : DbContext
  {
    public IDBConnectionCustom dbConnectionCustom;
    protected IConfiguration Configuration { get; set; }

    #region Authen Entities
    public DbSet<UserEntity> Users { set; get; }
    public DbSet<FCMEntity> FCMs { set; get; }
    public DbSet<ISoftProjectEntity> ISoftProjects { set; get; }
    #endregion

    public CommonDBContext(IConfiguration configuration)
    {
      this.Configuration = configuration;
    }
    public CommonDBContext(IDBConnectionCustom dbConnectionCustom)
    {
      this.dbConnectionCustom = dbConnectionCustom;
    }

    public CommonDBContext()
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
#if DEBUG
      optionsBuilder.EnableSensitiveDataLogging();
#endif
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<UserEntity>()
          .HasMany(e => e.ListISoftProject)
          .WithMany(e => e.ListUser)
          .UsingEntity<Dictionary<string, object>>(
                "refUserISoftProject",
                j => j
                    .HasOne<ISoftProjectEntity>()
                    .WithMany()
                    .HasForeignKey("ISoftProjectId")
                    .OnDelete(DeleteBehavior.SetNull),
                j => j
                    .HasOne<UserEntity>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.SetNull));

      modelBuilder.ConfigureDateTimeProperties("timestamp without time zone");

      base.OnModelCreating(modelBuilder);

      //// *** UserEntity
      //modelBuilder.Entity<UserEntity>().Property(e => e.Status).HasDefaultValue(EnumActiveStatus.Actived);
      //modelBuilder.Entity<UserEntity>().Property(e => e.DeletedFlag).HasDefaultValue(false);

      //// Create an index for a column
      //modelBuilder.Entity<UserEntity>().HasIndex(e => e.Username).IsUnique();
      //modelBuilder.Entity<UserEntity>().HasIndex(e => e.CreatedAt).IsDescending();

      //// Create the sequence
      //modelBuilder.HasSequence<int>("users_id_seq").StartsAt(1);

      //// Set auto-increment using a sequence
      //modelBuilder.Entity<UserEntity>().Property(e => e.Id).HasDefaultValueSql("nextval('users_id_seq')");

    }

    public static async Task CreateDatabase(IDBConnectionCustom dbConnectionCustom)
    {
      using (var dbcontext = new CommonDBContext(dbConnectionCustom))
      {
        bool result = await dbcontext.Database.EnsureCreatedAsync();
      }
    }
  }
}
