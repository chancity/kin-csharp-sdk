using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kin.BlockChain;
using Kin.Jwt;
using Kin.Jwt.Models;
using Kin.Marketplace;
using Kin.Marketplace.Models;
using Kin.Shared.Models.Device;
using Kin.Shared.Models.MarketPlace;
using Newtonsoft.Json;
using stellar_dotnet_sdk;

namespace kin_csharp_sample_app
{
    internal class Program
    {
        private static readonly Dictionary<string, JwtSecurityKey> SecurityKeys =
            new Dictionary<string, JwtSecurityKey>
            {
                {
                    "rs512_0",
                    new JwtSecurityKey("RS512",
                        Base64Decode(
                            "LS0tLS1CRUdJTiBSU0EgUFJJVkFURSBLRVktLS0tLQpNSUlDWGdJQkFBS0JnUURXM2c1QWN3anhkeWgrT0xpOE5IOWpPNEtFT3J1WG96Q2Joc1hMK0NZR0lHMXNZeWtrL1AxblBXaXk3cmxWSFd5NXg5QjJwWXRLYit2bS9EYVo1Z3BUcXpsYjlEOTY0eUtSdldsQ0xDT2p5TE4vemgxL3hIb2llUTUzMGt2ZVRZWSt0Wkh6MVF6WG5JT2x4RjZIanNjVThQdERXdzg1Q0tuZ3pnVkE5b1M0S3dJREFRQUJBb0dCQUp3cS91N0c3VndiUURvbFhkZWt6R1hDYmdWUGJ1TXl2L1I2U3k4SnFCRlI1bFlkNkZ5eTZEYnVRamV6SE04Sk9PbjZtY0J5WjcvdGd1YjZyM0RCNndRNkFMbXZtVnp0Q3Q5Qkg3ZVAxWC9WRlFhdnFZaG1yWEJmVEJWaEdtSy9xRHZCelpXSVFFTnVhU3NwQnp4cDdVamwwQ3VldnlmYlNnYi9zOEZsRXl4aEFrRUE3SjJlY1BIY29lTk1aYzM4OENpTFo5NENvd1NpNmlsM2xrT0RLOS9UWHR4L0x1VzNpVWh2U0hwOTB0Y01mSTdzV0d1WFVHWXF3dnUraWoyMGQ2ZW4yd0pCQU9oNFV4SVhOY1ljQWZhTGFDWWppNEdONEQ5ZnAzL3ZLM2h2azNmK1gzdUZnRFJTOHNwMFN1MHhZc3JZN0NNd0pCQ1ZIKytxZElvc0taNVJnU0VyQ2ZFQ1FRQ1p5dlVoeWtLeXdvOTBtRWV3UFZvbS84bE05Z1dDRjhQUDJqL1c4NXRxUy8wcW1Vc0xJeGFaMEd3Wjc0Y0JLdEI1eEN6TXFDdGhJc205QnRCVytaVURBa0JqYml1aHZqbXF6WW50YUwwWUt2WGRhTkIwYXJaYTJ2Sk41Zk0rVEplTVhwSnlUdFEzMGJ2R2Jld2lkTnV6UlVEM3NzRGhJcGdNRFUyVHdLcXBoQjRSQWtFQXArYTdyYTgwSmxiK3IxYWI5cmtPMExIZkhuZzJxbVJURHExaS9QUTdPVm9iL3F3VWtZUG43NURPZzJxT05GQm1FVWdjUVMvTVE5U0UxM21kc3pNdnp3PT0KLS0tLS1FTkQgUlNBIFBSSVZBVEUgS0VZLS0tLS0="))
                }
            };

        private static AuthToken _authToken;
        private static readonly Information DeviceInfo;
        private static readonly JwtProvider JwtProvider;
        private static readonly JwtProviderBuilder JwtProviderBuilder;
        private static readonly MarketPlaceClient MarketPlaceClient;
        private static readonly BlockChainHandler BlockChainHandler;
        private static readonly Config Config;
        private static readonly string UserId;
        private static readonly KeyPair KeyPair;

        static Program()
        {
            DeviceInfo = new Information();
            JwtProvider = new JwtProvider("test", SecurityKeys);
            JwtProviderBuilder = new JwtProviderBuilder(JwtProvider);

            MarketPlaceClient = new MarketPlaceClient("https://api.developers.kinecosystem.com/v1", DeviceInfo,
                AuthorizationHeaderValueGetter);

            Config = MarketPlaceClient.Config().Result;

            BlockChainHandler = new BlockChainHandler(Config);

            UserId = Guid.NewGuid().ToString();
            KeyPair = KeyPair.Random();
        }

        private static void Main(string[] args)
        {
            try
            {
                Test().Wait();
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;

                if (e is MarketPlaceException mex)
                {
                    Console.WriteLine(mex.MarketPlaceError);
                }
                else
                {
                    throw;
                }
            }

            Console.ReadLine();
        }

