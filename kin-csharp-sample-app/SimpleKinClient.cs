using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kin.BlockChain;
using Kin.Jwt;
using Kin.Jwt.JwtPayloadBuilders;
using Kin.Jwt.Models;
using Kin.Marketplace;
using Kin.Marketplace.Models;
using Kin.Shared.Models.Device;
using Kin.Shared.Models.MarketPlace;
using Kin.Stellar.Sdk;
using Kin.Stellar.Sdk.responses;
using Microsoft.IdentityModel.Tokens;

namespace kin_csharp_sample_app
{
    internal class SimpleKinClient
    {
        private static readonly Dictionary<string, JwtSecurityKey> SecurityKeys = new Dictionary<string, JwtSecurityKey>
        {
            {
                "rs512_0",
                new JwtSecurityKey("RS512",
                    Base64Decode(
                        "LS0tLS1CRUdJTiBSU0EgUFJJVkFURSBLRVktLS0tLQpNSUlDWGdJQkFBS0JnUURXM2c1QWN3anhkeWgrT0xpOE5IOWpPNEtFT3J1WG96Q2Joc1hMK0NZR0lHMXNZeWtrL1AxblBXaXk3cmxWSFd5NXg5QjJwWXRLYit2bS9EYVo1Z3BUcXpsYjlEOTY0eUtSdldsQ0xDT2p5TE4vemgxL3hIb2llUTUzMGt2ZVRZWSt0Wkh6MVF6WG5JT2x4RjZIanNjVThQdERXdzg1Q0tuZ3pnVkE5b1M0S3dJREFRQUJBb0dCQUp3cS91N0c3VndiUURvbFhkZWt6R1hDYmdWUGJ1TXl2L1I2U3k4SnFCRlI1bFlkNkZ5eTZEYnVRamV6SE04Sk9PbjZtY0J5WjcvdGd1YjZyM0RCNndRNkFMbXZtVnp0Q3Q5Qkg3ZVAxWC9WRlFhdnFZaG1yWEJmVEJWaEdtSy9xRHZCelpXSVFFTnVhU3NwQnp4cDdVamwwQ3VldnlmYlNnYi9zOEZsRXl4aEFrRUE3SjJlY1BIY29lTk1aYzM4OENpTFo5NENvd1NpNmlsM2xrT0RLOS9UWHR4L0x1VzNpVWh2U0hwOTB0Y01mSTdzV0d1WFVHWXF3dnUraWoyMGQ2ZW4yd0pCQU9oNFV4SVhOY1ljQWZhTGFDWWppNEdONEQ5ZnAzL3ZLM2h2azNmK1gzdUZnRFJTOHNwMFN1MHhZc3JZN0NNd0pCQ1ZIKytxZElvc0taNVJnU0VyQ2ZFQ1FRQ1p5dlVoeWtLeXdvOTBtRWV3UFZvbS84bE05Z1dDRjhQUDJqL1c4NXRxUy8wcW1Vc0xJeGFaMEd3Wjc0Y0JLdEI1eEN6TXFDdGhJc205QnRCVytaVURBa0JqYml1aHZqbXF6WW50YUwwWUt2WGRhTkIwYXJaYTJ2Sk41Zk0rVEplTVhwSnlUdFEzMGJ2R2Jld2lkTnV6UlVEM3NzRGhJcGdNRFUyVHdLcXBoQjRSQWtFQXArYTdyYTgwSmxiK3IxYWI5cmtPMExIZkhuZzJxbVJURHExaS9QUTdPVm9iL3F3VWtZUG43NURPZzJxT05GQm1FVWdjUVMvTVE5U0UxM21kc3pNdnp3PT0KLS0tLS1FTkQgUlNBIFBSSVZBVEUgS0VZLS0tLS0="))
            }
        };

        private static readonly JwtProvider MyAppJwtProvider;
        private static readonly JwtProviderBuilder JwtProviderBuilder;
        private readonly BlockChainHandler _blockChainHandler;
        private readonly Information _deviceInfo;

        private readonly KeyPair _keyPair;
        private readonly MarketPlaceClient _marketPlaceClient;
        private readonly JwtProvider _marketPlaceJwtProvider;
        private AuthToken _authToken;

        public string UserId { get; }

