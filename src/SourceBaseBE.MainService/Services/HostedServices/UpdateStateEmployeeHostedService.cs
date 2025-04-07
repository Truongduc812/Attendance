using iSoft.Common.ConfigsNS;
using iSoft.Common.Exceptions;
using iSoft.Common.ExtensionMethods;
using iSoft.Common.Utils;
using iSoft.Common;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using iSoft.RabbitMQ.Payload;
using System.Threading;
using SourceBaseBE.CommonFunc.DataService;
using SourceBaseBE.CommonFunc.EnvConfigData;
using iSoft.Common.Models.ConfigModel.Subs;
using iSoft.Redis.Services;
using iSoft.Common.Enums;
using RabbitMQ.Client.Events;
using iSoft.Common.Services;
using iSoft.InfluxDB.Services;
using SourceBaseBE.Database.DBContexts;
using SourceBaseBE.Database.Repository;
using Microsoft.Extensions.Hosting;
using iSoft.ConnectionCommon.MessageQueueNS;
using System.Transactions;
using System.Linq;
using SourceBaseBE.Database.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace iSoft.RabbitMQ.Services
{
    public class UpdateStateEmployeeHostedService : BackgroundService
    {
        public ILogger _logger;
        private CommonDBContext _dbContext;
        private WorkingDayRepository _workingDayRepository;
        private SystemSettingRepository _systemSettingRepositoty;
        public UpdateStateEmployeeHostedService(
            CommonDBContext dbContext,
            ILogger<UpdateStateEmployeeHostedService> logger)
        {
            _logger = logger;
            this._dbContext = dbContext;
            this._workingDayRepository = new WorkingDayRepository(_dbContext);
            this._systemSettingRepositoty = new SystemSettingRepository(_dbContext);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Run(() => Init(stoppingToken));
        }
        public virtual async Task Init(CancellationToken stoppingToken)
        {
            bool isFistTime = false;
            string timeIntervalEnv = Environment.GetEnvironmentVariable("IntervalTimeUpdate");
            int intervalUpdate = 0;
            if (!string.IsNullOrEmpty(timeIntervalEnv))
            {
                intervalUpdate = int.Parse(timeIntervalEnv);
            }
            if(intervalUpdate == 0)
            {
                intervalUpdate = 60000;
            }
            while (true)
            {
                try
                {
                    if(isFistTime)
                    {
                        Thread.Sleep(intervalUpdate);
                    }
                    isFistTime = true;

                    //get interval setting
                    var settingSystem = _systemSettingRepositoty.GetSystemSetting();

                    if (settingSystem == null)
                    {
                        SystemSettingEntity systemSettingEntity = new SystemSettingEntity();
                        {
                            systemSettingEntity.UpdateStateEmployeeInterval = 1;
                            systemSettingEntity.TimeSwitchInterval = 5;
                        }
                        _systemSettingRepositoty.Upsert(systemSettingEntity);
                        continue;
                    }
                    long intervalUpdateUnitDay = settingSystem.UpdateStateEmployeeInterval.Value;
                    long intervalUpdateUnitSecond = intervalUpdateUnitDay * 24 * 60 * 60;
                    //Get List Workingday have a time
                    var listWorkingDay = _workingDayRepository.GetListWorkingDayUpdateState(intervalUpdateUnitSecond);

                    //kiểm tra workingday nào có time in quá 24h chưa có timeOut thì cập nhập trạng thái 
                    if (listWorkingDay == null || listWorkingDay.Count == 0)
                    {
                        continue;
                    }

                    using (var transaction = this._dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (var workingday in listWorkingDay)
                            {
                                workingday.InOutState = EnumInOutTypeStatus.Outside;
                                _workingDayRepository.Upsert(workingday);
                            }

                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                        }
                    }

                }
                catch (Exception ex)
                {

                    _logger.LogMsg(Messages.ErrException, ex);
                }
            }
        }
        public override void Dispose()
        {
            try
            {
                base.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogMsg(Messages.ErrException, ex);
            }
        }
        //public override async Task Init(CancellationToken stoppingToken)
        //{

        //    try
        //    {
        //        //Get List Workingday have a time 
        //        var currentTime = DateTime.Now;
        //        var listWorkingDay = _workingDayRepository.GetListWorkingDayUpdateState();
        //        //kiểm tra workingday nào có time in quá 24h chưa có timeOut thì cập nhập trạng thái 
        //        if (listWorkingDay == null || listWorkingDay.Count == 0)
        //        {
        //            return;
        //        }
        //        foreach (var workingday in listWorkingDay)
        //        {
        //            DateTime? TimeIn = workingday.Time_In;
        //            TimeSpan duration = TimeSpan.Zero;

        //            if (TimeIn.HasValue)
        //            {
        //                duration = DateTime.Now - TimeIn.Value;
        //                if (duration.TotalSeconds > 28800)
        //                {
        //                    workingday.InOutState = EnumInOutTypeStatus.Outside;
        //                    _workingDayRepository.Upsert(workingday);
        //                }
        //            }
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogMsg(Messages.ErrException, ex);
        //    }
        //    Thread.Sleep(3000);
        //}
    }
}
