using System.Threading.Tasks;
using Kin.Tooling.Models;

namespace Kin.Tooling.Core
{
    public delegate Task NewMetricEventDelegate(IMetric newMetric);
}
