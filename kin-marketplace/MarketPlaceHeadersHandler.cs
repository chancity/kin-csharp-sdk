using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Kin.Marketplace.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

namespace Kin.Marketplace
{
    internal class MarketPlaceHeadersHandler : DelegatingHandler
    {
        private readonly MarketPlaceHttpHeaders _marketPlaceHttpHeaders;
        private readonly JSchema _schema;

        public MarketPlaceHeadersHandler(MarketPlaceHttpHeaders marketPlaceHttpHeaders)
        {
            JSchemaGenerator generator = new JSchemaGenerator();
            _schema = generator.Generate(typeof(MarketPlaceError));

            _marketPlaceHttpHeaders = marketPlaceHttpHeaders;
            InnerHandler = new HttpClientHandler();
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

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (TryParseMarketPlaceError(jsonResponse, out MarketPlaceError error))
                {
                    throw new MarketPlaceException(error);
                }
            }

            return response;
        }

        private bool TryParseMarketPlaceError(string jsonResponse, out MarketPlaceError error)
        {
            // Check expected error keywords presence :
            if (!jsonResponse.Contains("error") ||
                !jsonResponse.Contains("message") ||
                !jsonResponse.Contains("code"))
            {
                error = null;
                return false;
            }

            JObject jsonObject = JObject.Parse(jsonResponse);

            if (!jsonObject.IsValid(_schema))
            {
                error = null;
                return false;
            }

            // Try to deserialize :
            try
            {
                error = JsonConvert.DeserializeObject<MarketPlaceError>(jsonResponse);
                return true;
            }
            catch
            {
                error = null;
                return false;
            }
        }
    }
}