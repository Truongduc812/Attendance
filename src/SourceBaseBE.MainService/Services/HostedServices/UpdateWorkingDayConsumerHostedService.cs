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
using iSoft.RabbitMQ;
using iSoft.RabbitMQ.Services;
using SourceBaseBE.Database.Repository;
using SourceBaseBE.Database.DBContexts;
using static SourceBaseBE.MainService.ConstMain;
using iSoft.DBLibrary.DBConnections.Factory;
using iSoft.DBLibrary.DBConnections.Interfaces;
using iSoft.Common.Services;
using iSoft.InfluxDB.Services;
using SourceBaseBE.MainService.Models.Rabbit;
using Elasticsearch.Net;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Cryptography.Xml;
using System.Security.Policy;
using RestSharp;
using System.Net;
using System.IO;
using Microsoft.Extensions.Hosting;
using System.Linq;
using NPOI.POIFS.Crypt.Dsig;
using System.Timers;
using TwinCAT.Ads;
using SourceBaseBE.Database.Models.TrackDevice;
using SourceBaseBE.Database;
using SourceBaseBE.Database.Entities;
using iSoft.SocketIOClientNS.Services;
using SocketIOClient;
using iSoft.Common.CommonFunctionNS;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace SourceBaseBE.MainService.Services.HostedServices
{
    public class UpdateWorkingDayConsumerHostedService : IHostedService, IDisposable
    {
        CommonDBContext _dbContext;
        public EmployeeRepository _employeeRepository;
        public TimeSheetRepository _timeSheetRepository;
        public WorkingDayRepository _workingdayRepository;
        public WorkingDayService workingDayService;
        public WorkingTypeRepository workingTypeRepository;
        public UserRepository _userRepository;
        public MessageRepository _messageRepository;
        private ILogger<UpdateWorkingDayConsumerHostedService> _logger;
        private ILogger<WorkingDayService> _loggerWdService;
        private System.Timers.Timer _timer;
        private System.Timers.Timer _timer_check_connection;
        private bool? _previousStatus;
        private static readonly HttpClient _httpClient = new HttpClient();
        public UpdateWorkingDayConsumerHostedService(
          CommonDBContext db,
          ILogger<UpdateWorkingDayConsumerHostedService> logger,
          ILogger<WorkingDayService> loggerWdService
            )
        {
            this._logger = logger;
            _loggerWdService = loggerWdService;
            _userRepository = new UserRepository(db);
            _messageRepository = new MessageRepository(db);
            _previousStatus = null;
        }
        private async Task CreateTransactionDB(Action<CommonDBContext> action)
        {
            IDBConnectionCustom dBConnectionCustom = DBConnectionFactory.CreateDBConnection(CommonConfig.GetConfig().MasterDatabaseConfig);
            using (var db = new CommonDBContext(dBConnectionCustom))
            {
                try
                {
                    _employeeRepository = new EmployeeRepository(db);
                    _timeSheetRepository = new TimeSheetRepository(db);
                    _workingdayRepository = new WorkingDayRepository(db);
                    workingTypeRepository = new WorkingTypeRepository(db);
                    this.workingDayService = new WorkingDayService(db, _loggerWdService);
                    action(db);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                }
            }

        }

        private async Task CheckConnectServer()
        {
            string contentDisconnected = "Disconnected with Server FaceID";
            string contentConnected = "Connected with Server FaceID";
            var urlFaceId = Environment.GetEnvironmentVariable("AttendanceTrackAPI");

            string pattern = @"http://(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(urlFaceId);
            string ipFaceId = "";
            if (match.Success)
            {
                ipFaceId = match.Groups[1].Value;
            }
            else
            {
                ipFaceId = "";
            }
            //Ping thiết bị 
            bool isSend = true;
            bool currentStatus = await PingDeviceAsync(ipFaceId);
            if (_previousStatus == null || _previousStatus != currentStatus)
            {
                if (currentStatus)
                {
                    SendMessToSocketIOClient(contentConnected);
                }
                else
                {
                    SendMessToSocketIOClient(contentDisconnected);
                }
                _previousStatus = currentStatus;  // Update previous status
            }

        }
        public async Task GetDataTimeSheetFromDevices()
        {
            string errMessage = "";
            string dataJson = "";
            string funcName = "GetDataTimeSheetFromDevices";

            try
            {
                DateTime startTime = DateTime.Now;
                var serialNumberCheckIns = Environment.GetEnvironmentVariable("SerialNumberFaceIn").Split(",");
                var serialNumberCheckOuts = Environment.GetEnvironmentVariable("SerialNumberFaceOut").Split(",");
                var message = CallAPI().Result;
                if (message == null)
                {
                    _logger.LogMsg(Messages.ErrInputInvalid_0_1, funcName, $"err: message = null, {errMessage}, json: {dataJson}");
                    return;
                }
                var datas = message.Data.SelectMany(x => x);
                List<TimeSheetEntity> list = new List<TimeSheetEntity>();

                await CreateTransactionDB((context) =>
                {
                    foreach (var msg in datas)
                    {
                        // TODO:
                        if (serialNumberCheckIns.Contains(msg.Sn))
                        {
                            msg.AttState = EnumFaceId.Check_In;
                        }
                        if (serialNumberCheckOuts.Contains(msg.Sn))
                        {
                            msg.AttState = EnumFaceId.Check_Out;
                        }
                        var timesheet = _timeSheetRepository.UpsertIfNotExist(DateTime.Parse(msg.AttTime.ToString()), msg, msg.AttState).Result;
                        if (timesheet != null) // => just add new timesheet to recalculate working day
                        {
                            list.Add(timesheet);
                        }
                    }
                    if (list != null && list.Count() > 0)
                    {
                        var listGroup = list.GroupBy(x => x.WorkingDayId);
                        foreach (var group in listGroup)
                        {
                            var timesheets = group.Select(x => x).ToList();
                            workingDayService.ReCalculate(timesheets, null, null);
                        }
                        CachedFunc.ClearRedisByEntity(_workingdayRepository.GetName());
                        CachedFunc.ClearRedisByEntity(_timeSheetRepository.GetName());
                    }
                });


                _logger.LogMsg(Messages.ISuccess_0_1, funcName + " Call API Success", DateTimeUtil.GetHumanStr(DateTime.Now - startTime));
                return;
            }
            catch (JsonReaderException ex)
            {
                _logger.LogMsg(Messages.ErrBaseException.SetParameters($"json: {dataJson}", ex));
            }
            catch (DBException ex)
            {
                _logger.LogMsg(Messages.ErrDBException.SetParameters($"json: {dataJson}", ex));
                //throw ex;
            }
            catch (CriticalException ex)
            {
                _logger.LogMsg(Messages.ErrDBException.SetParameters($"json: {dataJson}", ex));
                //throw ex;
            }
            catch (BaseException ex)
            {
                _logger.LogMsg(Messages.ErrBaseException.SetParameters($"json: {dataJson}", ex));
                //throw ex;
            }
            catch (Exception ex)
            {
                _logger.LogMsg(Messages.ErrException.SetParameters($"json: {dataJson}", ex));
                //throw ex;
            }

        }
        private async Task<FaceIdPayload> CallAPI()
        {
            try
            {
                string funcName = "CallAPI";
                //client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("AttendanceTrackAPI"));

                // Add an Accept header for JSON format.
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                var str = Environment.GetEnvironmentVariable("AttendanceTrackAPI");
                int.TryParse(Environment.GetEnvironmentVariable("AttendanceTrackAPI_RangeDate"), out int range);
                _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                str += $"&param=[\"FromDate\",\"{DateTime.Now.AddDays(-range).ToString("yyyy-MM-dd")}\",\"ToDate\",\"{DateTime.Now.ToString("yyyy-MM-dd")}\"]";
                _logger.LogInformation($"API string: {str}");
                // List data response.
                HttpResponseMessage response = await _httpClient.GetAsync(str);  // Blocking call! Program will wait here until a response is received or a timeout occurs.
                if (response.IsSuccessStatusCode)
                {
                    _previousStatus = true;
                    // Parse the response body.
                    var res = response.Content.ReadAsStringAsync().Result;
                    var obj = JsonExtensionUtil.FromJson<FaceIdPayload>(res);  //Make sure to add a reference to System.Net.Http.Formatting.dll
                    return obj;
                }
                else
                {
                    string contentDisconnected = "Disconnected with Server FaceID";
                    string contentConnected = "Connected with Server FaceID";
                    bool currentStatus = false;
                    if (_previousStatus != null && _previousStatus != currentStatus)
                    {
                        if (currentStatus)
                        {
                            SendMessToSocketIOClient(contentConnected);
                        }
                        else
                        {
                            SendMessToSocketIOClient($"{contentDisconnected}: {response.StatusCode}-{response.ReasonPhrase}");
                        }

                    }
                    _previousStatus = currentStatus;  // Update previous status
                }
                // Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}\r\n{ex.StackTrace}");
                throw ex;
            }
        }

        public void SendMessToSocketIOClient(string content)
        {
            try
            {
                string funcName = "SendMessToSocketIOClient";
                DateTime currDate = DateTime.Now;
                List<EnumDepartmentAdmin?> listAdminRole = new List<EnumDepartmentAdmin?>();
                listAdminRole.Add(EnumDepartmentAdmin.Admin1);
                listAdminRole.Add(EnumDepartmentAdmin.Admin2);
                listAdminRole.Add(EnumDepartmentAdmin.Admin3);

                List<UserEntity> listAdmin = _userRepository.GetListByListAdminRole(listAdminRole);

                string channel = Environment.GetEnvironmentVariable("SOCKET_CHANNEL");
                string eventName = Environment.GetEnvironmentVariable("SOCKET_EVENT");
                string room = Environment.GetEnvironmentVariable("SOCKET_ROOM");

                var socketConfig = CommonConfig.GetConfig().SocketIOConfig;
                string address = string.Format("{0}:{1}", socketConfig.Address, socketConfig.Port);

                // TODO: Tách xử lý này ra common
                try
                {
                    var clientSocket = SocketIOClientService.NewConnection(address, (SocketIO? client) =>
                    {
                        client.OnConnected += async (sender, e) =>
                        {
                            await client?.EmitAsync("join_room", room, "");
                        };
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogMsg(Messages.ErrException, ex);
                }

                foreach (var admin in listAdmin)
                {
                    MessageEntity message = new MessageEntity();
                    message.Title = $"{content}";
                    message.Content = $"{content}";
                    message.URL = $"";      // TODO: Add URL
                    message.IsRead = false;
                    message.SendDate = currDate;
                    message.UserId = admin.Id;

                    try
                    {
                        this._logger.LogMsg(Messages.IFuncStart_0, funcName + $"Notification");

                        var message2 = message.Clone();
                        _messageRepository.Upsert(message2);
                    }
                    catch (Exception ex)
                    {
                        Serilog.Log.Error(Messages.ErrException.SetParameters(funcName, ex).GetMessage());
                    }
                    //});

                    string jsonStr = message.ToJson();
                    // send message to socket
                    Task.Run(async () =>
                    {
                        try
                        {
                            this._logger.LogMsg(Messages.IFuncStart_0, funcName + $" [socket] SendMessage");

                            for (int i = 0; i < 10; i++)
                            {
                                try
                                {
                                    await SocketIOClientService.SendMessageAsync(channel, room, "MainService", jsonStr);

                                    Serilog.Log.Information($"{funcName}, [socket] SendMessage successfully");

                                    break;
                                }
                                catch (Exception ex)
                                {
                                    Thread.Sleep(300);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Serilog.Log.Error(Messages.ErrException.SetParameters(funcName, ex).GetMessage());
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> PingDeviceAsync(string ipAddress)
        {
            using (Ping ping = new Ping())
            {
                try
                {
                    PingReply reply = await ping.SendPingAsync(ipAddress);
                    return reply.Status == IPStatus.Success;
                }
                catch (PingException ex)
                {
                    // Log exception or handle it accordingly
                    return false;
                }
            }
        }

        public void Dispose()
        {

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed CheckCreateWorkingDayJob Hosted Service start.");
            _timer = new System.Timers.Timer();
            _timer.Interval = 10000;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
            //_timer_check_connection.Interval = 5000;
            //_timer_check_connection.Elapsed += _timer_check_connection_Elapsed; ;
            //_timer_check_connection.Start();
            return Task.CompletedTask;
        }

        private async void _timer_check_connection_Elapsed(object sender, ElapsedEventArgs e)
        {
            _timer_check_connection.Stop();
            try
            {
                await CheckConnectServer();
            }
            catch (Exception ex)
            {

                _logger.LogError($"{ex.Message}\r\n{ex.StackTrace}");
            }
            finally
            {
                _timer_check_connection.Start();
            }

        }

        private async void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timer.Stop();
                await GetDataTimeSheetFromDevices();

            }
            catch (Exception ex)
            {
                _logger.LogError(Messages.ErrException.GetCode(), ex);
            }
            finally
            {
                _timer.Start();
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
        }
    }
}
