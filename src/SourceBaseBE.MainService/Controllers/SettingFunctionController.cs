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
using SourceBaseBE.Database.Models.RequestModels.Generate;
using SourceBaseBE.Database.Repository;
using SourceBaseBE.MainService.Models.RequestModels;
using iSoft.Common.Enums;
using SourceBaseBE.MainService.CustomAttributes;
using iSoft.Common.CommonFunctionNS;
using iSoft.Redis.Services;

namespace SourceBaseBE.MainService.Controllers
{

    [ApiController]
    [Route("api/v1/Setting")]
    public class SettingFunctionController : BaseCRUDController<EmployeeEntity, EmployeeRequestModel, EmployeeResponseModel>
    {
        private EmployeeService _employeeService;
        private DepartmentService _departmentService;
        private DepartmentAdminService _departmentAdminService;
        private WorkingTypeService _workingTypeService;
        private JobTitleService _jobTitleService;
        private MasterDataService _masterDataService;
        private UserService _userService;

        public SettingFunctionController(UserService userService, MasterDataService masterDataService, JobTitleService jobTitleService,
                                 WorkingTypeService workingTypeService, DepartmentAdminService departmentAdminService,
                                 EmployeeService service, DepartmentService departmentService,
          ILogger<EmployeeController> logger)
          : base(service, logger)
        {
            _baseCRUDService = service;
            _employeeService = (EmployeeService)_baseCRUDService;
            this._departmentService = departmentService;
            this._departmentAdminService = departmentAdminService;
            this._workingTypeService = workingTypeService;
            this._jobTitleService = jobTitleService;
            this._masterDataService = masterDataService;
            this._userService = userService;
        }

