using Azure;
using iSoft.Common;
using iSoft.Common.Exceptions;
using iSoft.Common.Models.RequestModels;
using iSoft.Common.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SourceBaseBE.Database.Models.RequestModels;
using SourceBaseBE.Database.Models.RequestModels.Report;
using SourceBaseBE.Database.Models.ResponseModels;
using SourceBaseBE.MainService.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SourceBaseBE.MainService.Controllers
{
	[ApiController]
	[Route("api/v1/Dashboard")]
	public class DashboardController : ControllerBase
	{
		internal ILogger _logger;
		private WorkingDayService _service;
		private MasterDataService masterDataService;
		public DashboardController(ILogger<DashboardController> logger, WorkingDayService service, MasterDataService masterDataService)
		{
			this._logger = logger;
			this._service = service;
			this.masterDataService = masterDataService;
		}

		[HttpGet]
		public async Task<IActionResult> Dashboard()
		{
			string funcName = "Dashboard";

			this._logger.LogMsg(Messages.IFuncStart_0, funcName);

			this._logger.LogMsg(Messages.ISuccess_0_1, funcName, "Dashboard Works");

			return this.ResponseOk("Dashboard Works");
		}
		[HttpGet]
		[Authorize]
		[Route("attendance")]
		public async Task<IActionResult> GetListAttendance([FromQuery] PagingFilterRequestModel request)
		{
			string funcName = nameof(GetListAttendance);
			Messages.Message errMessage = null;

			try
			{
				this._logger.LogMsg(Messages.IFuncStart_0, funcName);

				WorkingDayPagingResponseModel rs = new WorkingDayPagingResponseModel();
				rs = _service.GetListAttendancev2(request);

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
        [Route("attendance-group-department")]
        public async Task<IActionResult> GetListAttendanceGroupByDepartment([FromQuery] AttendanceGroupByDepartmentRequest request)
        {
            string funcName = nameof(GetListAttendanceGroupByDepartment);
            Messages.Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                WorkingDayGroupByDepartmentPagingResponseModel rs = new WorkingDayGroupByDepartmentPagingResponseModel();
                rs = _service.GetListAttendanceGroupByDepartment(request);

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

        [HttpGet("options")]
		public IActionResult GetMasterHomeDatas([FromQuery] MasterDataHomeRequestModel reqParams)
		{
			string funcName = nameof(GetMasterHomeDatas);
			iSoft.Common.Messages.Message errMessage = null;
			try
			{
				this._logger.LogMsg(Messages.IFuncStart_0, funcName);
				var ret = this.masterDataService.GetMasterHomeDatas(reqParams);
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
		[Route("export")]
		public async Task<IActionResult> ExportReport([FromQuery] PagingFilterRequestModel request)
		{
			string funcName = nameof(ExportReport);
			Messages.Message errMessage = null;

			try
			{
				this._logger.LogMsg(Messages.IFuncStart_0, funcName);

				var ret = await _service.ExportCurrentInoutState(request);
				this._logger.LogMsg(Messages.ISuccess_0_1, funcName, "Export  successfully");
				Response.Headers.Add("Content-Disposition", $"filename={ret}");
				Response.Headers.Add("content-type", $"application/excel");
				Response.ContentType = "application/vnd.xls";
				return DownloadFile(ret, true);

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
		public IActionResult DownloadFile(string filePath, bool deleteAfterRead = false)
		{
			var fileName = Path.GetFileName(filePath);
			var fileData = new MemoryStream(System.IO.File.ReadAllBytes(filePath));
			var ret = File(fileData.ToArray(), "application/vnd.ms-excel.sheet.macroEnabled.12", fileName);
			if (deleteAfterRead)
			{
				System.IO.File.Delete(filePath);
			}
			return ret;
		}
	}
}
