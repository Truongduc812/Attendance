using System.ComponentModel;

namespace SourceBaseBE.Database.Enums
{
  public enum EnumRegistrationAction
  {
    [Description("INITIALIZE")]
    INITIALIZE,
    [Description("ERROR")]
    ERROR,

    //* Approved
    [Description("SE_APPROVE")]
    SE_APPROVE, 
    [Description("MN_APPROVE")]
    MN_APPROVE,
    
    //* Rejected
    [Description("SE_REJECT")]
    SE_REJECT,
    [Description("MN_REJECT")]
    MN_REJECT,
    
    //* Waiting
    [Description("SE_REPORT")]
    SE_REPORT,
    
    //* Canceled
    [Description("ME_CANCEL")]
    ME_CANCEL, 
    [Description("OW_CANCEL")]
    OW_CANCEL, 
    [Description("MN_CANCEL")]
    MN_CANCEL,

    //* New
    [Description("ME_REGISTER")]
    ME_REGISTER, 
    [Description("OW_REGISTER")]
    OW_REGISTER,

    //* Restored
    [Description("ME_RESTORE")]
    ME_RESTORE, 
    [Description("OW_RESTORE")]
    OW_RESTORE, 
    [Description("MN_RESTORE")]
    MN_RESTORE
  }
}
