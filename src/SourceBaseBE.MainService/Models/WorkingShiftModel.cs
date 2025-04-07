using SourceBaseBE.Database.Models.SpecialModels;
using System.Collections.Generic;
using System;
using iSoft.Common.Enums;
using SourceBaseBE.Database.EnumProject;
using SourceBaseBE.Database.Entities;
using iSoft.Common;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Utils;
using System.Linq;
using SourceBaseBE.Database.Enums;
using Nest;
using SourceBaseBE.Database;

namespace SourceBaseBE.MainService.Models
{
    public class WorkingShiftModel
    {
        const int ConstTimePerDayInSeconds = 8 * 60 * 60;
        const int ConstMaxOTHoursPerDay = 4;
        const int ConstMaxHoursPerDay = 12;

        public WorkingDayEntity WorkingDayObj { get; set; }
        public List<TimeRangeModel> ListTimeRange = new List<TimeRangeModel>();
        public EnumShiftId ShiftId { get; set; }
        public EnumWorkingShiftType WorkingShiftType { get; set; }
        public EnumWorkingDayHighlight WorkingDayHighlight { get; set; }
        public string ReconnemdType { get; set; }
        public int Over4HHours { get; set; } = 0;
        public int OTHours { get; set; } = 0;
        public EnumOTHourType OTHourType { get; set; } = EnumOTHourType.None;

        public long ShiftNormalSubAM { get; set; }
        public long ShiftNormalSubPM { get; set; }
        public long ShiftNormalTotal { get; set; }
        public long ShiftNormalOTShift3Total { get; set; }
        public long ShiftNormalOTSubAM { get; set; }
        public long ShiftNormalOTSubPM { get; set; }
        public long ShiftNormalOT2Total { get; set; }
        public long ShiftNormalOT2SubAM { get; set; }
        public long ShiftNormalOT2SubPM { get; set; }
        private long DeviationNormal { get; set; }

        public long Shift1Total { get; set; }
        public long Shift2Total { get; set; }
        public long Shift3Total { get; set; }
        public long Shift3SubAM { get; set; }
        public long Shift3SubPM { get; set; }
        private long Deviation3 { get; set; }

        public WorkingShiftModel(WorkingDayEntity workingDay, List<TimeRangeModel> listTimeRange)
        {
            this.WorkingDayObj = workingDay;
            this.ListTimeRange = listTimeRange;
            //calculatorHours(this.ListTimeRange, EnumWorkingShiftType.Shift_8h_17h);
            //calculatorHours(this.ListTimeRange, EnumWorkingShiftType.Shift_6h_14h_22h);
            //CalculatorShiftId();
        }

        public void CalculatorShiftId()
        {
            if (this.ListTimeRange.Count <= 0)
            {
                this.ShiftId = EnumShiftId.Shift1;
                return;
            }
            DeviationNormal = this.ShiftNormalTotal + this.ShiftNormalOTShift3Total + this.ShiftNormalOT2Total - ConstTimePerDayInSeconds;
            Deviation3 = this.Shift1Total + this.Shift2Total + this.Shift3Total - ConstTimePerDayInSeconds;

            //if (WorkingShiftType == EnumWorkingShiftType.Shift_8h_17h)
            //{
            //    this.ShiftId = EnumShiftId.ShiftNormal;
            //}
            //else if (WorkingShiftType == EnumWorkingShiftType.Shift_6h_14h_22h)
            //{
            //    if (Shift1Total >= Shift2Total && Shift1Total >= Shift3Total)
            //    {
            //        this.ShiftId = EnumShiftId.Shift1;
            //    }
            //    else if (Shift2Total >= Shift1Total && Shift2Total >= Shift3Total)
            //    {
            //        this.ShiftId = EnumShiftId.Shift2;
            //    }
            //    else
            //    {
            //        this.ShiftId = EnumShiftId.NightShift;
            //    }
            //}
            //else
            //{
            if (isExactHour(EnumShiftId.ShiftNormal))
            {
                this.ShiftId = EnumShiftId.ShiftNormal;
                return;
            }
            else if (isExactHour(EnumShiftId.Shift1))
            {
                this.ShiftId = EnumShiftId.Shift1;
                return;
            }
            else if (isExactHour(EnumShiftId.Shift2))
            {
                this.ShiftId = EnumShiftId.Shift2;
                return;
            }
            else if (isExactHour(EnumShiftId.Shift3))
            {
                this.ShiftId = EnumShiftId.Shift3;
                return;
            }
            else
            {
                if (this.ListTimeRange.Count >= 1
                    && this.ListTimeRange[0].TimeRangeType != EnumTimeRangeType.IsNotCheckIn)
                {
                    TimeSpan startTime_6h = new TimeSpan(6, 0, 0);
                    TimeSpan startTime_8h = new TimeSpan(8, 0, 0);
                    TimeSpan startTime_14h = new TimeSpan(14, 0, 0);
                    TimeSpan startTime_22h = new TimeSpan(22, 0, 0);

                    if (ListTimeRange[0].StartTime.TimeOfDay <= startTime_6h)
                    {
                        this.ShiftId = EnumShiftId.Shift1;
                        return;
                    }
                    else if (ListTimeRange[0].StartTime.TimeOfDay > startTime_6h && ListTimeRange[0].StartTime.TimeOfDay <= startTime_8h)
                    {
                        this.ShiftId = EnumShiftId.ShiftNormal;
                        return;
                    }
                    else if (ListTimeRange[0].StartTime.TimeOfDay > startTime_14h.Add(TimeSpan.FromHours(-2)) && ListTimeRange[0].StartTime.TimeOfDay <= startTime_14h)
                    {
                        this.ShiftId = EnumShiftId.Shift2;
                        return;
                    }
                    else if (ListTimeRange[0].StartTime.TimeOfDay > startTime_22h.Add(TimeSpan.FromHours(-2)) && ListTimeRange[0].StartTime.TimeOfDay <= startTime_22h)
                    {
                        this.ShiftId = EnumShiftId.Shift3;
                        return;
                    }
                }

                if (ShiftNormalTotal > Shift1Total && ShiftNormalTotal > Shift2Total && ShiftNormalTotal > Shift3Total)
                {
                    this.ShiftId = EnumShiftId.ShiftNormal;
                    return;
                }
                else if (Shift1Total >= Shift2Total && Shift1Total >= Shift3Total)
                {
                    this.ShiftId = EnumShiftId.Shift1;
                    return;
                }
                else if (Shift2Total >= Shift1Total && Shift2Total >= Shift3Total)
                {
                    this.ShiftId = EnumShiftId.Shift2;
                    return;
                }
                else
                {
                    this.ShiftId = EnumShiftId.Shift3;
                    return;
                }
            }
            //}
        }

