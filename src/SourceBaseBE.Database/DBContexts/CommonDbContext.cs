using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using iSoft.DBLibrary.DBConnections.Interfaces;
using SourceBaseBE.Database.Entities;
using SourceBaseBE.Database.Enums;

namespace SourceBaseBE.Database.DBContexts
{
    public class CommonDBContext : DbContext
    {
        public IDBConnectionCustom dbConnectionCustom;
        protected IConfiguration Configuration { get; set; }
    public virtual DbSet<SystemSettingEntity> SystemSettings { get; set; }
        public virtual DbSet<MessageEntity> Messages { get; set; }
        public virtual DbSet<HolidayScheduleEntity> HolidaySchedules { get; set; }
        public virtual DbSet<HolidayWorkingTypeEntity> HolidayWorkingTypes { get; set; }
        public virtual DbSet<DepartmentAdminEntity> DepartmentAdmins { get; set; }
        public virtual DbSet<WorkingDayUpdateEntity> WorkingDayUpdates { get; set; }
        public virtual DbSet<WorkingDayApprovalEntity> WorkingDayApprovals { get; set; }
        public virtual DbSet<MasterDataEmployeeEntity> MasterDataEmployees { get; set; }
        public virtual DbSet<WorkingDayEntity> WorkingDays { get; set; }
        public virtual DbSet<LanguageEntity> Languages { get; set; }
        public virtual DbSet<TimeSheetEntity> TimeSheets { get; set; }
        public virtual DbSet<TimeSheetUpdateEntity> TimeSheetUpdates { get; set; }
        public virtual DbSet<TimeSheetApprovalEntity> TimeSheetApprovals { get; set; }
        public virtual DbSet<EmployeeEntity> Employees { get; set; }
        public virtual DbSet<WorkingTypeDescriptionEntity> WorkingTypeDescriptions { get; set; }
        public virtual DbSet<JobTitleEntity> JobTitles { get; set; }
        public virtual DbSet<DepartmentEntity> Departments { get; set; }
        public virtual DbSet<ParameterEntity> Parameters { get; set; }
        public virtual DbSet<LimitationEntity> Limitations { get; set; }
        public virtual DbSet<DeviceEntity> Devices { get; set; }
        public virtual DbSet<OrganizationEntity> Organizations { get; set; }
        public virtual DbSet<ISoftProjectEntity> ISoftProjects { get; set; }
        public virtual DbSet<iSoft.Database.Entities.FCMEntity> FCMEntities { get; set; }
        public virtual DbSet<Example001Entity> Example001s { get; set; }
        public virtual DbSet<UserEntity> Users { get; set; }
        public virtual DbSet<AuthGroupEntity> AuthGroups { get; set; }
        public virtual DbSet<AuthPermissionEntity> AuthPermissions { get; set; }
        public virtual DbSet<AuthTokenEntity> AuthTokens { get; set; }
        public virtual DbSet<GenTemplateEntity> GenTemplates { get; set; }
        public virtual DbSet<iSoft.Database.Entities.FCMEntity> FCMs { get; set; }

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
            dbConnectionCustom?.SetOptionBuilder(ref optionsBuilder);
#if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
#endif
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("unaccent");
            modelBuilder.Entity<AuthPermissionEntity>()
              .HasMany(e => e.ListAuthGroup)
              .WithMany(e => e.ListAuthPermission)
              .UsingEntity<Dictionary<string, object>>(
                "refAuthPermissionAuthGroup",
                j => j
                .HasOne<AuthGroupEntity>()
                .WithMany()
                .HasForeignKey("AuthGroupId")
                .OnDelete(DeleteBehavior.SetNull),
                j => j
                .HasOne<AuthPermissionEntity>()
                .WithMany()
                .HasForeignKey("AuthPermissionId")
                .OnDelete(DeleteBehavior.SetNull));