        [HttpGet("options/employee")]
        public IActionResult GetMasterDataEmployeeDepartment([FromQuery] MasterDataRequestModel reqParams)
        {
            string funcName = nameof(GetMasterDataEmployeeDepartment);
            iSoft.Common.Messages.Message errMessage = null;
            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                var ret = _masterDataService.GetMasterDatasFunctionPage(reqParams);
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

        [HttpGet("options/admin")]
        public IActionResult GetMasterDataAdmin([FromQuery] MasterDataAdminRequestModel reqParams)
        {
            string funcName = nameof(GetMasterDataEmployeeDepartment);
            iSoft.Common.Messages.Message errMessage = null;
            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                var ret = _masterDataService.GetMasterDatasAdmin(reqParams);
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
        [Route("department/employees")]
        [UserPermission(EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3)]
        public IActionResult GetListEmployeeDepartmentSetting([FromQuery] PagingParamRequestModel request)
        {
            string funcName = nameof(GetListEmployeeDepartmentSetting);
            Messages.Message errMessage = null;
            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                var rs = _employeeService.GetListEmployeeDepartment(request);

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
        [Route("department/admin1")]
        [UserPermission(EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3)]
        public IActionResult GetListAdminSetting([FromQuery] PagingParamRequestModel request)
        {
            string funcName = nameof(GetListAdminSetting);
            Messages.Message errMessage = null;
            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                var rs = _departmentService.GetListAdminSetting(EnumDepartmentAdmin.Admin1, request);

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
        [Route("department/admin2")]
        [UserPermission(EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3)]
        public IActionResult GetListAdmin2Setting([FromQuery] PagingParamRequestModel request)
        {
            string funcName = nameof(GetListAdminSetting);
            Messages.Message errMessage = null;
            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                var rs = _departmentService.GetListAdminSetting(EnumDepartmentAdmin.Admin2, request);

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
        [Route("department/admin3")]
        [UserPermission(EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3)]
        public IActionResult GetListAdmin3Setting([FromQuery] PagingParamRequestModel request)
        {
            string funcName = nameof(GetListAdminSetting);
            Messages.Message errMessage = null;
            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                var rs = _departmentService.GetListAdminSetting(EnumDepartmentAdmin.Admin3, request);

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
        [Route("department/user")]
        [UserPermission(EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3)]
        public IActionResult GetListUserSetting([FromQuery] PagingParamRequestModel request)
        {
            string funcName = nameof(GetListAdminSetting);
            Messages.Message errMessage = null;
            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                var rs = _departmentService.GetListUserAdmin(EnumDepartmentAdmin.User, request);

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

        [HttpPost]
        [Route("import/employee")]
        [UserPermission(EnumUserRole.Root, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3)]
        [RequestSizeLimit(1024 * 1000 * 800)]
        public async Task<IActionResult> ImportFileEmployeeDepartment([FromForm] DepartmentRequestModel request)
        {
            string funcName = nameof(ImportFileEmployeeDepartment);
            Messages.Message errMessage = null;
            Messages.Message errMessageCode = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                long? currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);
                errMessage = await _employeeService.ImportFileEmployeeDepartment(request, currentUserId);
                CachedFunc.ClearRedisByEntity(nameof(EmployeeEntity));
                if (errMessage == null)
                {
                    this._logger.LogMsg(Messages.ISuccess_0_1, funcName, errMessage);
                    return this.ResponseJSonObj(true);
                }
            }
            catch (DBException ex)
            {
                errMessageCode = Messages.ErrDBException.SetParameters(ex);
            }
            catch (BaseException ex)
            {
                errMessageCode = Messages.ErrBaseException.SetParameters(ex);
                errMessage = Messages.ErrBaseException.SetParameters(ex.Message);
            }
            catch (Exception ex)
            {
                errMessageCode = Messages.ErrException.SetParameters(ex);
            }
            this._logger.LogMsg(errMessageCode);
            return this.ResponseError(errMessage);
            //return this.ResponseError(errMessage);
        }

        [HttpPost]
        [Route("import/admin")]
        [UserPermission(EnumUserRole.Root, EnumDepartmentAdmin.Admin3)]
        [RequestSizeLimit(1024 * 1000 * 800)]
        public async Task<IActionResult> ImportFileAdminDepartment([FromForm] DepartmentAdminRequestModel request)
        {
            string funcName = nameof(ImportFileEmployeeDepartment);
            Messages.Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                //return NotFound();

                errMessage = await _departmentAdminService.ImportFileAdminDepartment(request);
                if (errMessage == null)
                {
                    this._logger.LogMsg(Messages.ISuccess_0_1, funcName, errMessage);
                    return this.ResponseJSonObj(true);
                }

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
        [UserPermission(EnumUserRole.Root, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3)]
        [Route("export-employee")]
        public async Task<IActionResult> ExportEmployeeDepartment([FromQuery] ExportEmployeeDepartmentRequest request)
        {
            string funcName = nameof(ExportEmployeeDepartment);
            Messages.Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                //Response.Headers.Add("Content-Disposition", $"filename=Employees.zip");
                //var ret = await _employeeService.ExportEmployeeDepartment(request);
                //this._logger.LogMsg(Messages.ISuccess_0_1, funcName, ret);
                var path = await this._employeeService.SetCurrentEmployeeToExcel();

                return DownloadFile(path, true);
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
        [Route("export-admin")]
        [UserPermission(EnumUserRole.Root, EnumDepartmentAdmin.Admin3)]
        public async Task<IActionResult> ExportEmployeeAdminDepartment([FromQuery] ExportEmployeeDepartmentRequest request)
        {
            string funcName = nameof(ExportEmployeeAdminDepartment);
            Messages.Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);


                //var ret = await _employeeService.ExportEmployeeAdminDepartment(request);
                //    Response.Headers.Add("Content-Disposition", $"filename=Employee_Department.zip");

                //    this._logger.LogMsg(Messages.ISuccess_0_1, funcName, ret);
                var path = $"DepartmentAdmin_{DateTime.Now.ToOADate()}.xlsx";
                await this._departmentAdminService.SetCurrentDepartmentToExcel(path);
                return DownloadFile(path, true);
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
