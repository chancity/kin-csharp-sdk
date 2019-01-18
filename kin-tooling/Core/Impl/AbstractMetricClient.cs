using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kin.Tooling.Models;

namespace Kin.Tooling.Core.Impl
{
    public class AbstractMetricClient : IMetricClient
    {
        private readonly HashSet<IMetricGather> _metricGathers;
        private readonly object _metricGathersLocker;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _metricGatherThread;
        public int Interval { get; set; }

        public AbstractMetricClient(int interval = 60000)
        {
            Interval = interval;
            _metricGathers = new HashSet<IMetricGather>();
            _metricGathersLocker = new object();
        }

        public event NewMetricEventDelegate OnNewMetricEvent;

        public IReadOnlyCollection<IMetricGather> MetricGathers
        {
            get
            {
                lock (_metricGathersLocker)
                {
                    return _metricGathers.ToList();
                }
            }
        }

        public IMetricClient AddGather(IMetricGather metricGather)
        {
            lock (_metricGathersLocker)
            {
                _metricGathers.Add(metricGather);
            }

            return this;
        }

        public Task StartAsync()
        {
            if (_metricGatherThread?.Status == TaskStatus.Running)
            {
                throw new Exception("Client gather is already running");
            }

            _metricGatherThread?.Dispose();
            _cancellationTokenSource?.Dispose();

            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = _cancellationTokenSource.Token;

            _metricGatherThread = new Task(QueueThread, cancellationToken, cancellationToken,
                TaskCreationOptions.LongRunning);
            _metricGatherThread.Start();

            return Task.Delay(-1, cancellationToken);
        }

        public Task StopAsync()
        {
            if (_cancellationTokenSource != null && _cancellationTokenSource.IsCancellationRequested ||
                _metricGatherThread != null && (_metricGatherThread.Status == TaskStatus.Canceled ||
                                                _metricGatherThread.Status == TaskStatus.Faulted ||
                                                _metricGatherThread.Status == TaskStatus.RanToCompletion))
            {
                throw new Exception("Client gather is already stopped");
            }

            _cancellationTokenSource?.Cancel();
            return Task.CompletedTask;
        }

        protected virtual async void NewMetricEvent(IMetric metric)
        {
            OnNewMetricEvent?.Invoke(metric);
        }

        private async void QueueThread(object ctx)
        {
            CancellationToken cancellationToken = (CancellationToken) ctx;

            while (!cancellationToken.IsCancellationRequested)
            {
                IMetricGather[] metricGathers;

                lock (_metricGathersLocker)
                {
                    metricGathers = _metricGathers.ToArray();
                }

                foreach (IMetricGather metricGather in metricGathers)
                {
                    IMetric metric = await metricGather.GatherMetric(cancellationToken).ConfigureAwait(false);
                    NewMetricEvent(metric);
                }


                await Task.Delay(Interval, cancellationToken);
            }
        }
    }
}