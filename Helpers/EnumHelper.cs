using System;
using System.ComponentModel.DataAnnotations;

namespace ConcessionariaMVC.Helpers
{
    public static class EnumHelpers
    {
        public static string GetEnumDisplayName(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var descriptionAttributes = (DisplayAttribute[])fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false);
            return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Name : value.ToString();
        }
    }
}
