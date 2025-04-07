using System;

namespace iSoft.TrackDeviceService.Services
{
	public class BaseService : IDisposable
	{
		public void Dispose()
		{
			// Suppress finalization.
			GC.SuppressFinalize(this);
		}
	}
}
