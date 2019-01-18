using System;
using System.Threading;
using System.Threading.Tasks;
using Kin.Tooling.Models;
using Kin.Tooling.Models.Impl;

namespace Kin.Tooling.Core.Impl
{
    public abstract class AbstractMetricGather<T>  : IMetricGather where T: class
    {
        public string Id { get; }

        protected AbstractMetricGather()
        {

            Id = GetType().Name;
        }

        public async Task<IMetric> GatherMetric(CancellationToken ctx)
        {
            var utcStartTime = CurrentUtcTimeMs();
            var processedData = await Process(ctx);
            var utcEndTime = CurrentUtcTimeMs();
            var metricTiming = new MetricTiming(utcStartTime, utcEndTime);
            var metricError = ErrorHandler(processedData.error);

            return new Metric(Id, metricTiming, metricError);
        }

        public async Task<(T data, Exception error)> Process(CancellationToken ctx)
        {
            T data = null;
            Exception error = null;
            try
            {
                data = await InternalProcess(ctx).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                error = e;
            }

            return (data, error);
        }
        protected abstract Task<T> InternalProcess(CancellationToken ctx);


        private long CurrentUtcTimeMs()
        {
            return (long) (DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }
        public virtual IMetricError ErrorHandler(Exception exception)
        {
            return new MetricError(exception.Message, 0);
        }

        protected bool Equals(AbstractMetricGather<T> other)
        {
            return string.Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((AbstractMetricGather<T>) obj);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }
}
