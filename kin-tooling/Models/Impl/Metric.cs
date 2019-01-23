using System;

namespace Kin.Tooling.Models.Impl
{
    public class Metric : IMetric
    {
        public string Host { get; private set; }
        public string Path { get; private set; }
        public IMetricTiming Timing { get; private set; }
        public IMetricError Error { get; private set; }

        public Metric(string host, string path, IMetricTiming metricTiming, IMetricError metricError = null)
        {
            if(string.IsNullOrEmpty(host))
                throw new ArgumentNullException($"{nameof(host)} can't be null or empty");

            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException($"{nameof(path)} can't be null or empty");

            Host = host;
            Path = path;
            Timing = metricTiming;
            Error = metricError;
        }
    }
}