        private bool isExactHour(EnumShiftId shiftId)
        {
            var recordTime1 = this.ListTimeRange.Min(x => x.StartTime);
            var recordTime2 = this.ListTimeRange.Max(x => x.EndTime);
            DateTime startTime = new DateTime();
            DateTime endTime = new DateTime();
            switch (shiftId)
            {
                case EnumShiftId.ShiftNormal:
                    startTime = new DateTime(recordTime1.Year, recordTime1.Month, recordTime1.Day, 8, 0, 0, DateTimeKind.Local);
                    endTime = new DateTime(recordTime1.Year, recordTime1.Month, recordTime1.Day, 17, 0, 0, DateTimeKind.Local);
                    break;
                case EnumShiftId.Shift1:
                    startTime = new DateTime(recordTime1.Year, recordTime1.Month, recordTime1.Day, 6, 0, 0, DateTimeKind.Local);
                    endTime = new DateTime(recordTime1.Year, recordTime1.Month, recordTime1.Day, 14, 0, 0, DateTimeKind.Local);
                    break;
                case EnumShiftId.Shift2:
                    startTime = new DateTime(recordTime1.Year, recordTime1.Month, recordTime1.Day, 14, 0, 0, DateTimeKind.Local);
                    endTime = new DateTime(recordTime1.Year, recordTime1.Month, recordTime1.Day, 22, 0, 0, DateTimeKind.Local);
                    break;
                case EnumShiftId.Shift3:
                    startTime = new DateTime(recordTime1.Year, recordTime1.Month, recordTime1.Day, 22, 0, 0, DateTimeKind.Local);
                    var tomorow = recordTime1.AddDays(1);
                    endTime = new DateTime(tomorow.Year, tomorow.Month, tomorow.Day, 6, 0, 0, DateTimeKind.Local);
                    break;
            }

            if ((startTime.AddHours(-1) < recordTime1 && recordTime1 <= startTime)
                && (endTime <= recordTime2 && recordTime2 < endTime.AddHours(1)))
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            if (this.ShiftId == EnumShiftId.ShiftNormal)
            {
                return $"[{this.WorkingDayObj.WorkingDate}] {this.ShiftId} ({DateTimeUtil.GetHours(this.DeviationNormal)}): {DateTimeUtil.GetHours(this.ShiftNormalOTSubAM)}-{DateTimeUtil.GetHours(this.ShiftNormalSubAM)}-{DateTimeUtil.GetHours(this.ShiftNormalSubPM)}-{DateTimeUtil.GetHours(this.ShiftNormalOTSubPM)}";
            }
            else
            {
                return $"[{this.WorkingDayObj.WorkingDate}] {this.ShiftId} ({DateTimeUtil.GetHours(this.Deviation3)}): {DateTimeUtil.GetHours(this.Shift3SubAM)}-{DateTimeUtil.GetHours(this.Shift1Total)}-{DateTimeUtil.GetHours(this.Shift2Total)}-{DateTimeUtil.GetHours(this.Shift3SubPM)}";
            }
        }

        public void CalculatorHours()
        {
            calculatorHours(this.ListTimeRange, EnumWorkingShiftType.Shift_8h_17h);
            calculatorHours(this.ListTimeRange, EnumWorkingShiftType.Shift_6h_14h_22h);
            CalculatorShiftId();
        }
        public void CalculatorRecommendType(Dictionary<string, HolidayScheduleEntity> dicHolidayScheduleStartDate)
        {
            this.ReconnemdType = this.GetRecommendType(dicHolidayScheduleStartDate);
        }
        private void calculatorHours(List<TimeRangeModel> listTimeRange, EnumWorkingShiftType workingShiftType)
        {
            if (listTimeRange == null || listTimeRange.Count == 0)
            {
                return;
            }

            if (workingShiftType == EnumWorkingShiftType.Shift_8h_17h)
            {
                var workingDay = DateTimeUtil.GetStartDate(listTimeRange[0].StartTime);
                var shiftNormalStartAM = workingDay.AddHours(8);
                var shiftNormalEndAM = workingDay.AddHours(12);

                var shiftNormalStartPM = workingDay.AddHours(13);
                var shiftNormalEndPM = workingDay.AddHours(17);

                var shift3StartAM = workingDay.AddDays(1).AddHours(0);
                var shift3EndAM = workingDay.AddDays(1).AddHours(6);

                var shift3StartPM = workingDay.AddHours(22);
                var shift3EndPM = workingDay.AddHours(24);

                var shift2StartAM = workingDay.AddHours(6);
                var shift2EndAM = workingDay.AddHours(8);

                var shift2StartPM = workingDay.AddHours(17);
                var shift2EndPM = workingDay.AddHours(22);

                ShiftNormalSubAM = 0;
                ShiftNormalSubPM = 0;
                ShiftNormalTotal = 0;

                ShiftNormalOTSubAM = 0;
                ShiftNormalOTSubPM = 0;
                ShiftNormalOTShift3Total = 0;

                ShiftNormalOT2SubAM = 0;
                ShiftNormalOT2SubPM = 0;
                ShiftNormalOT2Total = 0;

                foreach (var timeRange in listTimeRange)
                {
                    ShiftNormalSubAM += CalculateOverlap(timeRange, shiftNormalStartAM.Ticks, shiftNormalEndAM.Ticks);
                    ShiftNormalSubPM += CalculateOverlap(timeRange, shiftNormalStartPM.Ticks, shiftNormalEndPM.Ticks);
                    ShiftNormalTotal = ShiftNormalSubAM + ShiftNormalSubPM;

                    ShiftNormalOTSubAM += CalculateOverlap(timeRange, shift3StartAM.Ticks, shift3EndAM.Ticks);
                    ShiftNormalOTSubPM += CalculateOverlap(timeRange, shift3StartPM.Ticks, shift3EndPM.Ticks);
                    ShiftNormalOTShift3Total = (ShiftNormalOTSubAM + ShiftNormalOTSubPM);

                    ShiftNormalOT2SubAM += CalculateOverlap(timeRange, shift2StartAM.Ticks, shift2EndAM.Ticks);
                    ShiftNormalOT2SubPM += CalculateOverlap(timeRange, shift2StartPM.Ticks, shift2EndPM.Ticks);
                    ShiftNormalOT2Total = (ShiftNormalOT2SubAM + ShiftNormalOT2SubPM);
                }
            }
            else if (workingShiftType == EnumWorkingShiftType.Shift_6h_14h_22h)
            {
                var workingDay = DateTimeUtil.GetStartDate(listTimeRange[0].StartTime);
                var shift1Start = workingDay.AddHours(6);
                var shift1End = workingDay.AddHours(14);

                var shift2Start = workingDay.AddHours(14);
                var shift2End = workingDay.AddHours(22);

                var shift3StartAM = workingDay.AddDays(1).AddHours(0);
                var shift3EndAM = workingDay.AddDays(1).AddHours(6);

                var shift3StartPM = workingDay.AddHours(22);
                var shift3EndPM = workingDay.AddHours(24);

                Shift1Total = 0;
                Shift2Total = 0;

                Shift3SubAM = 0;
                Shift3SubPM = 0;
                Shift3Total = 0;

                foreach (var timeRange in listTimeRange)
                {
                    Shift1Total += CalculateOverlap(timeRange, shift1Start.Ticks, shift1End.Ticks);
                    Shift2Total += CalculateOverlap(timeRange, shift2Start.Ticks, shift2End.Ticks);

                    Shift1Total += CalculateOverlap(timeRange, shift1Start.AddDays(1).Ticks, shift1End.AddDays(1).Ticks);
                    Shift2Total += CalculateOverlap(timeRange, shift2Start.AddDays(1).Ticks, shift2End.AddDays(1).Ticks);

                    Shift3SubAM += CalculateOverlap(timeRange, shift3StartAM.Ticks, shift3EndAM.Ticks);
                    Shift3SubPM += CalculateOverlap(timeRange, shift3StartPM.Ticks, shift3EndPM.Ticks);
                    Shift3Total = (Shift3SubAM + Shift3SubPM);
                }
            }
        }
        //private void calculatorHours(List<TimeRangeModel> ListTimeRange, EnumWorkingShiftType WorkingShiftType)
        //{
        //    if (ListTimeRange == null || ListTimeRange.Count == 0)
        //    {
        //        return;
        //    }

