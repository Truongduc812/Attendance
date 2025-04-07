using Confluent.Kafka;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceBaseBE.Database.SQLScripts.Interface;

namespace SourceBaseBE.Database.SQLScripts
{
  public class PostgreSQLScripts: ISQLScripts
  {
    private PostgreSQLScripts() { }
    private static PostgreSQLScripts _instance;
    public static PostgreSQLScripts GetInstance()
    {
      if (_instance == null)
      {
        _instance = new PostgreSQLScripts();
      }
      return _instance;
    }
    public string GetSQL_AlterColumnTraceData()
    {
      return this.AlterColumnTraceData;
    }

    public string GetSQL_CreateTableTraceData()
    {
      return this.CreateTableTraceData;
    }

    public string GetSQL_IsExistsColumn()
    {
      return this.IsExistsColumn;
    }

    public string GetSQL_IsExistsTable()
    {
      return this.IsExistsTable;
    }

    public string IsExistsTable = @"SELECT table_name FROM information_schema.tables WHERE table_name = '@tableName';";
    public string CreateTableTraceData = @"
    DO
    $$
    BEGIN
	    CREATE TABLE IF NOT EXISTS ""@tableName"" (
	      ""Id"" BIGSERIAL PRIMARY KEY,
	      ""MessageId"" VARCHAR(27) NULL,
	      ""ConnectionId"" BIGINT NULL,
	      ""ExecuteAt"" TIMESTAMP NULL,
	      ""CreatedAt"" TIMESTAMP NULL,
	      @fields,
	      CONSTRAINT ""@tableName_MessageId"" UNIQUE (""MessageId"")
	    );
        CREATE INDEX IF NOT EXISTS ""index_@tableName_ExecuteAt"" ON ""@tableName"" (""ExecuteAt"");
    END
    $$;";
    public string IsExistsColumn = @"SELECT 1 FROM information_schema.columns WHERE column_name = '@columnName' AND table_name = '@tableName';";
    public string AlterColumnTraceData = @"ALTER TABLE ""@tableName"" ADD COLUMN ""@columnName"" @dataType;";

  }
}