using Microsoft.AspNetCore.Mvc;
using Serilog;
using iSoft.Common.Exceptions;
using iSoft.Common;
using System;
using SourceBaseBE.Database.Models.ResponseModels;
using SourceBaseBE.Database.Entities;
using SourceBaseBE.Database.Models.RequestModels;
using SourceBaseBE.MainService.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using iSoft.Common.Models.RequestModels;
using iSoft.Common.Models.ResponseModels;
using System.Linq;
using SourceBaseBE.MainService.Models.RequestModels.Report;
using Microsoft.AspNetCore.Authorization;
using static iSoft.Common.Messages;
using System.Threading.Channels;
using ConnectionCommon.MessageBroker;
using System.Net.Http;
using TwinCAT.Ams;
using MathNet.Numerics.Statistics.Mcmc;
using SourceBaseBE.Database.Models.RequestModels.Report;
using iSoft.Common.CommonFunctionNS;
using iSoft.Common.ExtensionMethods;
using iSoft.Common.Utils;
using static SourceBaseBE.MainService.ConstMain;
using NPOI.POIFS.FileSystem;
using iSoft.Common.Enums;
using PRPO.MainService.CustomAttributes;
using SourceBaseBE.MainService.CustomAttributes;
using SourceBaseBE.Database.Helpers;
using Nest;
using Microsoft.Extensions.DependencyInjection;
using iSoft.Common.ConfigsNS;
using iSoft.SocketIOClientNS.Services;
using SocketIOClient;
using System.Threading;
using iSoft.Redis.Services;
using iSoft.ElasticSearch.Models;
using NPOI.Util;

namespace SourceBaseBE.MainService.Controllers
{
    [ApiController]
    [Route("api/v1/Report")]
    public class ReportController : BaseCRUDController<EmployeeEntity, EmployeeRequestModel, EmployeeResponseModel>
    {
        private UserService _userService;
        private EmployeeService employeeService;
        private WorkingDayUpdateService workingDayUpdateService;
        private WorkingDayService workingDayService;
        //private NotifyService notifyService;
        private MasterDataService masterDataService;
        private MessageService _messageService;
        private DepartmentAdminService departmentAdminService;
        //public static IServiceProvider ServiceProvider;
        public ReportController(EmployeeService service, WorkingDayUpdateService workingDayUpdateService,
          WorkingDayService workingDayService,
          //NotifyService notifyService,
          UserService userService,
          MasterDataService masterDataService,
          MessageService messageService,
       DepartmentAdminService departmentAdminService,
          ILogger<EmployeeController> logger
          )
          : base(service, logger)
        {
            _baseCRUDService = service;
            employeeService = (EmployeeService)_baseCRUDService;
            this.workingDayUpdateService = workingDayUpdateService;
            this.workingDayService = workingDayService;
            //this.notifyService = notifyService;
            this._userService = userService;
            this.masterDataService = masterDataService;
            this._messageService = messageService;
            this.departmentAdminService = departmentAdminService;
            //ReportController.ServiceProvider = serviceProvider;
        }

        [HttpGet]
        //[Authorize]
        [Route("total")]
        public async Task<IActionResult> GetListAttendanceReport([FromQuery] TotalReportListRequest request)
        {
            string funcName = nameof(GetListAttendanceReport);
            Messages.Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                EmployeePagingResponseModel rs = new EmployeePagingResponseModel();
                // check role => just show employee in department that current user is admin
                var currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);
                var departmentRoles = departmentAdminService.GetUserAndRole(currentUserId.GetValueOrDefault()
                    , new List<EnumDepartmentAdmin>() { EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3 });
                request.DepartmentId = departmentRoles?.Select(x => x?.Id).ToList();
                rs = await employeeService.GetListAttendanceReport(request, currentUserId);

                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, rs.TotalRecord);

