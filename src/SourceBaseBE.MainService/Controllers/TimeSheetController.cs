using Microsoft.AspNetCore.Mvc;
using Serilog;
using iSoft.Common.Exceptions;
using iSoft.Common;
using System;
using SourceBaseBE.Database.Entities;
using SourceBaseBE.Database.Models.RequestModels;
using SourceBaseBE.MainService.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using SourceBaseBE.MainService.Models.RequestModels.Report;
using iSoft.Common.CommonFunctionNS;
using static iSoft.Common.Messages;
using SourceBaseBE.Database.Helpers;
using SourceBaseBE.Database.Models.RequestModels.Report;
using SourceBaseBE.MainService.CustomAttributes;
using iSoft.Common.Enums;
using iSoft.Common.ConfigsNS;
using iSoft.SocketIOClientNS.Services;
using SocketIOClient;
using iSoft.Common.ExtensionMethods;
using System.Threading;
using NPOI.Util;
using SourceBaseBE.Database.Models.ResponseModels;
using System.Linq;
using iSoft.Redis.Services;
using iSoft.Common.Utils;
using SourceBaseBE.MainService.CommonFuncNS;
using SourceBaseBE.Database.Enums;

namespace SourceBaseBE.MainService.Controllers
{
    [ApiController]
    [Route("api/v1/TimeSheet")]
    public class TimeSheetController : BaseCRUDController<TimeSheetEntity, TimeSheetRequestModel, Database.Models.ResponseModels.BaseCRUDResponseModel<TimeSheetEntity>>
    {
        private TimeSheetService _service;
        private WorkingDayService _workingDayService;
        private UserService _userService;
        private EmployeeService _employeeService;
        private MasterDataService _masterDataService;
        private DepartmentAdminService _departmentAdminService;
        private MessageService _messageService;
        private TimeSheetUpdateService _timeSheetUpdateService;
        private DepartmentService _departmentService;
        public TimeSheetController(TimeSheetService service
            , WorkingDayService workingDayService,
            MasterDataService masterDataService,
            EmployeeService employeeService,
            UserService userService,
            MessageService _messageService,
            DepartmentAdminService departmentAdminService,
            TimeSheetUpdateService timeSheetUpdateService,
            DepartmentService departmentService
            , ILogger<TimeSheetController> logger)
          : base(service, logger)
        {
            _baseCRUDService = service;
            _service = (TimeSheetService)_baseCRUDService;
            this._workingDayService = workingDayService;
            this._employeeService = employeeService;
            this._masterDataService = masterDataService;
            this._departmentAdminService = departmentAdminService;
            this._userService = userService;
            this._messageService = _messageService;
            this._timeSheetUpdateService = timeSheetUpdateService;
            this._departmentService = departmentService;
        }
        [HttpGet]
        [Route("get-form-data-v2")]
        [UserPermission(EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3)]
        public async Task<IActionResult> GetCreateFormData([FromQuery] CRUDTimesheetRequestPayload req)
        {
            string funcName = nameof(GetCreateFormData);
            Messages.Message errMessage = null;
            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                List<Dictionary<string, object>> formDataObj = null;
                if (req.Id != null)
                {
                    var entity = this._service.GetById((long)req.Id);
                    if (entity == null)
                    {
                        return NotFound();
                    }
                    var employee = this._employeeService.GetById(entity.EmployeeId);

                    formDataObj = _service.GetFormDataObjElement(entity, req.EmployeeId, employee);
                }
                else
                {
                    formDataObj = _service.GetFormDataObjElement(new TimeSheetEntity(), req.EmployeeId, null);
                }
                // check role => just show employee in department that current user is admin
                var currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);
                var departmentRoles = _departmentAdminService.GetUserAndRole(currentUserId.GetValueOrDefault()
                    , new List<EnumDepartmentAdmin>() { EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3 });
                //
                foreach (var obj in formDataObj)
                {
                    if (obj["key"] == null || obj["key"].ToString().Trim() != "EmployeeId")
                    {
                        continue;
                    }
                    obj["select_data"] = _employeeService.GetAll(departmentRoles?.Select(x => x.Id).ToList());
                }
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
        [HttpGet]
        [Route("get-timesheets")]
        [UserPermission(EnumDepartmentAdmin.User, EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3)]
        public async Task<IActionResult> GetListWdTimeSheet([FromQuery] TimeSheetListRequest req)
        {
            string funcName = nameof(GetListWdTimeSheet);
            Messages.Message errMessage = null;

            try
            {
                if (req.EmployeeId == null && req.WorkingdayId == null)
                {
                    return NotFound();
                }
                // check role => just show employee in department that current user is admin
                var currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);
                var departmentRoles = _departmentAdminService.GetUserAndRole(currentUserId.GetValueOrDefault()
                    , new List<EnumDepartmentAdmin>() { EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3 });
                //
                req.ViewDepartmentIds = departmentRoles?.Select(x => x.Id).ToList();
                var timeSheets = await _service.GetListByWdIdWithFilter(req);
                return this.ResponseJSonObj(timeSheets);
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
        [UserPermission(EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3)]
        [Route("upsert")]
        public override async Task<IActionResult> Upsert([FromForm] TimeSheetRequestModel model)
        {
            throw new NotImplementedException();
        }
        [HttpGet]
        [Route("export")]
        public async Task<IActionResult> ExportReport([FromQuery] ExportDepartmentAttendanceRequest request)
        {
            string funcName = nameof(ExportReport);
            Messages.Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                // check role => just show employee in department that current user is admin
                var currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);
                var departmentRoles = _departmentAdminService.GetUserAndRole(currentUserId.GetValueOrDefault()
                    , new List<EnumDepartmentAdmin>() { EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3 });
                //
                var departments = CommonFuncMainService.RemoveDuplicatesUsingLinq(departmentRoles);
                var ret = await _service.ExportAttendanceRecord(request, departments);
                Response.Headers.Add("Content-Disposition", $"filename=TimeSheet_{request.DateFrom}_{request.DateTo}.zip");
                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, "Export TimeSheet  successfully");
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
        [HttpGet("options")]
        public IActionResult GetMasterTimeSheetFilterDatas([FromQuery] bool? Status)
        {
            string funcName = nameof(GetMasterTimeSheetFilterDatas);
            iSoft.Common.Messages.Message errMessage = null;
            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                var ret = this._masterDataService.GetMasterTimesSheetDatas(Status.GetValueOrDefault());
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

        [HttpGet("request/option/request")]
        public IActionResult GetMasterFilterActionRequest([FromQuery] bool? Status)
        {
            string funcName = nameof(GetMasterFilterActionRequest);
            iSoft.Common.Messages.Message errMessage = null;
            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                var ret = this._masterDataService.GetMasterFilterRequest(Status.GetValueOrDefault());
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

        [HttpGet("request/option/status")]
        public IActionResult GetMasterFilterActionStatus([FromQuery] bool? Status)
        {
            string funcName = nameof(GetMasterFilterActionStatus);
            iSoft.Common.Messages.Message errMessage = null;
            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                var ret = this._masterDataService.GetMasterFilterStatus(Status.GetValueOrDefault());
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

        [HttpPost]
        [UserPermission(EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3)]
        [Route("edit")]
        public async Task<IActionResult> EditTimeSheet([FromForm] TimeSheetRequestModel request)
        {
            string funcName = nameof(EditTimeSheet);
            if (request == null || request.EmployeeId <= 0)
            {
                return this.ResponseErrorCode(new Message(EnumMessageType.Error, "[ERR_WRONG_EDIT]", $"Invalid Parameter"));
            }
            Message errMessage = null;
            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                long? currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);
                var currentUser = (UserEntity)CommonFunction.GetCurrentUser(this.HttpContext);
                //// check role => just show employee in department that current user is admin
                var empployee = _employeeService.GetById(request.EmployeeId);
                if (empployee == null)
                {
                    return this.ResponseError(new Message(EnumMessageType.Error, "400", "EMPLOYEE NOT FOUND!"));
                }
                var departmentRoles = _departmentAdminService.GetByUserAndRoleAndDepartment(currentUserId.GetValueOrDefault()
                    , new List<EnumDepartmentAdmin>() { EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3 },
                    empployee.DepartmentId.GetValueOrDefault());
                if (departmentRoles == null || departmentRoles.Count < 0)
                {
                    return this.ResponseError(new Message(EnumMessageType.Error, "400", "NO PERMISSION!"));
                }

                //Get Department
                //List<long?> departmentIds = departmentRoles.Select(x => x.DepartmentId).Distinct().ToList();   
                //List<DepartmentEntity>  departments = _departmentService.GetDepartmentByListIds(departmentIds, null);
                //////////////////////////////////////
                ///
                var existedTimeSheet = await _service.GetEmpTimeSheet(request.RecordedTime, empployee, request.Status);

                if (existedTimeSheet != null)
                {
                    return this.ResponseError(new Message(EnumMessageType.Error, "400", "TIMESHEET EXISTED!"));
                }
                var timesheet = _service.GetById(request.Id);
                //
                var timesheetUpdate = request.GetTimeSheetUpdateEntity(timesheet, currentUser);
                _timeSheetUpdateService.Upsert(timesheetUpdate);

                CachedFunc.ClearRedisByEntity(nameof(TimeSheetUpdateEntity));
                CachedFunc.ClearRedisByEntity(nameof(TimeSheetApprovalEntity));
                CachedFunc.ClearRedisByEntity(nameof(TimeSheetEntity));

                DateTime currDate = DateTime.Now;

                var departmentRolesToSocket = _departmentAdminService.GetByUserAndRoleAndDepartment(currentUserId.GetValueOrDefault()
                    , new List<EnumDepartmentAdmin>() { EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3 });

                //Get Department
                List<long?> departmentIds = departmentRolesToSocket.Select(x => x.DepartmentId).Distinct().ToList();

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
                    message.Title = $"New request to update timesheet - {empployee.GetShowName()}";
                    message.Content = @$"Request Update
Name {empployee.GetShowName()}
Code: {empployee.EmployeeCode}";
                    //message.URL = $"/admin/request/timesheet/detail/{empployee?.Id}";
                    message.URL = $"admin/time-sheets/request/detail/{empployee?.Id}";
                    message.IsRead = false;
                    message.SendDate = currDate;
                    message.UserId = admin2.Id;

                    try
                    {
                        this._logger.LogMsg(Messages.IFuncStart_0, funcName + $" Upsert");

                        var message2 = message.Clone();
                        _messageService.Upsert(message2, currentUserId);
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

                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, $"TimeSheetId: {request.Id}");
                CachedFunc.ClearRedisByEntity(nameof(TimeSheetEntity));
                CachedFunc.ClearRedisByEntity(nameof(TimeSheetUpdateEntity));
                CachedFunc.ClearRedisByEntity(nameof(TimeSheetApprovalEntity));
                return this.ResponseOk("Edit TimeSheet successfully");
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
        [UserPermission(EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3)]
        [Route("deleteV2")]
        public async Task<IActionResult> DeleteTimeSheet([FromQuery] TimeSheetDelateRequestModel request)
        {
            string funcName = nameof(DeleteTimeSheet);
            Message errMessage = null;
            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                long? currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);
                var currentUser = (UserEntity)CommonFunction.GetCurrentUser(this.HttpContext);

                var timesheet = _service.GetById(request.Id);
                if (timesheet == null)
                {
                    return this.ResponseError(new Message(EnumMessageType.Error, "400", "TIMESHEET DELETE NOT FOUND!"));
                }
                //// check role => just show employee in department that current user is admin
                var empployee = _employeeService.GetById(timesheet.EmployeeId);
                if (empployee == null)
                {
                    return this.ResponseError(new Message(EnumMessageType.Error, "400", "EMPLOYEE NOT FOUND!"));
                }
                var departmentRoles = _departmentAdminService.GetByUserAndRoleAndDepartment(currentUserId.GetValueOrDefault()
                    , new List<EnumDepartmentAdmin>() { EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3 },
                    empployee.DepartmentId.GetValueOrDefault()
                    );
                if (departmentRoles == null || departmentRoles.Count < 0)
                {
                    return this.ResponseError(new Message(EnumMessageType.Error, "400", "NO PERMISSION!"));
                }

                //////////////////////////////////////
                var timesheetUpdate = request.GetTimeSheetUpdateEntity(timesheet, currentUser);
                _timeSheetUpdateService.Upsert(timesheetUpdate);

                DateTime currDate = DateTime.Now;
                var departmentRolesSendSocket = _departmentAdminService.GetByUserAndRoleAndDepartment(currentUserId.GetValueOrDefault()
                    , new List<EnumDepartmentAdmin>() { EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3 }
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
                    message.Title = $"New request to update timesheet - {empployee.GetShowName()}";
                    message.Content = @$"Request Update
Name {empployee.GetShowName()}
Code: {empployee.EmployeeCode}";
                    //message.URL = $"/admin/request/timesheet/detail/{empployee?.Id}";
                    message.URL = $"admin/time-sheets/request/detail/{empployee?.Id}";
                    message.IsRead = false;
                    message.SendDate = currDate;
                    message.UserId = admin2.Id;

                    try
                    {
                        this._logger.LogMsg(Messages.IFuncStart_0, funcName + $" Upsert");

                        var message2 = message.Clone();
                        _messageService.Upsert(message2, currentUserId);
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

                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, $"TimeSheetId: {request.Id}");
                CachedFunc.ClearRedisByEntity(nameof(TimeSheetEntity));
                CachedFunc.ClearRedisByEntity(nameof(TimeSheetUpdateEntity));
                CachedFunc.ClearRedisByEntity(nameof(TimeSheetApprovalEntity));
                return this.ResponseOk("Edit TimeSheet successfully");
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

        [UserPermission(departmentUserRoles: EnumUserRole.Admin, EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3)]
        [Route("request-total")]
        public async Task<IActionResult> GetListTimeSheeetUpdate([FromQuery] TotalReportListRequest request)
        {
            string funcName = nameof(GetListTimeSheeetUpdate);
            if (request == null) throw new Exception("Invalid parameter");
            Message errMessage = null;
            try
            {
                EmployeePagingResponseModel rs = new EmployeePagingResponseModel();
                // check role => just show employee in department that current user is admin
                var currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);
                var departmentRoles = _departmentAdminService.GetUserAndRole(currentUserId.GetValueOrDefault()
                    , new List<EnumDepartmentAdmin>() { EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3 });
                request.DepartmentId = departmentRoles?.Select(x => x?.Id).ToList();
                var timesheetUpdate = await _timeSheetUpdateService.GetListPendingRequest(EnumApproveStatus.PENDING, request);
                return this.ResponseOk(timesheetUpdate);
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

        //[UserPermission(departmentUserRoles: EnumUserRole.Admin, EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3)]
        //[Route("historal-total")]
        //public async Task<IActionResult> GetListTimeSheeetApproval([FromQuery] TotalReportListRequest request)
        //{
        //    string funcName = nameof(GetListTimeSheeetApproval);
        //    if (request == null) throw new Exception("Invalid parameter");
        //    Message errMessage = null;
        //    try
        //    {
        //        EmployeePagingResponseModel rs = new EmployeePagingResponseModel();
        //        // check role => just show employee in department that current user is admin
        //        var currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);
        //        var departmentRoles = departmentAdminService.GetUserAndRole(currentUserId.GetValueOrDefault()
        //            , new List<EnumDepartmentAdmin>() { EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3 });
        //        request.DepartmentId = departmentRoles?.Select(x => x?.Id).ToList();
        //        var timesheetUpdate = await timeSheetUpdateService.GetListPendingRequest(EnumApproveStatus.ACCEPT,request);
        //        return this.ResponseOk(timesheetUpdate);
        //    }
        //    catch (DBException ex)
        //    {
        //        errMessage = Messages.ErrDBException.SetParameters(ex);
        //    }
        //    catch (BaseException ex)
        //    {
        //        errMessage = Messages.ErrBaseException.SetParameters(ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        errMessage = Messages.ErrException.SetParameters(ex);
        //    }
        //    this._logger.LogMsg(errMessage);
        //    return this.ResponseError(errMessage);
        //}

        [UserPermission(departmentUserRoles: EnumUserRole.Root, EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3)]
        [Route("request-detail")]
        public async Task<IActionResult> GetPersonalPendingRequest([FromQuery] PersonalPendingRequest pagReq)
        {
            string funcName = nameof(GetPersonalPendingRequest);
            if (pagReq == null) throw new Exception("Invalid parameter");
            Message errMessage = null;
            try
            {
                var workingday = await _timeSheetUpdateService.GetListPersonalPendingRequest(pagReq);
                var employee = _employeeService.GetByIdAndRelation(pagReq.EmployeeId.GetValueOrDefault(), true);
                return this.ResponseOk(workingday);
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

        [UserPermission(departmentUserRoles: EnumUserRole.Root, EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3)]
        [Route("historical-detail")]
        public async Task<IActionResult> GetListHistoricalTimeSheetRequest([FromQuery] HistoricalPendingRequest pagReq)
        {
            string funcName = nameof(GetPersonalPendingRequest);
            if (pagReq == null) throw new Exception("Invalid parameter");
            Message errMessage = null;
            try
            {
                var workingday = await _timeSheetUpdateService.GetListHistoricalPendingRequest(pagReq);
                //var employee = _employeeService.GetByIdAndRelation(pagReq.EmployeeId.GetValueOrDefault(), true);
                return this.ResponseOk(workingday);
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
        [Route("incomming")]
        public async Task<IActionResult> GetIncommingTimeSheet([FromQuery] EmployeeAttendanceDetailRequest request)
        {
            string funcName = nameof(GetIncommingTimeSheet);
            Messages.Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                // check role => just show employee in department that current user is admin
                var currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);
                var departmentRoles = _departmentAdminService.GetUserAndRole(currentUserId.GetValueOrDefault()
                    , new List<EnumDepartmentAdmin>() { EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3 });
                request.AcceptDepartmentId = departmentRoles?.Select(x => x.Id).ToList();

                var rs = await _service.GetListIncommingDetailTimeSheet(request);

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
    }
}