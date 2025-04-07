using System.ComponentModel;

namespace SourceBaseBE.Database.Enums
{
  public enum EnumEntryReportAction
  {
    [Description("INITIALIZE")]
    INITIALIZE,

    [Description("ERROR")]
    ERROR,

    //* Approved
    [Description("MN_APPROVE")]
    MN_APPROVE,
    
    //* Rejected
    [Description("MN_REJECT")]
    MN_REJECT,
    
    //* Canceled
    [Description("SE_CANCEL")]
    SE_CANCEL,
    [Description("MN_CANCEL")]
    MN_CANCEL,

    //* Restored
    [Description("SE_RESTORE")]
    ME_RESTORE, 
    [Description("MN_RESTORE")]
    MN_RESTORE
  }
}
