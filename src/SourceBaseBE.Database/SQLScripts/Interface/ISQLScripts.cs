using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.SQLScripts.Interface
{
  public interface ISQLScripts
  {
    public string GetSQL_IsExistsTable();
    public string GetSQL_CreateTableTraceData();
    public string GetSQL_IsExistsColumn();
    public string GetSQL_AlterColumnTraceData();
  }
}