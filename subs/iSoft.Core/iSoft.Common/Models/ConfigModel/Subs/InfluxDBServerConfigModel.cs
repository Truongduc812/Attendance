namespace iSoft.Common.Models.ConfigModel.Subs
{
  public class InfluxDBServerConfigModel : DBServerConfigModel
  {
    public string Organization {  get; set; }
    public string Token { get; set; }
    public InfluxDBServerConfigModel(string address, int port, string organization, string databaseName, string username, string password, string token)
    : base(Enums.DBProvider.EnumDBProvider.InfluxDB, address, port, databaseName, username, password)
    {
      Organization = organization;
      Token = token;
    }

    public override bool Equals(object? obj)
    {
      return base.Equals(obj)
        && this.Organization == ((InfluxDBServerConfigModel)obj).Organization
        && this.Token == ((InfluxDBServerConfigModel)obj).Token;
    }

    public override object GetLogStr()
    {
      return $"{base.GetLogStr()}, Organization: {Organization}, Token: {(Token.Length >= 2 ? Token.Substring(0, 2) : "")}*****";
    }
  }
}