using iSoft.Common.Enums.DBProvider;

namespace iSoft.Common.Models.ConfigModel.Subs
{
  public class DBServerConfigModel: ServerConfigModel
  {
    public EnumDBProvider DatabaseType { get; set; }
    public string DatabaseName { get; set; }

    public DBServerConfigModel(EnumDBProvider databaseType, string address, int port, string databaseName, string username, string password)
      :base(address, port, username, password)
    {
      DatabaseType = databaseType;
      DatabaseName = databaseName;
    }

    public override bool Equals(object? obj)
    {
      return base.Equals(obj) 
        && this.DatabaseType == ((DBServerConfigModel)obj).DatabaseType
        && this.DatabaseName == ((DBServerConfigModel)obj).DatabaseName;
    }

    public override object GetLogStr()
    {
      return $"{base.GetLogStr()}, DatabaseType: {DatabaseType}, DatabaseName: {DatabaseName}";
    }
  }
}