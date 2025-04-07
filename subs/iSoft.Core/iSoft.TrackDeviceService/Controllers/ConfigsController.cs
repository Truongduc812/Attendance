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
using UDF.TrackDeviceService;
using UDF.TrackDeviceService.Models.DTOs;
using UDF.TrackDeviceService.Services.Interfaces;
using UDF.TrackDeviceService.Utils;

namespace iSoft.TrackDeviceService.Api.Controllers
{
	public class ConfigsController : BaseController<Config>
	{
		protected override string TableName { get; set; } = Constant.ConfigDBName;
		private readonly IInputService pushDataService;
		public ConfigsController(IGenericRepository<Config> repository, IInputService backgroundService, ILoggerFactory loggerFactory)
			: base(repository, backgroundService, loggerFactory)
		{
		}
		// GET: api/connectionconfigs
		[HttpGet("all")]
		public async Task<IActionResult> GetConnectionConfigs()
		{
			try
			{
				var connectionConfigs = this.repository.GetAll();
				return this.ResponseOk(connectionConfigs.Select(s => s.ToDTO()));
			}
			catch (Exception ex)
			{
				return StatusCode(400, Messages.ErrGetItem_0_1.SetParameters(nameof(GetConnectionConfigs), ex));
			}
		}
		// GET: api/connectionconfigs/5
		[HttpGet]
		[Route("{Id:int}")]
		public async Task<IActionResult> GetConfigById(ulong id)
		{
			try
			{
				var connectionConfig = await repository.GetByIdAsync(id);
				if (connectionConfig == null)
				{
					return NotFound();
				}

				return this.ResponseOk(connectionConfig.ToDTO());
			}
			catch (Exception ex)
			{

				return StatusCode(400, Messages.ErrGetItem_0_1.SetParameters(nameof(GetConfigById), ex));
			}
		}
		[HttpGet("{ConnectionKey}")]
		public async Task<IActionResult> GetConfigByName(string name)
		{
			try
			{
				var config = await repository.GetByPropertyAsync(this.TableName, "ConnectionKey", name);
				if (config == null)
				{
					return NotFound();
				}

				return this.ResponseOk(config.Select(s => s.ToDTO()));
			}
			catch (Exception ex)
			{

				return StatusCode(400, Messages.ErrGetItem_0_1.SetParameters(nameof(GetConfigByName), ex));
			}
		}
		// POST: api/connectionconfigs
		[HttpPost]
		public async Task<ActionResult<ConfigDTO>> CreateConfig(ConfigDTO connectionConfig)
		{
			try
			{
				var config = connectionConfig.FromDTO();
				await repository.InsertAsync(config);
				return CreatedAtAction(nameof(CreateConfig), connectionConfig);
			}
			catch (Exception ex)
			{
				return StatusCode(400, Messages.ErrInsertItem_0_1.SetParameters(nameof(CreateConfig), ex));
			}
		}

		// PUT: api/connectionconfigs/5
		[HttpPut]
		public async Task<IActionResult> UpdateConfig(ConfigDTO connectionConfig)
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
				return StatusCode(400, Messages.ErrGetItem_0_1.SetParameters(nameof(UpdateConfig), ex));
			}
		}

		// DELETE: api/connectionconfigs/5
		[HttpDelete("{Id}")]
		public async Task<IActionResult> DeleteConfig(ulong id)
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
				return StatusCode(400, Messages.ErrGetItem_0_1.SetParameters(nameof(DeleteConfig), ex));
			}
		}


	}

}