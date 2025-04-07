using System;

namespace iSoft.Common.Utils
{
    public static class RandomDateTime
    {
        public static DateTime Next(DateTime from, DateTime to)
        {
            if (from >= to)
            {
                throw new ArgumentException("Invalid range");
            }

            Random random = new Random();

            long range = (to - from).Ticks;
            long randomTicks = (long)(random.NextDouble() * range);

            return from.AddTicks(randomTicks);
        }
    }

}
