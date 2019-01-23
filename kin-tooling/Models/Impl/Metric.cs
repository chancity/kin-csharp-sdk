using System;

namespace Kin.Tooling.Models.Impl
{
    public class Metric : IMetric
    {
        public string Path { get; private set; }
        public string RequestId { get; private set; }
        public IMetricTiming Timing { get; private set; }
        public IMetricError Error { get; private set; }

        public Metric(string requestId, string path, IMetricTiming metricTiming, IMetricError metricError = null)
        {
            if(string.IsNullOrEmpty(requestId))
                throw new ArgumentNullException($"{nameof(requestId)} can't be null or empty");

            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException($"{nameof(path)} can't be null or empty");

            RequestId = requestId;
            Path = path;
            Timing = metricTiming;
            Error = metricError;
        }
    }
}
