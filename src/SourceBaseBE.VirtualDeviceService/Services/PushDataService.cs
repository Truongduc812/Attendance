

using iSoft.Common;
using iSoft.Common.Exceptions;
using iSoft.Common.Utils;
//using iSoft.ConnectionCommon.MessageQueueNS;
using iSoft.ConnectionCommon.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using SourceBaseBE.CommonFunc.EnvConfigData;
//using SourceBaseBE.CommonFunc.MessageQueue;
using iSoft.Common.ConfigsNS;
using iSoft.RabbitMQ.Services;
using Sprache;

namespace SourceBaseBE.VirtualDeviceService.Services
{
  public partial class PushDataService
  {
    private readonly ILogger<PushDataService> _logger;
    private RabbitMQService _rabbitMQService;
    public PushDataService(ILogger<PushDataService> logger, RabbitMQService rabbitMQService)
    {
      this._logger = logger;
      this._rabbitMQService = rabbitMQService;
    }
    //public async Task Run()
    //{
    //  string funcName = "[RABBITMQ]";

    //  try
    //  {
    //    var guid = StringUtil.GenerateRandomKeyWithDateTime();
    //    await PushMessageAsyncTest(ExchangeName.SourceBaseBEEnvEx, guid);

    //    this._logger.LogMsg(Messages.ISuccess_0_1, funcName, $"{guid}");
    //  }
    //  catch (BaseException ex)
    //  {
    //    this._logger.LogMsg(Messages.ErrBaseException.SetParameters(ex));
    //    throw ex;
    //  }
    //  catch (Exception ex)
    //  {
    //    this._logger.LogMsg(Messages.ErrException.SetParameters(ex));
    //    throw ex;
    //  }
    //}
    static Dictionary<string, long> dicEnv2Counter = new Dictionary<string, long>();

    public async Task<DevicePayloadMessage> PushMessageAsyncTest(string exchangeName, DevicePayloadMessage message)
    {
      string funcName = "[RABBITMQ]";

      //await MessageQueue.PushMessageAsync(exchangeName, true, message);
      //Debug.WriteLine("[PushSearchData] " + j.ToString());
      //Debug.WriteLine("[PushMessageAsyncTest] Elapsed Time: [" + DateTimeUtil.GetHumanStr(DateTime.Now - startTime) + "]");

      _rabbitMQService.PushMessage(message, true, exchangeName);

      this._logger.LogMsg(Messages.ISuccess_0_1, funcName, $"{message.MessageId}");

      return message;
    }
    public List<DevicePayloadMessage> CreateTestMessage()
    {
      Random random = new Random();
      DateTime dt = DateTime.Now;
      DateTime startTime = DateTime.Now;

      List<DevicePayloadMessage> listRS = new List<DevicePayloadMessage>();

      Dictionary<string, object> dicData = new Dictionary<string, object>();

      int i = 0;
      var guid = StringUtil.GenerateRandomKeyWithDateTime();
      dicData.Add("checkTime", DateTimeUtil.GetLocalDateTime(DateTime.Now));
      dicData.Add("employeeMachineCode", $"{random.Next(0, 100).ToString().PadLeft(4, '0')}");
      dicData.Add("employeeFullName", $"Requester{random.Next(0, 100)}");
      dicData.Add("inOutType", random.Next(0, 2));
      DevicePayloadMessage message = new DevicePayloadMessage()
      {
        MessageId = guid,
        ConnectionId = 1,
        Data = dicData,
        ExecuteAt = DateTimeUtil.GetLocalDateTime(DateTime.Now),
      };
      listRS.Add(message);

      return listRS;
    }
    //public List<DevicePayloadMessage> CreateTestMessage()
    //{
    //  DateTime dt = DateTime.Now;

    //  List<DevicePayloadMessage> listRS = new List<DevicePayloadMessage>();

    //  var envConfigData = EnvConfigData.Ins;
    //  var listEnvConfig = envConfigData.GetListEnvConfigModel();

    //  Dictionary<string, object> dicData = new Dictionary<string, object>();
    //  Dictionary<long, bool> dicIsExist = new Dictionary<long, bool>();

    //  int i = 0;
    //  long lastConnectionId = 0;
    //  foreach (var envConfig in listEnvConfig)
    //  {
    //    if (lastConnectionId != envConfig.ConnectionId)
    //    {
    //      if (lastConnectionId >= 1)
    //      {
    //        if (!dicIsExist.ContainsKey(lastConnectionId))
    //        {
    //          dicIsExist.Add(lastConnectionId, true);
    //          var guid = StringUtil.GenerateRandomKeyWithDateTime();
    //          DevicePayloadMessage message = new DevicePayloadMessage()
    //          {
    //            MessageId = guid,
    //            ConnectionId = lastConnectionId,
    //            Data = dicData,
    //            ExecuteAt = DateTimeUtil.GetLocalDateTime(DateTime.Now),
    //          };
    //          listRS.Add(message);
    //          dicData = new Dictionary<string, object>();
    //        }
    //      }
    //    }
    //    switch (envConfig.Requester)
    //    {
    //      case iSoft.Common.Enums.EnumDataType.Bool:
    //        dicData.Add(envConfig.EnviromentVarName, true);
    //        break;
    //      case iSoft.Common.Enums.EnumDataType.DateTime:
    //        dicData.Add(envConfig.EnviromentVarName, DateTime.Now);
    //        break;
    //      case iSoft.Common.Enums.EnumDataType.Byte:
    //      case iSoft.Common.Enums.EnumDataType.Short:
    //        long valueS = 0;
    //        if ((dt.Minute / 3) % 2 == 0)
    //        {
    //          valueS = 2;
    //        }
    //        else
    //        {
    //          valueS = 1;
    //        }
    //        dicData.Add(envConfig.EnviromentVarName, valueS);
    //        break;
    //      case iSoft.Common.Enums.EnumDataType.Int:
    //      case iSoft.Common.Enums.EnumDataType.Long:
    //        long value = 100 + i * 10;
    //        dicData.Add(envConfig.EnviromentVarName, value);
    //        break;
    //      case iSoft.Common.Enums.EnumDataType.Double:
    //        dicData.Add(envConfig.EnviromentVarName, (NumberUtil.GetRandomDouble(10000000, 99999999)));
    //        break;
    //      case iSoft.Common.Enums.EnumDataType.String:
    //      case iSoft.Common.Enums.EnumDataType.String50:
    //      case iSoft.Common.Enums.EnumDataType.String255:
    //        dicData.Add(envConfig.EnviromentVarName, "str_" + i.ToString());
    //        break;
    //    }
    //    i++;

    //    lastConnectionId = envConfig.ConnectionId;
    //  }

    //  if (lastConnectionId >= 1)
    //  {
    //    if (!dicIsExist.ContainsKey(lastConnectionId))
    //    {
    //      dicIsExist.Add(lastConnectionId, true);
    //      var guid = StringUtil.GenerateRandomKeyWithDateTime();
    //      DevicePayloadMessage message = new DevicePayloadMessage()
    //      {
    //        MessageId = guid,
    //        ConnectionId = lastConnectionId,
    //        Data = dicData,
    //        ExecuteAt = DateTimeUtil.GetLocalDateTime(DateTime.Now),
    //      };
    //      listRS.Add(message);
    //    }
    //  }

    //  return listRS;
    //}
  }
}