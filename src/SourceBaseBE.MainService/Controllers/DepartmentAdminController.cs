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
using System.Collections.Generic;
using System.Threading.Tasks;
using iSoft.Common.CommonFunctionNS;
using static iSoft.Common.Messages;
using iSoft.Common.Enums;
using SourceBaseBE.MainService.CustomAttributes;
using iSoft.Common.ExtensionMethods;

namespace SourceBaseBE.MainService.Controllers
{
    [ApiController]
    [Route("api/v1/DepartmentAdmin")]
    public class DepartmentAdminController : BaseCRUDController<DepartmentAdminEntity, DepartmentAdminRequestModel, DepartmentAdminResponseModel>
    {
        private DepartmentAdminService _service;
        private UserService _userService;
        public DepartmentAdminController(UserService userService, DepartmentAdminService service, ILogger<DepartmentAdminController> logger)
          : base(service, logger)
        {
            _baseCRUDService = service;
            _service = (DepartmentAdminService)_baseCRUDService;
            _userService = userService;
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
                    formDataObj = _service.GetFormDataObjElement(new DepartmentAdminEntity());
                }
                else
                {
                    var entity = this._service.GetById((long)Id, false, false);
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
        [UserPermission(EnumUserRole.Root, EnumDepartmentAdmin.Admin3)]
        [Route("upsert")]
        public override async Task<IActionResult> Upsert([FromForm] DepartmentAdminRequestModel model)
        {
            string funcName = nameof(Upsert);
            Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                var currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);

                if (model.Role == null || model.Role.Count <= 0)
                {
                    errMessage = Messages.ErrNotFound_0_1.SetParameters("Role", model.Role);
                    this._logger.LogMsg(errMessage);
                    return this.ResponseError(errMessage);
                }

                if (model.UserId == null)
                {
                    errMessage = Messages.ErrNotFound_0_1.SetParameters("Account User not be null", model.Id);
                    this._logger.LogMsg(errMessage);
                    return this.ResponseError(errMessage);
                }
                if (model.DepartmentId == null)
                {
                    errMessage = Messages.ErrNotFound_0_1.SetParameters("Department not be null", model.DepartmentId);
                    this._logger.LogMsg(errMessage);
                    return this.ResponseError(errMessage);
                }

                var userEntity = _userService.GetById(model.UserId.Value);
                if (userEntity == null)
                {
                    errMessage = Messages.ErrNotFound_0_1.SetParameters("user", model.UserId);
                    this._logger.LogMsg(errMessage);
                    return this.ResponseError(errMessage);
                }

                //DepartmentAdminEntity departmentAdminEntity = null;
                //if (model.Id != null)
                //{
                //    departmentAdminEntity = this._service.GetById(model.Id.Value);
                //    if (departmentAdminEntity.UserId == null)
                //    {
                //        errMessage = Messages.ErrNotFound_0_1.SetParameters("departmentAdmin", model.Id);
                //        this._logger.LogMsg(errMessage);
                //        return this.ResponseError(errMessage);
                //    }
                //}

                //if (departmentAdminEntity.UserId == null)
                //{
                //    errMessage = Messages.ErrNotFound_0_1.SetParameters("departmentAdmin", model.Id);
                //    this._logger.LogMsg(errMessage);
                //    return this.ResponseError(errMessage);
                //}

                List<DepartmentAdminEntity> listDeptAdmin = new List<DepartmentAdminEntity>();
                foreach (var role in model.Role)
                {
                    var departmentAdminEntity2 = new DepartmentAdminEntity();
                    departmentAdminEntity2.Role = (EnumDepartmentAdmin?)role;
                    departmentAdminEntity2.DepartmentId = model.DepartmentId.Value;
                    listDeptAdmin.Add(departmentAdminEntity2);
                }

                var result = this._userService.UpsertTransaction(userEntity, listDeptAdmin, model.DepartmentId.Value, currentUserId);
                
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

        [HttpPost]
        //[Authorize]
        [UserPermission(EnumUserRole.Root, EnumDepartmentAdmin.Admin3)]
        [Route("delete")]
        public override async Task<IActionResult> Delete([FromQuery] long Id)
        {
            string funcName = nameof(Upsert);
            Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                long? currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);

                var entity = _service.GetById(Id, true, false);

                if (entity != null && entity.Role == EnumDepartmentAdmin.Admin3)
                {
                    return this.ResponseError(Messages.ErrPermission_0, "");
                }

                if (entity != null && entity.User != null)
                {
                    _service.Delete(entity.Id, currentUserId);
                    _userService.Delete((long)entity.UserId, currentUserId);
                }
                //CachedFunc.ClearRedisAll();
                return this.ResponseOk(Id);
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