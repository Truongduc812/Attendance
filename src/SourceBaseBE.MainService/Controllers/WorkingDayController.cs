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
using iSoft.Common.Models.RequestModels;
using iSoft.Common.Models.ResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.ComponentModel;
using MathNet.Numerics.Statistics.Mcmc;
using iSoft.Common.CommonFunctionNS;
using iSoft.Common.Utils;
using Microsoft.AspNetCore.Authorization;
using static iSoft.Common.Messages;
using iSoft.Common.ExtensionMethods;
using SourceBaseBE.MainService.Models.RequestModels.Report;
using iSoft.Common.Enums;
using PRPO.MainService.CustomAttributes;
using SourceBaseBE.MainService.CustomAttributes;
using iSoft.Redis.Services;

namespace SourceBaseBE.MainService.Controllers
{
    [ApiController]
    [Route("api/v1/WorkingDay")]
    public class WorkingDayController : BaseCRUDController<WorkingDayEntity, WorkingDayRequestModel, WorkingDayResponseModel>
    {
        private WorkingDayService _service;
        private EmployeeService employeeService;
        public WorkingDayController(WorkingDayService service, ILogger<WorkingDayController> logger, EmployeeService employeeService)
          : base(service, logger)
        {
            _baseCRUDService = service;
            _service = (WorkingDayService)_baseCRUDService;
            this.employeeService = employeeService;
        }
        [HttpGet]
        [UserPermission(EnumDepartmentAdmin.Admin1)]
        [Route("get-form-data-v2")]
        public async Task<IActionResult> GetCreateFormDatav2([FromQuery] CRUDReportRequestPayload req)
        {
            string funcName = nameof(GetCreateFormData);
            Messages.Message errMessage = null;

            try
            {
                var emp = await employeeService.GetByIdAsync(req.EmployeeId.GetValueOrDefault());
                if (emp == null)
                {
                    this._logger.LogMsg(Messages.ISuccess_0_1, funcName, emp);
                    return this.ResponseError(new Message(EnumMessageType.Error, "400", "EMPLOYEE NOT FOUND!"));
                }
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                List<Dictionary<string, object>> formDataObj = null;
                if (req.Id == null)
                {
                    formDataObj = _service.GetFormDataObjElement(new WorkingDayEntity()
                    {
                        EmployeeEntityId = emp.Id,
                        Employee = emp
                    });
                }
                else
                {
                    var entity = this._service.GetById((long)req.Id);
                    if (entity == null)
                    {
                        return NotFound();
                    }
                    entity.EmployeeEntityId = emp.Id;
                    entity.Employee = emp;
                    formDataObj = _service.GetFormDataObjElement(entity);
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
        [HttpPost]
        [UserPermission(EnumDepartmentAdmin.Admin1)]
        [Route("delete")]
        public override async Task<IActionResult> Delete([FromQuery] long Id)
        {
            return await base.Delete(Id);
        }

        [HttpPost]
        [Route("import/workingDay-day-off")]
        [UserPermission(EnumUserRole.Root, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3)]
        [RequestSizeLimit(1024 * 1000 * 800)]
        public IActionResult ImportFileWorkingDayDayOffEmployee([FromForm] WorkingDayOffRequestModel request)
        {
            string funcName = nameof(ImportFileWorkingDayDayOffEmployee);
            var errMessageCode = "";
            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                long? currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);
                var errMessage = _service.ImportFileWorkingDayDayOffEmployee(request, currentUserId);
                if (string.IsNullOrEmpty(errMessage))
                {
                    this._logger.LogMsg(Messages.ISuccess_0_1, funcName, errMessage);
                    return this.ResponseJSonObj(true);
                }
                else
                {
                    this._logger.LogMsg(Messages.ErrImportExcelProcessing, funcName, errMessage);
                    return this.ResponseJSonObj(errMessage);
                }
            }
            catch (DBException ex)
            {
                errMessageCode = ex.ToString();
                _logger.LogError(errMessageCode);
                return null;
            }
            catch (BaseException ex)
            {
                errMessageCode = ex.ToString();
                _logger.LogError(errMessageCode);
                return null;
            }
            catch (Exception ex)
            {
                errMessageCode = ex.ToString();
                _logger.LogError(errMessageCode);
                return null;
            }
        }
    }
}