using System;

namespace openhab.net.rest
{
    public sealed class UpdateStrategy
    {
        public static UpdateStrategy Default => new UpdateStrategy();

        public UpdateStrategy()
        {
        }

        public UpdateStrategy(bool realtime = false)
        {
            Realtime = realtime;
        }

        public UpdateStrategy(TimeSpan interval)
        {
            Interval = interval;
        }

        /// <summary>
        /// Default is false
        /// </summary>
        public bool Realtime { get; } = false;

        /// <summary>
        /// Default is true
        /// </summary>
        public bool AutoCommit { get; set; } = true;

        /// <summary>
        /// Default is Zero (Off)
        /// </summary>
        public TimeSpan Interval { get; } = TimeSpan.Zero;
    }
}
