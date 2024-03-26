using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LCIWebAPI.Helpers
{
    public static class EmailHelper
    {
        public static bool IsValidEmailAddress(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                EmailAddressAttribute emailChecker = new EmailAddressAttribute();
                return emailChecker.IsValid(email);
            }

            return false;
        }

        public static string ToHTMLString(this StringBuilder sb)
        {
            return sb.ToString().Replace("\r\n", "<br />");
        }
    }
}