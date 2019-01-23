using System;

namespace Kin.Tooling.Models
{
    public interface IMetric
    {
        string Host { get; }
        string Path { get; }
        IMetricTiming Timing { get; }
        IMetricError Error { get; }
    }
}
