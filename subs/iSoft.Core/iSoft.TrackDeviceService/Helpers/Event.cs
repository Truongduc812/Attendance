using iSoft.ConnectionCommon.Model;
using System.Threading.Tasks;

namespace iSoft.TrackDeviceService.Helpers
{
	public delegate Task StoreDataChange(ulong connId, iMagPayloadMessage data);
}