        static SimpleKinClient()
        {
            MyAppJwtProvider = new JwtProvider("test", SecurityKeys);
            JwtProviderBuilder = new JwtProviderBuilder(MyAppJwtProvider);
        }

        public SimpleKinClient()
        {
            _deviceInfo = new Information("KinCsharpClient", "BlazorWebApp", "Chrome", "Windows");

            _marketPlaceClient = new MarketPlaceClient("https://api.developers.kinecosystem.com/v1", _deviceInfo,
                AuthorizationHeaderValueGetter);

            Config config = _marketPlaceClient.Config().Result;

            Dictionary<string, JwtSecurityKey> kinsKeys = new Dictionary<string, JwtSecurityKey>();

            foreach (KeyValuePair<string, JwtKey> configJwtKey in config.JwtKeys)
            {
                kinsKeys.Add(configJwtKey.Key,
                    new JwtSecurityKey(configJwtKey.Value.Algorithm, configJwtKey.Value.Key));
            }

            UserId = Guid.NewGuid().ToString();
            _keyPair = KeyPair.Random();


            _marketPlaceJwtProvider = new JwtProvider("kin", kinsKeys);
            _blockChainHandler = new BlockChainHandler(config, "test");
        }

        public async Task FirstTest()
        {
            //   var orders1 = await _marketPlaceClient.GetOrderHistory();
            //   Console.WriteLine(JsonConvert.SerializeObject(orders1, Formatting.Indented));
            //Creating a market place user
            _authToken = await _marketPlaceClient.Users(GetSignInData()).ConfigureAwait(false);
            _authToken = await _marketPlaceClient.UsersMeActivate().ConfigureAwait(false);

            //Trusting the KIN asset
            await _blockChainHandler.TryUntilActivated(_keyPair).ConfigureAwait(false);

            //Completing the tutorial!
            await DoFirstOffer().ConfigureAwait(false);

            //Sending that p2p good stuff
            //await DoP2POffer().ConfigureAwait(false);

            //External speeeeeend offer"
            // await DoExternalSpendOffer().ConfigureAwait(false);

            //External earn offerrrr"
            await DoExternalEarnOffer().ConfigureAwait(false);


            OrderList orders = await _marketPlaceClient.GetOrderHistory().ConfigureAwait(false);
            // Console.WriteLine(UserId + "first order:\n" + JsonConvert.SerializeObject(orders.Orders.First(), Formatting.Indented));
            double balance = await _blockChainHandler.GetKinBalance(_keyPair).ConfigureAwait(false);

            Console.WriteLine(balance);

            //    await _blockChainHandler.SendPayment(_keyPair, "GBY5PZFDZ6Y25S6YRRZ3CXOAIUWOZ3ADONFY2OYCA7GPQCPPF2RDXXZC",
            //       balance);
        }

        private async Task DoFirstOffer()
        {
            OfferList offers = await _marketPlaceClient.GetOffers().ConfigureAwait(false);

            //lets do the tutorial!
            Offer offer = offers.Offers.SingleOrDefault(o => o.ContentType.Equals("tutorial"));

            //This won't go through because the Kin asset isn't trusted yet
            if (offer != null)
            {
                OpenOrder orderResponse = await _marketPlaceClient.CreateOrderForOffer(offer.Id).ConfigureAwait(false);

                Order submitOrder = await _marketPlaceClient.SubmitOrder(orderResponse.Id).ConfigureAwait(false);

                Order finishedOrder = await WaitForOrderCompletion(UserId, submitOrder.Id).ConfigureAwait(false);

                if (!string.IsNullOrEmpty(finishedOrder?.OrderResult?.Jwt))
                {
                    SecurityToken token = _marketPlaceJwtProvider.ValidateJwtToken(finishedOrder?.OrderResult?.Jwt);
                }
            }
        }

