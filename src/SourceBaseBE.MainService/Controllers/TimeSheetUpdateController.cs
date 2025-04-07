using Microsoft.AspNetCore.Mvc;
using Serilog;
using iSoft.Common.Exceptions;
using iSoft.Common;
using System;
using SourceBaseBE.Database.Models.ResponseModels;
using SourceBaseBE.Database.Entities;
using SourceBaseBE.MainService.Services;
using Microsoft.Extensions.Logging;
using SourceBaseBE.Database.Models.RequestModels.Generate;
using iSoft.Common.CommonFunctionNS;
using iSoft.Common.Enums;
using SourceBaseBE.Database.Helpers;
using SourceBaseBE.Database.Models.RequestModels;
using SourceBaseBE.MainService.CustomAttributes;
using System.Collections.Generic;
using System.Threading.Tasks;
using static iSoft.Common.Messages;
using SourceBaseBE.MainService.Models.RequestModels.Report;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using InfluxDB.Client.Core.Exceptions;

namespace SourceBaseBE.MainService.Controllers
{
    [ApiController]
    [Route("api/v1/TimeSheetUpdate")]
    public class TimeSheetUpdateController : BaseCRUDController<TimeSheetUpdateEntity, TimeSheetUpdateRequestModel, TimeSheetUpdateResponseModel>
    {
        private TimeSheetUpdateService _service;
        private EmployeeService employeeService;
        private DepartmentAdminService departmentAdminService;
        private TimeSheetService timeSheetService;
        private TimeSheetApprovalService approvalService;
        private WorkingDayService workingDayService;
        public TimeSheetUpdateController(TimeSheetUpdateService service,
            EmployeeService employeeService,
            ILogger<TimeSheetUpdateController> logger,
            TimeSheetService timeSheetService,
            DepartmentAdminService departmentAdminService,
            TimeSheetApprovalService approvalService,
            WorkingDayService workingDayService
            )
          : base(service, logger)
        {
            _baseCRUDService = service;
            this.employeeService = employeeService;
            this.timeSheetService = timeSheetService;
            this.departmentAdminService = departmentAdminService;
            _service = (TimeSheetUpdateService)_baseCRUDService;
            this.approvalService = approvalService;
            this.workingDayService = workingDayService;
        }
        [HttpPost]
        [UserPermission(EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3)]
        [Route("edit")]
        public async Task<IActionResult> Upsert([FromForm] EditTimeSheetPendingRequest request)
        {
            List<long> listAllowDepartmentId = CommonFunction.GetHasPermissionDepartmentIds(this.HttpContext);
            
            string funcName = nameof(Upsert);
            Message errMessage = null;
            if (request == null || request.ListTimeSheetUpdateId == null || request.ListTimeSheetUpdateId.Count <= 0)
            {
                return this.ResponseError(new Message(EnumMessageType.Error, "400", "INVALID PARAMETERS"));
            }
            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                var listUpdates = await _service.GetListUpdate(request.ListTimeSheetUpdateId);
                var currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);
                await _service.ChangeState(listUpdates, currentUserId.GetValueOrDefault(), request, listAllowDepartmentId);
                //workingDayService.ReCalculate''
                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, " successfully");
                return this.ResponseOk("CHANGE STATE SUCCESSFULLY");
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
        [Authorize]
        [Route("delete")]
        public override async Task<IActionResult> Delete([FromQuery] long Id)
        {
            return await base.Delete(Id);
            //throw NotAcceptableException.Create(new RestSharp.RestResponse()
            //{

            //}, "CAN NOT DELETE");
        }

        [HttpPost]
        [UserPermission(EnumDepartmentAdmin.Admin3)]
        [Route("deleteV2")]
        public async Task<IActionResult> DeleteV2([FromQuery] DeleteTimeSheetApprovalRequest request)
        {
            string funcName = nameof(DeleteV2);
            iSoft.Common.Messages.Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                var currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);

                //var count = this._service.Delete(Id, currentUserId, true);
                if (request.ListTimeSheetApprovalId == null || request.ListTimeSheetApprovalId.Count <= 0)
                {
                    throw new Exception("List TimeSheet Aproval Not Found");
                }

                var listTimeSheetApproval = this._service.GetList(request.ListTimeSheetApprovalId);
                var result = this._service.DeleteMulti(listTimeSheetApproval, currentUserId);
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