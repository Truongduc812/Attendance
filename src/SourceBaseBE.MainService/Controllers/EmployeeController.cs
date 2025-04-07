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
using iSoft.Common.CommonFunctionNS;
using iSoft.Common.Utils;
using MathNet.Numerics.Statistics.Mcmc;
using static iSoft.Common.Messages;
using Microsoft.AspNetCore.Authorization;
using iSoft.Common.Enums;
using SourceBaseBE.MainService.CustomAttributes;
using iSoft.Redis.Services;
using SourceBaseBE.Database.Repository;
using iSoft.Common.ExtensionMethods;

namespace SourceBaseBE.MainService.Controllers
{
    [ApiController]
    [Route("api/v1/Employee")]
    public class EmployeeController : BaseCRUDController<EmployeeEntity, EmployeeRequestModel, EmployeeResponseModel>
    {
        private EmployeeService _service;
        private DepartmentAdminService departmentAdminService;
        public EmployeeController(EmployeeService service, ILogger<EmployeeController> logger, DepartmentAdminService departmentAdminService)
          : base(service, logger)
        {
            _baseCRUDService = service;
            _service = (EmployeeService)_baseCRUDService;
            this.departmentAdminService = departmentAdminService;

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
                    formDataObj = _service.GetFormDataObjElement(new EmployeeEntity());
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
         
        [HttpPost]
        [UserPermission(EnumUserRole.Root, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3)]
        [Route("upsert")]
        public override async Task<IActionResult> Upsert([FromForm] EmployeeRequestModel model)
        {
            string funcName = nameof(Upsert);
            Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                var currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);

                EmployeeEntity entity = null;
                if (model.Id != null)
                {
                    entity = this._service.GetById((long)model.Id, true);
                    if (entity == null)
                    {
                        return NotFound();
                    }
                }
                else
                {
                    entity = new EmployeeEntity();
                }
                entity = model.GetEntity(entity);
                var dicFormFile = model.GetFiles();
                if (dicFormFile != null && dicFormFile.Count >= 1)
                {
                    Dictionary<string, string> dicImagePath = UploadUtil.UploadImage(dicFormFile);
                    entity.SetFileURL(dicImagePath);
                }

                entity = this._service.Upsert(entity, currentUserId);

                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, entity.ToJson());
                return this.ResponseJSonObj(new EmployeeResponseModel().SetData(entity));
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
        [Route("get-detail")]
        public override async Task<IActionResult> GetDetail([FromQuery] long? Id)
        {
            string funcName = nameof(GetDetail);
            Messages.Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                EmployeeEntity entity = null;
                if (Id == null || Id <= 0)
                {
                    return NotFound();
                }
                else
                {
                    entity = this._baseCRUDService.GetById((long)Id);
                    if (entity == null)
                    {
                        return NotFound();
                    }
                }

                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, entity);
                return this.ResponseJSonObj(new EmployeeResponseModel().SetData(entity));
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
        [Route("get-all-employee")]
        public async Task<IActionResult> GetAll()
        {
            string funcName = nameof(GetAll);
            Messages.Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                // check role => just show employee in department that current user is admin
                var currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);
                var departmentRoles = departmentAdminService.GetUserAndRole(currentUserId.GetValueOrDefault()
                    , new List<EnumDepartmentAdmin>() { EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3 });
                List<EmployeeEntity> listUser = new List<EmployeeEntity>();

                List<EmployeeEntity> listEntity = _service.GetAll();

                if (listEntity == null)
                {
                    return this.ResponseError(null);
                }
               
                var listItemResponse = new EmployeeResponseModel().SetData(listEntity, departmentRoles?.Select(x => x.Id).ToList());

                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, listEntity.Count);

                return this.ResponseJSonObj(listItemResponse);
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
        [UserPermission(EnumUserRole.Root, EnumDepartmentAdmin.Admin3)]
        [Route("delete")]
        public override async Task<IActionResult> Delete([FromQuery] long Id)
        {
            //CachedFunc.ClearRedisAll();
            return await base.Delete(Id);
        }
    }
}