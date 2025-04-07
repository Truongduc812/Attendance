using iSoft.Common.Cronjobs.DefaultCronjobs;
using iSoft.Common.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Text;

namespace iSoft.RemoteConfig
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
          .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] - {Message}{NewLine}{Exception}");

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

      services.AddCors();
      services.AddControllers();

      services.AddControllersWithViews();

      services.AddSingleton<ILoggerFactory, LoggerFactory>();
      services.AddHostedService<GCCollectCronjob>();

      services.AddSession(options =>
      {
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseSerilogRequestLogging();

      app.UseRouting();

      // global cors policy
      app.UseCors(x => x
          .AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());

      app.UseStaticFiles();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseSession();

      app.UseRouter(routes =>
      {
        routes.MapGet("/", async context =>
        {
          context.Response.Redirect("/Home/Setting");
        });
      });

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
