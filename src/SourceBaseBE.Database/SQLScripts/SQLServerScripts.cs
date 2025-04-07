using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceBaseBE.Database.SQLScripts.Interface;

namespace SourceBaseBE.Database.SQLScripts
{
  public class SQLServerScripts : ISQLScripts
  {
    private SQLServerScripts() { }
    private static SQLServerScripts _instance;
    public static SQLServerScripts GetInstance()
    {
      if (_instance == null)
      {
        _instance = new SQLServerScripts();
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
    public string IsExistsTable = @"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '@tableName'";
    public string CreateTableTraceData = @"
      IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '@tableName')
      BEGIN
        CREATE TABLE @tableName (
	        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
	        MessageId NVARCHAR(27) NULL,
	        ConnectionId BIGINT NULL,
	        ExecuteAt DATETIME2(7) NULL,
	        CreatedAt DATETIME2(7) NULL,
          @fields,
          CONSTRAINT @tableName_MessageId UNIQUE (MessageId)
        )
        CREATE INDEX index_@tableName_ExecuteAt ON @tableName (ExecuteAt)
      END;";
    public string IsExistsColumn = @"SELECT 1 FROM sys.columns WHERE Name = N'@columnName' AND Object_ID = Object_ID(N'@tableName')";
    public string AlterColumnTraceData = @"ALTER TABLE @tableName ADD @columnName @dataType NULL;";
  }

  
}