using iSoft.Common.Enums.DBProvider;
using iSoft.DBLibrary.SQLBuilder.Interfaces;

namespace iSoft.DBLibrary.SQLBuilder
{
  public class SQLServerSQLBuilder : BaseSQLBuilder
  {
    private static SQLServerSQLBuilder instance = null;
    public static SQLServerSQLBuilder GetInstance()
    {
      if (instance == null)
      {
        instance = new SQLServerSQLBuilder();
        instance.databaseType = EnumDBProvider.SqlServer;
      }
      return instance;
    }
    public override ISQLBuilder WhereILike(FieldName fieldName, string value)
    {
      return this.WhereLike(fieldName, value);
    }

    public override ISQLBuilder Offset(int value)
    {
      if (value < 0)
      {
        return this;
      }

      int paramIndex = parameterIndex + 1;
      offset = string.Format("\n OFFSET @param{0} ROWS ", paramIndex);
      this.dicParameters.Add(string.Format("param{0}", paramIndex), value);
      parameterIndex += 1;
      return this;
    }
    public override ISQLBuilder Limit(int value)
    {
      if (value < 0)
      {
        return this;
      }

      int paramIndex = parameterIndex + 1;
      limit = string.Format("\n FETCH NEXT @param{0} ROWS ONLY ", paramIndex);
      this.dicParameters.Add(string.Format("param{0}", paramIndex++), value);
      parameterIndex += 1;
      return this;
    }

  }
}
