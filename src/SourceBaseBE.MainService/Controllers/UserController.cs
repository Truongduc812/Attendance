using Microsoft.AspNetCore.Mvc;
using SourceBaseBE.Database.Models.ResponseModels;
using SourceBaseBE.Database.Entities;
using SourceBaseBE.Database.Models.RequestModels;
using SourceBaseBE.MainService.Services;
using Microsoft.Extensions.Logging;
using iSoft.Common.CommonFunctionNS;
using iSoft.Common.Exceptions;
using iSoft.Common.Utils;
using iSoft.Common;
using MathNet.Numerics.Statistics.Mcmc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using static iSoft.Common.Messages;
using iSoft.Common.ExtensionMethods;
using iSoft.Common.Models.RequestModels;
using iSoft.Common.Models.ResponseModels;
using Microsoft.AspNetCore.Identity;
using SocketIOClient.Messages;
using PRPO.MainService.CustomAttributes;
using iSoft.Common.Enums;
using NPOI.POIFS.FileSystem;
using SourceBaseBE.MainService.CustomAttributes;
using System.Security.Cryptography;

namespace SourceBaseBE.MainService.Controllers
{
    [ApiController]
    [Route("api/v1/User")]
    public class UserController : BaseCRUDController<UserEntity, UserRequestModel, UserResponseModel>
    {
        private UserService _service;
        private DepartmentAdminService _departmentAdminService;
        public UserController(UserService service, DepartmentAdminService departmentAdminService, ILogger<UserController> logger)
          : base(service, logger)
        {
            _baseCRUDService = service;
            _service = (UserService)_baseCRUDService;
            this._departmentAdminService = departmentAdminService;
        }

        [HttpPost]
        [UserPermission(EnumDepartmentAdmin.Admin3)]
        [Route("upsert")]
        public override async Task<IActionResult> Upsert([FromForm] UserRequestModel request)
        {
            string funcName = nameof(Upsert);
            Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                var currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);

                UserEntity userEntity = null;
                DepartmentAdminEntity departmentAdminEntity = null;
                List<DepartmentAdminEntity> listDepartmentAdmin = new List<DepartmentAdminEntity>();
                if (request.Id != null)
                {
                    userEntity = this._service.GetById((long)request.Id, true);
                    if (userEntity == null)
                    {
                        return NotFound();
                    }
                    //var isStrong = _service.IsStrongPassword(request.Password);
                    //if (!isStrong)
                    //{
                    //    return this.ResponseError(Messages.ErrRolePassword);
                    //}
                    userEntity = request.GetEntityUserEdit(userEntity);
                }
                else
                {
                    userEntity = new UserEntity();
                    var _existedUsername = _service.GetUserByName(request.Username);
                    if (_existedUsername != null)
                    {
                        return this.ResponseError(new Message(EnumMessageType.Error, "USERNAME EXISTED", "Username has been registered,Please choose another username"));
                    }
                    userEntity = request.GetEntity(userEntity);

                }


