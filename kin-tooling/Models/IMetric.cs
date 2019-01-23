using System;

namespace Kin.Tooling.Models
{
    public interface IMetric
    {
        string Path { get; }
        string RequestId { get; }
        IMetricTiming Timing { get; }
        IMetricError Error { get; }
    }
}