        //    if (WorkingShiftType == EnumWorkingShiftType.Shift_8h_17h)
        //    {
        //        var shiftNormalStartAM = new TimeSpan(8, 0, 0);
        //        var shiftNormalEndAM = new TimeSpan(12, 0, 0);

        //        var shiftNormalStartPM = new TimeSpan(13, 0, 0);
        //        var shiftNormalEndPM = new TimeSpan(17, 0, 0);

        //        var shift2StartAM = new TimeSpan(6, 0, 0);
        //        var shift2EndAM = new TimeSpan(8, 0, 0);

        //        var shift2StartPM = new TimeSpan(17, 0, 0);
        //        var shift2EndPM = new TimeSpan(22, 0, 0);

        //        var shift3StartAM = new TimeSpan(0, 0, 0);
        //        var shift3EndAM = new TimeSpan(6, 0, 0);

        //        var shift3StartPM = new TimeSpan(18, 0, 0);
        //        var shift3EndPM = new TimeSpan(0, 0, 0).Add(TimeSpan.FromDays(1));

        //        ShiftNormalSubAM = 0;
        //        ShiftNormalSubPM = 0;
        //        ShiftNormalTotal = 0;

        //        ShiftNormalOTSubAM = 0;
        //        ShiftNormalOTSubPM = 0;
        //        ShiftNormalOTShift3Total = 0;

        //        foreach (var timeRange in ListTimeRange)
        //        {
        //            ShiftNormalSubAM += CalculateOverlap(timeRange, shiftNormalStartAM, shiftNormalEndAM);
        //            ShiftNormalSubPM += CalculateOverlap(timeRange, shiftNormalStartPM, shiftNormalEndPM);
        //            ShiftNormalTotal = ShiftNormalSubAM + ShiftNormalSubPM;

        //            ShiftNormalOTSubAM += CalculateOverlap(timeRange, shift3StartAM, shift3EndAM);
        //            ShiftNormalOTSubPM += CalculateOverlap(timeRange, shift3StartPM, shift3EndPM);
        //            ShiftNormalOTShift3Total = (ShiftNormalOTSubAM + ShiftNormalOTSubPM);

        //            ShiftNormalOT2SubAM += CalculateOverlap(timeRange, shift2StartAM, shift2EndAM);
        //            ShiftNormalOT2SubPM += CalculateOverlap(timeRange, shift2StartPM, shift2EndPM);
        //            ShiftNormalOT2Total = (ShiftNormalOT2SubAM + ShiftNormalOT2SubPM);
        //        }
        //    }
        //    else if (WorkingShiftType == EnumWorkingShiftType.Shift_6h_14h_22h)
        //    {
        //        var shift1Start = new TimeSpan(6, 0, 0);
        //        var shift1End = new TimeSpan(14, 0, 0);

        //        var shift2Start = new TimeSpan(14, 0, 0);
        //        var shift2End = new TimeSpan(22, 0, 0);

        //        var shift3StartAM = new TimeSpan(0, 0, 0);
        //        var shift3EndAM = new TimeSpan(6, 0, 0);

        //        var shift3StartPM = new TimeSpan(22, 0, 0);
        //        var shift3EndPM = new TimeSpan(0, 0, 0).Add(TimeSpan.FromDays(1));

        //        Shift1Total = 0;
        //        Shift2Total = 0;

        //        Shift3SubAM = 0;
        //        Shift3SubPM = 0;
        //        Shift3Total = 0;

        //        foreach (var timeRange in ListTimeRange)
        //        {
        //            Shift1Total += CalculateOverlap(timeRange, shift1Start, shift1End);
        //            Shift2Total += CalculateOverlap(timeRange, shift2Start, shift2End);

        //            Shift3SubAM += CalculateOverlap(timeRange, shift3StartAM, shift3EndAM);
        //            Shift3SubPM += CalculateOverlap(timeRange, shift3StartPM, shift3EndPM);
        //            Shift3Total = (Shift3SubAM + Shift3SubPM);
        //        }
        //    }
        //}
        private static long CalculateOverlap(TimeRangeModel timeRange, long shiftStartTicks, long shiftEndTicks)
        {
            long rangeStartTicks = timeRange.StartTime.Ticks;
            long rangeEndTicks = timeRange.EndTime.Ticks;

            if (shiftStartTicks < shiftEndTicks)
            {
                if (rangeEndTicks <= shiftStartTicks || rangeStartTicks >= shiftEndTicks)
                {
                    return 0;
                }
                long overlapStartTicks = shiftStartTicks;
                if (rangeStartTicks > shiftStartTicks) // Thời gian thực bắt đầu sớm hơn giờ đầu ca
                {
                    overlapStartTicks = rangeStartTicks;
                }
                long overlapEndTicks = shiftEndTicks;
                if (rangeEndTicks < shiftEndTicks) // Thời gian thực kết thúc sớm hơn giờ kết thúc ca
                {
                    overlapEndTicks = rangeEndTicks;
                }
                return (overlapEndTicks - overlapStartTicks) / TimeSpan.TicksPerSecond;
            }

            return 0;
        }
        //private static long CalculateOverlap(TimeRangeModel timeRange, TimeSpan shiftStart, TimeSpan shiftEnd)
        //{
        //    var rangeStart = timeRange.StartTime.TimeOfDay;
        //    var rangeEnd = timeRange.EndTime.TimeOfDay;

        //    if (shiftStart < shiftEnd)
        //    {
        //        if (rangeEnd <= shiftStart || rangeStart >= shiftEnd)
        //        {
        //            return 0;
        //        }
        //        var overlapStart = shiftStart;
        //        if (rangeStart > shiftStart) // Thời gian thực bắt đầu sớm hơn giờ đầu ca
        //        {
        //            overlapStart = rangeStart;
        //        }
        //        var overlapEnd = shiftEnd;
        //        if (rangeEnd < shiftEnd) // Thời gian thực kết thúc sớm hơn giờ kết thúc ca
        //        {
        //            overlapEnd = rangeEnd;
        //        }
        //        return (long)(overlapEnd - overlapStart).TotalSeconds;
        //    }

        //    return 0;
        //}

