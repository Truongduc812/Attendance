using ConnectionCommon.Connection;
using iSoft.Common;
using iSoft.Extensions.EntityFrameworkCore.Repository;
using iSoft.TrackDeviceService.Api.Utils;
using iSoft.TrackDeviceService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
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
	public class ConnectionParamController : BaseController<ConnectionParam>
	{
		protected override string TableName { get; set; } = Constant.ConnectionParamDBName;
		public ConnectionParamController(IGenericRepository<ConnectionParam> repository, IInputService connectionService, ILoggerFactory loggerFactory)
			: base(repository, connectionService, loggerFactory)
		{
		}


		// GET: api/connectionconfigs/all
		[HttpGet("all")]
		public async Task<IActionResult> GetConnectionParams()
		{
			try
			{
				var connectionConfigs = await this.repository.GetAllAsync();
				return this.ResponseOk(connectionConfigs);
			}
			catch (Exception ex)
			{
				return StatusCode(400, Messages.ErrGetItem_0_1.SetParameters(nameof(GetConnectionParams), ex));
			}
		}
		// GET: api/connectionconfigs/5
		[HttpGet]
		[Route("{Id:int}")]
		public async Task<IActionResult> GetConnectionParamById(ulong id)
		{
			try
			{
				var connectionConfig = await repository.GetByIdAsync(id);
				if (connectionConfig == null)
					return NotFound();

				return this.ResponseOk(connectionConfig.ToDTO(connectionConfig.Connection, connectionConfig.Param));

			}
			catch (Exception ex)
			{

				return StatusCode(400, Messages.ErrGetItem_0_1.SetParameters(nameof(GetConnectionParamById), ex));
			}
		}
		[HttpGet("{ConnectionKey}")]
		public async Task<IActionResult> GetConnectionParamByName(string name)
		{
			try
			{
				var config = await repository.GetByPropertyAsync(this.TableName, "ConnectionKey", name);
				if (config == null)
				{
					return NotFound();
				}

				return this.ResponseOk(config.Select(s => s.ToDTO(s.Connection, s.Param)));
			}
			catch (Exception ex)
			{

				return StatusCode(400, Messages.ErrGetItem_0_1.SetParameters(nameof(GetConnectionParamByName), ex));
			}
		}
		// POST: api/connectionconfigs
		[HttpPost]
		public async Task<ActionResult<ConnectionParam>> CreateConnectionParam(ConnectionParamDTO connectionConfig)
		{
			try
			{
				await repository.InsertAsync(connectionConfig.FromDTO());
				return CreatedAtAction(nameof(CreateConnectionParam), new { id = connectionConfig.id }, connectionConfig);
			}
			catch (Exception ex)
			{

				return StatusCode(400, Messages.ErrInsertItem_0_1.SetParameters(nameof(CreateConnectionParam), ex));
			}
		}

		// PUT: api/connectionconfigs/5
		[HttpPut("{Id}")]
		public async Task<IActionResult> UpdateConnectionParam(ConnectionParamDTO connectionParam)
		{
			try
			{
				if (connectionParam.id == null)
					return BadRequest();

				var existingConnectionParam = await repository.GetByIdAsync(connectionParam.id.Value);

				if (existingConnectionParam == null)
					return NotFound();

				existingConnectionParam = connectionParam.FromDTO();
				await repository.UpdateAsync(existingConnectionParam);

				return this.ResponseOk(existingConnectionParam);
			}
			catch (Exception ex)
			{
				return StatusCode(400, Messages.ErrGetItem_0_1.SetParameters(nameof(UpdateConnectionParam), ex));
			}
		}

		// DELETE: api/connectionconfigs/5
		[HttpDelete("{Id}")]
		public async Task<IActionResult> DeleteConnectionParam(ulong id)
		{
			try
			{
				var existingConnectionConfig = await repository.GetByIdAsync(id);
				if (existingConnectionConfig == null)
					return NotFound();
				await repository.DeleteAsync(existingConnectionConfig);
				return this.ResponseOk(existingConnectionConfig);
			}
			catch (Exception ex)
			{

				return StatusCode(400, Messages.ErrDeleteItem_0_1.SetParameters(nameof(DeleteConnectionParam), ex));
			}
		}


	}

}