            modelBuilder.Entity<AuthPermissionEntity>()
              .HasMany(e => e.ListUser)
              .WithMany(e => e.ListAuthPermission)
              .UsingEntity<Dictionary<string, object>>(
                "refAuthPermissionUser",
                j => j
                .HasOne<Entities.UserEntity>()
                .WithMany()
                .HasForeignKey("EditerId")
                .OnDelete(DeleteBehavior.SetNull),
                j => j
                .HasOne<AuthPermissionEntity>()
                .WithMany()
                .HasForeignKey("AuthPermissionId")
                .OnDelete(DeleteBehavior.SetNull));
            modelBuilder.Entity<AuthGroupEntity>()
                .HasMany(e => e.ListAuthPermission)
                .WithMany(e => e.ListAuthGroup)
                .UsingEntity<Dictionary<string, object>>(
                "refAuthGroupAuthPermission",
                j => j
                  .HasOne<AuthPermissionEntity>()
                  .WithMany()
                  .HasForeignKey("AuthPermissionId")
                  .OnDelete(DeleteBehavior.SetNull),
                j => j
                  .HasOne<AuthGroupEntity>()
                  .WithMany()
                  .HasForeignKey("AuthGroupId")
                  .OnDelete(DeleteBehavior.SetNull));

            modelBuilder.Entity<AuthGroupEntity>()
              .HasMany(e => e.ListUser)
              .WithMany(e => e.ListAuthGroup)
              .UsingEntity<Dictionary<string, object>>(
                "refAuthGroupUser",
                j => j
                .HasOne<Entities.UserEntity>()
                .WithMany()
                .HasForeignKey("EditerId")
                .OnDelete(DeleteBehavior.SetNull),
                j => j
                .HasOne<AuthGroupEntity>()
                .WithMany()
                .HasForeignKey("AuthGroupId")
                .OnDelete(DeleteBehavior.SetNull));
            modelBuilder.Entity<Entities.UserEntity>()
                .HasMany(e => e.ListAuthGroup)
                .WithMany(e => e.ListUser)
                .UsingEntity<Dictionary<string, object>>(
                "refUserAuthGroup",
                j => j
                  .HasOne<AuthGroupEntity>()
                  .WithMany()
                  .HasForeignKey("AuthGroupId")
                  .OnDelete(DeleteBehavior.SetNull),
                j => j
                  .HasOne<Entities.UserEntity>()
                  .WithMany()
                  .HasForeignKey("EditerId")
                  .OnDelete(DeleteBehavior.SetNull));

            modelBuilder.Entity<Entities.UserEntity>()
              .HasMany(e => e.ListAuthPermission)
              .WithMany(e => e.ListUser)
              .UsingEntity<Dictionary<string, object>>(
                "refUserAuthPermission",
                j => j
                .HasOne<AuthPermissionEntity>()
                .WithMany()
                .HasForeignKey("AuthPermissionId")
                .OnDelete(DeleteBehavior.SetNull),
                j => j
                .HasOne<Entities.UserEntity>()
                .WithMany()
                .HasForeignKey("EditerId")
                .OnDelete(DeleteBehavior.SetNull));
            modelBuilder.Entity<Example001Entity>()
                .HasMany(e => e.ListGenTemplate)
                .WithMany(e => e.ListExample001)
                .UsingEntity<Dictionary<string, object>>(
                "refExample001GenTemplate",
                j => j
                  .HasOne<GenTemplateEntity>()
                  .WithMany()
                  .HasForeignKey("GenTemplateId")
                  .OnDelete(DeleteBehavior.SetNull),
                j => j
                  .HasOne<Example001Entity>()
                  .WithMany()
                  .HasForeignKey("Example001Id")
                  .OnDelete(DeleteBehavior.SetNull));

            modelBuilder.Entity<Entities.UserEntity>()
              .HasMany(e => e.ListISoftProject)
              .WithMany(e => e.ListUser)
              .UsingEntity<Dictionary<string, object>>(
                "refUserISoftProject",
                j => j
                .HasOne<Entities.ISoftProjectEntity>()
                .WithMany()
                .HasForeignKey("ISoftProjectId")
                .OnDelete(DeleteBehavior.SetNull),
                j => j
                .HasOne<Entities.UserEntity>()
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.SetNull));
            /*[GEN-15]*/

