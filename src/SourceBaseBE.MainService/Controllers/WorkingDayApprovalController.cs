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
using System.Threading.Tasks;
using SourceBaseBE.MainService.Models.RequestModels;
using iSoft.Common.Models.RequestModels;
using iSoft.Common.CommonFunctionNS;
using iSoft.Common.Enums;
using SourceBaseBE.MainService.CustomAttributes;
using SourceBaseBE.MainService.Models.RequestModels.Report;

namespace SourceBaseBE.MainService.Controllers
{
	[ApiController]
	[Route("api/v1/WorkingDayApproval")]
	public class WorkingDayApprovalController : BaseCRUDController<WorkingDayApprovalEntity, WorkingDayApprovalRequestModel, WorkingDayApprovalResponseModel>
	{
		private WorkingDayApprovalService _service;
		public WorkingDayApprovalController(WorkingDayApprovalService service, ILogger<WorkingDayApprovalController> logger)
		  : base(service, logger)
		{
			_baseCRUDService = service;
			_service = (WorkingDayApprovalService)_baseCRUDService;
		}
		public async Task<IActionResult> GetListRequest(PagingFilterRequestModel request)
		{
			string funcName = nameof(GetListRequest);
			Messages.Message errMessage = null;

			try
			{
				this._logger.LogMsg(Messages.IFuncStart_0, funcName);


				var rs = _service.GetList(request);

				this._logger.LogMsg(Messages.ISuccess_0_1, funcName, rs.Count);

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
        [Route("deleteV2")]
        public async Task<IActionResult> Delete([FromQuery] DeleteWorkingdayApprovalRequest request)
        {
            string funcName = nameof(Delete);
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

                var listTimeSheetApproval = this._service.GetList(request.ListWorkingdayApprovalId);
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