using System;
using System.Threading;
using System.Threading.Tasks;
using Kin.Tooling.Models;

namespace Kin.Tooling.Core
{
    public interface IMetricGather
    {
        string Id { get; }
        Task<IMetric> GatherMetric(CancellationToken ctx);
        IMetricError ErrorHandler(Exception exception);
    }
}
