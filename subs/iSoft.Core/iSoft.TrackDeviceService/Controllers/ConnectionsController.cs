using ConnectionCommon.Connection;
using iSoft.Common;
using iSoft.ConnectionCommon.IndustrialConn.TcpGroup.EtherCAT;
using iSoft.ConnectionCommon.Model;
using iSoft.Extensions.EntityFrameworkCore.Repository;
using iSoft.TrackDeviceService.Api.Utils;
using iSoft.TrackDeviceService.Models;
using iSoft.TrackDeviceService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UDF.TrackDeviceService.Models;
using UDF.TrackDeviceService.Models.DTOs;
using UDF.TrackDeviceService.Services.Interfaces;
using UDF.TrackDeviceService.Utils;
using static iSoft.Common.Messages;

namespace iSoft.TrackDeviceService.Api.Controllers
{

	public class ConnectionsController : BaseController<Connection>
	{

		protected override string TableName { get; set; } = Constant.ConnectionDBName;
		public ConnectionsController(IGenericRepository<Connection> repository, IInputService connectionService, ILoggerFactory loggerFactory)
			: base(repository, connectionService, loggerFactory)
		{
		}
		// GET ALL
		[HttpGet("all")]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				var cnnS = cnnService.Connections.Where(s => { return s.Connection.isDelete == null || s.Connection.isDelete == false; });
				if (cnnS == null)
				{
					return NotFound();
				}
				var ret = cnnS.Select(s => s.Connection.ToDTO(s.IsConnect)).ToList();
				Console.WriteLine("GetAll Success");
				return this.ResponseOk(ret);
			}
			catch (Exception ex)
			{

				Console.WriteLine("GetAll Error", ex.Message, ex.StackTrace);
				return StatusCode(400, Messages.ErrGetItem_0_1.SetParameters(nameof(GetConnectionById), ex));
			}
		}
		// GET: api/connections/5
		[HttpGet]
		[Route("{Id:int}")]
		public async Task<IActionResult> GetConnectionById(ulong id)
		{
			try
			{
				var cnnS = cnnService.Connections.FirstOrDefault(s => s.Connection.Id == id);
				if (cnnS == null)
				{
					return NotFound();
				}

				return this.ResponseOk(cnnS.Connection.ToDTO(cnnS.IsConnect));
			}
			catch (Exception ex)
			{

				return StatusCode(400, Messages.ErrGetItem_0_1.SetParameters(nameof(GetConnectionById), ex));
			}
		}
		// GET: api/connections/MCProtocol
		[HttpGet("{ConnectionKey}")]
		public async Task<ActionResult<IQueryable<ConnectionDTO>>> GetConnectionByName(string name)
		{
			try
			{
				//var connection = await repository.GetByIdAsync(Id);
				var cnnS = cnnService.Connections.Where(s => s.Connection.DisplayName == name);
				if (cnnS == null)
					return NotFound();
				return Ok(cnnS.Select(s => s.Connection.ToDTO(s.IsConnect)));
			}
			catch (Exception ex)
			{

				return StatusCode(400, Messages.ErrGetItem_0_1.SetParameters(nameof(GetConnectionById), ex));
			}
		}
		// GET: api/connections/MCProtocol
		[HttpGet]
		public async Task<ActionResult<IQueryable<ConnectionDTO>>> GetConnectionByParam([FromQuery] ParamRequest param)
		{
			Dictionary<string, object> dict = new Dictionary<string, object>();
			try
			{
				List<Connection> conn;
				List<IConnection> cnnsS;
				foreach (var p in param.GetType().GetProperties())
				{
					var val = p.GetValue(param);
					if (val != null)
						dict.Add(p.Name, p.GetValue(param));
				}
				if (dict.Count > 0)
					conn = (await repository.GetByMultiPropertiesAsync(TableName, dict)).ToList();
				else
				{
					conn = (await repository.GetAllAsync()).ToList();
				}
				cnnsS = cnnService.Connections.Where(s => conn.Contains(s.Connection)).ToList();
				return Ok(cnnsS.Select(s => s.Connection.ToDTO(s.IsConnect)));
			}
			catch (Exception ex)
			{

				return StatusCode(400, Messages.ErrGetItem_0_1.SetParameters(nameof(GetConnectionById), ex));
			}
		}
		// POST: api/connections
		[HttpPost]
		public async Task<ActionResult<Connection>> CreateConnection(ConnectionDTO connection)
		{
			try
			{
				var cnn = connection.FromDTO();
				await repository.InsertAsync(cnn);
				await cnnService.AppendAndStartConnection(cnn);
				return CreatedAtAction(nameof(CreateConnection), connection);
			}
			catch (Exception ex)
			{

				return StatusCode(400, Messages.ErrInsertItem_0_1.SetParameters(nameof(CreateConnection), ex));
			}
		}

		// PUT: api/connections/5
		[HttpPut("{Id}")]
		public async Task<IActionResult> UpdateConnection(ulong id, Connection connection)
		{
			try
			{
				if (id != connection.Id)
					return BadRequest();

				var existingConnection = await repository.GetByIdAsync(id);

				if (existingConnection == null)
					return NotFound();

				existingConnection.ConnType = connection.ConnType;
				existingConnection.ConnectionConfigs = connection.ConnectionConfigs;
				existingConnection.ConnectionParams = connection.ConnectionParams;
				await repository.UpdateAsync(existingConnection);

				return Ok(existingConnection);
			}
			catch (Exception ex)
			{

				return StatusCode(400, Messages.ErrUpdateItem_0_1.SetParameters(nameof(UpdateConnection), ex));
			}
		}

		[HttpPost]
		[Route("write-devices")]
		public async Task<IActionResult> WriteDevice(WriteDevicePayload message)
		{
			const string nameFunc = "WriteDevice";
			string errFunc = "";
			try
			{
				var conn = (cnnService.Connections[0] as EtherCAT);
				if (conn == null)
				{
					errFunc = "Error Connection NOT FOUND";
				}
				else
				{
					foreach (var env in message.Data)
					{
						try
						{
							await conn.WriteDevice(env.ParamName, env.Value);
						}
						catch (Exception ex)
						{
							errFunc += $"Error write {env.ParamName} cause: {ex.Message}\r\n";
						}
					}
				}
				if (!string.IsNullOrWhiteSpace(errFunc))
				{
					Message errMessage = Messages.ErrInputInvalid_0_1.SetParameters(nameFunc, errFunc);
					return this.ResponseError(errMessage);
				}
				return this.ResponseOk(message);
			}
			catch (Exception ex)
			{
				errFunc = ex.Message;
				Message errMessage = Messages.ErrInputInvalid_0_1.SetParameters(nameFunc, errFunc);
				return this.ResponseError(errMessage);
			}
		}
		[HttpDelete("{Id}")]
		public async Task<IActionResult> DeleteConnection(ulong id)
		{
			try
			{
				var existingConnection = await repository.GetByIdAsync(id);

				if (existingConnection == null)
					return NotFound();

				await repository.DeleteAsync(existingConnection);

				return NoContent();
			}
			catch (Exception ex)
			{
				return StatusCode(400, Messages.ErrDeleteItem_0_1.SetParameters(nameof(DeleteConnection), ex));
			}
		}
	}
}
