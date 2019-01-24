using System;

namespace Kin.Tooling.Models.Impl
{
    public class MetricTiming : IMetricTiming
    {
        public long UtcStartTime { get; private set; }
        public long UtcEndTime { get; private set; }
        public long CompletionTimeMs { get; private set; }

        public MetricTiming(long utcStartTime, long utcEndTime)
        {
            if (utcStartTime == 0)
                throw new ArgumentOutOfRangeException($"{nameof(utcStartTime)} can't be {utcStartTime}");

            if (utcEndTime == 0)
                throw new ArgumentOutOfRangeException($"{nameof(utcEndTime)} can't be {utcEndTime}");

            CompletionTimeMs = utcEndTime - utcStartTime;
            UtcStartTime = utcStartTime / 1000;
            UtcEndTime = utcEndTime / 1000;
        }
    }
}