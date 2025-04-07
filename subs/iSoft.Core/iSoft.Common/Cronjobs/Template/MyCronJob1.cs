//using iSoft.Common.Cronjobs.Template.Base;
//using Microsoft.Extensions.Logging;

//namespace iSoft.Common.Cronjobs.Template
//{
//    public class MyCronJob1 : CronJobService
//  {
//    private int executionCount = 0;
//    private readonly ILogger<MyCronJob1> _logger;

//    public MyCronJob1(IScheduleConfig<MyCronJob1> config, ILogger<MyCronJob1> logger)
//        : base(config.CronExpression, config.TimeZoneInfo)
//    {
//      _logger = logger;
//    }

//    public override Task StartAsync(CancellationToken cancellationToken)
//    {
//      _logger.LogInformation("CronJob 1 start.");

//      return base.StartAsync(cancellationToken);
//    }

//    public override Task DoWork(CancellationToken cancellationToken)
//    {
//      var count = Interlocked.Increment(ref executionCount);

//      _logger.LogInformation("CronJob 1 is working. Count: {Count}", count);

//      return Task.CompletedTask;
//    }

//    public override Task StopAsync(CancellationToken cancellationToken)
//    {
//      _logger.LogInformation("CronJob 1 is stopping.");
//      return base.StopAsync(cancellationToken);
//    }

//    public override void Dispose()
//    {
//      base.Dispose();
//    }
//  }
//}