using iSoft.Common.Enums;
using iSoft.Common.Models.ConfigModel.Subs;
using iSoft.ConnectionCommon.MessageBroker;
using iSoft.ConnectionCommon.Model;
using iSoft.DBLibrary.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;


namespace ConnectionCommon.Connection
{
    public enum eConnType
	{
		ADS,
		ModbusTCP,
		ModbusRTU,
		MCProtocol,
		SLMP,
		Excel,
		TCP_IP,
		Rabbit,
		Kafka,
		OPC_UA
	}
	public interface IConnection : IDisposable
	{
		public bool IsConnect { get; set; }
		public Connection Connection { get; set; }
		#region Methods
		public abstract Task SendAsync<T>(T data) where T : class;
		public abstract Task Init(Connection connection, ILoggerFactory loggerFactory, ServerConfigModel rabbitMQConfig);
		public abstract Task<int[]> ReadAsync();
		public abstract Task<T> ReadAsync<T>() where T : class;
		public abstract Task<int[]> ReadDevice(string deviceId, int quantity = 1);
		public abstract Task<T> ReadDevice<T>(string deviceId, int quantity = 1);
		public abstract Task StartRead(Func<BaseMessage, Task<bool>> callbackSuccess, Action<Exception> callbackException);
		public abstract Task StopRead();
		public abstract Task<bool> Connect();
		public abstract Task<bool> Disconnect();
		public abstract Task CheckConnectionLoop();

		public void Dispose()
		{
			throw new NotImplementedException();
		}
		#endregion
	}
	public class ReceivePayload
	{
		public Connection Sender { get; set; }
		public object Msg { get; set; }
		public static ReceivePayload CreateReceivePayload(Connection Sender, object Msg)
		{
			var payload = new ReceivePayload { Sender = Sender, Msg = Msg };
			return payload;
		}
	}
	public class Connection : BaseEntity
	{

		#region Properties
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public ulong Id { get; set; } = 0;
		[NotNull]
		public eConnType Name { get; set; }
		[NotNull]
		[NotMapped]
		public string DisplayName { get => this.Name.ToString(); }
		[JsonProperty("createAt")]
		[NotNull]

		public DateTime CreateAt { get; set; } = DateTime.Now;
		[JsonProperty("updateAt")]
		[NotNull]
		public DateTime UpdateAt { get; set; } = DateTime.Now;
		#endregion
		#region Relations
		public ICollection<ConnectionConfig>? ConnectionConfigs { get; set; }
		public ICollection<ConnectionParam>? ConnectionParams { get; set; }
		#endregion

	}
	public class ConnectionConfig : BaseEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public ulong Id { get; set; } = 0;
		[NotNull]
		public string Value { get; set; }
		[JsonProperty("createAt")]
		[NotNull]
		public DateTime CreateAt { get; set; } = DateTime.Now;
		[JsonProperty("updateAt")]
		[NotNull]
		public DateTime UpdateAt { get; set; } = DateTime.Now;
		public ulong? ConnectionId { get; set; }
		public Connection? Connection { get; set; }
		public ulong? ConfigId { get; set; }
		public Config? Config { get; set; }

	}
	public class Param : BaseEntity
	{
		[JsonIgnore]
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public ulong Id { get; set; } = 0;
		[NotNull]
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("type")]
		[NotNull]
		public string Type { get; set; }
		[DefaultValue(EnumReadWrite.ReadOnly)]
		public EnumReadWrite? ReadWrite { get; set; }
		[JsonProperty("createAt")]
		[NotNull]
		public DateTime CreateAt { get; set; } = DateTime.Now;
		[JsonProperty("updateAt")]
		[NotNull]
		public DateTime UpdateAt { get; set; } = DateTime.Now;
	}
	public class Config : BaseEntity
	{
		[JsonIgnore]
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public ulong Id { get; set; } = 0;
		[NotNull]
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("type")]
		[NotNull]
		public string Type { get; set; }
		[JsonProperty("createAt")]
		[NotNull]
		public DateTime CreateAt { get; set; } = DateTime.Now;
		[JsonProperty("updateAt")]
		[NotNull]
		public DateTime UpdateAt { get; set; } = DateTime.Now;
	}
	public class ConnectionParam : BaseEntity
	{
		[JsonIgnore]
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public ulong Id { get; set; } = 0;
		[JsonProperty("readinterval")]
		[NotNull]
		public int ReadInterval { get; set; }
		[NotMapped]
		public DateTime LastUpdate { get; set; } = DateTime.Now;
		[JsonProperty("value")]
		[NotNull]
		public string Value { get; set; }
		public ulong? ConnectionId { get; set; }
		public Connection? Connection { get; set; }
		public ulong? ParamId { get; set; }
		public Param? Param { get; set; }
		[JsonProperty("createAt")]
		[NotNull]
		public DateTime CreateAt { get; set; } = DateTime.Now;
		[JsonProperty("updateAt")]
		[NotNull]
		public DateTime UpdateAt { get; set; } = DateTime.Now;
	}
}