        private static async Task Test()
        {
            Console.WriteLine("Creating a market place user");
            _authToken = await MarketPlaceClient.Users(GetSignInData()).ConfigureAwait(false);
            _authToken = await MarketPlaceClient.UsersMeActivate().ConfigureAwait(false);

            Console.WriteLine("Trusting the KIN asset");
            await BlockChainHandler.TryUntilActivated(KeyPair).ConfigureAwait(false);
            Console.WriteLine("Finally the wallet is trusted....");


            Console.WriteLine("Completing the tutorial!");
            await DoFirstOffer().ConfigureAwait(false);
            Console.WriteLine($"Kin Balance: {await BlockChainHandler.GetKinBalance(KeyPair).ConfigureAwait(false)}");

            Console.WriteLine("Sending that p2p good stuff");
            await DoP2pOffer().ConfigureAwait(false);
            Console.WriteLine($"Kin Balance: {await BlockChainHandler.GetKinBalance(KeyPair).ConfigureAwait(false)}");

            Console.WriteLine("External speeeeeend offer");
            await DoExternalSpendOffer().ConfigureAwait(false);
            Console.WriteLine($"Kin Balance: {await BlockChainHandler.GetKinBalance(KeyPair).ConfigureAwait(false)}");

            Console.WriteLine("External earn offerrrr");
            await DoExternalEarnOffer().ConfigureAwait(false);
            Console.WriteLine($"Kin Balance: {await BlockChainHandler.GetKinBalance(KeyPair).ConfigureAwait(false)}");


            OrderList orders = await MarketPlaceClient.GetOrderHistory().ConfigureAwait(false);
            Console.WriteLine(JsonConvert.SerializeObject(orders, Formatting.Indented));
        }

        private static JwtSignInData GetSignInData()
        {
            string registerJwt = JwtProviderBuilder.Register.AddUserId(UserId).Jwt;
            return new JwtSignInData(DeviceInfo.XDeviceId, KeyPair.Address, registerJwt);
        }

        private static async Task DoFirstOffer()
        {
            OfferList offers = await MarketPlaceClient.GetOffers().ConfigureAwait(false);

            //lets do the tutorial!
            Offer offer = offers.Offers.SingleOrDefault(o => o.ContentType.Equals("tutorial"));

            //This won't go through because the Kin asset isn't trusted yet
            if (offer != null)
            {
                OpenOrder orderResponse = await MarketPlaceClient.CreateOrderForOffer(offer.Id).ConfigureAwait(false);

                Order submitOrder = await MarketPlaceClient.SubmitOrder(orderResponse.Id).ConfigureAwait(false);

                await WaitForOrderCompletion(UserId, submitOrder.Id).ConfigureAwait(false);
            }
        }

        private static async Task DoP2pOffer()
        {
            string p2POffer = JwtProviderBuilder.P2P
                .AddOffer("p2p", "1")
                .AddSender(UserId, "p2p", "to myself")
                .AddRecipient(UserId, "p2p", "to him?")
                .Jwt;

            OpenOrder createExternalP2POffer =
                await MarketPlaceClient.CreateExternalOffer(p2POffer).ConfigureAwait(false);

            Order submitP2POffer = await MarketPlaceClient.SubmitOrder(createExternalP2POffer.Id).ConfigureAwait(false);

            var submitTransactionResponse = await BlockChainHandler.SendPayment(KeyPair, submitP2POffer.BlockChainData.RecipientAddress,
                submitP2POffer.Amount, submitP2POffer.Id).ConfigureAwait(false);

            await WaitForOrderCompletion(UserId, submitP2POffer.Id).ConfigureAwait(false);
        }

        private static async Task DoExternalSpendOffer()
        {
            string externalSpendOffer = JwtProviderBuilder.Spend
                .AddOffer("spendit", "111")
                .AddSender(UserId, "speeend it", "block chain sutff isn't coded yet lawl, i'm neva goonna spend")
                .Jwt;

            OpenOrder createExternalSpendOffer =
                await MarketPlaceClient.CreateExternalOffer(externalSpendOffer).ConfigureAwait(false);

            Order submitSpendOffer =
                await MarketPlaceClient.SubmitOrder(createExternalSpendOffer.Id).ConfigureAwait(false);

          var submitTransactionResponse = await BlockChainHandler.SendPayment(KeyPair, submitSpendOffer.BlockChainData.RecipientAddress,
                submitSpendOffer.Amount, submitSpendOffer.Id).ConfigureAwait(false);

            await WaitForOrderCompletion(UserId, submitSpendOffer.Id).ConfigureAwait(false);
        }

        private static async Task DoExternalEarnOffer()
        {
            string externalEarnOffer = JwtProviderBuilder.Earn
                .AddOffer("earrrnit", "111")
                .AddRecipient(UserId, "earrrnit it", "block chain sutff isn't coded yet lawl, i'm neva goonna earn")
                .Jwt;

            OpenOrder createExternalEarnOffer =
                await MarketPlaceClient.CreateExternalOffer(externalEarnOffer).ConfigureAwait(false);

            Order submitEarnOffer =
                await MarketPlaceClient.SubmitOrder(createExternalEarnOffer.Id).ConfigureAwait(false);

            await WaitForOrderCompletion(UserId, submitEarnOffer.Id).ConfigureAwait(false);
        }

        private static async Task<string> AuthorizationHeaderValueGetter()
        {
            if (_authToken == null || DateTime.UtcNow > _authToken.ExpirationDate)
            {
                _authToken = await MarketPlaceClient.Users(GetSignInData()).ConfigureAwait(false);
            }

            if (string.IsNullOrEmpty(_authToken.Token))
            {
                throw new Exception("oh nooooooooo");
            }


            return _authToken.Token;
        }

        public static async Task WaitForOrderCompletion(string userId, string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                Console.WriteLine($"WaitForOrderCompletion order id is null for tx {orderId}");
                return;
            }

            int tries = 15;
            Order orderResponse = null;

            do
            {
                tries--;

                try
                {
                    orderResponse = await MarketPlaceClient.GetOrder(orderId).ConfigureAwait(false);

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