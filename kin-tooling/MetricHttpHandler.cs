using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Kin.Shared.Models.MarketPlace;

namespace Kin.Tooling.Models.Impl
{
    public class MetricHttpHandler : DelegatingHandler
    {
        private const string XRequestId = "X-REQUEST-ID";
        public static event NewMetricEventDelegate NewMetricEvent;
        public static bool BenchMarkEnabled = false;
        public MetricHttpHandler(HttpMessageHandler innerHandler = null)
        {
            InnerHandler = innerHandler ?? new HttpClientHandler();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,CancellationToken cancellationToken)
        {
            if(!BenchMarkEnabled)
                return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            string requestId = "";
            if (!request.Headers.Contains(XRequestId))
            {
                requestId = Guid.NewGuid().ToString();
                request.Headers.Add(XRequestId, requestId);
            }
            else
            {
                requestId = request.Headers.GetValues(XRequestId).FirstOrDefault();
            }
           
            var utcStartTime = CurrentUtcTimeMs();
            long utcEndTime;
            string message = "";
            int statusCode = 0;
            string path = request.RequestUri.AbsolutePath;

            try
            {
                HttpResponseMessage response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

                statusCode = (int) response.StatusCode;
                message = response.ReasonPhrase;
                return response;
            }
            catch (WebException wex)
            {
                message = wex.Message;
                statusCode = GetStatusCodeFromWebException(wex);
                wex?.Response?.Dispose();
                throw;
            }
            catch (MarketPlaceException mex)
            {
                message = mex.MarketPlaceError.Error;
                statusCode = mex.MarketPlaceError.Code;
                throw;
            }
            catch (Exception ex)
            {
                message = ex.ToString();
                throw;
            }
            finally
            {
                utcEndTime = CurrentUtcTimeMs();
                var metricTiming = new MetricTiming(utcStartTime, utcEndTime);
                var metricError = ErrorHandler(message, statusCode);
                var metric = new Metric(requestId, path, metricTiming, metricError);
                OnNewMetricEvent(metric);
            }
        }

        private int GetStatusCodeFromWebException(WebException wex)
        {
            if (wex.Status == WebExceptionStatus.ProtocolError)
            {
                var response = wex.Response as HttpWebResponse;
                if (response != null)
                {
                   
                    return (int)response.StatusCode;
                }
            }

            return -1;
        }
        private long CurrentUtcTimeMs()
        {
            return (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }
        private IMetricError ErrorHandler(string message, int statusCode)
        {
            if (statusCode >= 200 && statusCode <= 299)
                return null;

            return new MetricError(message, statusCode);
        }

        private static Task OnNewMetricEvent(IMetric newmetric)
        {

            NewMetricEvent?.Invoke(newmetric);
            return Task.CompletedTask;
        }
    }
}