        public bool IsEmptyHour()
        {
            if (this.Shift1Total == 0
                && this.Shift2Total == 0
                && this.Shift3Total == 0
                && this.ShiftNormalTotal == 0
                && this.ShiftNormalOTShift3Total == 0)
            {
                return true;
            }
            return false;
        }
        public long GetTimeDeviation()
        {
            if (this.ShiftId == EnumShiftId.ShiftNormal)
            {
                return this.DeviationNormal;
            }
            else
            {
                return this.Deviation3;
            }
        }
        public bool IsWeekenOrHoliday(Dictionary<string, HolidayScheduleEntity> dicHolidayScheduleStartDate)
        {
            DateTime checkDay = this.WorkingDayObj.WorkingDate.Value;
            string holidayKey = DateTimeUtil.GetDateTimeStr(checkDay, ConstDateTimeFormat.YYYYMMDD);

            var isWeekend = checkDay.DayOfWeek == DayOfWeek.Sunday || checkDay.DayOfWeek == DayOfWeek.Saturday;
            var isHoliday = dicHolidayScheduleStartDate.ContainsKey(holidayKey);

            return isWeekend || isHoliday;
        }
        public string GetRecommendType(Dictionary<string, HolidayScheduleEntity> dicHolidayScheduleStartDate)
        {
            string type = EnumWorkingDayType.Type_0;

            if (this.ShiftId != EnumShiftId.ShiftNormal)
            {
                if (IsWeekenOrHoliday(dicHolidayScheduleStartDate))
                {
                    type = EnumWorkingDayType.Type_0;
                    if (DateTimeUtil.GetHours(this.Shift1Total) == 0 && DateTimeUtil.GetHours(this.Shift2Total) == 0 && DateTimeUtil.GetHours(this.Shift3Total) == 0)
                    {
                        type = EnumWorkingDayType.Type_0;
                        return type;
                    }

                    if (DateTimeUtil.GetHours(this.Shift1Total) > 0 && DateTimeUtil.GetHours(this.Shift2Total) > 0 && DateTimeUtil.GetHours(this.Shift3Total) > 0)
                    {
                        this.WorkingDayHighlight = EnumWorkingDayHighlight.ShiftIdAbnormal;
                    }

                    if (this.ShiftId == EnumShiftId.Shift1 || this.ShiftId == EnumShiftId.Shift2)
                    {
                        int shift3Hours = DateTimeUtil.GetHours(Shift3Total);
                        if (shift3Hours >= 1)
                        {
                            if (shift3Hours > ConstMaxOTHoursPerDay)
                            {
                                this.WorkingDayHighlight = EnumWorkingDayHighlight.OTHoursOver4H;

                                this.Over4HHours = shift3Hours - ConstMaxOTHoursPerDay;
                                //shift3Hours = ConstMaxOTHoursPerDay;
                            }
                            this.OTHours = shift3Hours;
                            this.OTHourType = EnumOTHourType.WeekendOrHolidayAndShift3;

                            EnumWorkingDayHighlight workingDayHighlight1 = EnumWorkingDayHighlight.None;
                            type = getXC3(shift3Hours, ref workingDayHighlight1);
                            this.WorkingDayHighlight = workingDayHighlight1;
                            return type;
                        }
                        else
                        {
                            int shift1Hours = DateTimeUtil.GetHours(Shift1Total);
                            int shift2Hours = DateTimeUtil.GetHours(Shift2Total);

                            int shift12Hours = shift1Hours + shift2Hours;
                            if (shift12Hours >= 1)
                            {
                                if (shift12Hours > ConstMaxHoursPerDay)
                                {
                                    this.WorkingDayHighlight = EnumWorkingDayHighlight.OTHoursOver4H;

                                    this.Over4HHours = shift12Hours - ConstMaxHoursPerDay;
                                    //shift12Hours = ConstMaxHoursPerDay;
                                }
                                this.OTHours = shift12Hours;
                                this.OTHourType = EnumOTHourType.WeekendOrHoliday;

                                EnumWorkingDayHighlight workingDayHighlight1 = EnumWorkingDayHighlight.None;
                                type = getType_x(shift12Hours, ref workingDayHighlight1);
                                this.WorkingDayHighlight = workingDayHighlight1;
                                return type;
                            }
                        }
                    }
                }
                else
                {
                    type = EnumWorkingDayType.Type_0;
                    if (DateTimeUtil.GetHours(this.Shift1Total) == 0 && DateTimeUtil.GetHours(this.Shift2Total) == 0 && DateTimeUtil.GetHours(this.Shift3Total) == 0)
                    {
                        type = EnumWorkingDayType.Type_P;
                        return type;
                    }

                    if (DateTimeUtil.GetHours(this.Shift1Total) > 0 && DateTimeUtil.GetHours(this.Shift2Total) > 0 && DateTimeUtil.GetHours(this.Shift3Total) > 0)
                    {
                        this.WorkingDayHighlight = EnumWorkingDayHighlight.ShiftIdAbnormal;
                    }

                    if (this.ShiftId == EnumShiftId.Shift1 || this.ShiftId == EnumShiftId.Shift2)
                    {
                        int shift3Hours = DateTimeUtil.GetHours(Shift3Total);
                        if (shift3Hours >= 1)
                        {
                            if (shift3Hours > ConstMaxOTHoursPerDay)
                            {
                                this.WorkingDayHighlight = EnumWorkingDayHighlight.OTHoursOver4H;

                                this.Over4HHours = shift3Hours - ConstMaxOTHoursPerDay;
                                //shift3Hours = ConstMaxOTHoursPerDay;
                            }
                            this.OTHours = shift3Hours;
                            this.OTHourType = EnumOTHourType.NightShift;

                            EnumWorkingDayHighlight workingDayHighlight1 = EnumWorkingDayHighlight.None;
                            type = getXC3(shift3Hours, ref workingDayHighlight1);
                            this.WorkingDayHighlight = workingDayHighlight1;
                            return type;
                        }
                    }
                    int shiftxHours = 0;
                    if (this.ShiftId == EnumShiftId.Shift1)
                    {
                        shiftxHours = DateTimeUtil.GetHours(Shift2Total);
                    }
                    if (this.ShiftId == EnumShiftId.Shift2)
                    {
                        shiftxHours = DateTimeUtil.GetHours(Shift1Total);
                    }
                    if (shiftxHours >= 1)
                    {
                        if (shiftxHours > ConstMaxOTHoursPerDay)
                        {
                            this.WorkingDayHighlight = EnumWorkingDayHighlight.OTHoursOver4H;

                            this.Over4HHours = shiftxHours - ConstMaxOTHoursPerDay;
                            //shiftxHours = ConstMaxOTHoursPerDay;
                        }
                        this.OTHours = shiftxHours;
                        this.OTHourType = EnumOTHourType.Normal;

                        EnumWorkingDayHighlight workingDayHighlight1 = EnumWorkingDayHighlight.None;
                        type = getType_x(shiftxHours, ref workingDayHighlight1);
                        this.WorkingDayHighlight = workingDayHighlight1;
                        return type;
                    }
                }

                if (this.ShiftId == EnumShiftId.Shift3)
                {
                    int shift1Hours = DateTimeUtil.GetHours(Shift1Total);
                    int shift2Hours = DateTimeUtil.GetHours(Shift2Total);

                    int shift12Hours = shift1Hours + shift2Hours;
                    if (shift12Hours <= 0)
                    {
                        return EnumWorkingDayType.Type_C3;
                    }
                    else
                    {
                        if (shift12Hours > ConstMaxOTHoursPerDay)
                        {
                            this.WorkingDayHighlight = EnumWorkingDayHighlight.OTHoursOver4H;

                            this.Over4HHours = shift12Hours - ConstMaxOTHoursPerDay;
                            //shift12Hours = ConstMaxOTHoursPerDay;
                        }
                        this.OTHours = shift12Hours;
                        this.OTHourType = EnumOTHourType.Normal;

                        EnumWorkingDayHighlight workingDayHighlight1 = EnumWorkingDayHighlight.None;
                        type = getXPlushC3(shift12Hours, ref workingDayHighlight1);
                        this.WorkingDayHighlight = workingDayHighlight1;
                        return type;
                    }
                }
                return type;
            }
            else // this.ShiftId == EnumShiftId.ShiftNormal
            {
                if (IsWeekenOrHoliday(dicHolidayScheduleStartDate))
                {
                    type = EnumWorkingDayType.Type_0;
                    if (DateTimeUtil.GetHours(this.ShiftNormalTotal) == 0 && DateTimeUtil.GetHours(this.ShiftNormalOTShift3Total) == 0 && DateTimeUtil.GetHours(this.ShiftNormalOT2Total) == 0)
                    {
                        type = EnumWorkingDayType.Type_0;
                        return type;
                    }

                    if (DateTimeUtil.GetHours(this.ShiftNormalTotal) > 0 && DateTimeUtil.GetHours(this.ShiftNormalOTShift3Total) > 0 && DateTimeUtil.GetHours(this.ShiftNormalOT2Total) > 0)
                    {
                        this.WorkingDayHighlight = EnumWorkingDayHighlight.ShiftIdAbnormal;
                    }

                    int shiftOTShift3Hour = DateTimeUtil.GetHours(ShiftNormalOTShift3Total);
                    if (shiftOTShift3Hour >= 1)
                    {
                        if (shiftOTShift3Hour > ConstMaxOTHoursPerDay)
                        {
                            this.WorkingDayHighlight = EnumWorkingDayHighlight.OTHoursOver4H;

                            this.Over4HHours = shiftOTShift3Hour - ConstMaxOTHoursPerDay;
                            //shiftOTShift3Hour = ConstMaxOTHoursPerDay;
                        }
                        this.OTHours = shiftOTShift3Hour;
                        this.OTHourType = EnumOTHourType.WeekendOrHolidayAndShift3;

                        EnumWorkingDayHighlight workingDayHighlight1 = EnumWorkingDayHighlight.None;
                        type = getXC3(shiftOTShift3Hour, ref workingDayHighlight1);
                        this.WorkingDayHighlight = workingDayHighlight1;
                        return type;
                    }
                    else
                    {
                        int shift1Hours = DateTimeUtil.GetHours(ShiftNormalSubAM);
                        int shift2Hours = DateTimeUtil.GetHours(ShiftNormalSubPM);
                        int shiftNormalOT2SubAMHours = DateTimeUtil.GetHours(ShiftNormalOT2SubAM);
                        int shiftNormalOT2SubPMHours = DateTimeUtil.GetHours(ShiftNormalOT2SubPM);

                        int shiftNormalHours = shift1Hours + shift2Hours + shiftNormalOT2SubAMHours + shiftNormalOT2SubPMHours;
                        if (shiftNormalHours >= 1)
                        {
                            if (shiftNormalHours > ConstMaxHoursPerDay)
                            {
                                this.WorkingDayHighlight = EnumWorkingDayHighlight.OTHoursOver4H;

                                this.Over4HHours = shiftNormalHours - ConstMaxHoursPerDay;
                                //shiftNormalHours = ConstMaxHoursPerDay;
                            }
                            this.OTHours = shiftNormalHours;
                            this.OTHourType = EnumOTHourType.WeekendOrHoliday;

                            EnumWorkingDayHighlight workingDayHighlight1 = EnumWorkingDayHighlight.None;
                            type = getType_x(shiftNormalHours, ref workingDayHighlight1);
                            this.WorkingDayHighlight = workingDayHighlight1;
                            return type;
                        }
                    }
                }
                else
                {
                    type = EnumWorkingDayType.Type_0;
                    if (DateTimeUtil.GetHours(this.ShiftNormalTotal) == 0 && DateTimeUtil.GetHours(this.ShiftNormalOTShift3Total) == 0 && DateTimeUtil.GetHours(this.ShiftNormalOT2Total) == 0)
                    {
                        type = EnumWorkingDayType.Type_P;
                        return type;
                    }

                    if (DateTimeUtil.GetHours(this.ShiftNormalTotal) > 0 && DateTimeUtil.GetHours(this.ShiftNormalOTShift3Total) > 0 && DateTimeUtil.GetHours(this.ShiftNormalOT2Total) > 0)
                    {
                        this.WorkingDayHighlight = EnumWorkingDayHighlight.ShiftIdAbnormal;
                    }

                    int shiftOTShift3Hour = DateTimeUtil.GetHours(this.ShiftNormalOTShift3Total);
                    if (shiftOTShift3Hour >= 1)
                    {
                        if (shiftOTShift3Hour > ConstMaxOTHoursPerDay)
                        {
                            this.WorkingDayHighlight = EnumWorkingDayHighlight.OTHoursOver4H;

                            this.Over4HHours = shiftOTShift3Hour - ConstMaxOTHoursPerDay;
                            //shiftOTShift3Hour = ConstMaxOTHoursPerDay;
                        }
                        this.OTHours = shiftOTShift3Hour;
                        this.OTHourType = EnumOTHourType.NightShift;

                        EnumWorkingDayHighlight workingDayHighlight1 = EnumWorkingDayHighlight.None;
                        type = getXC3(shiftOTShift3Hour, ref workingDayHighlight1);
                        this.WorkingDayHighlight = workingDayHighlight1;
                        return type;
                    }
                    int shiftNormalOT2SubAMHours = DateTimeUtil.GetHours(ShiftNormalOT2SubAM);
                    int shiftNormalOT2SubPMHours = DateTimeUtil.GetHours(ShiftNormalOT2SubPM);

                    int shiftNormalHours = shiftNormalOT2SubAMHours + shiftNormalOT2SubPMHours;
                    if (shiftNormalHours >= 1)
                    {
                        if (shiftNormalHours > ConstMaxOTHoursPerDay)
                        {
                            this.WorkingDayHighlight = EnumWorkingDayHighlight.OTHoursOver4H;

                            this.Over4HHours = shiftNormalHours - ConstMaxOTHoursPerDay;
                            //shiftNormalHours = ConstMaxOTHoursPerDay;
                        }
                        this.OTHours = shiftNormalHours;
                        this.OTHourType = EnumOTHourType.Normal;

                        EnumWorkingDayHighlight workingDayHighlight1 = EnumWorkingDayHighlight.None;
                        type = getType_x(shiftNormalHours, ref workingDayHighlight1);
                        this.WorkingDayHighlight = workingDayHighlight1;
                        return type;
                    }
                }

                return type;
            }
        }

