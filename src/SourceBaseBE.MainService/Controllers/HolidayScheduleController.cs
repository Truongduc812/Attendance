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
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using iSoft.Common.Models.RequestModels;
using SourceBaseBE.Database.Models.RequestModels;
using System.Collections.Generic;
using iSoft.Common.Enums;
using SourceBaseBE.MainService.CustomAttributes;
using iSoft.ExportLibrary.Models;
using iSoft.Redis.Services;
using iSoft.Common.CommonFunctionNS;
using iSoft.Common.ExtensionMethods;
using System.Net.Http;
using System.IO;
using System.Net;
using elFinder.NetCore;

namespace SourceBaseBE.MainService.Controllers
{
    [ApiController]

    [Route("api/v1/HolidaySchedule")]
    public class HolidayScheduleController : BaseCRUDController<HolidayScheduleEntity, HolidayScheduleRequestModel, HolidayScheduleResponseModel>
    {
        private HolidayScheduleService _service;
        public HolidayScheduleController(HolidayScheduleService service, ILogger<HolidayScheduleController> logger)
          : base(service, logger)
        {
            _baseCRUDService = service;
            _service = (HolidayScheduleService)_baseCRUDService;
        }

        [HttpGet]
        [Route("holiday")]
        public async Task<IActionResult> GetListHolidaySchedule([FromQuery] PagingHolidayRequestModel request)
        {
            string funcName = nameof(GetListHolidaySchedule);
            Messages.Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                HolidaySchedulePagingResponseModel rs = new HolidaySchedulePagingResponseModel();

                rs = _service.GetListHolidaySchedule(request);

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
        [UserPermission(EnumDepartmentAdmin.Admin3)]
        [Route("import")]
        public async Task<IActionResult> ImportFileHolidaySchedule([FromForm] HolidayScheduleRequestModel request)
        {
            string funcName = nameof(ImportFileHolidaySchedule);
            Messages.Message errMessage = null;
            Messages.Message errMessageShow = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                long? currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);

                var rs = await _service.ImportFileHolidaySchedule(request, currentUserId);

                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, rs.ToJson());

                return this.ResponseJSonObj(true);
            }
            catch (DBException ex)
            {
                errMessage = Messages.ErrDBException.SetParameters(ex);
                errMessageShow = Messages.ErrDBException.SetParameters(ex.Message);
            }
            catch (BaseException ex)
            {
                errMessage = Messages.ErrBaseException.SetParameters(ex);
                errMessageShow = Messages.ErrBaseException.SetParameters(ex.Message);
            }
            catch (Exception ex)
            {
                errMessage = Messages.ErrException.SetParameters(ex);
                errMessageShow = Messages.ErrException.SetParameters(ex.Message);
            }
            this._logger.LogMsg(errMessage);
            return this.ResponseError(Messages.ErrImportExcelProcessing, errMessageShow);
        }

        [HttpGet]
        [Route("export")]
        [UserPermission(EnumDepartmentAdmin.Admin3)]
        public async Task<IActionResult> ExportFileHolidaySchedule([FromQuery] PagingHolidayReportModel request)
        {
            string funcName = nameof(ExportFileHolidaySchedule);
            Messages.Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                var path =await _service.SetCurrentYearHolidayToExcel(request);

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
        [Route("get-form-data")]
        public override async Task<IActionResult> GetCreateFormData([FromQuery] long? Id)
        {
            string funcName = nameof(GetCreateFormData);
            Messages.Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                List<Dictionary<string, object>> formDataObj = null;
                if (Id == null)
                {
                    formDataObj = _service.GetFormDataObjElement(new HolidayScheduleEntity());
                }
                else
                {
                    var entity = this._service.GetById((long)Id);
                    if (entity == null)
                    {
                        return NotFound();
                    }
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



    }
}