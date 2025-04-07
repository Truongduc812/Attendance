//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using NPOI.SS.Formula.Functions;
//using System;
//using System.Threading;
//using System.Threading.Tasks;

//namespace SourceBaseBE.Main.TraceDataService.Services
//{
//  public class TraceDataHostedService : IHostedService, IDisposable
//  {
//    private int executionCount = 0;
//    private readonly ILogger<TraceDataHostedService> _logger;
//    private readonly TraceDataService _tracedataService;

//    public TraceDataHostedService(ILogger<TraceDataHostedService> logger)
//    {
//      _logger = logger;
//      _tracedataService = new TraceDataService();
//    }

//    public Task StartAsync(CancellationToken stoppingToken)
//    {
//      _logger.LogInformation(nameof(TraceDataHostedService) + " start.");

//      DoWork(null);

//      return Task.CompletedTask;
//    }

//    private void DoWork(object state)
//    {
//      var count = Interlocked.Increment(ref executionCount);

//      _logger.LogInformation(nameof(TraceDataHostedService) + " is working. Count: {Count}", count);

//      _tracedataService.Run();

//    }

//    public Task StopAsync(CancellationToken stoppingToken)
//    {
//      _logger.LogInformation(nameof(TraceDataHostedService) + " is stopping.");

//      return Task.CompletedTask;
//    }

//    public void Dispose()
//    {
//      //throw new NotImplementedException();
//    }
//  }
//}