                var dicFormFile = request.GetFiles();
                if (dicFormFile != null && dicFormFile.Count >= 1)
                {
                    Dictionary<string, string> dicImagePath = UploadUtil.UploadFile(dicFormFile);
                    userEntity.SetFileURL(dicImagePath);
                }
                userEntity.Role = EnumUserRole.User.ToString();
                if (!request.DepartmentAdmins.Contains((int)EnumDepartmentAdmin.User))
                {
                    request.DepartmentAdmins.Add((int)EnumDepartmentAdmin.User);
                }
                if (request.DepartmentAdmins == null || request.DepartmentAdmins.Count <= 0)
                {
                    var retUser = _service.GetById(userEntity.Id, true);
                    if (retUser == null)
                    {
                        var retEntity = _service.Upsert(userEntity, currentUserId);
                        if (retEntity == null)
                        {
                            this._logger.LogMsg(Messages.ErrNotFound_0_1, funcName, false);
                            return this.ResponseJSonObj(false);
                        }
                        var data = _service.GetProfileById(retEntity.Id, true);
                        return this.ResponseJSonObj(data);
                    }
                }
                else
                {
                    foreach (var item in request.DepartmentAdmins)
                    {
                        departmentAdminEntity = new DepartmentAdminEntity();
                        departmentAdminEntity.Role = (EnumDepartmentAdmin)item;
                        departmentAdminEntity.DepartmentId = request.DepartmentId;
                        listDepartmentAdmin.Add(departmentAdminEntity);
                    }
                }
                if (listDepartmentAdmin == null || listDepartmentAdmin.Count < 1)
                {
                    this._logger.LogMsg(Messages.ISuccess_0_1, funcName, true);
                    return this.ResponseJSonObj(true);
                }
                var ret = _service.UpsertTransaction(userEntity, listDepartmentAdmin, request.DepartmentId, currentUserId);

                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, ret);
                return this.ResponseJSonObj(ret);

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
            return await base.Delete(Id);
        }

        [HttpPost]
        [UserPermission]
        [Route("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UserRequestModel request)
        {
            string funcName = nameof(Upsert);
            Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                var currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);
                UserEntity? currentUser = (UserEntity)CommonFunction.GetCurrentUser(this.HttpContext);

                var currentUserTracking = _service.GetById(currentUser.Id, true, true);
                if (currentUserTracking == null)
                {
                    return NotFound();
                }

                currentUserTracking = request.GetEntity(currentUserTracking);

                var dicFormFile = request.GetFiles();
                if (dicFormFile != null && dicFormFile.Count >= 1)
                {
                    Dictionary<string, string> dicImagePath = UploadUtil.UploadFile(dicFormFile);
                    currentUserTracking.SetFileURL(dicImagePath);
                }

                var retEntity = _service.Upsert(currentUserTracking, currentUserId);
                if (retEntity == null)
                {
                    this._logger.LogMsg(Messages.ErrNotFound_0_1, funcName, false);
                    return this.ResponseJSonObj(null);
                }
                var data = _service.GetProfileById(retEntity.Id, true);
                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, data);
                return this.ResponseJSonObj(data);
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

        [Authorize]
        [HttpGet("Profile")]
        public IActionResult GetProfile()
        {
            var currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);

            if (currentUserId == null)
                return NotFound();

            //var user = _service.GetById((long)currentUserId);
            var user = _service.GetProfileById((long)currentUserId, false);

            if (user == null)
                return NotFound();
            return this.ResponseJSonObj(user);
        }
        [HttpGet]
        [Route("get-form")]
        public async Task<IActionResult> GetCreateFormDataUser([FromQuery] long? userId, long? DepartmentId)
        {

            string funcName = nameof(GetCreateFormData);
            Messages.Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                List<Dictionary<string, object>> formDataObj = null;
                if (userId == null)
                {
                    formDataObj = _service.GetFormDataObjElement(new UserEntity(), DepartmentId, true);
                }
                else
                {
                    var entity = this._service.GetById((long)userId);
                    if (entity == null)
                    {
                        return NotFound();
                    }
                    formDataObj = _service.GetFormDataObjElement(entity, DepartmentId, false);
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


        [HttpGet]
        [Route("get-all-user")]
        public async Task<IActionResult> GetAll()
        {
            string funcName = nameof(GetAll);
            Messages.Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                List<UserEntity> listUser = new List<UserEntity>();

                List<UserEntity> listEntity = _service.GetAll();

                if (listEntity == null)
                {
                    return this.ResponseError(null);
                }

                var listItemResponse = new UserResponseModel().SetData(listEntity);

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
        [UserPermission]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordRequestModel model)
        {
            long? currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);
            string funcName = nameof(ChangePassword);
            var user = _service.GetById((long)currentUserId, true);
            if (user == null)
                return NotFound();

            if (model.CurrentPassword == null || model.CurrentPassword == "" ||
              model.NewPassword == null || model.NewPassword == "" ||
              model.ConfirmPassword == null || model.ConfirmPassword == "")
                return this.ResponseError(Messages.ErrRolePassword);

            string currentPassword = EncodeUtil.MD5(model.CurrentPassword.ToString());
            if (currentPassword != user.Password)
            {
                return this.ResponseError(Messages.ErrWrongPassword);
            }

            if (string.Compare(model.NewPassword, model.ConfirmPassword) == 1)
            {
                return this.ResponseError(Messages.ErrDifferrentChangePassword);
            }
            var isStrong = _service.IsStrongPassword(model.NewPassword);
            if (!isStrong)
            {
                return this.ResponseError(Messages.ErrRolePassword);
            }
            var ret = await _service.ChangePassword(user, model.NewPassword);
            this._logger.LogMsg(Messages.ISuccess_0_1, funcName, ret);
            if (!ret)
            {
                return this.ResponseError(Messages.ErrChangePasswordFailded);
            }
            return this.ResponseOk(ret);
        }

        [HttpPost]
        [UserPermission(EnumDepartmentAdmin.Admin3)]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromQuery] long id)
        {
            string funcName = nameof(Upsert);
            Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                var currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);

                UserEntity userEntity = null;

                if (id == 0)
                {
                    return this.ResponseError(new Message(EnumMessageType.Error, "USERNAME NOT EXIST", ""));
                }

                userEntity = this._service.GetById((long)id, true);
                if (userEntity == null)
                {
                    return this.ResponseError(new Message(EnumMessageType.Error, "USERNAME NOT EXIST", ""));
                }
                else
                {
                    string PassDefault = "123123";
                    userEntity.Password = EncodeUtil.MD5(PassDefault);
                    var ret = _service.Upsert(userEntity, currentUserId);

                    this._logger.LogMsg(Messages.ISuccess_0_1, funcName, ret);
                    if (ret == null)
                    {
                        return this.ResponseError(new Message(EnumMessageType.Error, "Reset Password Failed", "Try Again"));
                    }
                    else
                    {
                        return this.ResponseOk(true);
                    }
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
    }
}