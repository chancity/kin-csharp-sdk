namespace Kin.Tooling.Models {
    public interface IMetricError
    {
        string Message { get; }
        long StatusCode { get; }
    }
}