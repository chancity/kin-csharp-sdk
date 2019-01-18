namespace Kin.Tooling.Models
{
    public interface IMetricTiming
    {
        long UtcStartTime { get; }
        long UtcEndTime { get; }
        long CompletionTimeMs { get; }
    }
}