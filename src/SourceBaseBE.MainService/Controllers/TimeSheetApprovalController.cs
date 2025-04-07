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
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Confluent.Kafka;
using iSoft.Common.Enums;
using SourceBaseBE.MainService.CustomAttributes;
using SourceBaseBE.MainService.Models.RequestModels.Report;
using Nest;

namespace SourceBaseBE.MainService.Controllers
{
    [ApiController]
    [Route("api/v1/TimeSheetApproval")]
    public class TimeSheetApprovalController : BaseCRUDController<TimeSheetApprovalEntity, TimeSheetApprovalRequestModel, TimeSheetApprovalResponseModel>
    {
        private TimeSheetApprovalService _service;
        public TimeSheetApprovalController(TimeSheetApprovalService service, ILogger<TimeSheetApprovalController> logger)
          : base(service, logger)
        {
            _baseCRUDService = service;
            _service = (TimeSheetApprovalService)_baseCRUDService;
        }

        [HttpPost]
        [UserPermission(EnumDepartmentAdmin.Admin3)]
        [Route("deleteV2")]
        public async Task<IActionResult> Delete([FromQuery] DeleteTimeSheetApprovalRequest request)
        {
            string funcName = nameof(Delete);
            iSoft.Common.Messages.Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                var currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);

                //var count = this._service.Delete(Id, currentUserId, true);
                if(request.ListTimeSheetApprovalId == null || request.ListTimeSheetApprovalId.Count <= 0)
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