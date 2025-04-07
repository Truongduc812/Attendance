using ConnectionCommon.Connection;
using iSoft.Common;
using iSoft.Common.Errors;
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
	public class ConnectionConfigController : BaseController<ConnectionConfig>
	{
		protected override string TableName { get; set; } = Constant.ConnectionConfigDBName;
		public ConnectionConfigController(IGenericRepository<ConnectionConfig> repository, IInputService backgroundService, ILoggerFactory loggerFactory)
			: base(repository, backgroundService, loggerFactory)
		{
		}


		// GET: api/connectionconfigs
		[HttpGet("all")]
		public async Task<IActionResult> GetConnectionConfigs()
		{
			try
			{
				var connectionConfigs = (await this.repository.GetAllAsync()).Select(s => s.ToDTO());
				return this.ResponseOk(connectionConfigs);
			}
			catch (Exception ex)
			{
				return StatusCode(400, Messages.ErrGetItem_0_1.SetParameters(nameof(GetConnectionConfigs), ex));
			}
		}
		// GET: api/connectionconfigs/5
		[HttpGet]
		[Route("{Id:int}")]
		public async Task<IActionResult> GetConnectionConfigById(ulong id)
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

				return StatusCode(400, Messages.ErrGetItem_0_1.SetParameters(nameof(GetConnectionConfigById), ex));
			}
		}
		// GET: api/connectionconfigs/5
		[HttpGet("{ConnectionKey}")]
		public async Task<IActionResult> GetConnectionConfigByName(string name)
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

				return StatusCode(400, Messages.ErrGetItem_0_1.SetParameters(nameof(GetConnectionConfigByName), ex));
			}
		}
		// POST: api/connectionconfigs
		[HttpPost]
		public async Task<IActionResult> CreateConnectionConfig(ConnectionConfigDTO connectionConfig)
		{
			try
			{
				await repository.InsertAsync(connectionConfig.FromDTO());
				return CreatedAtAction(nameof(CreateConnectionConfig), connectionConfig);
			}
			catch (Exception ex)
			{
				return StatusCode(400, Messages.ErrUpdateItem_0_1.SetParameters(nameof(CreateConnectionConfig), ex));
			}
		}

		// PUT: api/connectionconfigs/5
		[HttpPut("{Id}")]
		public async Task<IActionResult> UpdateConnectionConfig(ConnectionConfigDTO connectionConfig)
		{
			try
			{
				if (connectionConfig.id == null)
					return BadRequest();

				var existingConnectionConfig = await repository.GetByIdAsync(connectionConfig.id.Value);
				if (existingConnectionConfig == null)
					return NotFound();
				existingConnectionConfig = connectionConfig.FromDTO();
				await repository.UpdateAsync(existingConnectionConfig);

				return this.ResponseOk(existingConnectionConfig);
			}
			catch (Exception ex)
			{
				return StatusCode(400, Messages.ErrGetItem_0_1.SetParameters(nameof(UpdateConnectionConfig), ex));
			}
		}

		// DELETE: api/connectionconfigs/5
		[HttpDelete("{Id}")]
		public async Task<IActionResult> DeleteConnectionConfig(ulong id)
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
				return StatusCode(400, Messages.ErrUpdateItem_0_1.SetParameters(nameof(DeleteConnectionConfig), ex));
			}
		}


	}

}