using iSoft.Common.Models.ConfigModel.RemoteConfig.Subs;
using iSoft.Common.Models.ConfigModel.Subs;
using System.ComponentModel.DataAnnotations;

namespace iSoft.Common.Models.ConfigModel
{
  public class RemoteConfigModel : BaseConfigModel
  {
    public string ENV { get; set; }
    public SyncDataConfigModel SyncDataConfig { get; set; }
    public object GetLogStr()
    {
      return $"Version: {Version}, " +
        $"[ENV: {ENV}], " +
        $"[SyncDataConfig: {SyncDataConfig?.GetLogStr()}], ";
    }
  }
}