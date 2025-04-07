using ConnectionCommon.Connection;
using iSoft.Common;
using iSoft.Extensions.EntityFrameworkCore.Repository;
using iSoft.TrackDeviceService.Api.Utils;
using iSoft.TrackDeviceService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UDF.TrackDeviceService.Models.DTOs;
using UDF.TrackDeviceService.Services.Interfaces;
using UDF.TrackDeviceService.Utils;

namespace iSoft.TrackDeviceService.Api.Controllers
{

	public class ParamsController : BaseController<Param>
	{
		protected override string TableName { get; set; } = Constant.ParamDBName;
		public ParamsController(IGenericRepository<Param> repository, IInputService connectionService, ILoggerFactory loggerFactory)
			: base(repository, connectionService, loggerFactory)
		{
		}
		// GET: api/connectionparams
		[HttpGet("all")]
		public async Task<IActionResult> GetParams()
		{
			try
			{
				var connectionConfigs = (await this.repository.GetAllAsync()).Select(s => s.ToDTO());
				return this.ResponseOk(connectionConfigs);
			}
			catch (Exception ex)
			{
				return StatusCode(400, Messages.ErrGetItem_0_1.SetParameters(nameof(GetParams), ex));
			}
		}

		// GET: api/connectionparams/5
		[HttpGet]
		[Route("{Id:int}")]
		public async Task<IActionResult> GetParamById(ulong id)
		{
			try
			{
				var connectionConfig = (await repository.GetByIdAsync(id)).ToDTO();

				if (connectionConfig == null)
					return NotFound();

				return this.ResponseOk(connectionConfig);
			}
			catch (Exception ex)
			{

				return StatusCode(400, Messages.ErrGetItem_0_1.SetParameters(nameof(GetParamById), ex));
			}
		}
		// GET: api/connectionconfigs/5
		[HttpGet("{ConnectionKey}")]
		public async Task<IActionResult> GetParamByName(string name)
		{
			try
			{
				var connectionConfig = (await repository.GetByPropertyAsync(TableName, "ConnectionKey", name)).Select(s => s.ToDTO());
				if (connectionConfig == null)
					return NotFound();

				return this.ResponseOk(connectionConfig);
			}
			catch (Exception ex)
			{

				return StatusCode(400, Messages.ErrGetItem_0_1.SetParameters(nameof(GetParamByName), ex));
			}
		}
		// POST: api/connectionparams
		[HttpPost]
		public async Task<ActionResult<Param>> CreateParam(ParamDTO connectionParam)
		{
			try
			{
				await repository.InsertAsync(connectionParam.FromDTO());
				return CreatedAtAction(nameof(CreateParam), connectionParam);
			}
			catch (Exception ex)
			{
				return StatusCode(400, Messages.ErrGetItem_0_1.SetParameters(nameof(CreateParam), ex));
			}
		}

		// PUT: api/connectionparams/5
		[HttpPut("{Id}")]
		public async Task<IActionResult> UpdateParam(ParamDTO param)
		{
			try
			{
				if (param.id == null)
					return BadRequest();
				var requestChangeParam = param.FromDTO();
				var existingConnectionParam = await repository.GetByIdAsync(param.id);
				if (existingConnectionParam == null)
					return NotFound();
				existingConnectionParam = requestChangeParam;
				await repository.UpdateAsync(existingConnectionParam);
				return this.ResponseOk(existingConnectionParam);
			}
			catch (Exception ex)
			{
				return StatusCode(400, Messages.ErrGetItem_0_1.SetParameters(nameof(UpdateParam), ex));
			}
		}

		// DELETE: api/connectionparams/5
		[HttpDelete("{Id}")]
		public async Task<IActionResult> DeleteParam(ulong id)
		{
			try
			{
				var existingConnectionParam = await repository.GetByIdAsync(id);

				if (existingConnectionParam == null)
					return NotFound();

				await repository.DeleteAsync(existingConnectionParam);
				return this.ResponseOk(existingConnectionParam);
			}
			catch (Exception ex)
			{
				return StatusCode(400, Messages.ErrGetItem_0_1.SetParameters(nameof(DeleteParam), ex));
			}
		}


	}
}