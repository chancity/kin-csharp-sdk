using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Kin.Stellar.Sdk.responses;

namespace Kin.Stellar.Sdk.requests
{
    public class ResponseHandler<T> where T : class
    {
        public async Task<T> HandleResponse(HttpResponseMessage response)
        {
            HttpStatusCode statusCode = response.StatusCode;
            string content = await response.Content.ReadAsStringAsync();

            if ((int) statusCode == 429)
            {
                int retryAfter = int.Parse(response.Headers.GetValues("Retry-After").First());
                throw new TooManyRequestsException(retryAfter);
            }

            if ((int) statusCode >= 300)
            {
                throw new HttpResponseException((int) statusCode, response.ReasonPhrase);
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ClientProtocolException("Response contains no content");
            }

            T responseObj = JsonSingleton.GetInstance<T>(content);

            if (responseObj is Response)
            {
                Response responseInstance = responseObj as Response;
                responseInstance.SetHeaders(response.Headers);
            }

            return responseObj;
        }
    }
}