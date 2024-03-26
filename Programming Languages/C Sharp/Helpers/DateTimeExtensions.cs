using System;
using System.Collections.Generic;

namespace LCIWebAPI.Helpers
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Compare this datetime to another datetime and take the greater datetime
        /// </summary>
        /// <param name="input"></param>
        /// <param name="compareTo"></param>
        /// <returns></returns>
        public static DateTime Max(this DateTime input, DateTime compareTo)
        {
            if (Comparer<DateTime>.Default.Compare(input, compareTo) > 0)
                return input;
            return compareTo;
        }

        /// <summary>
        /// Compare this datetime to another datetime and take the lesser datetime
        /// </summary>
        /// <param name="input"></param>
        /// <param name="compareTo"></param>
        /// <returns></returns>
        public static DateTime Min(this DateTime input, DateTime compareTo)
        {
            if (Comparer<DateTime>.Default.Compare(input, compareTo) < 0)
                return input;
            return compareTo;
        }
    }
}