using System;
using System.Collections.Generic;
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
        //From: https://stackoverflow.com/questions/11463734/split-a-list-into-smaller-lists-of-n-size
        public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}