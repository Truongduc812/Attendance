using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using iSoft.DBLibrary.Entities;

namespace iSoft.TrackDeviceService.Models
{
	public class Store : BaseEntity
	{
		public ulong ConnectionId { get; set; }
		public object Data { get; set; }
		public Queue<object> QueueDatas { get; set; }
	}
}
