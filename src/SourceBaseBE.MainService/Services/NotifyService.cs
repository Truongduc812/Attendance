using iSoft.DBLibrary.DBConnections.Factory;
using iSoft.Common.Enums.DBProvider;
using Serilog;
using iSoft.Common.ConfigsNS;
using SourceBaseBE.Database.Repository;
using SourceBaseBE.Database.DBContexts;
using MathNet.Numerics.Statistics.Mcmc;

using System;
using iSoft.Common.Exceptions;
using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using iSoft.Common.Models.RequestModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using iSoft.Database.Entities;
using System.Linq;
using SourceBaseBE.MainService.Models;
using SourceBaseBE.Database.Enums;

using iSoft.Database.Extensions;
using iSoft.Common.Models;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common;
using iSoft.Common.Enums;
using iSoft.Database.Models;
using Microsoft.Extensions.Logging;
using iSoft.SocketIOClientNS.Services;
using Microsoft.AspNetCore.Mvc;
using SocketIOClient;
using System.Threading.Tasks;
using iSoft.Common.Services;
using System.Net.Http;
using iSoft.Common.ExtensionMethods;

namespace SourceBaseBE.MainService.Services
{
	public class NotifyService
	{
		private readonly ILogger<NotifyService> _logger;
		private SocketIO _socketIOClient;
		private HttpService httpService;
		public NotifyService(ILogger<NotifyService> logger)
		{
			this._logger = logger;
			InitSocket();
		}
		public async Task<bool> InitSocket()
		{

			var socketConfig = CommonConfig.GetConfig().SocketIOConfig;
			var portStr = "";
			if ((socketConfig.Port != null && socketConfig.Port != 0))
			{
				portStr = socketConfig.Port.ToString();
			}
			string address = $"{socketConfig.Address}{portStr}";
			_socketIOClient = SocketIOClientService.NewConnection(address, (SocketIO? client) =>
			{
				client.OnConnected += async (sender, e) =>
				{
					await client.EmitAsync("join_room", "SourceBaseBE", "User1");
				};

				client.On("SourceBaseBE", response =>
				{
					string data = response.ToString();
					_logger.LogInformation(data);

					client.EmitAsync("ack", "SourceBaseBE");
				});
			});
			return true;
		}
		public async Task<bool> SendMessage(string jsonStr, string room = "AT", string channel = "input", string eventName = "AT_notify")
		{

			string funcName = nameof(SendMessage);
			if (_socketIOClient == null)
			{
				throw new InvalidOperationException("Socket Not inited");
			}
			this._logger.LogMsg(Messages.IFuncStart_0, funcName);
			await SocketIOClientService.SendMessageAsync(channel, room, eventName, jsonStr);
			//var firbaseBase = Environment.GetEnvironmentVariable("FIREBASE_BASE_URL");
			//var urlFirebase = firbaseBase + "send-message";
			//var WoU = (data as WorkingDayUpdateEntity);
			//dynamic datFCM = new
			//{
			//	Title = "Update Working day",
			//	Body = (data as WorkingDayUpdateEntity).WorkingType != null ? (data as WorkingDayUpdateEntity).WorkingType.ToString() : "Unknown",
			//	ClickAction = "1",
			//	ListUserId = new int[] { 8 }
			//};
			//await HttpService.PostAsync(urlFirebase, HttpMethod.Post, null, datFCM);
			this._logger.LogMsg(Messages.ISuccess_0_1, funcName, "");
			return true;
		}
	}
}