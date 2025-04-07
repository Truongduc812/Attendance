using iSoft.Common.Util;
using iSoft.Common.Exceptions;
using iSoft.Common.Models.ConfigModel;

namespace iSoft.Common.ConfigsNS
{
  public class CommonConfig
  {
    private static SourceBaseBEConfigModel instance;

    public static SourceBaseBEConfigModel GetConfig()
    {
      try
      {
        if (instance == null)
        {
          var configRS = new SourceBaseBEConfigModel();
          ConfigUtil.FillEnvKey(configRS);
          if (!string.IsNullOrEmpty(configRS.Version))
          {
            instance = configRS;
          }
          else
          {
            throw new BaseException("Get config from ENV error, Version is empty");
          }
        }
        if (instance == null)
        {
          throw new BaseException("Get config from ENV error");
        }
        return instance;
      }
      catch(Exception ex)
      {
        throw new BaseException("Get config from ENV error", ex);
      }
    }
  }
}
