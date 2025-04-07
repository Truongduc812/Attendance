using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iSoft.Common.ConstCommon;

namespace iSoft.Common.Utils
{
    public class DateTimeUtil
    {
        public static string GetDateTimeStr(DateTime dt, string format = "yyyy-MM-dd HH:mm:ss.fff")
        {
            return dt.ToString(format);
        }

        public static string GetHumanStr(long miliseconds, bool shortMode = true)
        {
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(miliseconds);
            return GetHumanStr(timeSpan, shortMode);
        }

        public static string GetHumanStrBySeconds(long? seconds, bool shortMode = true)
        {
            if (seconds == null)
            {
                return GetHumanStrBySeconds(0, shortMode);
            }
            TimeSpan timeSpan = TimeSpan.FromMilliseconds((double)(seconds * 1000));
            return GetHumanStr(timeSpan, shortMode);
        }

        public static string GetHumanStrByHours_TextHours(long? hours, bool shortMode = true)
        {
            if (hours == null)
            {
                return GetHumanStrByHours_TextHours(0, shortMode);
            }
            TimeSpan timeSpan = TimeSpan.FromMilliseconds((double)(hours * 3600 * 1000));
            return GetHumanStr_TextHours(timeSpan, shortMode);
        }

        public static string GetHumanStr(TimeSpan t, bool shortMode = true)
        {
            if (shortMode)
            {
                string days = t.Days >= 1 ? string.Format("{0}d", t.Days) : "";
                string hours = t.Hours >= 1 ? string.Format("{0}h", t.Hours) : "";
                string minutes = t.Minutes >= 1 ? string.Format("{0}m", t.Minutes) : "";
                string seconds = t.Seconds >= 1 ? string.Format("{0}s", t.Seconds) : "";
                string miliseconds = t.Milliseconds >= 1 ? string.Format("{0}ms", t.Milliseconds) : "";

                string secondsStr = string.Format($"{days} {hours} {minutes} {seconds}".Trim());
                string milisecondsStr = string.Format($"{miliseconds}".Trim());
                if (secondsStr != "")
                {
                    return secondsStr;
                }
                if (milisecondsStr != "")
                {
                    return milisecondsStr;
                }
                return "0s";
            }
            else
            {
                string days = t.Days == 1 ? string.Format("{0} day", t.Days) : "";
                string hours = t.Hours == 1 ? string.Format("{0} hour", t.Hours) : "";
                string minutes = t.Minutes == 1 ? string.Format("{0} minute", t.Minutes) : "";
                string seconds = t.Seconds == 1 ? string.Format("{0} second", t.Seconds) : "";
                string miliseconds = t.Milliseconds == 1 ? string.Format("{0} milisecond", t.Milliseconds) : "";

                days = t.Days > 1 ? string.Format("{0} days", t.Days) : "";
                hours = t.Hours > 1 ? string.Format("{0} hours", t.Hours) : "";
                minutes = t.Minutes > 1 ? string.Format("{0} minutes", t.Minutes) : "";
                seconds = t.Seconds > 1 ? string.Format("{0} hours", t.Seconds) : "";
                miliseconds = t.Milliseconds > 1 ? string.Format("{0} miliseconds", t.Milliseconds) : "";

                string secondsStr = string.Format($"{days} {hours} {minutes} {seconds}".Trim());
                string milisecondsStr = string.Format($"{miliseconds}".Trim());
                if (secondsStr != "")
                {
                    return secondsStr;
                }
                if (milisecondsStr != "")
                {
                    return milisecondsStr;
                }
                return "0 second";
            }
        }

        public static string GetHumanStr_TextHours(TimeSpan t, bool shortMode = true)
        {
            if (shortMode)
            {
                string days = t.Days >= 1 ? string.Format("{0}d", t.Days) : "";
                string hours = t.Hours >= 1 ? string.Format("{0}h", t.Hours) : "";

                string rs = string.Format($"{days} {hours}".Trim());
                if (rs != "")
                {
                    return rs;
                }
                return "0h";
            }
            else
            {
                string days = t.Days >= 1 ? string.Format("{0} days", t.Days) : "";
                string hours = t.Hours >= 1 ? string.Format("{0} hours", t.Hours) : "";

                string rs = string.Format($"{days} {hours}".Trim());
                if (rs != "")
                {
                    return rs;
                }
                return "0 hour";
            }
        }

        public static long? RoundTicks(long? ticks, int minutesForRound)
        {
            if (ticks == null)
            {
                return null;
            }
            long? roundedTicks = ticks - (ticks % (TimeSpan.TicksPerMinute * minutesForRound));
            return roundedTicks;
        }

