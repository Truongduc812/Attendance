namespace SourceBaseBE.Database.Enums
{
    public enum EnumWorkingDayHighlight
    {
        None = 0,
        ShiftIdAbnormal = 1,
        OTHoursOver4H = 2,
        OTHoursOver4H_Moved = 3,
        OTHoursOver4H_Added = 4,
    }
    public enum EnumViewOTList
    {
        All,
        Recommend,
        Real
    }
}
