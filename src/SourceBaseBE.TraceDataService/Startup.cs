using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SourceBaseBE.Database.DTOs;
using AutoMapper;
using SourceBaseBE.Database;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using iSoft.Common.ConfigsNS;
using System.IO;
//using SourceBaseBE.Main.TraceDataService.Services;
using System.Threading.Tasks;
using iSoft.Common.Util;
using iSoft.Database.DBContexts;
using System.Data.Entity;
using SourceBaseBE.TraceDataServiceNS.Cronjobs;
using iSoft.Common.Cronjobs.DefaultCronjobs;
using iSoft.RabbitMQ.Services;
using iSoft.RabbitMQ;
using SourceBaseBE.Database.DBContexts;
using iSoft.Common.ExtensionMethods;
using iSoft.Common.Services;
using iSoft.InfluxDB.Services;
using Prometheus;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using SourceBaseBE.CommonFunc.HealthCheck;

namespace SourceBaseBE.TraceDataServiceNS
{
  public class Startup
  {

    public static ILoggerFactory loggerFactory = null;

    //private readonly ILoggerFactory loggerFactory;

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
      //this.loggerFactory = loggerFactory;
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

      services.AddMetricServer(options =>
      {
        options.Port = 12014;
      });
      services.AddHealthChecks()
          .AddCheck<ServiceHealthCheck>("TraceData Service")
          .ForwardToPrometheus();

      services.AddCors();
      services.AddControllers();

      //// configure DI for application services
      services.AddScoped<TraceDBContext>();
      //services.AddSingleton<TraceDataService, TraceDataService>();
      //services.AddHostedService<TraceDataHostedService>();
      services.AddHostedService<CheckConnectRedisJob>();
      services.AddHostedService<GCCollectCronjob>();
      services.AddHostedService<ResetMetricsCronjob>();

      services.AddSingleton(provider => new InfluxDBService(CommonConfig.GetConfig().InfluxDBConfig));

      services.AddSingleton(provider =>
      {
        //var influxDBService = provider.GetRequiredService<InfluxDBService>();
        return new RabbitMQService(CommonConfig.GetConfig().RabbitMQConfig);
      });

      services.AddHostedService(provider =>
      {
        //var influxDBService = provider.GetRequiredService<InfluxDBService>();
        //var traceDBContext = provider.GetRequiredService<TraceDBContext>();
        var rabbitMQService = provider.GetRequiredService<RabbitMQService>();
        var logger = provider.GetRequiredService<ILogger<TraceDataConsumerHostedService>>();
        return new TraceDataConsumerHostedService(logger, rabbitMQService, TopicName.TraceDataTopic);
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseSerilogRequestLogging();

      app.UseRouting();
      ////app.UseStaticFiles();

      app.UseHttpMetrics();

      //// global cors policy
      //app.UseCors(x => x
      //    .AllowAnyOrigin()
      //    .AllowAnyMethod()
      //    .AllowAnyHeader());

      //app.UseAuthentication();
      //app.UseAuthorization();

      //app.UseRouter(routes =>
      //{
      //  routes.MapGet("/", async context =>
      //  {
      //    context.Response.Redirect("/api/v1/dashboard");
      //  });
      //});

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
