using iSoft.Common.Models.ConfigModel.Subs;
using System.ComponentModel.DataAnnotations;

namespace iSoft.Common.Models.ConfigModel
{
  public class SourceBaseBEConfigModel : BaseConfigModel
  {
    public string ENV { get; set; }
    public string AuthenticationSecret { get; set; }
    public DBServerConfigModel MasterDatabaseConfig { get; set; }
    public DBServerConfigModel TraceDatabaseConfig { get; set; }
    public TrackDeviceConfig TrackDeviceConfig { get; set; }
    public ServerConfigModel RabbitMQConfig { get; set; }
    public ServerConfigModel ElasticSearchConfig { get; set; }
    public ServerConfigModel RedisConfig { get; set; }
    public ServerConfigModel SocketIOConfig { get; set; }
    public ServerConfigModel RemoteConfig { get; set; }
    public string RemoteConfigAPIKey { get; set; }
    public InfluxDBServerConfigModel InfluxDBConfig { get; set; }
    public FCMConfigModel FCMConfig { get; set; }
    public object GetLogStr()
    {
      return $"Version: {Version}, " +
        $"[ENV: {ENV}], " +
        $"[AuthenticationSecret: {AuthenticationSecret?.Substring(0, 4) + "******"}], " +
        $"[MasterDatabaseConfig: {MasterDatabaseConfig?.GetLogStr()}], " +
        $"[TraceDatabaseConfig: {TraceDatabaseConfig?.GetLogStr()}], " +
        //$"[TrackDeviceConfig: {TrackDeviceConfig?.GetLogStr()}], " +
        $"[RabbitMQConfig: {RabbitMQConfig?.GetLogStr()}], " +
        $"[ElasticSearchConfig: {ElasticSearchConfig?.GetLogStr()}]" +
        $"[RedisConfig: {RedisConfig?.GetLogStr()}], " +
        $"[SocketIOConfig: {SocketIOConfig?.GetLogStr()}], " +
        $"[RemoteConfig: {RemoteConfig?.GetLogStr()}], " +
        $"[RemoteConfigAPIKey: {RemoteConfigAPIKey?.Substring(0, 4) + "******"}], " +
        $"[InfluxDBConfig: {InfluxDBConfig?.GetLogStr()}], " +
        $"[FCMConfig: {FCMConfig?.GetLogStr()}]";
    }
  }
}