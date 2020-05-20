using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace LoveMusic
{

    public static class Extensions
    {
        public static string GetAttributeNameProperty<TType, TAttribute>(string enumName)
        {
            var field = typeof(TType).GetField(enumName);
            var attribute = Attribute.GetCustomAttribute(field, typeof(TAttribute));
            if (attribute != null)
            {
                return typeof(TAttribute).GetProperty("Name").GetValue(attribute)?.ToString();
            }
            return string.Empty;
        }
    }
}