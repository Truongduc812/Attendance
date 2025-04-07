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
using MathNet.Numerics.Statistics.Mcmc;
using System.Collections.Generic;
using System.Threading.Tasks;
using iSoft.Common.Models.RequestModels;
using iSoft.Common.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using iSoft.Common.Enums;
using SourceBaseBE.MainService.CustomAttributes;
using iSoft.Redis.Services;
using iSoft.Common.CommonFunctionNS;
using iSoft.Common.Utils;
using static iSoft.Common.Messages;
using iSoft.Common.ExtensionMethods;

namespace SourceBaseBE.MainService.Controllers
{
	[ApiController]
	[Route("api/v1/WorkingTypeDescription")]
	public class WorkingTypeDescriptionController : BaseCRUDController<WorkingTypeDescriptionEntity, WorkingTypeDescriptionRequestModel, WorkingTypeDescriptionResponseModel>
	{
		private WorkingTypeDescriptionService _service;
		public WorkingTypeDescriptionController(WorkingTypeDescriptionService service, ILogger<WorkingTypeDescriptionController> logger)
		  : base(service, logger)
		{
			_baseCRUDService = service;
			_service = (WorkingTypeDescriptionService)_baseCRUDService;
		}

        [HttpPost]
        [Authorize]
        [Route("upsert")]
        public override async Task<IActionResult> Upsert([FromForm] WorkingTypeDescriptionRequestModel model)
        {
            string funcName = nameof(Upsert);
            Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                var currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);

                WorkingTypeDescriptionEntity entity = null;
                if (model.Id != null)
                {
                    entity = this._baseCRUDService.GetById((long)model.Id, true);
                    if (entity == null)
                    {
                        return NotFound();
                    }
                }
                else
                {
                    if (model.WorkingTypeId == null)
                    {
                        throw new Exception($"WorkingTypeId can not null");
                    }
                    var existed = _service.GetByWorkingTypeId(model.WorkingTypeId.Value);
                    if (existed != null)
                    {
                        throw new Exception($"WorkingTypeDescription for WorkingTypeId( {model.WorkingTypeId}) existed");
                    }

                    entity = new WorkingTypeDescriptionEntity();
                }
                entity = model.GetEntity(entity);

                var dicFormFile = model.GetFiles();
                if (dicFormFile != null && dicFormFile.Count >= 1)
                {
                    Dictionary<string, string> dicImagePath = UploadUtil.UploadFile(dicFormFile);
                    entity.SetFileURL(dicImagePath);
                }

                entity = this._baseCRUDService.Upsert(entity, currentUserId);
                //CachedFunc.ClearRedisAll();
                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, entity.ToJson());
                return this.ResponseJSonObj(new WorkingTypeDescriptionResponseModel().SetData(entity));
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