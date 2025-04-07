using Microsoft.AspNetCore.Mvc;
using iSoft.Firebase.Services;
using System.Threading.Tasks;
using iSoft.Common;
using System;
using Microsoft.Extensions.Logging;
using iSoft.Database.Entities;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using iSoft.Common.ConfigsNS;
using System.Linq;
using iSoft.Firebase.Models;
using iSoft.Firebase;
using Microsoft.AspNetCore.Authorization;
using iSoft.Common.ExtensionMethods;
using static iSoft.Common.ConstCommon;

namespace iSoft.Firebase.Controllers
{
	//[Authorize]
	[ApiController]
	[Route("api/v1/[controller]")]
	public class FirebaseController : Controller
	{
		private readonly ILogger<FirebaseController> logger;
		private readonly FirebaseService firebaseService;

		public FirebaseController(ILoggerFactory loggerFactory, FirebaseService firebaseService)
		{
			this.logger = loggerFactory.CreateLogger<FirebaseController>();
			this.firebaseService = firebaseService;
		}

		//[HttpGet]
		//[Route("")]
		//public IActionResult Index()
		//{
		//  return View("firebase");
		//}

		//[HttpPost]
		//[Route("StoreToken")]
		//public IActionResult StoreToken(string token)
		//{
		//  try
		//  {
		//    string funcName = nameof(RegisterToken);

		//    FCMEntity entity = new FCMEntity();
		//    entity.UserId = 8; // TODO: userId = logined userId
		//    entity.Token = token;

		//    var rs = firebaseService.Upsert(entity);

		//    this.logger.LogMsg(Messages.ISuccess_0_1, funcName, rs);
		//    return this.ResponseOk(rs);
		//  }
		//  catch (Exception ex)
		//  {
		//    var message = Messages.ErrException.SetParameters(ex);
		//    this.logger.LogMsg(message);
		//    return this.ResponseError(message);
		//  }
		//}

		[HttpGet]
		[Route("get-client-config")]
		public IActionResult GetClientConfig()
		{
			try
			{
				string funcName = nameof(GetClientConfig);

				var rs = CommonConfig.GetConfig().FCMConfig.FMCClient;

				this.logger.LogMsg(Messages.ISuccess_0_1, funcName, rs);

				return this.ResponseOk(rs);
			}
			catch (Exception ex)
			{
				var message = Messages.ErrException.SetParameters(ex);
				this.logger.LogMsg(message);
				return this.ResponseError(message);
			}
		}

		[HttpPost]
		[Route("register-token")]
		public IActionResult RegisterToken([FromBody] RegisterTokenModel body)
		{
			try
			{
				string funcName = nameof(RegisterToken);

				FCMEntity entity = new FCMEntity();
				entity.UserId = body.UserId;
				entity.Token = body.Token;

				var rs = firebaseService.Upsert(entity);

				this.logger.LogMsg(Messages.ISuccess_0_1, funcName, rs);
				return this.ResponseOk(rs);
			}
			catch (Exception ex)
			{
				var message = Messages.ErrException.SetParameters(ex);
				this.logger.LogMsg(message);
				return this.ResponseError(message);
			}
		}

		[HttpPost]
		[Route("delete-token")]
		public IActionResult DeleteToken([FromQuery] long userId)
		{
			try
			{
				string funcName = nameof(DeleteToken);

				FCMEntity entity = new FCMEntity();
				entity.UserId = userId;
				var rs = firebaseService.Delete(entity, userId);

				this.logger.LogMsg(Messages.ISuccess_0_1, funcName, rs);
				return this.ResponseOk(rs);
			}
			catch (Exception ex)
			{
				var message = Messages.ErrException.SetParameters(ex);
				this.logger.LogMsg(message);
				return this.ResponseError(message);
			}
		}

		[HttpPost]
		[Route("send-message")]
		public async Task<IActionResult> SendMessage([FromBody] FirebaseMessageModel firebaseMessage)
		{
			try
			{
				string funcName = nameof(SendMessage);
				this.logger.LogMsg(Messages.IFuncStart_0, funcName);

				Notification model = new Notification()
				{
					Tag = Guid.NewGuid().ToString("N"),
					Icon = CommonConfig.GetConfig().FCMConfig.FMCServer.DefaultIcon,
					Vibrate = CommonConfig.GetConfig().FCMConfig.FMCServer.DefaultVibrate.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => { return int.Parse(x); }).ToArray(),
				};

				if (firebaseMessage != null)
				{
					if (firebaseMessage.Tag != null)
					{
						model.Tag = firebaseMessage.Tag;
					}
					if (firebaseMessage.ClickAction != null)
					{
						model.ClickAction = firebaseMessage.ClickAction;
					}
					if (firebaseMessage.Icon != null)
					{
						model.Icon = firebaseMessage.Icon;
					}
					if (firebaseMessage.Vibrate != null)
					{
						model.Vibrate = firebaseMessage.Vibrate.ToArray();
					}
					if (firebaseMessage.Title != null)
					{
						model.Title = firebaseMessage.Title;
					}
					if (firebaseMessage.Body != null)
					{
						model.Body = firebaseMessage.Body;
					}
				}

				var serverKey = CommonConfig.GetConfig().FCMConfig.FMCServer.ServerKey;
				var messagingSenderId = CommonConfig.GetConfig().FCMConfig.FMCServer.MessagingSenderId;
				var tokens = firebaseService.GetByListUserId(firebaseMessage.ListUserId).Select(x => x.Token);
				tokens = tokens.Distinct().ToArray();

				foreach (var userToken in tokens)
				{
					if (string.IsNullOrEmpty(userToken) || userToken.Trim() == "")
					{
						continue;
					}

					var pn = new MessageModel
					{
						To = userToken,
						Notification = model
					};

					var client = new RestClient(ConstFirebase.FCMSendURL);
					var request = new RestRequest(ConstFirebase.FCMSendURL, Method.Post);
					request.AddHeader("Cache-Control", "no-cache");
					request.AddHeader("Authorization", $"key={serverKey}");
					request.AddHeader("Sender", $"id={messagingSenderId}");
					request.AddHeader("Content-Type", "application/json");
					request.AddParameter("undefined", JsonConvert.SerializeObject(pn), ParameterType.RequestBody);

					// TODO: testing below function:
					var response = await client.ExecuteAsync(request);

					if (response.StatusCode != HttpStatusCode.OK)
					{
						var message = Messages.ErrAbnormalData_0_1.SetParameters(funcName, $"fcm send error, response: {response.StatusCode}");
						this.logger.LogMsg(message);
						return this.ResponseError(message);
					}
				}

				this.logger.LogMsg(Messages.ISuccess_0_1, funcName, firebaseMessage.ToJson());

				return this.ResponseOk(model);
			}
			catch (Exception ex)
			{
				var message = Messages.ErrException.SetParameters(ex);
				this.logger.LogMsg(message);
				return this.ResponseError(message);
			}
		}

	}
}
