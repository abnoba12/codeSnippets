using System;
using System.Reflection;

namespace SmartStartWebAPI.Business.Helpers
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class EnumDisplayNameAttribute : Attribute
    {
        public string DisplayName { get; }

        public EnumDisplayNameAttribute(string displayName) => DisplayName = displayName;
    }

    public static class EnumExtensions
    {
        /// <summary>
        /// Decorate your enum with your display name [EnumDisplayName("Smart Start")]. Then on your enum call company.GetDisplayName() and you will get back your EnumDisplayName.
        /// This allows spaces in your enum names.
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetDisplayName(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var memberInfos = enumType.GetMember(enumValue.ToString());
            if (memberInfos.Length > 0)
            {
                var customAttributes = memberInfos[0].GetCustomAttributes(typeof(EnumDisplayNameAttribute), false);
                if (customAttributes.Length > 0)
                {
                    return ((EnumDisplayNameAttribute)customAttributes[0]).DisplayName;
                }
            }
            return enumValue.ToString();
        }
    }
}