        /// <summary>
        /// CompareDateTime
        /// </summary>
        /// <param name="ticks1"></param>
        /// <param name="ticks2"></param>
        /// <param name="defaultValue"></param>
        /// <returns>Seconds</returns>
        public static long? CompareDateTime(long? ticks1, long? ticks2, long? defaultValue, bool isReturnMiliseconds = false)
        {
            if (ticks1 != null && ticks2 != null)
            {
                if (isReturnMiliseconds)
                {
                    return (ticks2.Value - ticks1.Value) / TimeSpan.TicksPerMillisecond;
                }
                else
                {
                    return (ticks2.Value - ticks1.Value) / TimeSpan.TicksPerSecond;
                }
            }
            else
            {
                return defaultValue;
            }
        }

        /*
            listTimeSheet.Sort((x, y) =>
            {
              int result = DateTimeUtil.CompareDateTime2(x.RecordedTime, y.RecordedTime);
              if (result == 0)
              {
                result = x.Id.CompareTo(y.Id);
              }
              return result;
            });
        */
        /// <summary>
        /// CompareDateTime
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <param name="defaultValue"></param>
        /// <returns>0/1/-1</returns>
        public static int CompareDateTime2(DateTime? date1, DateTime? date2)
        {
            if (date1.HasValue && date2.HasValue)
            {
                return date1.Value.CompareTo(date2.Value);
            }
            if (!date1.HasValue)
            {
                return -1;
            }
            if (!date2.HasValue)
            {
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// CompareDateTime
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <param name="defaultValue"></param>
        /// <returns>Seconds</returns>
        public static long CompareDateTime(DateTime date1, DateTime date2)
        {
            return (long)(date2 - date1).TotalSeconds;
        }
        /// <summary>
        /// CompareDateTime
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <param name="defaultValue"></param>
        /// <returns>Seconds</returns>
        public static long? CompareDateTime(DateTime? date1, DateTime? date2, long? defaultValue, bool isReturnMiliseconds = false)
        {
            if (date1 != null && date2 != null)
            {
                if (isReturnMiliseconds)
                {
                    return (long)(date2.Value - date1.Value).TotalMilliseconds;
                }
                else
                {
                    return (long)(date2.Value - date1.Value).TotalSeconds;
                }
            }
            else
            {
                return defaultValue;
            }
        }

        public static DateTime GetLocalDateTime(DateTime dt)
        {
            //return dt.AddHours(7);

            return DateTime.Now;
        }

        public static DateTime GetUTCDateTime(DateTime dt)
        {
            return dt.AddHours(-7);
        }

        public static DateTime GetDateTimeFromString(string dateTimeStr, string dateTimeFormat = ConstDateTimeFormat.YYYYMMDD_HHMMSS)
        {
            DateTime dt = DateTime.ParseExact(dateTimeStr, dateTimeFormat, CultureInfo.InvariantCulture);
            return dt;
        }

        public static string GetDuration(DateTime? startAt, DateTime? endAt)
        {
            if (endAt is null)
            {
                long? seconds = DateTimeUtil.CompareDateTime(startAt, DateTime.Now, 0);
                return DateTimeUtil.GetHumanStrBySeconds(seconds);
            }
            else
            {
                long? seconds = DateTimeUtil.CompareDateTime(startAt, endAt, 0);
                return DateTimeUtil.GetHumanStrBySeconds(seconds);
            }
        }

        public static string GetTimeSpanStr(TimeSpan timeSpan)
        {
            return timeSpan.ToString(@"hh\:mm\:ss");
        }
        public static DateTime GetStartDate(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }
        public static DateTime GetEndDate(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }
        public static DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
        }
        public static DateTime RoundDown(DateTime dt, TimeSpan d)
        {
            var delta = dt.Ticks % d.Ticks;
            return new DateTime(dt.Ticks - delta, dt.Kind);
        }
        public static DateTime RoundToNearest(DateTime dt, TimeSpan d)
        {
            var delta = dt.Ticks % d.Ticks;
            bool roundUp = delta > d.Ticks / 2;
            var offset = roundUp ? d.Ticks : 0;

            return new DateTime(dt.Ticks + offset - delta, dt.Kind);
        }
        public static DateTime[] ReferToSamedate(DateTime dt1, DateTime dt2, TimeSpan timeSpan, bool isUp = true, bool isRound = true)
        {
            dt1 = new DateTime(1900, 1, 1, dt1.Hour, dt1.Minute, dt1.Second);
            dt2 = new DateTime(1900, 1, 1, dt2.Hour, dt2.Minute, dt2.Second);
            dt1 = isRound ? isUp ? RoundUp(dt1, timeSpan) : RoundDown(dt1, timeSpan) : dt1;
            dt2 = isRound ? isUp ? RoundUp(dt2, timeSpan) : RoundDown(dt2, timeSpan) : dt2;
            return new DateTime[] { dt1, dt2 };
        }

        public static int GetHours(long totalSeconds)
        {
            return (int)Math.Floor((totalSeconds + ConstCommon.ConstDeltaSeconds * 1.0) / 3600);
        }
    }
}