        public async Task DoP2POffer(string toUserId = null)
        {
            P2PBuilder p2POffer = JwtProviderBuilder.P2P
                .AddOffer("p2p-" + toUserId, 1)
                .AddSender(UserId, "p2p", "to myself")
                .AddRecipient(toUserId ?? UserId, "p2p", "to him?");

            Console.WriteLine(p2POffer.ToString());

            OpenOrder createExternalP2POffer =
                await _marketPlaceClient.CreateExternalOffer(p2POffer.Jwt).ConfigureAwait(false);

            Order submitP2POffer =
                await _marketPlaceClient.SubmitOrder(createExternalP2POffer.Id).ConfigureAwait(false);

            SubmitTransactionResponse submitTransactionResponse = await _blockChainHandler
                .SendPayment(_keyPair, submitP2POffer.BlockChainData.RecipientAddress, submitP2POffer.Amount,
                    submitP2POffer.Id).ConfigureAwait(false);

            Order finishedOrder = await WaitForOrderCompletion(UserId, submitP2POffer.Id).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(finishedOrder?.OrderResult?.Jwt))
            {
                SecurityToken token = _marketPlaceJwtProvider.ValidateJwtToken(finishedOrder?.OrderResult?.Jwt);
            }
        }

        private async Task DoExternalSpendOffer()
        {
            string externalSpendOffer = JwtProviderBuilder.Spend
                .AddOffer("spendit", 111)
                .AddSender(UserId, "speeend it", "block chain sutff isn't coded yet lawl, i'm neva goonna spend")
                .Jwt;

            OpenOrder createExternalSpendOffer =
                await _marketPlaceClient.CreateExternalOffer(externalSpendOffer).ConfigureAwait(false);

            Order submitSpendOffer =
                await _marketPlaceClient.SubmitOrder(createExternalSpendOffer.Id).ConfigureAwait(false);

            SubmitTransactionResponse submitTransactionResponse = await _blockChainHandler.SendPayment(_keyPair,
                submitSpendOffer.BlockChainData.RecipientAddress,
                submitSpendOffer.Amount, submitSpendOffer.Id).ConfigureAwait(false);


            Order finishedOrder = await WaitForOrderCompletion(UserId, submitSpendOffer.Id).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(finishedOrder?.OrderResult?.Jwt))
            {
                SecurityToken token = _marketPlaceJwtProvider.ValidateJwtToken(finishedOrder?.OrderResult?.Jwt);
            }
        }

        private async Task DoExternalEarnOffer()
        {
            string externalEarnOffer = JwtProviderBuilder.Earn
                .AddOffer("earrrnit", 2)
                .AddRecipient(UserId, "earrrnit it", "block chain sutff isn't coded yet lawl, i'm neva goonna earn")
                .Jwt;

            OpenOrder createExternalEarnOffer =
                await _marketPlaceClient.CreateExternalOffer(externalEarnOffer).ConfigureAwait(false);

            Order submitEarnOffer =
                await _marketPlaceClient.SubmitOrder(createExternalEarnOffer.Id).ConfigureAwait(false);

            Order finishedOrder = await WaitForOrderCompletion(UserId, submitEarnOffer.Id).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(finishedOrder?.OrderResult?.Jwt))
            {
                SecurityToken token = _marketPlaceJwtProvider.ValidateJwtToken(finishedOrder?.OrderResult?.Jwt);
            }
        }

        public async Task<Order> WaitForOrderCompletion(string userId, string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                Console.WriteLine($"WaitForOrderCompletion order id is null for order {orderId}");
                return null;
            }

            int tries = 15;
            Order orderResponse = null;

            do
            {
                tries--;

                try
                {
                    orderResponse = await _marketPlaceClient.GetOrder(orderId).ConfigureAwait(false);

                    if (orderResponse.Status == OrderStatusEnum.Pending)
                    {
                        await Task.Delay(1000).ConfigureAwait(false);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            } while (orderResponse?.Status == OrderStatusEnum.Pending && tries > 0);

            return orderResponse;
        }

        private JwtSignInData GetSignInData()
        {
            string registerJwt = JwtProviderBuilder.Register.AddUserId(UserId).Jwt;
            JwtSignInData signInData = new JwtSignInData(_deviceInfo.XDeviceId, _keyPair.Address, registerJwt);
            return signInData;
        }

        private async Task<string> AuthorizationHeaderValueGetter()
        {
            if (_authToken == null || DateTime.UtcNow > _authToken.ExpirationDate)
            {
                _authToken = await _marketPlaceClient.Users(GetSignInData()).ConfigureAwait(false);
            }

            if (string.IsNullOrEmpty(_authToken.Token))
            {
                throw new Exception("oh nooooooooo");
            }


            return _authToken.Token;
        }

        private static string Base64Decode(string base64)
        {
            if (string.IsNullOrEmpty(base64))
            {
                throw new ArgumentNullException();
            }

            byte[] data = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(data);
        }
    }
}