                return this.ResponseJSonObj(rs);
            }
            catch (DBException ex)
            {
                errMessage = Messages.ErrDBException.SetParameters(ex);
            }
            catch (BaseException ex)
            {
                errMessage = Messages.ErrBaseException.SetParameters(ex);
            }
            catch (Exception ex)
            {
                errMessage = Messages.ErrException.SetParameters(ex);
            }
            this._logger.LogMsg(errMessage);
            return this.ResponseError(errMessage);
        }
        [HttpGet]
        [Route("detail")]
        public async Task<IActionResult> GetDetailReport([FromQuery] EmployeeAttendanceDetailRequest request)
        {
            string funcName = nameof(GetDetailReport);
            Messages.Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                // check role => just show employee in department that current user is admin
                var empDepartmentId = employeeService.GetById(request.EmployeeId.GetValueOrDefault());
                if (empDepartmentId == null)
                {
                    return this.ResponseError(new Message(EnumMessageType.Error, "400", "EMPLOYEE NOT FOUND!"));
                }
                var currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);

                var departmentRoles = departmentAdminService.GetByUserAndRoleAndDepartment(currentUserId.GetValueOrDefault()
                    , new List<EnumDepartmentAdmin>() { EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3 },
                       empDepartmentId.DepartmentId.GetValueOrDefault());

                if (departmentRoles == null || departmentRoles.Count < 0)
                {
                    return this.ResponseError(new Message(EnumMessageType.Error, "400", "NO PERMISSION!"));
                }
                //
                var rs = await employeeService.GetListDetailAttendanceReport(request);
                var ots = await workingDayService.GetOTsHours(request.EmployeeId.GetValueOrDefault(), request.DateFrom.GetValueOrDefault());
                rs.SummarizeData.Add(new Dictionary<string, object> {
                  {"display_name","Total OT Hours"},
                  {"value", ots},
                  {"type", EnumFormDataType.label.ToString()},
                  });
                //this._logger.LogMsg(Messages.ISuccess_0_1, funcName, rs.TotalRecord);

                return this.ResponseJSonObj(rs);
            }
            catch (DBException ex)
            {
                errMessage = Messages.ErrDBException.SetParameters(ex);
            }
            catch (BaseException ex)
            {
                errMessage = Messages.ErrBaseException.SetParameters(ex);
            }
            catch (Exception ex)
            {
                errMessage = Messages.ErrException.SetParameters(ex);
            }
            this._logger.LogMsg(errMessage);
            return this.ResponseError(errMessage);
        }
        [HttpGet]
        [Route("incomming-detail")]
        public async Task<IActionResult> GetIncommingDetailReport([FromQuery] EmployeeAttendanceDetailRequest request)
        {
            string funcName = nameof(GetIncommingDetailReport);
            Messages.Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);


                var rs = await employeeService.GetListIncommingDetailAttendanceReport(request);

                //this._logger.LogMsg(Messages.ISuccess_0_1, funcName, rs.TotalRecord);

                return this.ResponseJSonObj(rs);
            }
            catch (DBException ex)
            {
                errMessage = Messages.ErrDBException.SetParameters(ex);
            }
            catch (BaseException ex)
            {
                errMessage = Messages.ErrBaseException.SetParameters(ex);
            }
            catch (Exception ex)
            {
                errMessage = Messages.ErrException.SetParameters(ex);
            }
            this._logger.LogMsg(errMessage);
            return this.ResponseError(errMessage);
        }
        [HttpPost]
        [UserPermission(EnumDepartmentAdmin.Admin1)]
        [Route("edit")]
        public async Task<IActionResult> EditWorkingDay([FromForm] WorkingDayUpdateRequestModel request)
        {
            string funcName = nameof(EditWorkingDay);
            if (request == null || request.EditerId == null)
            {
                return this.ResponseErrorCode(new Message(EnumMessageType.Error, "[ERR_WRONG_EDIT]", $"Invalid Parameter"));
            }
            Message errMessage = null;
            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                long? currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);
                UserEntity? currentUser = (UserEntity)CommonFunction.GetCurrentUser(this.HttpContext);
                // check role => just show employee in department that current user is admin
                var empDepartmentId = employeeService.GetById(request.EmployeeId.GetValueOrDefault());
                if (empDepartmentId == null)
                {
                    return this.ResponseError(new Message(EnumMessageType.Error, "400", "EMPLOYEE NOT FOUND!"));
                }

                var departmentRoles = departmentAdminService.GetByUserAndRoleAndDepartment(currentUserId.GetValueOrDefault(),
                    new List<EnumDepartmentAdmin>() { EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3 },
                    empDepartmentId.DepartmentId.GetValueOrDefault()
                    );

                if (departmentRoles == null || departmentRoles.Count < 0)
                {
                    return this.ResponseError(new Message(EnumMessageType.Error, "400", "NO PERMISSION!"));
                }

                var user = await _userService.GetByIdAsync(request.EditerId.GetValueOrDefault());
                var employee = employeeService.GetById(request.EmployeeId.GetValueOrDefault());
                var workday = await workingDayService.GetByIdOrDate(request.WorkingDayId.GetValueOrDefault(), request.EmployeeId.GetValueOrDefault(), request.WorkingDate);
                var userRoles = _userService.GetListRoleUser(user.Id, employee?.DepartmentId);
                if (userRoles == null || userRoles.Count <= 0 || !userRoles.Contains(EnumDepartmentAdmin.Admin1))
                {
                    return this.ResponseErrorCode(new Message(EnumMessageType.Error, "[ERR_NO_PERMISSION]", $"You don't have permission"));
                }
                if (user == null)
                {
                    return this.ResponseErrorCode(new Message(EnumMessageType.Error, "[ERR_WRONG_EDIT]", $"Requester not found"));
                }
                if (workday == null && request.EmployeeId == null)
                {
                    return this.ResponseErrorCode(new Message(EnumMessageType.Error, "[ERR_WRONG_EDIT]", $"Workday and EmployeeId not found"));
                }
                if (request.Time_In != null && !DateTimeHelper.IsSameDay(request.WorkingDate, DateTime.Parse(request.Time_In)))
                {
                    return this.ResponseErrorCode(new Message(EnumMessageType.Error, "[ERR_WRONG_EDIT]", "'Time In' must have same day with 'Working Date'"));
                }
                else if (request.Time_Out != null)
                {
                    var requestTO = DateTime.Parse(request.Time_Out);
                    if (requestTO <= workday?.Time_In)
                    {
                        return this.ResponseErrorCode(new Message(EnumMessageType.Error, "[ERR_WRONG_EDIT]", "'Time Out' must greater than  'Existed Time In'"));
                    }
                }
                if (request.Time_In != null && request.Time_Out != null)
                {
                    if (DateTime.Parse(request.Time_In) > DateTime.Parse(request.Time_Out))
                    {
                        return this.ResponseErrorCode(new Message(EnumMessageType.Error, "[ERR_WRONG_EDIT]", "'Time Out Request' must greater than 'Time In Request'"));
                    }
                }
                var workingDayApprove = new WorkingDayApprovalEntity()
                {
                    ApproveStatus = Database.Enums.EnumApproveStatus.PENDING,
                    WorkingDayId = (workday != null && workday.Id > 0) ? workday.Id : null,
                    ApproverId = request.EditerId,
                    CreatedAt = DateTime.Now,
                    CreatedBy = request.EditerId,
                };
                var newUpdate = request.GetEntity(workday, request);
                newUpdate.WorkingDayApprovals = new List<WorkingDayApprovalEntity>();
                newUpdate.WorkingDayApprovals.Add(workingDayApprove);
                workingDayUpdateService.Upsert(newUpdate);
                //CachedFunc.ClearRedisAll();
                long employeeId = request.EmployeeId.GetValueOrDefault();

                DateTime currDate = DateTime.Now;
                var departmentRolesSendSocket = departmentAdminService.GetByUserAndRoleAndDepartment(currentUserId.GetValueOrDefault(),
                   new List<EnumDepartmentAdmin>() { EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3 }
                   );


                //Get Department
                List<long?> departmentIds = departmentRolesSendSocket.Select(x => x.DepartmentId).Distinct().ToList();

                List<UserEntity> listAdmin2 = _userService.GetListByAdminRoleAndDepartment(EnumDepartmentAdmin.Admin2, departmentIds);

                string channel = Environment.GetEnvironmentVariable("SOCKET_CHANNEL");
                string eventName = Environment.GetEnvironmentVariable("SOCKET_EVENT");
                string room = Environment.GetEnvironmentVariable("SOCKET_ROOM");

                var socketConfig = CommonConfig.GetConfig().SocketIOConfig;
                string address = string.Format("{0}:{1}", socketConfig.Address, socketConfig.Port);

                // TODO: Tách xử lý này ra common
                try
                {
                    var client = SocketIOClientService.NewConnection(address, (SocketIO? client) =>
                    {
                        client.OnConnected += async (sender, e) =>
              {
                  await client?.EmitAsync("join_room", room, currentUser?.Username);
              };
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogMsg(Messages.ErrException, ex);
                }

                foreach (var admin2 in listAdmin2)
                {
                    MessageEntity message = new MessageEntity();
                    message.Title = $"New request to update working day - {employee.GetShowName()}";
                    message.Content = @$"Request Update
Name {employee.GetShowName()}
Code: {employee.EmployeeCode}";
                    message.URL = $"admin/report/request/detail/{employeeId}";
                    message.IsRead = false;
                    message.SendDate = currDate;
                    message.UserId = admin2.Id;

                    //Task.Run(() =>
                    //{
                    try
                    {
                        this._logger.LogMsg(Messages.IFuncStart_0, funcName + $" Upsert");

                        var message2 = message.Clone();
                        //using (var scope = ReportController.ServiceProvider.CreateScope())
                        //{
                        //var messageService2 = scope.ServiceProvider.GetRequiredService<MessageService>();
                        _messageService.Upsert(message2, currentUserId);
                        //}
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

                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, $"WorkingDayId: {request.WorkingDayId}");

                return this.ResponseOk("Edit working day successfully");
            }
            catch (DBException ex)
            {
                errMessage = Messages.ErrDBException.SetParameters(ex);
            }
            catch (BaseException ex)
            {
                errMessage = Messages.ErrBaseException.SetParameters(ex);
            }
            catch (Exception ex)
            {
                errMessage = Messages.ErrException.SetParameters(ex);
            }
            this._logger.LogMsg(errMessage);
            return this.ResponseError(errMessage);
        }

        //Delete
        [HttpPost]
        [UserPermission(EnumDepartmentAdmin.Admin1)]
        [Route("deleteV2")]
        public async Task<IActionResult> DeleteWorkingDay([FromQuery] WorkingDayDeleteRequestModel request)
        {
            string funcName = nameof(EditWorkingDay);
            if (request == null || request.EditerId == null)
            {
                return this.ResponseErrorCode(new Message(EnumMessageType.Error, "[ERR_WRONG_EDIT]", $"Invalid Parameter"));
            }
            Message errMessage = null;
            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                long? currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);
                UserEntity? currentUser = (UserEntity)CommonFunction.GetCurrentUser(this.HttpContext);

                // check role => just show employee in department that current user is admin
                var workday = workingDayService.GetById(request.Id);

                if (workday == null)
                {
                    return this.ResponseError(new Message(EnumMessageType.Error, "400", "WORKDAY NOT FOUND!"));
                }
                var empDepartmentId = employeeService.GetById(workday.EmployeeEntityId);
                if (empDepartmentId == null)
                {
                    return this.ResponseError(new Message(EnumMessageType.Error, "400", "EMPLOYEE NOT FOUND!"));
                }

                var departmentRoles = departmentAdminService.GetByUserAndRoleAndDepartment(currentUserId.GetValueOrDefault(),
                    new List<EnumDepartmentAdmin>() { EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3 },
                    empDepartmentId.DepartmentId.GetValueOrDefault()
                    );

                if (departmentRoles == null || departmentRoles.Count < 0)
                {
                    return this.ResponseError(new Message(EnumMessageType.Error, "400", "NO PERMISSION!"));
                }

                //
                var user = await _userService.GetByIdAsync(request.EditerId.GetValueOrDefault());
                var employee = employeeService.GetById(workday.EmployeeEntityId);
                var userRoles = _userService.GetListRoleUser(user.Id, employee?.DepartmentId);
                if (userRoles == null || userRoles.Count <= 0 || !userRoles.Contains(EnumDepartmentAdmin.Admin1))
                {
                    return this.ResponseErrorCode(new Message(EnumMessageType.Error, "[ERR_NO_PERMISSION]", $"You don't have permission"));
                }

                if (user == null)
                {
                    return this.ResponseErrorCode(new Message(EnumMessageType.Error, "[ERR_WRONG_EDIT]", $"Requester not found"));
                }

                if (workday == null && workday.EmployeeEntityId == null)
                {
                    return this.ResponseErrorCode(new Message(EnumMessageType.Error, "[ERR_WRONG_EDIT]", $"Workday and EmployeeId not found"));
                }

                var workingDayApprove = new WorkingDayApprovalEntity()
                {
                    ApproveStatus = Database.Enums.EnumApproveStatus.PENDING,
                    WorkingDayId = (workday != null && workday.Id > 0) ? workday.Id : null,
                    ApproverId = request.EditerId,
                    CreatedAt = DateTime.Now,
                    CreatedBy = request.EditerId,
                };

                var newUpdate = request.GetEntity(workday);
                newUpdate.WorkingDayApprovals = new List<WorkingDayApprovalEntity>();
                newUpdate.WorkingDayApprovals.Add(workingDayApprove);
                workingDayUpdateService.Upsert(newUpdate);
                //CachedFunc.ClearRedisAll();
                //long employeeId = workday.EmployeeEntityId;

                DateTime currDate = DateTime.Now;
                var departmentRolesSendSocket = departmentAdminService.GetByUserAndRoleAndDepartment(currentUserId.GetValueOrDefault(),
                    new List<EnumDepartmentAdmin>() { EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3 },
                    empDepartmentId.DepartmentId.GetValueOrDefault()
                    );

                //Get Department
                List<long?> departmentIds = departmentRolesSendSocket.Select(x => x.DepartmentId).Distinct().ToList();

                List<UserEntity> listAdmin2 = _userService.GetListByAdminRoleAndDepartment(EnumDepartmentAdmin.Admin2, departmentIds);

                string channel = Environment.GetEnvironmentVariable("SOCKET_CHANNEL");
                string eventName = Environment.GetEnvironmentVariable("SOCKET_EVENT");
                string room = Environment.GetEnvironmentVariable("SOCKET_ROOM");

                var socketConfig = CommonConfig.GetConfig().SocketIOConfig;
                string address = string.Format("{0}:{1}", socketConfig.Address, socketConfig.Port);

                // TODO: Tách xử lý này ra common
                try
                {
                    var client = SocketIOClientService.NewConnection(address, (SocketIO? client) =>
                    {
                        client.OnConnected += async (sender, e) =>
                        {
                            await client?.EmitAsync("join_room", room, currentUser?.Username);
                        };
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogMsg(Messages.ErrException, ex);
                }

                foreach (var admin2 in listAdmin2)
                {
                    MessageEntity message = new MessageEntity();
                    message.Title = $"New request to update working day - {employee.GetShowName()}";
                    message.Content = @$"Request Update
Name {employee.GetShowName()}
Code: {employee.EmployeeCode}";
                    message.URL = $"admin/report/request/detail/{workday.EmployeeEntityId}";
                    message.IsRead = false;
                    message.SendDate = currDate;
                    message.UserId = admin2.Id;

                    //Task.Run(() =>
                    //{
                    try
                    {
                        this._logger.LogMsg(Messages.IFuncStart_0, funcName + $" Upsert");

                        var message2 = message.Clone();
                        //using (var scope = ReportController.ServiceProvider.CreateScope())
                        //{
                        //var messageService2 = scope.ServiceProvider.GetRequiredService<MessageService>();
                        _messageService.Upsert(message2, currentUserId);
                        //}
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

                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, $"WorkingDayId: {request.Id}");

                return this.ResponseOk("Edit working day successfully");
            }
            catch (DBException ex)
            {
                errMessage = Messages.ErrDBException.SetParameters(ex);
            }
            catch (BaseException ex)
            {
                errMessage = Messages.ErrBaseException.SetParameters(ex);
            }
            catch (Exception ex)
            {
                errMessage = Messages.ErrException.SetParameters(ex);
            }
            this._logger.LogMsg(errMessage);
            return this.ResponseError(errMessage);
        }

        [HttpPost]
        [UserPermission(EnumDepartmentAdmin.Admin1)]
        [Route("create")]
        public async Task<IActionResult> CreateNewWorkingday([FromForm] WorkingDayRequestModel model)
        {
            string funcName = nameof(CreateNewWorkingday);
            Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                var currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);

                WorkingDayEntity entity = model.GetEntity(new WorkingDayEntity());
                var dicFormFile = model.GetFiles();
                if (dicFormFile != null && dicFormFile.Count >= 1)
                {
                    Dictionary<string, string> dicImagePath = UploadUtil.UploadFile(dicFormFile);
                    entity.SetFileURL(dicImagePath);
                }
                entity = this.workingDayService.Upsert(entity, currentUserId);

                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, entity.ToJson());
                return this.ResponseJSonObj(new WorkingDayResponseModel().SetData(entity));
            }
            catch (DBException ex)
            {
                errMessage = Messages.ErrDBException.SetParameters(ex);
            }
            catch (BaseException ex)
            {
                errMessage = Messages.ErrBaseException.SetParameters(ex);
            }
            catch (Exception ex)
            {
                errMessage = Messages.ErrException.SetParameters(ex);
            }
            this._logger.LogMsg(errMessage);
            return this.ResponseError(errMessage);
        }
        [HttpGet]
        [Route("export")]
        public async Task<IActionResult> ExportReport([FromQuery] ExportDepartmentAttendanceRequest request)
        {
            string funcName = nameof(ExportReport);
            Messages.Message errMessage = null;

            try
            {
                if (request.DateFrom == null || request.DateTo == null)
                {
                    this.ResponseErrorCode(new Message(EnumMessageType.Error, "ERR_Report_Parameter", "PLEASE CHOOSE START DATE AND END DATE"));
                }
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                var ret = await employeeService.ExportAttendanceRecord(request);
                Response.Headers.Add("Content-Disposition", $"filename=AttendanceReport_{request.DateFrom}_{request.DateTo}.zip");
                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, "Export  successfully");
                return ret;
            }
            catch (DBException ex)
            {
                errMessage = Messages.ErrDBException.SetParameters(ex);
            }
            catch (BaseException ex)
            {
                errMessage = Messages.ErrBaseException.SetParameters(ex);
            }
            catch (Exception ex)
            {
                errMessage = Messages.ErrException.SetParameters(ex);
            }
            this._logger.LogMsg(errMessage);
            return this.ResponseError(errMessage);
        }

        [HttpGet]
        [Route("export-OT-time")]
        public async Task<IActionResult> ExportReportOTTime([FromQuery] ExportOTTimeRequest request)
        {
            string funcName = nameof(ExportReport);
            Messages.Message errMessage = null;

            try
            {
                if (request.DateFrom == null || request.DateTo == null)
                {
                    this.ResponseErrorCode(new Message(EnumMessageType.Error, "ERR_Report_Parameter", "PLEASE CHOOSE START DATE AND END DATE"));
                }
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                var ret = await employeeService.ExportSumarizeOTTimeEmployee(request);
                Response.Headers.Add("Content-Disposition", $"filename=AttendanceReport_{request.DateFrom}_{request.DateTo}.zip");
                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, "Export  successfully");
                return ret;
            }
            catch (DBException ex)
            {
                errMessage = Messages.ErrDBException.SetParameters(ex);
            }
            catch (BaseException ex)
            {
                errMessage = Messages.ErrBaseException.SetParameters(ex);
            }
            catch (Exception ex)
            {
                errMessage = Messages.ErrException.SetParameters(ex);
            }
            this._logger.LogMsg(errMessage);
            return this.ResponseError(errMessage);
        }

        [HttpGet]
        [Route("export-detail")]
        public async Task<IActionResult> ExpprtDetail([FromQuery] ExportEmployeeAttendanceRequest request)
        {
            string funcName = nameof(ExpprtDetail);
            Messages.Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);


                var ret = await employeeService.ExportEmployeeAttendanceRecord(request);

                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, "Export detail successfully");
                Response.Headers.Add("Content-Disposition", $"filename=EmployeeAttendanceReport_{request.DateFrom}_{request.DateTo}_{request.ListEmployeeId}.zip");
                return ret;
            }
            catch (DBException ex)
            {
                errMessage = Messages.ErrDBException.SetParameters(ex);
            }
            catch (BaseException ex)
            {
                errMessage = Messages.ErrBaseException.SetParameters(ex);
            }
            catch (Exception ex)
            {
                errMessage = Messages.ErrException.SetParameters(ex);
            }
            this._logger.LogMsg(errMessage);
            return this.ResponseError(errMessage);
        }
        [HttpGet("detail/options")]
        public IActionResult GetMasterDetailReportDatas([FromQuery] MasterDataReportRequestModel reqParams)
        {
            string funcName = nameof(GetMasterDetailReportDatas);
            iSoft.Common.Messages.Message errMessage = null;
            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                var ret = this.masterDataService.GetMasterDetailReportDatas(reqParams);
                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, ret);

                return this.ResponseJSonObj(ret);
            }
            catch (BaseException ex)
            {
                errMessage = Messages.ErrBaseException.SetParameters(ex);
            }
            catch (Exception ex)
            {
                errMessage = Messages.ErrException.SetParameters(ex);
            }
            this._logger.LogMsg(errMessage);
            return this.ResponseError(errMessage);
        }

        [HttpGet("options")]
        public IActionResult GetMasterEmployeeReportDatas([FromQuery] MasterDataRequestModel reqParams)
        {
            string funcName = nameof(GetMasterEmployeeReportDatas);
            iSoft.Common.Messages.Message errMessage = null;
            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                var ret = this.masterDataService.GetMasterEmployeeReportDatas(reqParams);
                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, ret);

                return this.ResponseJSonObj(ret);
            }
            catch (BaseException ex)
            {
                errMessage = Messages.ErrBaseException.SetParameters(ex);
            }
            catch (Exception ex)
            {
                errMessage = Messages.ErrException.SetParameters(ex);
            }
            this._logger.LogMsg(errMessage);
            return this.ResponseError(errMessage);
        }

        [HttpGet("list-ots/options")]
        public IActionResult GetMasterListOTReportDatas([FromQuery] MasterDataRequestModel reqParams)
        {
            string funcName = nameof(GetMasterListOTReportDatas);
            iSoft.Common.Messages.Message errMessage = null;
            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                var ret = this.masterDataService.GetMasterEmployeeReportDatas(reqParams);
                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, ret);

                return this.ResponseJSonObj(ret);
            }
            catch (BaseException ex)
            {
                errMessage = Messages.ErrBaseException.SetParameters(ex);
            }
            catch (Exception ex)
            {
                errMessage = Messages.ErrException.SetParameters(ex);
            }
            this._logger.LogMsg(errMessage);
            return this.ResponseError(errMessage);
        }

        [HttpGet]
        [UserPermission(EnumDepartmentAdmin.Admin1)]
        [Route("pending-form-data")]
        public override async Task<IActionResult> GetCreateFormData([FromQuery] long? Id)
        {
            string funcName = nameof(GetCreateFormData);
            Messages.Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                List<Dictionary<string, object>> formDataObj = null;
                var wd = await workingDayUpdateService.GetByIdAsync(Id.GetValueOrDefault());
                formDataObj = workingDayUpdateService.GetFormDataObjElement(wd, (long)CommonFunction.GetCurrentUserId(this.HttpContext));
                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, formDataObj);
                return this.ResponseJSonObj(formDataObj);
            }
            catch (DBException ex)
            {
                errMessage = Messages.ErrDBException.SetParameters(ex);
            }
            catch (BaseException ex)
            {
                errMessage = Messages.ErrBaseException.SetParameters(ex);
            }
            catch (Exception ex)
            {
                errMessage = Messages.ErrException.SetParameters(ex);
            }
            this._logger.LogMsg(errMessage);
            return this.ResponseError(errMessage);
        }
        /// <summary>
        ///  Get List Working day that have Recommend Type is OT types
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("list-ots")]
        public async Task<IActionResult> GetOTsRecord([FromQuery] EmployeeAttendanceListOTRequest request)
        {
            string funcName = nameof(GetOTsRecord);
            Messages.Message errMessage = null;
            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                // check role => just show employee in department that current user is admin
                var currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);
                var departmentRoles = departmentAdminService.GetUserAndRole(currentUserId.GetValueOrDefault(),
                    new List<EnumDepartmentAdmin>() { EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3 });
                if (departmentRoles == null || departmentRoles.Count < 0)
                {
                    return this.ResponseError(new Message(EnumMessageType.Error, "400", "NO PERMISSION!"));
                }
                request.DepartmentId = departmentRoles.Select(x => x.Id).ToList();

                var rs = await employeeService.GetListOTs(request);
                return this.ResponseJSonObj(rs);
            }
            catch (DBException ex)
            {
                errMessage = Messages.ErrDBException.SetParameters(ex);
            }
            catch (BaseException ex)
            {
                errMessage = Messages.ErrBaseException.SetParameters(ex);
            }
            catch (Exception ex)
            {
                errMessage = Messages.ErrException.SetParameters(ex);
            }
            this._logger.LogMsg(errMessage);
            return this.ResponseError(errMessage);
        }
    }
}