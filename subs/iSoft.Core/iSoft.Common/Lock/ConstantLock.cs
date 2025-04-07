using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.DBLibrary
{
  public class ConstantLock
  {
    public static string lockExecuteSQL = "ExecuteSQL";
    public static string lockDataSwitchingKeyPrefix = "DataSwitchingService_";
    public const string lockTraceDataKeyPrefix = "TraceDataService_";
    public static string lockSearchDataService2KeyPrefix = "SearchDataService2_";
    public static string lockWriteInput = "lockWriteInput";
    public static string lockSaveESLastData = "lockSaveESLastData";
    public static string lockSaveTraceLastData = "lockSaveTraceLastData";
  }
}
