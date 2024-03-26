using System;
using System.Collections.Generic;
using System.Linq;

namespace WE.Freamwork.Logic.Helpers
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
            if (paddingSide.Trim().ToLower() == "right")
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

        /// <summary>
        /// Counts the minimum number of edits needed to transform one string into the other. The higher the number the more different the strings are.
        /// </summary>
        public static int LevenshteinDistance(this string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }
    }
}
