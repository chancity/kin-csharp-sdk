using System;

namespace Kin.Tooling.Models.Impl
{
    public class Metric : IMetric
    {
        public string Id { get; private set; }
        public IMetricTiming Timing { get; private set; }
        public IMetricError Error { get; private set; }

        public Metric(string id, IMetricTiming metricTiming, IMetricError metricError = null)
        {
            if(string.IsNullOrEmpty(id))
                throw new ArgumentNullException($"{nameof(id)} can't be null or empty");

            Id = id;
            Timing = metricTiming;
            Error = metricError;
        }
    }
}
