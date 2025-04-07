namespace iSoft.Common.Models.ConfigModel.Subs
{
    public class ApiKeyModel
    {
        public string Name { get; set; }
        public string ApiKey { get; set; }
        public string JsonFilePath { get; set; }
        public LimitModel[] AccessLimits { get; set; }
        public bool? IsEnable { get; set; }
    }
}