        private string getXC3(int shiftOTHour, ref EnumWorkingDayHighlight workingDayHighlight1)
        {
            string type = "";
            switch (shiftOTHour)
            {
                case 1:
                    type = EnumWorkingDayType.Type_1C3;
                    break;
                case 2:
                    type = EnumWorkingDayType.Type_2C3;
                    break;
                case 3:
                    type = EnumWorkingDayType.Type_3C3;
                    break;
                case 4:
                    type = EnumWorkingDayType.Type_4C3;
                    break;
                case 5:
                    type = EnumWorkingDayType.Type_5C3;
                    break;
                case 6:
                    type = EnumWorkingDayType.Type_6C3;
                    break;
                case 7:
                    type = EnumWorkingDayType.Type_7C3;
                    break;
                case 8:
                    type = EnumWorkingDayType.Type_8C3;
                    break;
                case 9:
                    type = EnumWorkingDayType.Type_9C3;
                    break;
                case 10:
                    type = EnumWorkingDayType.Type_10C3;
                    break;
                case 11:
                    type = EnumWorkingDayType.Type_11C3;
                    break;
                case 12:
                    type = EnumWorkingDayType.Type_12C3;
                    break;
                default:
                    type = EnumWorkingDayType.Type_NULL;
                    workingDayHighlight1 = EnumWorkingDayHighlight.ShiftIdAbnormal;
                    break;
                    //throw new NotImplementedException($"[getXC3] shiftOTShift3Hour not found: {shiftOTHour}");
            }
            if (shiftOTHour > 12)
            {
                type = EnumWorkingDayType.Type_12C3;
                workingDayHighlight1 = EnumWorkingDayHighlight.OTHoursOver4H;
            }
            return type;
        }

