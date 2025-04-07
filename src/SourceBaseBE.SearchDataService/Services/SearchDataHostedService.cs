//using iSoft.Common.ConfigsNS;
//using iSoft.Common.Utils;
//using iSoft.ConnectionCommon.MessageQueueNS;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using NPOI.SS.Formula.Functions;
//using SourceBaseBE.CommonFunc.MessageQueue;
//using SourceBaseBE.Main.SearchDataServiceNS.Services;
//using SourceBaseBE.VirtualDeviceService.Services;
//using System;
//using System.Threading;
//using System.Threading.Tasks;

//namespace SourceBaseBE.VirtualDeviceService.Services
//{
//  public class SearchDataHostedService : IHostedService, IDisposable
//  {
//    private int executionCount = 0;
//    private readonly ILogger<SearchDataHostedService> _logger;
//    SearchDataService _searchDataService;

//    public SearchDataHostedService(ILogger<SearchDataHostedService> logger)
//    {
//      _logger = logger;
//      _searchDataService = new SearchDataService();
//    }

//    public Task StartAsync(CancellationToken stoppingToken)
//    {
//      _logger.LogInformation(nameof(SearchDataHostedService) + " start.");

//      DoWork(null);

//      return Task.CompletedTask;
//    }

//    private async void DoWork(object state)
//    {
//      var count = Interlocked.Increment(ref executionCount);

//      _logger.LogInformation(nameof(SearchDataHostedService) + " is working. Count: {Count}", count);

//      await _searchDataService.Run();
//    }

//    public Task StopAsync(CancellationToken stoppingToken)
//    {
//      _logger.LogInformation(nameof(SearchDataHostedService) + " is stopping.");

//      return Task.CompletedTask;
//    }

//    public void Dispose()
//    {
//      //throw new NotImplementedException();
//    }
//  }
//}