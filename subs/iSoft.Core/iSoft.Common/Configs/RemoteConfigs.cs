using System.Threading.Tasks;
using Serilog;
using iSoft.Common.Util;
using iSoft.Common.Exceptions;
using iSoft.Common.Models.ConfigModel;

namespace iSoft.Common.ConfigsNS
{
  public class RemoteConfigs
  {
    private static RemoteConfigModel instance;

    public static RemoteConfigModel GetConfig()
    {
      if (instance == null)
      {
        Task.Run(async () =>
        {
          instance = await ConfigUtil.GetRemoteConfig((instance == null ? "0" : instance.Version), false);
        }).Wait();
      }
      if (instance == null)
      {
        throw new BaseException("GetRemoteConfig error");
      }
      return instance;
    }

    public static async Task RefreshConfig()
    {
      var newConfig = await ConfigUtil.GetRemoteConfig((instance == null ? "0" : instance.Version), true);
      if (newConfig != null)
      {
        instance = newConfig;
      }
    }
  }
}
