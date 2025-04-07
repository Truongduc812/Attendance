using ConnectionCommon.Connection;
using iSoft.DBLibrary.Entities;
using Newtonsoft.Json;
using System;
using System.Linq;
using UDF.TrackDeviceService.Models.DTOs;

namespace iSoft.TrackDeviceService.Api.Utils
{
	public static class Extension
	{

		public static ConfigDTO ToDTO(this Config self)
		{
			return new ConfigDTO(
				self.Id,
				self.Name, self.isEnable.GetValueOrDefault(),
				self.isDelete.GetValueOrDefault(),
				self.Type);
		}
		public static Config FromDTO(this ConfigDTO self)
		{
			return new Config
			{
				Id = self.id != null ? self.id.Value : 0,
				Name = self.name,
				Type = self.type,
				isEnable = self.isEnable,
				isDelete = self.isDelete,
			};
		}
		public static ConnectionDTO ToDTO(this Connection self, bool isConnected)
		{
			var paramDTO = self.ConnectionParams?.Select(s => s.ToDTO(self, s.Param)).ToArray();
			var configDTO = self.ConnectionConfigs?.Select(s => s.ToDTO()).ToArray();
			return new ConnectionDTO(self.Id, self.DisplayName, paramDTO, configDTO, isConnected, self.isEnable.GetValueOrDefault(), self.isDelete.GetValueOrDefault());
		}
		public static Connection FromDTO(this ConnectionDTO self)
		{
			return new Connection
			{
				Id = self.id != null ? self.id.Value : 0,
				ConnType = Enum.Parse<eConnType>(self.name),
				ConnectionParams = self.paras.Select(s => s.FromDTO()).ToList(),
				ConnectionConfigs = self.configs.Select(s => s.FromDTO()).ToList(),
				isEnable = self.isEnable,
				isDelete = self.isDelete,
			};
		}
		public static ParamDTO ToDTO(this Param self)
		{
			return new ParamDTO(self.Id,
				self.Name,
				self.isEnable.GetValueOrDefault(),
				self.isDelete.GetValueOrDefault(),
				self.Type);
		}
		public static Param FromDTO(this ParamDTO self)
		{
			return new Param
			{
				Id = self.id != null ? self.id.Value : 0,
				Name = self.name,
				Type = self.type,
				isEnable = self.isEnable,
				isDelete = self.isDelete,
			};
		}
		public static ConnectionConfigDTO ToDTO(this ConnectionConfig self)
		{
			return new ConnectionConfigDTO(
				self.Id,
				self.Value,
				self.ConnectionId,
				self.ConfigId,
				self.isEnable.GetValueOrDefault(),
				self.isDelete.GetValueOrDefault()
				);
		}
		public static ConnectionConfig FromDTO(this ConnectionConfigDTO self)
		{
			return new ConnectionConfig
			{
				Id = self.id != null ? self.id.Value : 0,
				ConfigId = self.configId,
				ConnectionId = self.connId,
				Value = self.value,
				isEnable = self.isEnable,
				isDelete = self.isDelete,
			};
		}
		public static ConnectionParamDTO ToDTO(this ConnectionParam self, Connection conn, Param param)
		{
			var a = self.isEnable.GetValueOrDefault();
			return new ConnectionParamDTO(
				self.Id,
				self.Value,
				conn.Id,
				param.Id,
				self.isEnable.GetValueOrDefault(),
				self.isDelete.GetValueOrDefault()
				);
		}
		public static ConnectionParam FromDTO(this ConnectionParamDTO self)
		{
			return new ConnectionParam
			{
				Id = self.id != null ? self.id.Value : 0,
				ParamId = self.paramId,
				ConnectionId = self.connId,
				Value = self.value,
				isEnable = self.isEnable,
				isDelete = self.isDelete,
			};
		}
	}
}
