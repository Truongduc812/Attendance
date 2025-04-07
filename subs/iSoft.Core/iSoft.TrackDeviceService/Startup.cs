using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AutoMapper;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using UDF.TrackDeviceService.CronjobsNS;
using UDF.TrackDeviceService.Services;
using UDF.TrackDeviceService.Services.Interfaces;
using System.IO;
using iSoft.ConnectionCommon.IndustrialConn.TcpGroup.EtherCAT;
using ConnectionCommon.Connection;
using iSoft.Extensions.EntityFrameworkCore.Repository;
using iSoft.DBLibrary.DBConnections.Factory;
using iSoft.DBLibrary.Enums.Provider;
using UDF.TrackDeviceService.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using iSoft.TrackDeviceService.Services;
using iSoft.Database.DBContexts;
using iSoft.TrackDeviceService.Repository;
using iSoft.TrackDeviceService.Context;
using iSoft.TrackDeviceService.Helpers;
using iSoft.ConnectionCommon.SocketIOs;
using iSoft.TrackDeviceService.CronjobsNS;
using iSoft.ConnectionCommon.MessageQueueNS;
using iSoft.ConnectionCommon.MessageBroker.MQTT;
using iSoft.ConnectionCommon.iSoft.ConnectionCommon.SocketIOs;

namespace UDF.TrackDeviceService
{
	public class Startup
	{

		private ILoggerFactory loggerFactory = null;

		//private readonly ILoggerFactory loggerFactory;

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors();
			services.AddControllers();

			// configure strongly typed settings objects
			var appSettingsSection = Configuration.GetSection("AppSettings");
			services.Configure<AppSettings>(appSettingsSection);

			// configure jwt authentication
			var appSettings = appSettingsSection.Get<AppSettings>();
			var key = Encoding.ASCII.GetBytes(appSettings.Secret);
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


			// Add SwaggerDoc
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen(option =>
			{
				option.SwaggerDoc("v1", new OpenApiInfo { Version = "1.0", Title = "UDF TrackDevice Backend" });
			});

			//services.AddApiVersioning(options =>
			//{
			//	options.DefaultApiVersion = new ApiVersion(1, 0);
			//	options.AssumeDefaultVersionWhenUnspecified = true;
			//	options.ReportApiVersions = true;
			//});

			// configure DI for application services
			services.AddSingleton<IInputService, ReadDeviceService>();
			services.AddSingleton<IOutputService, OutDataService>();
			services.AddSingleton<MessageQueue, MessageQueue>();
			services.AddSingleton<RealtimeService, RealtimeService>();
			services.AddSingleton<IMQService, MQTTService>();
			services.AddSingleton<HandleConnJob, HandleConnJob>();
			services.AddSingleton<IRealtime, SocketIoClient>();
			services.AddSingleton<CommonDBContext, TrackDeviceContext>();
			services.AddSingleton<StoreHolder, StoreHolder>();
			services.AddSingleton<StoreHolderService, StoreHolderService>();

			services.AddSingleton<TrackDeviceConfigService, TrackDeviceConfigService>();
			services.AddScoped<IGenericRepository<Connection>, ConnectionRepository>();
			services.AddScoped<IGenericRepository<Param>, ParamRepository>();
			services.AddScoped<IGenericRepository<Config>, ConfigRepository>();
			services.AddScoped<IGenericRepository<ConnectionConfig>, ConnectionConfigRepository>();
			services.AddScoped<IGenericRepository<ConnectionParam>, ConnectionParamRepository>();
			services.AddHostedService<Cronjobs>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			// Cấu hình Serilog
			Log.Logger = new LoggerConfiguration()
					//.MinimumLevel.Error()
					.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
					.WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] - {Message}{NewLine}{Exception}")
					.CreateLogger();

			loggerFactory = LoggerFactory.Create(builder =>
			{
				builder.AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Warning)
									 .AddFilter(DbLoggerCategory.Query.Name, LogLevel.Information)
									 .AddConsole()
									 .AddDebug().ClearProviders().AddSerilog(Log.Logger);
			}
			);
			string startStr = File.ReadAllText("isoft-starup.txt");
			Log.Logger.Information(startStr);

			//loggerFactory.AddSerilog(Log.Logger);

			//app.UseSerilogRequestLogging();

			app.UseRouting();
			app.UseStaticFiles();

			// global cors policy
			app.UseCors(x => x
					.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader());

			app.UseAuthentication();
			app.UseAuthorization();

			if (env.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseRouter(routes =>
			{
				routes.MapGet("/", async context =>
				{
					context.Response.Redirect("/swagger");
				});
			});

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

		}
	}
}
