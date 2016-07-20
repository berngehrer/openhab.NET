using System;

namespace openhab.net.rest.Core
{
    internal static class IdProvider
    {
        static long _number = DateTime.Now.Ticks;
        public static long GetNext() => _number++;
    }
}
