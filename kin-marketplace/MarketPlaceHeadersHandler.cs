using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Kin.Marketplace.Models;
using Newtonsoft.Json;
using Refit;

namespace Kin.Marketplace
{
    internal class MarketPlaceHeadersHandler : DelegatingHandler
    {
        private readonly MarketPlaceHttpHeaders _marketPlaceHttpHeaders;

        public MarketPlaceHeadersHandler(MarketPlaceHttpHeaders marketPlaceHttpHeaders)
        {
            _marketPlaceHttpHeaders = marketPlaceHttpHeaders;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains(MarketPlaceHttpHeaders.XRequestId))
            {
                string requestId = Guid.NewGuid().ToString();
                request.Headers.Add(MarketPlaceHttpHeaders.XRequestId, requestId);
            }

            foreach (KeyValuePair<string, string> kv in _marketPlaceHttpHeaders.Values)
            {
                if (!request.Headers.Contains(kv.Key))
                {
                    request.Headers.Add(kv.Key, kv.Value);
                }
            }

            try
            {
                return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
            catch (ApiException e)
            {
                if (!string.IsNullOrEmpty(e.Content))
                {
                    throw new MarketPlaceException(JsonConvert.DeserializeObject<MarketPlaceError>(e.Content));
                }

                throw;
            }
        }
    }
}