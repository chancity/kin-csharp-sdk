using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Kin.Stellar.Sdk.requests;
using Newtonsoft.Json;

namespace Kin.Stellar.Sdk.responses.page
{
    /// <summary>
    ///     Represents page of objects.
    ///     https://www.stellar.org/developers/horizon/reference/resources/page.html
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Page<T> : Response
    {
        [JsonProperty(PropertyName = "records")]
        public List<T> Records { get; private set; }

        [JsonProperty(PropertyName = "links")]
        public PageLinks Links { get; private set; }

        /// <summary>
        ///     The next page of results or null when there is no more results
        /// </summary>
        /// <returns></returns>
        public async Task<Page<T>> NextPage()
        {
            if (Links.Next == null)
            {
                return null;
            }

            ResponseHandler<Page<T>> responseHandler = new ResponseHandler<Page<T>>();
            Uri uri = new Uri(Links.Next.Href);

            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(uri);
                return await responseHandler.HandleResponse(response);
            }
        }
    }
}