        private string getType_x(int shiftOTHour, ref EnumWorkingDayHighlight workingDayHighlight1)
        {
            string type = "";
            switch (shiftOTHour)
            {
                case 1:
                    type = EnumWorkingDayType.Type_1;
                    break;
                case 2:
                    type = EnumWorkingDayType.Type_2;
                    break;
                case 3:
                    type = EnumWorkingDayType.Type_3;
                    break;
                case 4:
                    type = EnumWorkingDayType.Type_4;
                    break;
                case 5:
                    type = EnumWorkingDayType.Type_5;
                    break;
                case 6:
                    type = EnumWorkingDayType.Type_6;
                    break;
                case 7:
                    type = EnumWorkingDayType.Type_7;
                    break;
                case 8:
                    type = EnumWorkingDayType.Type_8;
                    break;
                case 9:
                    type = EnumWorkingDayType.Type_9;
                    break;
                case 10:
                    type = EnumWorkingDayType.Type_10;
                    break;
                case 11:
                    type = EnumWorkingDayType.Type_11;
                    break;
                case 12:
                    type = EnumWorkingDayType.Type_12;
                    break;
                default:
                    type = EnumWorkingDayType.Type_NULL;
                    workingDayHighlight1 = EnumWorkingDayHighlight.ShiftIdAbnormal;
                    break;
                    //throw new NotImplementedException($"[getType_x] shiftOTShift3Hour not found: {shiftOTHour}");
            }
            if (shiftOTHour > 12)
            {
                type = EnumWorkingDayType.Type_12;
                workingDayHighlight1 = EnumWorkingDayHighlight.OTHoursOver4H;
            }
            return type;
        }

        private string getXPlushC3(int shiftOTHour, ref EnumWorkingDayHighlight workingDayHighlight1)
        {
            string type = "";
            switch (shiftOTHour)
            {
                case 1:
                    type = EnumWorkingDayType.Type_1PlusC3;
                    break;
                case 2:
                    type = EnumWorkingDayType.Type_2PlusC3;
                    break;
                case 3:
                    type = EnumWorkingDayType.Type_3PlusC3;
                    break;
                case 4:
                    type = EnumWorkingDayType.Type_4PlusC3;
                    break;
                case 5:
                    type = EnumWorkingDayType.Type_5PlusC3;
                    break;
                case 6:
                    type = EnumWorkingDayType.Type_6PlusC3;
                    break;
                case 7:
                    type = EnumWorkingDayType.Type_7PlusC3;
                    break;
                case 8:
                    type = EnumWorkingDayType.Type_8PlusC3;
                    break;
                case 9:
                    type = EnumWorkingDayType.Type_9PlusC3;
                    break;
                case 10:
                    type = EnumWorkingDayType.Type_10PlusC3;
                    break;
                case 11:
                    type = EnumWorkingDayType.Type_11PlusC3;
                    break;
                case 12:
                    type = EnumWorkingDayType.Type_12PlusC3;
                    break;
                default:
                    type = EnumWorkingDayType.Type_NULL;
                    workingDayHighlight1 = EnumWorkingDayHighlight.ShiftIdAbnormal;
                    break;
                    //throw new NotImplementedException($"[getXPlushC3] shiftOTShift3Hour not found: {shiftOTHour}");
            }
            if (shiftOTHour > 12)
            {
                type = EnumWorkingDayType.Type_12PlusC3;
                workingDayHighlight1 = EnumWorkingDayHighlight.OTHoursOver4H;
            }
            return type;
        }
        internal static int IsOTType(string wktype)
        {
            return (wktype != EnumWorkingDayType.Type_0
                && wktype != EnumWorkingDayType.Type_C3
                && wktype != EnumWorkingDayType.Type_C3PlusM
                && wktype != EnumWorkingDayType.Type_P
                && wktype != EnumWorkingDayType.Type_PDivide2
                && wktype != EnumWorkingDayType.Type_S
                    && wktype != EnumWorkingDayType.Type_S75
                    && wktype != EnumWorkingDayType.Type_SW
                     && wktype != EnumWorkingDayType.Type_UP
                     && wktype != EnumWorkingDayType.Type_MR)
                     ? 1
                     : 0;
        }

