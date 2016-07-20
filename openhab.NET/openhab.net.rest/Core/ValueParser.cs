using System.Reflection;

namespace openhab.net.rest.Core
{
    internal static class ValueParser
    {
        public static bool TryParse<T>(string s, out T result)
        {
            if (typeof(T).Is<string>())
            {
                result = (T)(object)s;
                return true;
            }

            var method = typeof(T).GetRuntimeMethod
            (
                "TryParse",
                new[] { typeof(string), typeof(T).MakeByRefType() }
            );

            if (method != null)
            {
                s = s.Replace(".", ",");  // Use Globalization?!?
                var args = new object[] { s, null };
                var success = (bool)method.Invoke(null, args);
                if (success)
                {
                    result = (T)args[1];
                    return true;
                }
            }

            result = default(T);
            return false;
        }
    }
}
