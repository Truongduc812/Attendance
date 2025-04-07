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
using MathNet.Numerics.Statistics.Mcmc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using iSoft.Common.CommonFunctionNS;
using iSoft.Common.Utils;
using Microsoft.AspNetCore.Authorization;
using static iSoft.Common.Messages;
using iSoft.Common.Enums;
using SourceBaseBE.MainService.CustomAttributes;
using System.Net.NetworkInformation;
using NPOI.POIFS.FileSystem;
using System.Text.RegularExpressions;

namespace SourceBaseBE.MainService.Controllers
{
    [ApiController]
    [Route("api/v1/SystemSetting")]
    public class SystemSettingController : BaseCRUDController<SystemSettingEntity, SystemSettingRequestModel, SystemSettingResponseModel>
    {
        private SystemSettingService _service;
        public SystemSettingController(SystemSettingService service, ILogger<SystemSettingController> logger)
          : base(service, logger)
        {
            _baseCRUDService = service;
            _service = (SystemSettingService)_baseCRUDService;
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

                var entity = this._service.GetAll().FirstOrDefault();
                if (entity == null)
                {
                    return NotFound();
                }
                formDataObj = _baseCRUDService.GetFormDataObjElement(entity);

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
        public override async Task<IActionResult> Upsert([FromForm] SystemSettingRequestModel model)
        {
            string funcName = nameof(Upsert);
            Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                var currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);

                SystemSettingEntity entity = null;

                entity = this._service.GetAll().FirstOrDefault();

                entity = model.GetEntity(entity);

                var dicFormFile = model.GetFiles();
                if (dicFormFile != null && dicFormFile.Count >= 1)
                {
                    Dictionary<string, string> dicImagePath = UploadUtil.UploadFile(dicFormFile);
                    entity.SetFileURL(dicImagePath);
                }

                entity = this._baseCRUDService.Upsert(entity, currentUserId);
                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, entity);
                return this.ResponseJSonObj(entity);
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
        [Route("get-system-setting-by-key")]
        public async Task<IActionResult> GetSystemSettingByKey([FromQuery] string Key)
        {
            string funcName = nameof(GetSystemSettingByKey);
            Messages.Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                SystemSettingEntity entity = null;
                long? result = 0;
                if (string.IsNullOrEmpty(Key))
                {
                    return NotFound();
                }

                entity = this._baseCRUDService.GetList().FirstOrDefault();

                if (Key == nameof(SystemSettingEntity.TimeSwitchInterval))
                {
                    if (entity == null || entity.TimeSwitchInterval == null || entity.TimeSwitchInterval == 0)
                    {
                        result = 10; //get default 10s
                    }
                    else
                    {
                        result = entity.TimeSwitchInterval; 
                    }
                }

                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, entity);
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

        [HttpGet]
        //[Authorize]
        //[UserPermission(EnumUserRole.Root, EnumDepartmentAdmin.Admin3)]
        [Route("status-connect")]
        public async Task<IActionResult> GetStatus()
        {
            string funcName = nameof(GetStatus);
            Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);

                var urlFaceId = Environment.GetEnvironmentVariable("AttendanceTrackAPI");
                string ipFaceId = "";
                string pattern = @"http://(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})";

                //Progress urlFaceId
                Regex regex = new Regex(pattern);
                Match match = regex.Match(urlFaceId);
                if (match.Success)
                {
                    ipFaceId = match.Groups[1].Value;
                }
                else
                {
                    ipFaceId = "";
                }

                //Ping devices
                Ping ping = new Ping();
                PingReply reply = await ping.SendPingAsync(ipFaceId,3000);
                if (reply.Status == IPStatus.Success)
                {
                    this._logger.LogMsg(Messages.ISuccess_0_1, funcName, reply.Status.ToString());
                    return this.ResponseJSonObj(true);
                }

                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, reply.Status.ToString());
                return this.ResponseJSonObj(false);
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
        //[Authorize]
        [Route("ping-device")]
        public async Task<IActionResult> PingDevice([FromQuery] string ip)
        {
            string funcName = nameof(PingDevice);
            Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                if (string.IsNullOrEmpty(ip))
                {
                    throw new Exception("Please, Input IP Address Before Ping Device !");
                }
                //Ping devices
                Ping ping = new Ping();
                PingReply reply = await ping.SendPingAsync(ip,3000);
                if (reply.Status == IPStatus.Success)
                {
                    this._logger.LogMsg(Messages.ISuccess_0_1, funcName, reply.Status.ToString());
                    return this.ResponseJSonObj(true);
                }

                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, reply.Status.ToString());
                return this.ResponseJSonObj(false);
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
        //[Authorize]
        //[UserPermission(EnumUserRole.Root, EnumDepartmentAdmin.Admin3)]
        [Route("get-address")]
        public async Task<IActionResult> PingConnectDevice()
        {
            string funcName = nameof(GetStatus);
            Message errMessage = null;

            try
            {
                this._logger.LogMsg(Messages.IFuncStart_0, funcName);
                var urlFaceId = Environment.GetEnvironmentVariable("AttendanceTrackAPI");
                string ipFaceId = "";
                string pattern = @"http://(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})";

                //Progress urlFaceId
                Regex regex = new Regex(pattern);
                Match match = regex.Match(urlFaceId);
                if (match.Success)
                {
                    ipFaceId = match.Groups[1].Value;
                }
                else
                {
                    ipFaceId = "";
                }

                this._logger.LogMsg(Messages.ISuccess_0_1, funcName, ipFaceId);
                return this.ResponseJSonObj(ipFaceId);
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