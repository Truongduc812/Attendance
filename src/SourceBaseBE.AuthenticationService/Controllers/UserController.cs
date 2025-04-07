using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using iSoft.Auth.Services;
using iSoft.Auth.Models;
using System.Threading.Tasks;
using iSoft.Common;
using static iSoft.Common.Messages;
using iSoft.Common.Exceptions;
using System;
using Microsoft.Extensions.Logging;
using iSoft.Database.Entities;
using iSoft.Common.Utils;
using iSoft.Common.ExtensionMethods;
using iSoft.Common.Enums;
using iSoft.Common.CommonFunctionNS;
using static iSoft.Common.ConstCommon;
using iSoft.Database.Models.RequestModels;
using iSoft.Database.Models.ResponseModels;
using iSoft.Auth.ExtensionMethods;

namespace iSoft.Auth.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/User")]
    public class UserController : BaseCRUDController<UserEntity, UserRequestModel, UserResponseModel>
    {
        private readonly ILogger<UserController> _logger;
        private UserService _serviceImp;
        public UserController(ILoggerFactory loggerFactory, UserService service)
          : base(loggerFactory, service)
        {
            _logger = loggerFactory.CreateLogger<UserController>();
            _baseCRUDService = service;
            _serviceImp = (UserService)_baseCRUDService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequestModel model)
        {
            string funcName = "Authenticate";
            Message errMessage = null;
            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                model.Password = EncodeUtil.MD5(model.Password);

                var user = await _serviceImp.Authenticate(model.Username, model.Password);

                if (user == null)
                {
                    this._logger.LogInformation("Response: " + Messages.ErrLogin.GetMessage() + ", " + model.Username);
                    return this.ResponseError(Messages.ErrLogin);
                }

                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, user.Username);
                return this.ResponseJSonObj(user);
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
        [Authorize(Roles = "Admin,Root")]
        [HttpPost("upsert")]
        public async override Task<IActionResult> Upsert([FromBody] UserRequestModel userRequest)
        {
            string funcName = "Upsert";
            Message errMessage = null;
            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                if (userRequest.Role == EnumUserRole.Root.ToString() && !User.IsInRole(EnumUserRole.Root.ToString()))
                {
                    this._logger.LogMsg(Messages.ErrPermission_0, userRequest.Role);
                    return this.ResponseError(Messages.ErrPermission_0, userRequest.Role);
                }
                var user = userRequest.GetEntity(userRequest);
                var exist = _serviceImp.IsExistUsername(user.Username);
                if (exist)
                {
                    this._logger.LogMsg(Messages.ErrAlreadyExists_0_1, "Username", exist);
                    return this.ResponseError(Messages.ErrAlreadyExists_0_1, "Username Already Exists", exist);
                }
                user = _serviceImp.Upsert(user).WithoutPassword();
                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, user.ToJson());

                return this.ResponseOk(user);
            }
            catch (DBException ex)
            {
                errMessage = Messages.ErrDBException.SetParameters(userRequest.ToJson(), ex);
            }
            catch (BaseException ex)
            {
                errMessage = Messages.ErrBaseException.SetParameters(userRequest.ToJson(), ex);
            }
            catch (Exception ex)
            {
                errMessage = Messages.ErrException.SetParameters(userRequest.ToJson(), ex);
            }
            this._logger.LogMsg(errMessage);
            return this.ResponseError(errMessage);
        }
        [Authorize(Roles = "Admin,Root")]
        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] UserEntity user)
        {
            string funcName = "Insert";
            Message errMessage = null;
            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                if (user.Role == EnumUserRole.Root.ToString() && !User.IsInRole(EnumUserRole.Root.ToString()))
                {
                    this._logger.LogMsg(Messages.ErrPermission_0, user.Role);
                    return this.ResponseError(Messages.ErrPermission_0, user.Role);
                }

                user.Password = EncodeUtil.MD5(user.Password);

                string existsUserName = this._serviceImp.Insert(user);
                if (existsUserName != "")
                {
                    this._logger.LogMsg(Messages.ErrAlreadyExists_0_1, "Username", existsUserName);
                    return this.ResponseError(Messages.ErrAlreadyExists_0_1, "Username", existsUserName);
                }

                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, user.ToJson());

                return this.ResponseOk(user);
            }
            catch (DBException ex)
            {
                errMessage = Messages.ErrDBException.SetParameters(user.ToJson(), ex);
            }
            catch (BaseException ex)
            {
                errMessage = Messages.ErrBaseException.SetParameters(user.ToJson(), ex);
            }
            catch (Exception ex)
            {
                errMessage = Messages.ErrException.SetParameters(user.ToJson(), ex);
            }
            this._logger.LogMsg(errMessage);
            return this.ResponseError(errMessage);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);
            if (id != currentUserId && !User.IsInRole(EnumUserRole.Root.ToString()))
                return Forbid();

            var user = _serviceImp.GetById(id);

            if (user == null)
                return NotFound();

            return this.ResponseOk(user);
        }
        [Authorize(Roles = "Admin,Root,User")]
        [HttpGet("Profile")]
        public IActionResult GetProfile()
        {
            var currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);

            var user = _serviceImp.GetById((long)currentUserId);

            if (user == null)
                return NotFound();

            return this.ResponseOk(user);
        }
    }
}