        internal static bool IsLeaveFullShiftType(string wktype)
        {
            return (wktype == EnumWorkingDayType.Type_P
                 || wktype == EnumWorkingDayType.Type_S
                 || wktype == EnumWorkingDayType.Type_S75
                 || wktype == EnumWorkingDayType.Type_SW
                 || wktype == EnumWorkingDayType.Type_UP
                 || wktype == EnumWorkingDayType.Type_MR)
                 ? true
                 : false;
        }
        internal static bool IsLeaveHaftShiftType(string wktype)
        {
            return (wktype == EnumWorkingDayType.Type_PDivide2)
                 ? true
                 : false;
        }
        internal static bool IsOTTypeWithRegularShift(string wktype)
        {
            return (wktype == EnumWorkingDayType.Type_1C3
                || wktype == EnumWorkingDayType.Type_2C3
                || wktype == EnumWorkingDayType.Type_3C3
                || wktype == EnumWorkingDayType.Type_4C3
                || wktype == EnumWorkingDayType.Type_5C3
                || wktype == EnumWorkingDayType.Type_6C3
                || wktype == EnumWorkingDayType.Type_7C3
                || wktype == EnumWorkingDayType.Type_8C3
                || wktype == EnumWorkingDayType.Type_1PlusC3
                || wktype == EnumWorkingDayType.Type_2PlusC3
                || wktype == EnumWorkingDayType.Type_3PlusC3
                || wktype == EnumWorkingDayType.Type_4PlusC3
                || wktype == EnumWorkingDayType.Type_5PlusC3
                || wktype == EnumWorkingDayType.Type_6PlusC3
                || wktype == EnumWorkingDayType.Type_7PlusC3
                || wktype == EnumWorkingDayType.Type_8PlusC3)
                ? true
                : false;
        }
        internal static int GetHourNormalWorking(string wktype)
        {
            if (wktype == EnumWorkingDayType.Type_PDivide2)
            {
                return GlobalConsts.MAXIMUM_HOUR_IN_SHIFT / 2; // Haft shift
            }
            var isLeaveFullShiftType = IsLeaveFullShiftType(wktype); //Full shift
            if (isLeaveFullShiftType)
            {
                return 0;
            }
            else
            {
                return GlobalConsts.MAXIMUM_HOUR_IN_SHIFT; //Full shift
            }
        }
        internal static string ConvertOTType2DefaultType(string recommendType, DateTime? workingDate, Dictionary<string, HolidayScheduleEntity> dicHolidayScheduleStartDate, ref double? timeDeviation)
        {
            DateTime checkDay = workingDate.Value;
            string holidayKey = DateTimeUtil.GetDateTimeStr(checkDay, ConstDateTimeFormat.YYYYMMDD);

            var isWeekend = checkDay.DayOfWeek == DayOfWeek.Sunday || checkDay.DayOfWeek == DayOfWeek.Saturday;
            var isHoliday = dicHolidayScheduleStartDate.ContainsKey(holidayKey);

            string typeRS;

            if (isWeekend || isHoliday)
            {
                typeRS = ConvertOTType2DefaultType_weekendHoliday(recommendType, ref timeDeviation);
                return typeRS;
            }

            typeRS = ConvertOTType2DefaultType_normal(recommendType);
            return typeRS;


            //if (recommendType == EnumWorkingDayType.Type_C3PlusM
            //    || recommendType == EnumWorkingDayType.Type_P
            //    || recommendType == EnumWorkingDayType.Type_PDivide2
            //    || recommendType == EnumWorkingDayType.Type_UP
            //    || recommendType == EnumWorkingDayType.Type_S
            //    || recommendType == EnumWorkingDayType.Type_S75
            //    || recommendType == EnumWorkingDayType.Type_MR
            //    || recommendType == EnumWorkingDayType.Type_SW)
            //{
            //    return EnumWorkingDayType.;
            //}

            return recommendType;
        }

        private static string ConvertOTType2DefaultType_weekendHoliday(string recommendType, ref double? timeDeviation)
        {
            if (recommendType == EnumWorkingDayType.Type_1C3
                || recommendType == EnumWorkingDayType.Type_2C3
                || recommendType == EnumWorkingDayType.Type_3C3)
            {
                return EnumWorkingDayType.Type_8;
            }
            if (recommendType == EnumWorkingDayType.Type_4C3
                || recommendType == EnumWorkingDayType.Type_5C3
                || recommendType == EnumWorkingDayType.Type_6C3
                || recommendType == EnumWorkingDayType.Type_7C3
                || recommendType == EnumWorkingDayType.Type_8C3
                || recommendType == EnumWorkingDayType.Type_9C3
                || recommendType == EnumWorkingDayType.Type_10C3
                || recommendType == EnumWorkingDayType.Type_11C3
                || recommendType == EnumWorkingDayType.Type_12C3
                || recommendType == EnumWorkingDayType.Type_13C3
                || recommendType == EnumWorkingDayType.Type_14C3
                || recommendType == EnumWorkingDayType.Type_15C3
                || recommendType == EnumWorkingDayType.Type_16C3)
            {
                timeDeviation = timeDeviation - (4 * 3600);
                return EnumWorkingDayType.Type_4C3;
            }

            if (recommendType == EnumWorkingDayType.Type_1PlusC3
                || recommendType == EnumWorkingDayType.Type_2PlusC3
                || recommendType == EnumWorkingDayType.Type_3PlusC3)
            {
                return EnumWorkingDayType.Type_C3;
            }

            if (recommendType == EnumWorkingDayType.Type_4PlusC3
                || recommendType == EnumWorkingDayType.Type_5PlusC3
                || recommendType == EnumWorkingDayType.Type_6PlusC3
                || recommendType == EnumWorkingDayType.Type_7PlusC3
                || recommendType == EnumWorkingDayType.Type_8PlusC3
                || recommendType == EnumWorkingDayType.Type_9PlusC3
                || recommendType == EnumWorkingDayType.Type_10PlusC3
                || recommendType == EnumWorkingDayType.Type_11PlusC3
                || recommendType == EnumWorkingDayType.Type_12PlusC3
                || recommendType == EnumWorkingDayType.Type_13PlusC3
                || recommendType == EnumWorkingDayType.Type_14PlusC3
                || recommendType == EnumWorkingDayType.Type_15PlusC3
                || recommendType == EnumWorkingDayType.Type_16PlusC3)
            {
                timeDeviation = timeDeviation - (4 * 3600);
                return EnumWorkingDayType.Type_4PlusC3;
            }

            if (recommendType == EnumWorkingDayType.Type_1
                || recommendType == EnumWorkingDayType.Type_2
                || recommendType == EnumWorkingDayType.Type_3
                || recommendType == EnumWorkingDayType.Type_4
                || recommendType == EnumWorkingDayType.Type_5
                || recommendType == EnumWorkingDayType.Type_6
                || recommendType == EnumWorkingDayType.Type_7
                || recommendType == EnumWorkingDayType.Type_8
                || recommendType == EnumWorkingDayType.Type_9
                || recommendType == EnumWorkingDayType.Type_10
                || recommendType == EnumWorkingDayType.Type_11)
            {
                return EnumWorkingDayType.Type_8;
            }

            if (recommendType == EnumWorkingDayType.Type_12
                || recommendType == EnumWorkingDayType.Type_13
                || recommendType == EnumWorkingDayType.Type_14
                || recommendType == EnumWorkingDayType.Type_15
                || recommendType == EnumWorkingDayType.Type_16)
            {
                timeDeviation = timeDeviation - (4 * 3600);
                return EnumWorkingDayType.Type_12;
            }

            if (recommendType == EnumWorkingDayType.Type_1M
                || recommendType == EnumWorkingDayType.Type_2M
                || recommendType == EnumWorkingDayType.Type_3M
                || recommendType == EnumWorkingDayType.Type_4M
                || recommendType == EnumWorkingDayType.Type_5M
                || recommendType == EnumWorkingDayType.Type_6M
                || recommendType == EnumWorkingDayType.Type_7M
                || recommendType == EnumWorkingDayType.Type_8M
                || recommendType == EnumWorkingDayType.Type_9M
                || recommendType == EnumWorkingDayType.Type_10M
                || recommendType == EnumWorkingDayType.Type_11M)
            {
                return EnumWorkingDayType.Type_8M;
            }

            if (recommendType == EnumWorkingDayType.Type_12M
                || recommendType == EnumWorkingDayType.Type_13M
                || recommendType == EnumWorkingDayType.Type_14M
                || recommendType == EnumWorkingDayType.Type_15M
                || recommendType == EnumWorkingDayType.Type_16M)
            {
                timeDeviation = timeDeviation - (4 * 3600);
                return EnumWorkingDayType.Type_12M;
            }
            return recommendType;
        }

