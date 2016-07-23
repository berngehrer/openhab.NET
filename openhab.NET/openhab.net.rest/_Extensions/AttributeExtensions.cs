using System;
using System.Linq;
using System.Reflection;

namespace openhab.net.rest
{
    internal static class AttributeExtensions
    {
        public static T[] GetAttributes<T>(this Enum value)
                          where T : Attribute
        {
            var fieldInfo = value.GetType().GetTypeInfo().GetDeclaredField(value.ToString());
            var attributes = fieldInfo.GetCustomAttributes(typeof(T), false);
            if (attributes == null)
                return default(T[]);
            return (T[])attributes;
        }

        public static T GetAttribute<T>(this Enum value)
                        where T : Attribute
        {
            var attributes = value.GetAttributes<T>();
            if (attributes != null && attributes.Any())
                return attributes[0];
            return default(T);
        }
    }
}
