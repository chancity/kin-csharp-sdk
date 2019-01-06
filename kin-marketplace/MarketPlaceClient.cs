using System;
using System.Net.Http;
using System.Threading.Tasks;
using Kin.Marketplace.Models;
using Kin.Shared.Models.Device;
using Kin.Shared.Models.MarketPlace;
using Newtonsoft.Json;
using Refit;

namespace Kin.Marketplace
{
    public class MarketPlaceClient
    {
        private readonly IMarketPlaceClient _apiClient;

        public MarketPlaceClient(string baseEndPoint, Information info,
            Func<Task<string>> authorizationHeaderValueGetter, HttpMessageHandler innerHandler = null)
        {
            MarketPlaceHttpHeaders marketPlaceHttpHeaders = new MarketPlaceHttpHeaders(info);

            RefitSettings refitSetting = new RefitSettings
            {
                HttpMessageHandlerFactory = () => new MarketPlaceHeadersHandler(marketPlaceHttpHeaders, innerHandler),
                JsonSerializerSettings = new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore},
                AuthorizationHeaderValueGetter = authorizationHeaderValueGetter
            };

            _apiClient = RestService.For<IMarketPlaceClient>(baseEndPoint, refitSetting);
            //var httpHandler = new HttpClientHandler();
            //httpHandler.Proxy = new WebProxy("http://192.168.1.9:9000");
            //var httpClient = new HttpClient(httpHandler);
            //httpClient.BaseAddress = new Uri(baseEndPoint);
            //
            //
            //_apiClient = RestService.For<IMarketPlaceClient>(httpClient, refitSetting);
        }

        public async Task<Config> Config()
        {
            return await _apiClient.Config().ConfigureAwait(false);
        }

        public async Task<OfferList> GetOffers()
        {
            try
            {
                return await _apiClient.GetOffers().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<AuthToken> Users(CommonSignInData commonSignInData)
        {
            return await _apiClient.Users(commonSignInData).ConfigureAwait(false);
        }

        public async Task<AuthToken> UsersMeActivate()
        {
            return await _apiClient.UsersMeActivate().ConfigureAwait(false);
        }

        public async Task<OrderList> GetOrderHistory()
        {
            return await _apiClient.GetOrderHistory().ConfigureAwait(false);
        }

        public async Task<Order> GetOrder(string orderId)
        {
            return await _apiClient.GetOrder(orderId).ConfigureAwait(false);
        }

        public async Task<Order> SubmitOrder(string orderId, string content = null)
        {
            return await _apiClient.SubmitOrder(orderId, new {content}).ConfigureAwait(false);
        }

        public async Task<bool> CancelOrder(string orderId)
        {
            string ret = await _apiClient.CancelOrder(orderId).ConfigureAwait(false);
            return string.IsNullOrEmpty(ret);
        }

        public async Task<Order> ChangeOrder(string orderId, MarketPlaceError marketPlaceError)
        {
            return await _apiClient.ChangeOrder(orderId, marketPlaceError).ConfigureAwait(false);
        }

        public async Task<OpenOrder> CreateExternalOffer(string jwt)
        {
            return await _apiClient.CreateExternalOrder(new {jwt}).ConfigureAwait(false);
        }

        public async Task<OpenOrder> CreateOrderForOffer(string offerId)
        {
            return await _apiClient.CreateMarketPlaceOrder(offerId).ConfigureAwait(false);
        }
    }
}