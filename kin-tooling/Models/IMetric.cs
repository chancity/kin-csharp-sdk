using System;

namespace Kin.Tooling.Models
{
    public interface IMetric
    {
        string Id { get; }
        IMetricTiming Timing { get; }
        IMetricError Error { get; }
    }
}
