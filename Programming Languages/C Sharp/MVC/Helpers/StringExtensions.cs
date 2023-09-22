using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCIWebAPI.StateReporting
{
    public static class StringExtensions
    {
        /// <summary>
        /// Will left pad a sting even if it is null. Nulls are treated as empty strings
        /// </summary>
        /// <param name="totalWidth"></param>
        /// <param name="paddingChar"></param>
        /// <returns></returns>
        public static string NPadLeft(this string item, int totalWidth, char paddingChar = ' ')
        {
            if (string.IsNullOrEmpty(item))
            {
                return string.Empty.PadLeft(totalWidth, paddingChar);
            }
            else
            {
                return item.PadLeft(totalWidth, paddingChar);
            }
        }

        /// <summary>
        /// Will right pad a sting even if it is null. Nulls are treated as empty strings
        /// </summary>
        /// <param name="item"></param>
        /// <param name="totalWidth"></param>
        /// <param name="paddingChar"></param>
        /// <returns></returns>
        public static string NPadRight(this string item, int totalWidth, char paddingChar = ' ')
        {
            if (string.IsNullOrEmpty(item))
            {
                return string.Empty.PadRight(totalWidth, paddingChar);
            }
            else
            {
                return item.PadRight(totalWidth, paddingChar);
            }
        }

        /// <summary>
        /// Will cause an error to be thrown using the msg if the string is not null or empty
        /// </summary>
        /// <param name="msg">Error Message</param>
        /// <returns></returns>
        public static string Required(this string input, string msg)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new Exception(msg);
            }
            else
            {
                return input;
            }
        }

        public static DateTime Required(this DateTime input, string msg)
        {
            if (input == DateTime.MinValue)
            {
                throw new Exception(msg);
            }
            else
            {
                return input;
            }
        }

        /// <summary>
        /// This will format the string to be exactly the width specified.
        /// If it is too short then it will use the paddingChar to pad the left until at width
        /// If it is too long then the right side of the string will be truncated to width
        /// </summary>
        /// <param name="width">The desired length of the string</param>
        /// <param name="paddingChar">Char to pad the string with if it is too short</param>
        /// <param name="paddingSide">the side to adding the padding. "left" or "right"</param>
        /// <returns></returns>
        public static string ExactLength(this string input, int width, char paddingChar = ' ', string paddingSide = "right")
        {
            if(paddingSide.Trim().ToLower() == "right")
            {
                input = input.NPadRight(width, paddingChar);
            }
            else
            {
                input = input.NPadLeft(width, paddingChar);
            }
            
            if (input.Length > width)
            {
                input = input.Substring(0, width);
            }
            return input;
        }

        public static IEnumerable<String> SplitInParts(this String s, Int32 partLength)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if (partLength <= 0)
                throw new ArgumentException("Part length has to be positive.", nameof(partLength));

            for (var i = 0; i < s.Length; i += partLength)
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
        }

        public static string DigitsOnly(this string input)
        {
            return string.Concat(input.Where(Char.IsDigit));
        }
    }
}
