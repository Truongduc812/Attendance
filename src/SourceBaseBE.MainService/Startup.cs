using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using Serilog.Events;
using Microsoft.EntityFrameworkCore;
using System.IO;
using SourceBaseBE.MainService.Services;
using SourceBaseBE.MainService.Services.Generate;
using iSoft.Common.Util;
using iSoft.Common.ConfigsNS;
using iSoft.Common.Cronjobs.DefaultCronjobs;
using SourceBaseBE.MainService.Cronjobs;
using iSoft.ElasticSearch.Services;
using SourceBaseBE.Database.DBContexts;
using Microsoft.Extensions.Logging;
using iSoft.Common.ExtensionMethods;
using iSoft.RabbitMQ.Services;
using iSoft.DBLibrary.DBConnections.Factory;
using iSoft.DBLibrary.DBConnections.Interfaces;
using iSoft.RabbitMQ;
using SourceBaseBE.MainService.Services.HostedServices;
using Prometheus;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using iSoft.Redis.Services;
using iSoft.DBLibrary.DBConnections;
using iSoft.InfluxDB.Services;
using iSoft.InfluxDB.Cronjobs;
using SourceBaseBE.CommonFunc.HealthCheck;
using Microsoft.AspNetCore.Routing;
using System;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SourceBaseBE.ServerApp
{
    public class Startup
    {

        public static ILoggerFactory loggerFactory = null;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    //.WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] ({SourceContext:lj}) - {Message}{NewLine}{Exception}");
                    .WriteTo.Console(new SerilogFormat());

            bool minimumLevelFlag = ConfigUtil.GetAppSetting<bool>("AppSettings:MinimumLevelErrorFlag");
            if (minimumLevelFlag)
            {
                loggerConfiguration.MinimumLevel.Error();
            }

            Log.Logger = loggerConfiguration.CreateLogger();

            loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Warning)
       .AddFilter(DbLoggerCategory.Query.Name, LogLevel.Information)
       .AddConsole()
       .AddDebug()
       .ClearProviders();
            }
            );
            loggerFactory.AddSerilog(Log.Logger);

            string startStr = File.ReadAllText("start.txt");
            Log.Logger.Information(startStr);

            // configure jwt authentication
            string authenticationSecret = CommonConfig.GetConfig().AuthenticationSecret;
            var key = Encoding.ASCII.GetBytes(authenticationSecret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddMetricServer(options =>
            {
                options.Port = 13014;
            });
            services.AddHealthChecks()
              .AddRedis(RedisService.GetConnectionString(CommonConfig.GetConfig().RedisConfig), "Redis")
              //.AddElasticsearch(ElasticSearchService.GetConnectionString(CommonConfig.GetConfig().ElasticSearchConfig), "ElasticSearch")
              .AddNpgSql(PostgresDbConnection.GetConnectionString(CommonConfig.GetConfig().MasterDatabaseConfig), "SELECT 1;", null, "Postgres")
        //.AddSqlServer(SqlServerConnection.GetConnectionString(CommonConfig.GetConfig().MasterDatabaseConfig))
        .AddCheck<ServiceHealthCheck>("Main Service")
        .AddCheck<ServiceHealthCheck_RabbitMQ>("RabbitMQ")
        .AddCheck<ServiceHealthCheck_SocketIO>("SocketIO")
        .ForwardToPrometheus();

            services.AddCors();
            services.AddControllers();

            services.AddControllersWithViews();

            // configure DI for application services
            services.AddSingleton(provider => new InfluxDBService(CommonConfig.GetConfig().InfluxDBConfig));
            services.AddScoped(provider =>
              {
                  IDBConnectionCustom dBConnectionCustom = DBConnectionFactory.CreateDBConnection(CommonConfig.GetConfig().MasterDatabaseConfig);
                  return new CommonDBContext(dBConnectionCustom);
              });
            services.AddScoped<GenerateSourceService>();
            services.AddScoped<GenTemplateService>();
            services.AddScoped<SystemSettingService>();
            services.AddScoped<TimeSheetApprovalService>();
            services.AddScoped<TimeSheetUpdateService>();
            services.AddScoped<MessageService>();
            services.AddScoped<HolidayScheduleService>();
            services.AddScoped<HolidayWorkingTypeService>();
            services.AddScoped<DepartmentAdminService>();
            services.AddScoped<ParameterService>();
            services.AddScoped<DeviceService>();
            services.AddScoped<OrganizationService>();
            services.AddScoped<ISoftProjectService>();
            services.AddScoped<Example001Service>();
            services.AddScoped<UserService>();
            services.AddScoped<AuthGroupService>();
            services.AddScoped<AuthPermissionService>();
            services.AddScoped<AuthTokenService>();
            services.AddScoped<ElasticSearchService>();
            services.AddScoped<DepartmentService>();
            services.AddScoped<JobTitleService>();
            services.AddScoped<EmployeeService>();
            services.AddScoped<TimeSheetService>();
            services.AddScoped<WorkingDayService>();
            services.AddScoped<MasterDataService>();
            services.AddScoped<WorkingTypeService>();
            services.AddScoped<WorkingDayApprovalService>();
            services.AddScoped<WorkingDayUpdateService>();
            services.AddScoped<WorkingTypeDescriptionService>();
            //services.AddScoped<NotifyService>();
            //services.AddHostedService<CheckConnectRedisJob>();
            //services.AddHostedService<GetRemoteConfigCronjob>();
            //services.AddCronJob<MyCronJob1>(c =>
            //{
            //  c.TimeZoneInfo = TimeZoneInfo.Local;
            //  c.CronExpression = @"*/5 * * * * *";
            //});
            services.AddHostedService<CheckConnectRedisJob>();
            services.AddHostedService<GCCollectCronjob>();
            services.AddHostedService<ResetMetricsCronjob>();

            services.AddSingleton(provider =>
            {
                //var influxDBService = provider.GetRequiredService<InfluxDBService>();
                return new RabbitMQService(CommonConfig.GetConfig().RabbitMQConfig);
            });

            services.AddHostedService(provider =>
            {
                IDBConnectionCustom dBConnectionCustom = DBConnectionFactory.CreateDBConnection(CommonConfig.GetConfig().MasterDatabaseConfig);
                var dbContext = new CommonDBContext(dBConnectionCustom);
                var logger = provider.GetRequiredService<ILogger<UpdateWorkingDayConsumerHostedService>>();
                var loggerWdService = provider.GetRequiredService<ILogger<WorkingDayService>>();
                return new UpdateWorkingDayConsumerHostedService(dbContext, logger, loggerWdService);
            });

            //services.AddHostedService<TestInfluxDBCronjob>();

            services.AddHostedService(provider =>
            {
                IDBConnectionCustom dBConnectionCustom = DBConnectionFactory.CreateDBConnection(CommonConfig.GetConfig().MasterDatabaseConfig);
                var dbContext = new CommonDBContext(dBConnectionCustom);
                //var dbContext = provider.GetRequiredService<CommonDBContext>();
                var rabbitMQService = provider.GetRequiredService<RabbitMQService>();
                var logger = provider.GetRequiredService<ILogger<UpdateStateEmployeeHostedService>>();
                return new UpdateStateEmployeeHostedService(dbContext, logger);
            });

            //services.AddHostedService(provider =>
            //{
            //	var logger = provider.GetRequiredService<ILogger<CheckCreateWorkingDayJob>>();
            //	var loggerService = provider.GetRequiredService<ILogger<WorkingDayService>>();
            //	IDBConnectionCustom dBConnectionCustom = DBConnectionFactory.CreateDBConnection(CommonConfig.GetConfig().MasterDatabaseConfig);
            //	var dbContext = new CommonDBContext(dBConnectionCustom);
            //	return new CheckCreateWorkingDayJob(logger,
            //												new WorkingDayService(dbContext, loggerService));
            //});

            //services.AddTransient<WriteRandomPlaneAltitudeInvocable>();
            //services.AddScheduler();

            // Add for realtime topic
            //services.AddHostedService(provider =>
            //{
            //  //var influxDBService = provider.GetRequiredService<InfluxDBService>();
            //  var rabbitMQService = provider.GetRequiredService<RabbitMQService>();
            //  var logger = provider.GetRequiredService<ILogger<RealtimeDataConsumerHostedService>>();
            //  return new RealtimeDataConsumerHostedService(logger, rabbitMQService, TopicName.RealtimeDataTopic);
            //});

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSerilogRequestLogging();

            //app.ApplicationServices.UseScheduler(scheduler =>
            //{
            //  scheduler
            //      .Schedule<WriteRandomPlaneAltitudeInvocable>()
            //      .EveryFiveSeconds();
            //});

            app.UseRouting();
            app.UseStaticFiles();

            app.UseHttpMetrics();

            // global cors policy
            app.UseCors(x => x
          .AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader()
          .WithExposedHeaders("Content-Disposition")
          );

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            //if (env.IsDevelopment())
            //{
            //	app.UseDeveloperExceptionPage();
            //	app.UseSwagger();
            //	app.UseSwaggerUI(c =>
            //	{
            //		c.SwaggerEndpoint("v1/swagger.json", "SourceBaseBE-BE V1");
            //	});
            //}

            app.UseRouter(routes =>
                  {
                      routes.MapGet("/", async context =>
              {
                  //context.Response.Redirect("/api/v1/dashboard");
                  context.Response.Redirect("/Home/Setting");
              });
                  });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapMetrics("/api/v1/metrics").RequireAuthorization(a => a.RequireRole("Root"));
                endpoints.MapHealthChecks("/api/v1/_health", new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                }).RequireAuthorization(a => a.RequireRole("Root"));
            });
        }
    }

}