        private static string ConvertOTType2DefaultType_normal(string recommendType)
        {
            if (recommendType == EnumWorkingDayType.Type_1C3
                || recommendType == EnumWorkingDayType.Type_2C3
                || recommendType == EnumWorkingDayType.Type_3C3
                || recommendType == EnumWorkingDayType.Type_4C3
                || recommendType == EnumWorkingDayType.Type_5C3
                || recommendType == EnumWorkingDayType.Type_6C3
                || recommendType == EnumWorkingDayType.Type_7C3
                || recommendType == EnumWorkingDayType.Type_8C3
                || recommendType == EnumWorkingDayType.Type_9C3
                || recommendType == EnumWorkingDayType.Type_10C3
                || recommendType == EnumWorkingDayType.Type_11C3
                || recommendType == EnumWorkingDayType.Type_12C3
                || recommendType == EnumWorkingDayType.Type_13C3
                || recommendType == EnumWorkingDayType.Type_14C3
                || recommendType == EnumWorkingDayType.Type_15C3
                || recommendType == EnumWorkingDayType.Type_16C3)
            {
                return EnumWorkingDayType.Type_0;
            }

            if (recommendType == EnumWorkingDayType.Type_1PlusC3
                || recommendType == EnumWorkingDayType.Type_2PlusC3
                || recommendType == EnumWorkingDayType.Type_3PlusC3
                || recommendType == EnumWorkingDayType.Type_4PlusC3
                || recommendType == EnumWorkingDayType.Type_5PlusC3
                || recommendType == EnumWorkingDayType.Type_6PlusC3
                || recommendType == EnumWorkingDayType.Type_7PlusC3
                || recommendType == EnumWorkingDayType.Type_8PlusC3
                || recommendType == EnumWorkingDayType.Type_9PlusC3
                || recommendType == EnumWorkingDayType.Type_10PlusC3
                || recommendType == EnumWorkingDayType.Type_11PlusC3
                || recommendType == EnumWorkingDayType.Type_12PlusC3
                || recommendType == EnumWorkingDayType.Type_13PlusC3
                || recommendType == EnumWorkingDayType.Type_14PlusC3
                || recommendType == EnumWorkingDayType.Type_15PlusC3
                || recommendType == EnumWorkingDayType.Type_16PlusC3)
            {
                return EnumWorkingDayType.Type_C3;
            }

            if (recommendType == EnumWorkingDayType.Type_1
                || recommendType == EnumWorkingDayType.Type_2
                || recommendType == EnumWorkingDayType.Type_3
                || recommendType == EnumWorkingDayType.Type_4
                || recommendType == EnumWorkingDayType.Type_5
                || recommendType == EnumWorkingDayType.Type_6
                || recommendType == EnumWorkingDayType.Type_7
                || recommendType == EnumWorkingDayType.Type_8
                || recommendType == EnumWorkingDayType.Type_9
                || recommendType == EnumWorkingDayType.Type_10
                || recommendType == EnumWorkingDayType.Type_11
                || recommendType == EnumWorkingDayType.Type_12
                || recommendType == EnumWorkingDayType.Type_13
                || recommendType == EnumWorkingDayType.Type_14
                || recommendType == EnumWorkingDayType.Type_15
                || recommendType == EnumWorkingDayType.Type_16)
            {
                return EnumWorkingDayType.Type_0;
            }

            if (recommendType == EnumWorkingDayType.Type_1M
                || recommendType == EnumWorkingDayType.Type_2M
                || recommendType == EnumWorkingDayType.Type_3M
                || recommendType == EnumWorkingDayType.Type_4M
                || recommendType == EnumWorkingDayType.Type_5M
                || recommendType == EnumWorkingDayType.Type_6M
                || recommendType == EnumWorkingDayType.Type_7M
                || recommendType == EnumWorkingDayType.Type_8M
                || recommendType == EnumWorkingDayType.Type_9M
                || recommendType == EnumWorkingDayType.Type_10M
                || recommendType == EnumWorkingDayType.Type_11M
                || recommendType == EnumWorkingDayType.Type_12M
                || recommendType == EnumWorkingDayType.Type_13M
                || recommendType == EnumWorkingDayType.Type_14M
                || recommendType == EnumWorkingDayType.Type_15M
                || recommendType == EnumWorkingDayType.Type_16M)
            {
                return EnumWorkingDayType.Type_0;
            }
            return recommendType;
        }

        //public void CalculatorOTRecommentType(int oTHours)
        //{
        //    switch (this.OTHourType)
        //    {
        //        case EnumOTHourType.WeekendOrHolidayAndShift3:
        //            if (this.ReconnemdType == EnumWorkingDayType.Type_0)
        //            {
        //                this.ReconnemdType = this.getXC3(oTHours);
        //                this.WorkingDayHighlight = EnumWorkingDayHighlight.OTHoursOver4H_Added;
        //            }
        //            break;
        //        case EnumOTHourType.WeekendOrHoliday:
        //            if (this.ReconnemdType == EnumWorkingDayType.Type_0)
        //            {
        //                if (this.ShiftId == EnumShiftId.Shift3)
        //                {
        //                    this.ReconnemdType = this.getXPlushC3(oTHours);
        //                    this.WorkingDayHighlight = EnumWorkingDayHighlight.OTHoursOver4H_Added;
        //                    return;
        //                }
        //                this.ReconnemdType = this.getType_x(oTHours);
        //                this.WorkingDayHighlight = EnumWorkingDayHighlight.OTHoursOver4H_Added;
        //            }
        //            break;
        //        case EnumOTHourType.NightShift:
        //            if (this.ReconnemdType == EnumWorkingDayType.Type_0)
        //            {
        //                this.ReconnemdType = this.getXC3(oTHours);
        //                this.WorkingDayHighlight = EnumWorkingDayHighlight.OTHoursOver4H_Added;
        //            }
        //            break;
        //        case EnumOTHourType.Normal:
        //            if (this.ReconnemdType == EnumWorkingDayType.Type_0)
        //            {
        //                if (this.ShiftId == EnumShiftId.Shift3)
        //                {
        //                    this.ReconnemdType = this.getXPlushC3(oTHours);
        //                    this.WorkingDayHighlight = EnumWorkingDayHighlight.OTHoursOver4H_Added;
        //                    return;
        //                }
        //                this.ReconnemdType = this.getType_x(oTHours);
        //                this.WorkingDayHighlight = EnumWorkingDayHighlight.OTHoursOver4H_Added;
        //            }
        //            break;
        //        default:
        //            throw new NotImplementedException("CalculatorOTRecommentType error");
        //    }
        //}

        //public void SetWorkingShiftType(EnumWorkingShiftType WorkingShiftType)
        //{
        //    this.WorkingShiftType = WorkingShiftType;
        //}
    }
}
