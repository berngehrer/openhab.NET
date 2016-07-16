using System;
using System.Reflection;

namespace openhab.net.rest.Extensions
{
    internal static class TypeExtensions
    {
        public static bool Is<T>(this Type type)
        {
            return type.GetTypeInfo().Is<T>();
        }

        public static bool Is<T>(this TypeInfo type)
        {
            return typeof(T).GetTypeInfo().IsAssignableFrom(type);
        }
    }
}