            modelBuilder.Entity<Entities.UserEntity>()
              .HasOne(e => e.ItemEmployee)
              .WithMany()
              .HasForeignKey(e => e.EmployeeId)
              .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Entities.UserEntity>()
              .HasOne(e => e.ItemAuthAccountType)
              .WithMany()
              .HasForeignKey(e => e.AuthAccountTypeId)
              .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Entities.UserEntity>()
              .HasOne(e => e.ItemAuthToken)
              .WithMany()
              .HasForeignKey(e => e.AuthTokenId)
              .OnDelete(DeleteBehavior.SetNull);
            //modelBuilder.Entity<EmployeeEntity>()
            // .HasAlternateKey(x => x.EmployeeCode);
            //modelBuilder.Entity<EmployeeEntity>()
            // .HasAlternateKey(x => x.EmployeeMachineCode);
            modelBuilder.Entity<Example001Entity>()
                .HasOne(e => e.ItemGenTemplate)
                .WithMany()
                .HasForeignKey(e => e.GenTemplateId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<EmployeeEntity>().
                            HasMany(x => x.WorkingDayEntitys)
                            .WithOne(x => x.Employee)
                            .HasForeignKey(x => x.EmployeeEntityId)
                            .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<WorkingDayEntity>().
                    HasMany(x => x.WorkingDayUpdates)
                    .WithOne(x => x.WorkingDay)
                    .HasForeignKey(x => x.WorkingDayId)
                    .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<WorkingDayEntity>().
                    HasMany(x => x.TimeSheets)
                    .WithOne(x => x.WorkingDay)
                    .HasForeignKey(x => x.WorkingDayId)
                    .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<WorkingDayUpdateEntity>().
                    HasMany(x => x.WorkingDayApprovals)
                    .WithOne(x => x.WorkingDayUpdate)
                    .HasForeignKey(x => x.WorkingDayUpdateId)
                    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MessageEntity>()
                  .HasOne(e => e.ItemUser)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<WorkingTypeDescriptionEntity>()
                  .HasOne(e => e.WorkingTypeItem)
                  .WithMany()
                  .HasForeignKey(e => e.WorkingTypeId)
                  .OnDelete(DeleteBehavior.SetNull);
            //modelBuilder.Entity<TimeSheetUpdateEntity>()
            //.HasOne(e => e.TimeSheetEntity)
            //.WithMany()
            //.HasForeignKey(e => e.TimeSheetEntityId)
            //.OnDelete(DeleteBehavior.SetNull);

            //modelBuilder.Entity<TimeSheetUpdateEntity>()
            //      .HasOne(e => e.Employee)
            //      .WithMany()
            //      .HasForeignKey(e => e.EmployeeId)
            //      .OnDelete(DeleteBehavior.SetNull);

            //modelBuilder.Entity<TimeSheetUpdateEntity>()
            //      .HasOne(e => e.UserEntity)
            //      .WithMany()
            //      .HasForeignKey(e => e.UserEntityId)
            //      .OnDelete(DeleteBehavior.SetNull);
            //modelBuilder.Entity<TimeSheetApprovalEntity>()
            //            .HasOne(e => e.UserEntity)
            //            .WithMany()
            //            .HasForeignKey(e => e.UserEntityId)
            //            .OnDelete(DeleteBehavior.SetNull);

            //modelBuilder.Entity<TimeSheetApprovalEntity>()
            //      .HasOne(e => e.UserEntity)
            //      .WithMany()
            //      .HasForeignKey(e => e.TimeSheetUpdateId)
            //      .OnDelete(DeleteBehavior.SetNull);
            /*[GEN-16]*/

            modelBuilder.ConfigureDateTimeProperties("timestamp without time zone");

            modelBuilder.Entity<WorkingDayUpdateEntity>().HasIndex(x => x.EmployeeId);
            modelBuilder.Entity<WorkingDayApprovalEntity>().HasIndex(x => x.ApproveStatus);
            modelBuilder.Entity<WorkingDayEntity>().HasIndex(x => x.WorkingDayStatus);
            modelBuilder.Entity<WorkingDayEntity>().HasIndex(x => x.Time_In);
            modelBuilder.Entity<WorkingDayEntity>().HasIndex(x => x.Time_Out);
            modelBuilder.Entity<WorkingDayEntity>().HasIndex(x => x.TimeDeviation);
            modelBuilder.Entity<WorkingDayEntity>().HasIndex(x => x.WorkingDate);
            modelBuilder.Entity<WorkingDayEntity>().HasIndex(x => x.InOutState);
            modelBuilder.Entity<WorkingTypeEntity>().HasIndex(x => x.Name);
            modelBuilder.Entity<EmployeeEntity>().HasIndex(x => x.Name);
            modelBuilder.Entity<EmployeeEntity>().HasIndex(x => x.EmployeeCode);
            modelBuilder.Entity<EmployeeEntity>().HasIndex(x => x.EmployeeMachineCode);
            modelBuilder.Entity<EmployeeEntity>().HasIndex(x => x.EmployeeStatus);
            modelBuilder.Entity<DepartmentEntity>().HasIndex(x => x.Name);
            modelBuilder.Entity<JobTitleEntity>().HasIndex(x => x.Name);
            modelBuilder.Entity<TimeSheetEntity>().HasIndex(x => x.RecordedTime);
            modelBuilder.Entity<HolidayScheduleEntity>().HasIndex(x => x.StartDate);
            modelBuilder.Entity<HolidayScheduleEntity>().HasIndex(x => x.EndDate);
            modelBuilder.Entity<WorkingTypeDescriptionEntity>().HasIndex(x => x.WorkingTypeId);
            modelBuilder.Entity<WorkingTypeDescriptionEntity>().HasIndex(x => x.Name);

            // CreatedAt
            modelBuilder.Entity<MessageEntity>().HasIndex(x => x.CreatedAt);
            modelBuilder.Entity<HolidayScheduleEntity>().HasIndex(x => x.CreatedAt);
            modelBuilder.Entity<HolidayWorkingTypeEntity>().HasIndex(x => x.CreatedAt);
            modelBuilder.Entity<DepartmentAdminEntity>().HasIndex(x => x.CreatedAt);
            modelBuilder.Entity<WorkingDayUpdateEntity>().HasIndex(x => x.CreatedAt);
            modelBuilder.Entity<WorkingDayApprovalEntity>().HasIndex(x => x.CreatedAt);
            modelBuilder.Entity<MasterDataEmployeeEntity>().HasIndex(x => x.CreatedAt);
            modelBuilder.Entity<WorkingDayEntity>().HasIndex(x => x.CreatedAt);
            modelBuilder.Entity<LanguageEntity>().HasIndex(x => x.CreatedAt);
            modelBuilder.Entity<TimeSheetEntity>().HasIndex(x => x.CreatedAt);
            modelBuilder.Entity<EmployeeEntity>().HasIndex(x => x.CreatedAt);
            modelBuilder.Entity<JobTitleEntity>().HasIndex(x => x.CreatedAt);
            modelBuilder.Entity<DepartmentEntity>().HasIndex(x => x.CreatedAt);
            modelBuilder.Entity<ParameterEntity>().HasIndex(x => x.CreatedAt);
            modelBuilder.Entity<LimitationEntity>().HasIndex(x => x.CreatedAt);
            modelBuilder.Entity<DeviceEntity>().HasIndex(x => x.CreatedAt);
            modelBuilder.Entity<OrganizationEntity>().HasIndex(x => x.CreatedAt);
            modelBuilder.Entity<ISoftProjectEntity>().HasIndex(x => x.CreatedAt);
            modelBuilder.Entity<iSoft.Database.Entities.FCMEntity>().HasIndex(x => x.CreatedAt);
            modelBuilder.Entity<Example001Entity>().HasIndex(x => x.CreatedAt);
            modelBuilder.Entity<UserEntity>().HasIndex(x => x.CreatedAt);
            modelBuilder.Entity<AuthGroupEntity>().HasIndex(x => x.CreatedAt);
            modelBuilder.Entity<AuthPermissionEntity>().HasIndex(x => x.CreatedAt);
            modelBuilder.Entity<AuthTokenEntity>().HasIndex(x => x.CreatedAt);
            modelBuilder.Entity<GenTemplateEntity>().HasIndex(x => x.CreatedAt);

            // UpdateAt
            modelBuilder.Entity<MessageEntity>().HasIndex(x => x.UpdatedAt);
            modelBuilder.Entity<HolidayScheduleEntity>().HasIndex(x => x.UpdatedAt);
            modelBuilder.Entity<HolidayWorkingTypeEntity>().HasIndex(x => x.UpdatedAt);
            modelBuilder.Entity<DepartmentAdminEntity>().HasIndex(x => x.UpdatedAt);
            modelBuilder.Entity<WorkingDayUpdateEntity>().HasIndex(x => x.UpdatedAt);
            modelBuilder.Entity<WorkingDayApprovalEntity>().HasIndex(x => x.UpdatedAt);
            modelBuilder.Entity<MasterDataEmployeeEntity>().HasIndex(x => x.UpdatedAt);
            modelBuilder.Entity<WorkingDayEntity>().HasIndex(x => x.UpdatedAt);
            modelBuilder.Entity<LanguageEntity>().HasIndex(x => x.UpdatedAt);
            modelBuilder.Entity<TimeSheetEntity>().HasIndex(x => x.UpdatedAt);
            modelBuilder.Entity<EmployeeEntity>().HasIndex(x => x.UpdatedAt);
            modelBuilder.Entity<JobTitleEntity>().HasIndex(x => x.UpdatedAt);
            modelBuilder.Entity<DepartmentEntity>().HasIndex(x => x.UpdatedAt);
            modelBuilder.Entity<ParameterEntity>().HasIndex(x => x.UpdatedAt);
            modelBuilder.Entity<LimitationEntity>().HasIndex(x => x.UpdatedAt);
            modelBuilder.Entity<DeviceEntity>().HasIndex(x => x.UpdatedAt);
            modelBuilder.Entity<OrganizationEntity>().HasIndex(x => x.UpdatedAt);
            modelBuilder.Entity<ISoftProjectEntity>().HasIndex(x => x.UpdatedAt);
            modelBuilder.Entity<iSoft.Database.Entities.FCMEntity>().HasIndex(x => x.UpdatedAt);
            modelBuilder.Entity<Example001Entity>().HasIndex(x => x.UpdatedAt);
            modelBuilder.Entity<UserEntity>().HasIndex(x => x.UpdatedAt);
            modelBuilder.Entity<AuthGroupEntity>().HasIndex(x => x.UpdatedAt);
            modelBuilder.Entity<AuthPermissionEntity>().HasIndex(x => x.UpdatedAt);
            modelBuilder.Entity<AuthTokenEntity>().HasIndex(x => x.UpdatedAt);
            modelBuilder.Entity<GenTemplateEntity>().HasIndex(x => x.UpdatedAt);

            // Order
            modelBuilder.Entity<MessageEntity>().HasIndex(x => x.Order);
            modelBuilder.Entity<HolidayScheduleEntity>().HasIndex(x => x.Order);
            modelBuilder.Entity<HolidayWorkingTypeEntity>().HasIndex(x => x.Order);
            modelBuilder.Entity<DepartmentAdminEntity>().HasIndex(x => x.Order);
            modelBuilder.Entity<WorkingDayUpdateEntity>().HasIndex(x => x.Order);
            modelBuilder.Entity<WorkingDayApprovalEntity>().HasIndex(x => x.Order);
            modelBuilder.Entity<MasterDataEmployeeEntity>().HasIndex(x => x.Order);
            modelBuilder.Entity<WorkingDayEntity>().HasIndex(x => x.Order);
            modelBuilder.Entity<LanguageEntity>().HasIndex(x => x.Order);
            modelBuilder.Entity<TimeSheetEntity>().HasIndex(x => x.Order);
            modelBuilder.Entity<EmployeeEntity>().HasIndex(x => x.Order);
            modelBuilder.Entity<JobTitleEntity>().HasIndex(x => x.Order);
            modelBuilder.Entity<DepartmentEntity>().HasIndex(x => x.Order);
            modelBuilder.Entity<ParameterEntity>().HasIndex(x => x.Order);
            modelBuilder.Entity<LimitationEntity>().HasIndex(x => x.Order);
            modelBuilder.Entity<DeviceEntity>().HasIndex(x => x.Order);
            modelBuilder.Entity<OrganizationEntity>().HasIndex(x => x.Order);
            modelBuilder.Entity<ISoftProjectEntity>().HasIndex(x => x.Order);
            modelBuilder.Entity<iSoft.Database.Entities.FCMEntity>().HasIndex(x => x.Order);
            modelBuilder.Entity<Example001Entity>().HasIndex(x => x.Order);
            modelBuilder.Entity<UserEntity>().HasIndex(x => x.Order);
            modelBuilder.Entity<AuthGroupEntity>().HasIndex(x => x.Order);
            modelBuilder.Entity<AuthPermissionEntity>().HasIndex(x => x.Order);
            modelBuilder.Entity<AuthTokenEntity>().HasIndex(x => x.Order);
            modelBuilder.Entity<GenTemplateEntity>().HasIndex(x => x.Order);

            // DeletedFlag
            modelBuilder.Entity<MessageEntity>().HasIndex(x => x.DeletedFlag);
            modelBuilder.Entity<HolidayScheduleEntity>().HasIndex(x => x.DeletedFlag);
            modelBuilder.Entity<HolidayWorkingTypeEntity>().HasIndex(x => x.DeletedFlag);
            modelBuilder.Entity<DepartmentAdminEntity>().HasIndex(x => x.DeletedFlag);
            modelBuilder.Entity<WorkingDayUpdateEntity>().HasIndex(x => x.DeletedFlag);
            modelBuilder.Entity<WorkingDayApprovalEntity>().HasIndex(x => x.DeletedFlag);
            modelBuilder.Entity<MasterDataEmployeeEntity>().HasIndex(x => x.DeletedFlag);
            modelBuilder.Entity<WorkingDayEntity>().HasIndex(x => x.DeletedFlag);
            modelBuilder.Entity<LanguageEntity>().HasIndex(x => x.DeletedFlag);
            modelBuilder.Entity<TimeSheetEntity>().HasIndex(x => x.DeletedFlag);
            modelBuilder.Entity<EmployeeEntity>().HasIndex(x => x.DeletedFlag);
            modelBuilder.Entity<JobTitleEntity>().HasIndex(x => x.DeletedFlag);
            modelBuilder.Entity<DepartmentEntity>().HasIndex(x => x.DeletedFlag);
            modelBuilder.Entity<ParameterEntity>().HasIndex(x => x.DeletedFlag);
            modelBuilder.Entity<LimitationEntity>().HasIndex(x => x.DeletedFlag);
            modelBuilder.Entity<DeviceEntity>().HasIndex(x => x.DeletedFlag);
            modelBuilder.Entity<OrganizationEntity>().HasIndex(x => x.DeletedFlag);
            modelBuilder.Entity<ISoftProjectEntity>().HasIndex(x => x.DeletedFlag);
            modelBuilder.Entity<iSoft.Database.Entities.FCMEntity>().HasIndex(x => x.DeletedFlag);
            modelBuilder.Entity<Example001Entity>().HasIndex(x => x.DeletedFlag);
            modelBuilder.Entity<UserEntity>().HasIndex(x => x.DeletedFlag);
            modelBuilder.Entity<AuthGroupEntity>().HasIndex(x => x.DeletedFlag);
            modelBuilder.Entity<AuthPermissionEntity>().HasIndex(x => x.DeletedFlag);
            modelBuilder.Entity<AuthTokenEntity>().HasIndex(x => x.DeletedFlag);
            modelBuilder.Entity<GenTemplateEntity>().HasIndex(x => x.DeletedFlag);



            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<AreaRegistrationEntity>()
            //  .HasKey(e => new { e.AreaCodeId, e.EntranceRegistrationFormId });

            //modelBuilder.Entity<CameraSettingEntity>()
            //.HasKey(e => new { e.AreaCodeId, e.CameraId });

            //modelBuilder.Entity<DriverRegistrationEntity>()
            //.HasKey(e => new { e.DriverId, e.EntryRequestId });
            //modelBuilder.Entity<WorkingDayEntity>().Ignore(x => x.InOutState);

        }

        public static async Task<bool> CreateDatabase(IDBConnectionCustom dbConnectionCustom)
        {
            using (var dbcontext = new CommonDBContext(dbConnectionCustom))
            {
                bool result = await dbcontext.Database.EnsureCreatedAsync();
                return result;
            }
        }

        public static async Task<bool> DeleteDatabase(IDBConnectionCustom dbConnectionCustom)
        {
            using (var dbcontext = new CommonDBContext(dbConnectionCustom))
            {
                bool result = await dbcontext.Database.EnsureDeletedAsync();
                return result;
            }
        }
    }
}
