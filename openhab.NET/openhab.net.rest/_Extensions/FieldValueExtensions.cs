using openhab.net.rest.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace openhab.net.rest
{
    internal static class FieldValueExtensions
    {
        public static object GetValue(this Enum e) => e.GetAttribute<FieldValueAttribute>()?.Value;


        public static Dictionary<string,T> ToDictionary<T>() where T : struct
        {
            return GetFieldValuePairs(typeof(T)).ToDictionary(x => x.Key, x => (T)x.Value);
        }

        static IEnumerable<KeyValuePair<string,object>> GetFieldValuePairs(Type type) 
        {
            string value;
            foreach (var field in Enum.GetValues(type))
            {
                if ((value = ((Enum)field).GetValue()?.ToString()) != null)
                {
                    yield return new KeyValuePair<string,object>(value, field);
                }
            }
        }
    }
}
