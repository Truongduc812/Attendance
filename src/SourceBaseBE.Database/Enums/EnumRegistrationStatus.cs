using System.ComponentModel;

namespace SourceBaseBE.Database.Enums
{
  public enum EnumRegistrationStatus
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
    [Description("NEW")]
    NEW,
    [Description("RESTORED")]
    RESTORED,
    [Description("ERROR")]
    ERROR,
  }
}
