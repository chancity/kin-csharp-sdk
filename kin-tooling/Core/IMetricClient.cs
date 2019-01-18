using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kin.Tooling.Models;

namespace Kin.Tooling.Core
{
    public interface IMetricClient
    {
        event NewMetricEventDelegate OnNewMetricEvent;

        IReadOnlyCollection<IMetricGather> MetricGathers { get; }
        IMetricClient AddGather(IMetricGather metricGather);
        Task StartAsync();
        Task StopAsync();

    }
}
