using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Kin.Stellar.Sdk.requests;
using Nett;

namespace Kin.Stellar.Sdk.federation
{
    /// <summary>
    ///     FederationServer handles a network connection to a
    ///     federation server (https://www.stellar.org/developers/learn/concepts/federation.html)
    ///     instance and exposes an interface for requests to that instance.
    ///     For resolving a stellar address without knowing which federation server
    ///     to query use Federation#resolve(String).
    ///     See Federation docs: https://www.stellar.org/developers/learn/concepts/federation.html
    /// </summary>
    public class FederationServer : IDisposable
    {
        private static HttpClient _httpClient;

        public Uri ServerUri { get; }

        public string Domain { get; }

        public HttpClient HttpClient
        {
            set => _httpClient = value;
        }

        public FederationServer(Uri serverUri, string domain)
        {
            if (serverUri.Scheme != "https")
            {
                throw new FederationServerInvalidException();
            }

            ServerUri = serverUri;

            if (Uri.CheckHostName(domain) == UriHostNameType.Unknown)
            {
                throw new ArgumentException("Invalid internet domain name supplied.", nameof(domain));
            }

            Domain = domain;
        }

        public FederationServer(string serverUri, string domain)
            : this(new Uri(serverUri), domain) { }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        /// <summary>
        ///     reates a <see cref="FederationServer" /> instance for a given domain.
        ///     It tries to find a federation server URL in stellar.toml file.
        ///     See: https://www.stellar.org/developers/learn/concepts/stellar-toml.html
        /// </summary>
        /// <param name="domain">Domain to find a federation server for</param>
        /// <returns>
        ///     <see cref="FederationServer" />
        /// </returns>
        public static async Task<FederationServer> CreateForDomain(string domain)
        {
            StringBuilder uriBuilder = new StringBuilder();
            uriBuilder.Append("https://");
            uriBuilder.Append(domain);
            uriBuilder.Append("/.well-known/stellar.toml");
            Uri stellarTomUri = new Uri(uriBuilder.ToString());

            TomlTable stellarToml;

            try
            {
                HttpResponseMessage response =
                    await _httpClient.GetAsync(stellarTomUri, HttpCompletionOption.ResponseContentRead);

                if ((int) response.StatusCode >= 300)
                {
                    throw new StellarTomlNotFoundInvalidException();
                }

                string responseToml = await response.Content.ReadAsStringAsync();
                stellarToml = Toml.ReadString(responseToml);
            }
            catch (HttpRequestException e)
            {
                throw new ConnectionErrorException(e.Message);
            }

            string federationServer = stellarToml.Rows.Single(a => a.Key == "FEDERATION_SERVER").Value.Get<string>();

            if (string.IsNullOrWhiteSpace(federationServer))
            {
                throw new NoFederationServerException();
            }

            return new FederationServer(federationServer, domain);
        }

        public async Task<FederationResponse> ResolveAddress(string address)
        {
            string[] tokens = Regex.Split(address, "\\*");

            if (tokens.Length != 2)
            {
                throw new MalformedAddressException();
            }

            UriBuilder uriBuilder = new UriBuilder(ServerUri);
            uriBuilder.SetQueryParam("type", "name");
            uriBuilder.SetQueryParam("q", address);
            Uri uri = uriBuilder.Uri;

            try
            {
                ResponseHandler<FederationResponse> federationResponse = new ResponseHandler<FederationResponse>();

                HttpResponseMessage response = await _httpClient.GetAsync(uri);
                return await federationResponse.HandleResponse(response);
            }
            catch (HttpResponseException e)
            {
                if (e.StatusCode == 404)
                {
                    throw new NotFoundException();
                }

                throw new ServerErrorException();
            }
            catch (Exception e)
            {
                throw new ConnectionErrorException(e.Message);
            }
        }
    }
}