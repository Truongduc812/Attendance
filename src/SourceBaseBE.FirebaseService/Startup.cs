using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using iSoft.Firebase.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Routing;
using System.IO;
using iSoft.Common.Util;
using iSoft.Common.ConfigsNS;
using iSoft.Common.Cronjobs.DefaultCronjobs;

namespace iSoft.Firebase
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

      // configure DI for application services
      services.AddScoped<FirebaseService, FirebaseService>();
      services.AddHostedService<GCCollectCronjob>();

      //services.AddControllers().AddNewtonsoftJson(options =>
      //{
      //  options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
      //});
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

      app.UseHttpsRedirection();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseRouter(routes =>
      {
        routes.MapGet("/", async context =>
        {
          context.Response.Redirect("/home");
        });
      });

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
