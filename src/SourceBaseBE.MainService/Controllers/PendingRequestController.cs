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
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using static iSoft.Common.Messages;
using SourceBaseBE.MainService.Models.RequestModels.Report;
using iSoft.Common.Enums;
using SourceBaseBE.MainService.CustomAttributes;
using iSoft.Redis.Services;
using InfluxDB.Client.Api.Domain;
using SourceBaseBE.Database.Repository;
using System.Linq;
using SourceBaseBE.Database.Models.RequestModels.Report;
using iSoft.Common.CommonFunctionNS;
using System.Collections.Generic;
using SourceBaseBE.Database.Enums;

namespace SourceBaseBE.MainService.Controllers
{
    [ApiController]
    [Route("api/v1/WorkingDayUpdate")]
    public class PendingRequestController : BaseCRUDController<WorkingDayUpdateEntity, WorkingDayUpdateRequestModel, WorkingDayUpdateResponseModel>
    {

        private EmployeeService employeeService;
        private WorkingDayUpdateService workingDayUpdateService;
        private UserService _userService;
        private MasterDataService masterDataService;
        private DepartmentAdminService departmentAdminService;
        public PendingRequestController(WorkingDayUpdateService service,
            MasterDataService masterDataService,
            EmployeeService employeeService,
            UserService userService,
            DepartmentAdminService departmentService,
            ILogger<PendingRequestController> logger)
          : base(service, logger)
        {
            this.workingDayUpdateService = service;
            this.employeeService = employeeService;
            _userService = userService;
            this.departmentAdminService = departmentService;
            this.masterDataService = masterDataService;
        }
        [UserPermission(departmentUserRoles: EnumUserRole.Admin, EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3)]
        [Route("total")]
        public async Task<IActionResult> GetListWorkingUpdate([FromQuery] TotalReportListRequest request)
        {
            string funcName = nameof(GetListWorkingUpdate);
            if (request == null) throw new Exception("Invalid parameter");
            Message errMessage = null;
            try
            {
                EmployeePagingResponseModel rs = new EmployeePagingResponseModel();
                // check role => just show employee in department that current user is admin
                var currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);
                var departmentRoles = departmentAdminService.GetUserAndRole(currentUserId.GetValueOrDefault()
                    , new List<EnumDepartmentAdmin>() { EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3 });
                request.DepartmentId = departmentRoles?.Select(x => x?.Id).ToList();
                var workingday = await workingDayUpdateService.GetListPendingRequest(EnumApproveStatus.PENDING, request);
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
        [Route("detail")]
        public async Task<IActionResult> GetPersonalPendingRequest([FromQuery] PersonalPendingRequest pagReq)
        {
            string funcName = nameof(GetPersonalPendingRequest);
            if (pagReq == null) throw new Exception("Invalid parameter");
            Message errMessage = null;
            try
            {
                var workingday = await workingDayUpdateService.GetListPersonalPendingRequest(pagReq);
                var employee = employeeService.GetByIdAndRelation(pagReq.EmployeeId.GetValueOrDefault(), true);
                workingday.EmployeeInformation = employeeService.GetEmployeeDetailData(employee);
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
        [Route("historical")]
        public async Task<IActionResult> GetHistoricalListRequest([FromQuery] HistoricalPendingRequest pagReq)
        {
            string funcName = nameof(GetHistoricalListRequest);
            if (pagReq == null) throw new Exception("Invalid parameter");
            Message errMessage = null;
            try
            {
                var workingday = await workingDayUpdateService.GetListHistoricalRequest(pagReq);
                //var employee = employeeService.GetByIdAndRelation(pagReq.EmployeeId.GetValueOrDefault(), true);
                //workingday.EmployeeInformation = employeeService.GetEmployeeDetailData(employee);
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

        [UserPermission(departmentUserRoles: EnumUserRole.Root, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3)]
        [HttpPost("edit")]
        public async Task<IActionResult> EditPersonalWorkingUpdate([FromForm] EditPersonPendingRequest pagReq)
        {
            string funcName = nameof(EditPersonalWorkingUpdate);
            if (pagReq == null || pagReq.ListWorkingDayUpdateId == null || pagReq.ListWorkingDayUpdateId.Count <= 0) throw new Exception("Invalid parameter");
            Message errMessage = null;
            try
            {
                var wduId = pagReq.ListWorkingDayUpdateId.FirstOrDefault();
                var wdU = workingDayUpdateService.GetById(wduId);
                var employee = employeeService.GetById(wdU.EmployeeId.GetValueOrDefault());
                var userRoles = _userService.GetListRoleUser(pagReq.UserId.GetValueOrDefault(), employee?.DepartmentId);
                if (userRoles == null || !userRoles.Contains(EnumDepartmentAdmin.Admin2) && !userRoles.Contains(EnumDepartmentAdmin.Admin3))
                {
                    return this.ResponseErrorCode(new Message(EnumMessageType.Error, "[ERR_NO_PERMISSION]", $"You don't have permission"));
                }
                var workingday = await workingDayUpdateService.ChangeStateWorkingdayUpdate(pagReq);
                return this.ResponseOk($"Update status requests successfully");
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
        [HttpGet("options/total")]
        public async Task<IActionResult> GetMasterTotalPendingRequestDatas([FromQuery] MasterDataTotalPendingRequestModel reqParams)
        {
            string funcName = nameof(GetMasterTotalPendingRequestDatas);
            iSoft.Common.Messages.Message errMessage = null;
            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                var ret = await this.masterDataService.GetMasterPendingRequestDatas(reqParams);
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
        [HttpGet("options/detail")]
        public async Task<IActionResult> GetMasterDetailPendingRequestDatas([FromQuery] MasterDataDetailPendingRequestModel reqParams)
        {
            string funcName = nameof(GetMasterDetailPendingRequestDatas);
            iSoft.Common.Messages.Message errMessage = null;
            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                var ret = await this.masterDataService.GetMasterDetailPendingRequestDatas(reqParams);
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
        //[Authorize]
        [Route("export")]
        public async Task<IActionResult> ExportEmployeePendingRequest([FromQuery] ExportEmployeePendingRequest pagReq)
        {
            string funcName = nameof(ExportEmployeePendingRequest);
            if (pagReq == null) throw new Exception("Invalid parameter");
            Message errMessage = null;
            try
            {

                var workingday = await workingDayUpdateService.ExportEmployeePendingRequest(pagReq);
                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, "Export detail successfully");
                return workingday;
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
        public override Task<IActionResult> Delete([FromQuery] long Id)
        {
            CachedFunc.ClearRedisByEntity(nameof(DetailAttendancePagingResponseModel));
            return base.Delete(Id);
        }

        [HttpPost]
        [UserPermission(EnumDepartmentAdmin.Admin3)]
        [Route("deleteV2")]
        public async Task<IActionResult> Delete2([FromQuery] DeleteWorkingdayApprovalRequest request)
        {
            string funcName = nameof(Delete2);
            iSoft.Common.Messages.Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                var currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);

                //var count = this._service.Delete(Id, currentUserId, true);
                if (request.ListWorkingdayApprovalId == null || request.ListWorkingdayApprovalId.Count <= 0)
                {
                    throw new Exception("List TimeSheet Aproval Not Found");
                }

                var listTimeSheetApproval = this.workingDayUpdateService.GetList(request.ListWorkingdayApprovalId);
                var result = this.workingDayUpdateService.DeleteMulti(listTimeSheetApproval, currentUserId);
                //CachedFunc.ClearRedisAll();
                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, result);
                return this.ResponseJSonObj(result);
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