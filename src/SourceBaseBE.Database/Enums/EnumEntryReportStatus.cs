using System.ComponentModel;

namespace SourceBaseBE.Database.Enums
{
  public enum EnumEntryReportStatus
  {
    [Description("INITIALIZED")]
    INITIALIZED,
    [Description("APPROVED")]
    APPROVED,
    [Description("REJECTED")]
    REJECTED,
    [Description("WAITING")]
    WAITING,
    [Description("CANCELED")]
    CANCELED,
    [Description("RESTORED")]
    RESTORED,
    [Description("ERROR")]
    ERROR,
  }
}
