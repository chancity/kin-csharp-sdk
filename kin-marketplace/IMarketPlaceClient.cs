using System.Threading.Tasks;
using Kin.Marketplace.Models;
using Kin.Shared.Models.MarketPlace;
using Refit;

namespace Kin.Marketplace
{
    [Headers("User-Agent: KinCSharpClient", "Api-Version: 1")]
    public interface IMarketPlaceClient
    {
        [Get("/config")]
        Task<Config> Config();

        [Post("/users")]
        Task<AuthToken> Users([Body] CommonSignInData commonSignInData);

        [Post("/users/me/activate")]
        [Headers("Authorization: Bearer")]
        Task<AuthToken> UsersMeActivate();

        [Get("/offers")]
        [Headers("Authorization: Bearer")]
        Task<OfferList> GetOffers();

        [Get("/orders")]
        [Headers("Authorization: Bearer")]
        Task<OrderList> GetOrderHistory();

        [Get("/orders/{order_id}")]
        [Headers("Authorization: Bearer")]
        Task<Order> GetOrder([AliasAs("order_id")] string orderId);

        [Patch("/orders/{order_id}")]
        [Headers("Authorization: Bearer")]
        Task<Order> ChangeOrder([AliasAs("order_id")] string orderId, [Body] MarketPlaceError marketPlaceError);

        [Post("/orders/{order_id}")]
        [Headers("Authorization: Bearer")]
        Task<Order> SubmitOrder([AliasAs("order_id")] string orderId, [Body] object body);

        [Delete("/orders/{order_id}")]
        [Headers("Authorization: Bearer")]
        Task<string> CancelOrder([AliasAs("order_id")] string orderId);

        [Post("/offers/external/orders")]
        [Headers("Authorization: Bearer")]
        Task<OpenOrder> CreateExternalOrder([Body] object jwtRequest);

        [Post("/offers/{offer_id}/orders")]
        [Headers("Authorization: Bearer")]
        Task<OpenOrder> CreateMarketPlaceOrder([AliasAs("offer_id")] string offerId);
    }
}