using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SmartStartWebAPI.Business.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    /**
     * USAGE:
    ---------------------------------------------------------------------------------------     
    var phone = new PhoneNumberHelper("(817) 555-1234 x204")
    .RequireAreaCode()
    .RequireExtension()
    .Validate();

    string formatted = phone.Format("AAA-EEE-LLLL xX"); // "817-555-1234 x204"
    ---------------------------------------------------------------------------------------
    var shortPhone = new PhoneNumberHelper("555-1234")
    .RequireAreaCode(false)
    .Validate()
    .Format("E-L"); // "555-1234"
    ---------------------------------------------------------------------------------------
    try
    {
        var invalidPhone = new PhoneNumberHelper("555-1234")
            .RequireAreaCode() // this makes it invalid
            .Validate();
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine(ex.Message); // "Missing required parts: Area Code"
    }    
    ---------------------------------------------------------------------------------------
    **/
    namespace SmartStartWebAPI.Business.Utility
    {
        /// <summary>
        /// Helper class for parsing, validating, and formatting U.S.-style phone numbers.
        /// </summary>
        public class PhoneNumberHelper
        {
            /// <summary>
            /// Country code part of the phone number (e.g., "1" for U.S.).
            /// </summary>
            public string CountryCode { get; private set; } = "";

            /// <summary>
            /// Area code part of the phone number (e.g., "817").
            /// </summary>
            public string AreaCode { get; private set; } = "";

            /// <summary>
            /// Exchange code part of the phone number (e.g., "555").
            /// </summary>
            public string ExchangeCode { get; private set; } = "";

            /// <summary>
            /// Line number part of the phone number (e.g., "1234").
            /// </summary>
            public string LineNumber { get; private set; } = "";

            /// <summary>
            /// Optional extension (e.g., "204").
            /// </summary>
            public string Extension { get; private set; } = "";

            private bool requireCountryCode = false;
            private bool requireAreaCode = true;
            private bool requireExchangeCode = true;
            private bool requireLineNumber = true;
            private bool requireExtension = false;

            /// <summary>
            /// Initializes a new instance of the PhoneNumberHelper class by parsing a raw phone number string.
            /// </summary>
            /// <param name="raw">The raw phone number string (e.g., "(817) 555-1234 x204").</param>
            public PhoneNumberHelper(string raw)
            {
                // First detect and extract the extension
                var extMatch = Regex.Match(raw, @"(ext\.?|x)\s*(\d+)", RegexOptions.IgnoreCase);
                if (extMatch.Success)
                {
                    Extension = extMatch.Groups[2].Value;
                    raw = raw.Replace(extMatch.Value, ""); // remove the matched extension part
                }

                var digits = new string(raw.Where(char.IsDigit).ToArray());

                // Parse from the right side
                if (digits.Length >= 4)
                    LineNumber = digits.Substring(digits.Length - 4, 4);

                if (digits.Length >= 7)
                    ExchangeCode = digits.Substring(digits.Length - 7, 3);

                if (digits.Length >= 10)
                    AreaCode = digits.Substring(digits.Length - 10, 3);

                if (digits.Length > 10)
                    CountryCode = digits.Substring(0, digits.Length - 10);
            }

            /// <summary>
            /// Requires the phone number to include a country code.
            /// Also implicitly requires area code, exchange code, and line number.
            /// </summary>
            public PhoneNumberHelper RequireCountryCode(bool required = true)
            {
                requireCountryCode = required;
                if (required)
                {
                    requireAreaCode = true;
                    requireExchangeCode = true;
                    requireLineNumber = true;
                }
                return this;
            }

            /// <summary>
            /// Requires the phone number to include an area code.
            /// Also implicitly requires exchange code and line number.
            /// </summary>
            public PhoneNumberHelper RequireAreaCode(bool required = true)
            {
                requireAreaCode = required;
                if (required)
                {
                    requireExchangeCode = true;
                    requireLineNumber = true;
                }
                return this;
            }

            /// <summary>
            /// Requires the phone number to include an extension.
            /// </summary>
            public PhoneNumberHelper RequireExtension(bool required = true)
            {
                requireExtension = required;
                return this;
            }

            /// <summary>
            /// Validates that all required parts of the phone number are present.
            /// Throws InvalidOperationException if any required part is missing.
            /// </summary>
            public PhoneNumberHelper Validate()
            {
                var missing = new List<string>();
                if (requireCountryCode && string.IsNullOrEmpty(CountryCode)) missing.Add("Country Code");
                if (requireAreaCode && string.IsNullOrEmpty(AreaCode)) missing.Add("Area Code");
                if (requireExchangeCode && string.IsNullOrEmpty(ExchangeCode)) missing.Add("Exchange Code");
                if (requireLineNumber && string.IsNullOrEmpty(LineNumber)) missing.Add("Line Number");
                if (requireExtension && string.IsNullOrEmpty(Extension)) missing.Add("Extension");

                if (missing.Any())
                    throw new InvalidOperationException($"Missing required parts: {string.Join(", ", missing)}");

                return this;
            }

            /// <summary>
            /// Formats the phone number using a custom pattern.
            /// Pattern symbols:
            /// - C: Country code
            /// - A: Area code
            /// - E: Exchange code
            /// - L: Line number
            /// - X: Extension
            /// </summary>
            /// <param name="pattern">Formatting pattern (e.g., "AEEELLLL", "AAA-EEE-LLLL xX").</param>
            /// <returns>The formatted phone number string.</returns>
            public string Format(string pattern)
            {
                if (string.IsNullOrEmpty(pattern))
                {
                    return $"{CountryCode}{AreaCode}{ExchangeCode}{LineNumber}";
                }

                return pattern
                    .Replace("C", CountryCode)
                    .Replace("A", AreaCode)
                    .Replace("E", ExchangeCode)
                    .Replace("L", LineNumber)
                    .Replace("X", Extension);
            }
        }
    }
}
