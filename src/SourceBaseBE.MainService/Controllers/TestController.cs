using iSoft.Common;
using iSoft.SocketIOClientNS.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SocketIOClient;
using System.Diagnostics;
using System.Threading.Tasks;
using iSoft.Common.ConfigsNS;
using System.IO;
using System;
using System.Threading;

namespace SourceBaseBE.MainService.Controllers
{
  [ApiController]
  [Route("api/v1/Test")]
  public class TestController : ControllerBase
  {
    private ILogger _logger = Serilog.Log.Logger;
    public TestController()
    {

    }

    [HttpGet]
    [Route("list-attendance")]
    public async Task<IActionResult> GetListAttendance()
    {
      Messages.Message errMessage = null;

      try
      {
        string jsonPath = Path.Combine("Jsons", "list-attendance-ex-rs.json");
        string jsonStr = System.IO.File.ReadAllText(jsonPath);
        return this.ResponseJSonStr(jsonStr);
      }
      catch (Exception ex)
      {
        errMessage = Messages.ErrException.SetParameters(ex);
      }
      this._logger.LogMsg(errMessage);
      return this.ResponseError(errMessage);
    }

    [HttpGet]
    [Route("send-message")]
    public async Task<IActionResult> SendMessage()
    {
      string funcName = nameof(SendMessage);

      this._logger.LogMsg(Messages.IFuncStart_0, funcName);

      var socketConfig = CommonConfig.GetConfig().SocketIOConfig;
      string address = string.Format("{0}:{1}", socketConfig.Address, socketConfig.Port);
      string message = "Message from C#";
      var client = SocketIOClientService.NewConnection(address, (SocketIO? client) =>
      {
        client.OnConnected += async (sender, e) =>
        {
          await client.EmitAsync("join_room", "Room1", "User1");
        };

        client.On("SourceBaseBE", response =>
        {
          string data = response.ToString();
          Debug.WriteLine(data);

          client.EmitAsync("ack", "Room1");
        });
      });

      this._logger.LogMsg(Messages.ISuccess_0_1, funcName, "");

      return this.ResponseOk("");
    }

    [HttpGet]
    [Route("send-message-2")]
    public async Task<IActionResult> SendMessage2()
    {
      string funcName = nameof(SendMessage);

      this._logger.LogMsg(Messages.IFuncStart_0, funcName);

      string channel = Environment.GetEnvironmentVariable("SOCKET_CHANNEL");
      string eventName = Environment.GetEnvironmentVariable("SOCKET_EVENT");
      string room = Environment.GetEnvironmentVariable("SOCKET_ROOM");
      string jsonStr = "jsonStr";

      var socketConfig = CommonConfig.GetConfig().SocketIOConfig;
      string address = string.Format("{0}:{1}", socketConfig.Address, socketConfig.Port);

      var client = SocketIOClientService.NewConnection(address, (SocketIO? client) =>
      {
        client.OnConnected += async (sender, e) =>
        {
          await client.EmitAsync("join_room", room, "currentUser.Username");
        };
      });

      Thread.Sleep(1000);

      await SocketIOClientService.SendMessageAsync(channel, room, "MainService", jsonStr);

      this._logger.LogMsg(Messages.ISuccess_0_1, funcName, "");

      return this.ResponseOk("");
    }
  }
}
