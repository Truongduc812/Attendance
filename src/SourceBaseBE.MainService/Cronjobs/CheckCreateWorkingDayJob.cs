using iSoft.Common;
using iSoft.Common.ConfigsNS;
using iSoft.Common.Enums;
using iSoft.Redis.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using SourceBaseBE.MainService.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SourceBaseBE.MainService.Cronjobs
{
	public class CheckCreateWorkingDayJob : IHostedService, IDisposable
	{
		private int executionCount = 0;
		private ILogger<CheckCreateWorkingDayJob> _logger;
		private Timer _timer;
		private WorkingDayService workingDayService;
		public CheckCreateWorkingDayJob(ILogger<CheckCreateWorkingDayJob> logger, WorkingDayService workingDayService)
		{
			_logger = logger;
			this.workingDayService = workingDayService;
		}

		public Task StartAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("Timed CheckCreateWorkingDayJob Hosted Service start.");

			_timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

			return Task.CompletedTask;
		}

		private async void DoWork(object state)
		{
			try
			{
				var count = Interlocked.Increment(ref executionCount);

				_logger.LogInformation("Timed CheckCreateWorkingDayJob Hosted Service is working. Count: " + count);

				await CheckAllEmployeeBeenCreateWD();
			}
			catch (Exception ex)
			{
				_logger.LogMsg(Messages.ErrException, ex);
			}
		}
		private async Task CheckAllEmployeeBeenCreateWD()
		{
			try
			{
				_timer.Dispose();
				for (int i = 1; i <= DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); i++)
				{
					await workingDayService.CreateWdIfNotExist(new DateTime(DateTime.Now.Year, DateTime.Now.Month, i));
				}
			}
			catch (Exception ex)
			{

				_logger.LogMsg(Messages.ErrException, ex);
			}
			finally
			{
				_timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
			}
		}

		public Task StopAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("Timed CheckCreateWorkingDayJob Hosted Service is stopping.");
			//_timer?.Change(Timeout.Infinite, 0);
			if (_timer != null)
			{
				_timer.Dispose();
			}
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_timer?.Dispose();
		}
	}
}