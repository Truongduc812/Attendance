
using iSoft.Common;
using iSoft.Common.ExtensionMethods;
using iSoft.Common.Models.ConfigModel.Subs;
using iSoft.Common.Payloads;
using iSoft.Common.Utils;
using iSoft.ConnectionCommon.Model;
using iSoft.SocketIOClientNS.Services;
using Microsoft.Extensions.Logging;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SourceBaseBE.VirtualDeviceService.Services
{
  public partial class PushRealtimeService
  {
    private readonly ILogger<PushRealtimeService> _logger;


    //private DB
    public PushRealtimeService(ILogger<PushRealtimeService> logger)
    {
      this._logger = logger;
    }
    //public async Task Run()
    //{
    //  string funcName = "[SocketIO]";

    //  try
    //  {
    //    var guid = StringUtil.GenerateRandomKeyWithDateTime();

    //    Dictionary<string, object> dicVal = new Dictionary<string, object>();
    //    dicVal.Add("Value1", NumberUtil.GetRandomInt(100, 2000));
    //    dicVal.Add("Value2", NumberUtil.GetRandomDouble(-1, 3));

    //    TestRealtimeMessage message = new TestRealtimeMessage()
    //    {
    //      MessageId = guid,
    //      ConnectionId = NumberUtil.GetRandomInt(1, 200),
    //      DicVal = dicVal,
    //      ExecuteAt = DateTimeUtil.GetLocalDateTime(DateTime.Now),
    //    };

    //    string messageStr = message.ToJson();
    //    await SocketIOClientService.SendMessageAsync("input", "SourceBaseBE", "VirtualDevice", messageStr);

    //    this._logger.LogMsg(Messages.ISuccess_0_1, funcName, $"{guid}");
    //  }
    //  catch (Exception ex)
    //  {
    //    this._logger.LogMsg(Messages.ErrException.SetParameters(ex));
    //    throw ex;
    //  }
    //}
    public async Task SendMessage(DevicePayloadMessage message)
    {
      string funcName = "[SocketIO]";

      try
      {
        //var guid = StringUtil.GenerateRandomKeyWithDateTime();

        //Dictionary<string, object> dicVal = new Dictionary<string, object>();
        //dicVal.Add("Value1", NumberUtil.GetRandomInt(100, 2000));
        //dicVal.Add("Value2", NumberUtil.GetRandomDouble(-1, 3));

        //TestRealtimeMessage message = new TestRealtimeMessage()
        //{
        //  MessageId = guid,
        //  ConnectionId = NumberUtil.GetRandomInt(1, 200),
        //  DicVal = dicVal,
        //  ExecuteAt = DateTimeUtil.GetLocalDateTime(DateTime.Now),
        //};

        string messageStr = message.ToJson();
        await SocketIOClientService.SendMessageAsync("input", "VMS", "VirtualDevice", messageStr);

        this._logger.LogMsg(Messages.ISuccess_0_1, funcName, $"{message.MessageId}");
      }
      catch (Exception ex)
      {
        this._logger.LogMsg(Messages.ErrException.SetParameters(ex));
        throw ex;
      }
    }

    internal void Init(ServerConfigModel newConfig)
    {
      string address = string.Format("{0}:{1}", newConfig.Address, newConfig.Port);

      var client = SocketIOClientService.NewConnection(address, (SocketIO? client) =>
      {
        client.OnConnected += async (sender, e) =>
        {
          //await client.SendMessageAsync("input", message);
          _logger.LogInformation($"SocketIOServer connected.");
        };

        //client.On("alert", response =>
        //{
        //  string data = response.ToString();
        //  Debug.WriteLine(data);
        //});
      });
    }
  }
}