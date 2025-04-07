using iSoft.Common.Enums.DBProvider;

namespace SourceBaseBE.Database.SQLScripts.Interface
{
  public class SQLScripts
  {
    public static ISQLScripts GetSQLScriptInstance(EnumDBProvider databaseType)
    {
      switch (databaseType)
      {
        case EnumDBProvider.SqlServer:
          return SQLServerScripts.GetInstance();
        case EnumDBProvider.Postgres:
          return PostgreSQLScripts.GetInstance();
        default:
          throw new NotImplementedException("[GetSQLScriptInstance]");
      }
    }
  }
}