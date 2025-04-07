using Newtonsoft.Json;

namespace iSoft.Common.ExtensionMethods
{
  public static class JsonExtensionUtil
  {
    public static T FromJson<T>(string json) => JsonConvert.DeserializeObject<T>(json, Converter.Settings);
    public static string ToJson<T>(this T self) => JsonConvert.SerializeObject(self, Converter.Settings);
  }
  public static class Converter
  {
    private static readonly JsonSerializerSettings settings;

    static Converter()
    {
      settings = new JsonSerializerSettings
      {
        DateFormatString = "yyyy-MM-ddTHH:mm:ss.fff",
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore, // Ignore null values to reduce size
        //DefaultValueHandling = DefaultValueHandling.Ignore, // Ignore default values to reduce size
      };
    }

    public static JsonSerializerSettings Settings => settings;
  }
}