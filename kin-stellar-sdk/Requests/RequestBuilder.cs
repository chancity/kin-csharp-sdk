using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kin.Stellar.Sdk.requests
{
    public enum OrderDirection
    {
        ASC,
        DESC
    }

    /// <summary>
    ///     Abstract class for request builders.
    /// </summary>
    public class RequestBuilder<T> where T : class
    {
        private readonly List<string> _segments;
        private bool _segmentsAdded;
        protected UriBuilder UriBuilder;

        public static HttpClient HttpClient { get; set; }

        public string Uri => BuildUri().ToString();

        public RequestBuilder(Uri serverUri, string defaultSegment, HttpClient httpClient)
        {
            UriBuilder = new UriBuilder(serverUri);
            _segments = new List<string>();

            if (!string.IsNullOrEmpty(defaultSegment))
            {
                SetSegments(defaultSegment);
            }

            _segmentsAdded = false; //Allow overwriting segments
            HttpClient = httpClient;
        }

        public async Task<TZ> Execute<TZ>(Uri uri) where TZ : class
        {
            ResponseHandler<TZ> responseHandler = new ResponseHandler<TZ>();

            HttpResponseMessage response = await HttpClient.GetAsync(uri);
            return await responseHandler.HandleResponse(response);
        }

        protected RequestBuilder<T> SetSegments(params string[] segments)
        {
            if (_segmentsAdded)
            {
                throw new Exception("URL segments have been already added.");
            }

            _segmentsAdded = true;

            //Remove default segments
            _segments.Clear();

            foreach (string segment in segments)
            {
                _segments.Add(segment);
            }

            return this;
        }

        /// <summary>
        ///     Sets <code>cursor</code> parameter on the request.
        ///     A cursor is a value that points to a specific location in a collection of resources.
        ///     The cursor attribute itself is an opaque value meaning that users should not try to parse it.
        ///     Read https://www.stellar.org/developers/horizon/reference/resources/page.html for more information.
        /// </summary>
        /// <param name="cursor"></param>
        /// <returns></returns>
        public virtual T Cursor(string cursor)
        {
            UriBuilder.SetQueryParam("cursor", cursor);

            return this as T;
        }

        /// <summary>
        ///     Sets <code>limit</code> parameter on the request.
        ///     It defines maximum number of records to return.
        ///     For range and default values check documentation of the endpoint requested.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public virtual T Limit(int number)
        {
            UriBuilder.SetQueryParam("limit", number.ToString());

            return this as T;
        }

        /// <summary>
        ///     Sets order parameter on request.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public virtual T Order(OrderDirection direction)
        {
            UriBuilder.SetQueryParam("order", direction.ToString().ToLower());

            return this as T;
        }

        /// <summary>
        ///     llows to stream SSE events from horizon.
        ///     Certain endpoints in Horizon can be called in streaming mode using Server-Sent Events.
        ///     This mode will keep the connection to horizon open and horizon will continue to return
        ///     http://www.w3.org/TR/eventsource/
        ///     "https://www.stellar.org/developers/horizon/learn/responses.html
        ///     responses as ledgers close.
        /// </summary>
        /// <param name="listener">
        ///     EventListener implementation with AccountResponse type
        ///     <returns>EventSource object, so you can close() connection when not needed anymore</returns>
        public Uri BuildUri()
        {
            if (_segments.Count > 0)
            {
                string path = "";

                foreach (string segment in _segments)
                {
                    path += "/" + segment;
                }

                UriBuilder.Path = path;

                return UriBuilder.Uri;
            }

            throw new NotSupportedException("No segments defined.");
        }
    }
}