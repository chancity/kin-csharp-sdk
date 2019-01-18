using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Kin.BlockChain;
using Kin.Jwt;
using Kin.Jwt.Models;
using Kin.Marketplace;
using Kin.Shared.Models.Device;
using Kin.Shared.Models.MarketPlace;
using Kin.Tooling.Models;

namespace kin_tooling_impl
{
    class Program
    {
        public static JwtProvider MyAppJwtProvider;
        public static JwtProviderBuilder JwtProviderBuilder;
        public static BlockChainHandler BlockChainHandler;
        public static Information DeviceInfo;
        public static string MarketPlaceEndpoint;
        public static string AppId;
        public static JwtProvider MarketPlaceJwtProvider;
        public static Config Config;

        private static readonly Dictionary<string, JwtSecurityKey> SecurityKeys = new Dictionary<string, JwtSecurityKey>
        {
            {
                "rs512_0",
                new JwtSecurityKey("RS512",
                    Base64Decode(
                        "LS0tLS1CRUdJTiBSU0EgUFJJVkFURSBLRVktLS0tLQpNSUlDWGdJQkFBS0JnUURXM2c1QWN3anhkeWgrT0xpOE5IOWpPNEtFT3J1WG96Q2Joc1hMK0NZR0lHMXNZeWtrL1AxblBXaXk3cmxWSFd5NXg5QjJwWXRLYit2bS9EYVo1Z3BUcXpsYjlEOTY0eUtSdldsQ0xDT2p5TE4vemgxL3hIb2llUTUzMGt2ZVRZWSt0Wkh6MVF6WG5JT2x4RjZIanNjVThQdERXdzg1Q0tuZ3pnVkE5b1M0S3dJREFRQUJBb0dCQUp3cS91N0c3VndiUURvbFhkZWt6R1hDYmdWUGJ1TXl2L1I2U3k4SnFCRlI1bFlkNkZ5eTZEYnVRamV6SE04Sk9PbjZtY0J5WjcvdGd1YjZyM0RCNndRNkFMbXZtVnp0Q3Q5Qkg3ZVAxWC9WRlFhdnFZaG1yWEJmVEJWaEdtSy9xRHZCelpXSVFFTnVhU3NwQnp4cDdVamwwQ3VldnlmYlNnYi9zOEZsRXl4aEFrRUE3SjJlY1BIY29lTk1aYzM4OENpTFo5NENvd1NpNmlsM2xrT0RLOS9UWHR4L0x1VzNpVWh2U0hwOTB0Y01mSTdzV0d1WFVHWXF3dnUraWoyMGQ2ZW4yd0pCQU9oNFV4SVhOY1ljQWZhTGFDWWppNEdONEQ5ZnAzL3ZLM2h2azNmK1gzdUZnRFJTOHNwMFN1MHhZc3JZN0NNd0pCQ1ZIKytxZElvc0taNVJnU0VyQ2ZFQ1FRQ1p5dlVoeWtLeXdvOTBtRWV3UFZvbS84bE05Z1dDRjhQUDJqL1c4NXRxUy8wcW1Vc0xJeGFaMEd3Wjc0Y0JLdEI1eEN6TXFDdGhJc205QnRCVytaVURBa0JqYml1aHZqbXF6WW50YUwwWUt2WGRhTkIwYXJaYTJ2Sk41Zk0rVEplTVhwSnlUdFEzMGJ2R2Jld2lkTnV6UlVEM3NzRGhJcGdNRFUyVHdLcXBoQjRSQWtFQXArYTdyYTgwSmxiK3IxYWI5cmtPMExIZkhuZzJxbVJURHExaS9QUTdPVm9iL3F3VWtZUG43NURPZzJxT05GQm1FVWdjUVMvTVE5U0UxM21kc3pNdnp3PT0KLS0tLS1FTkQgUlNBIFBSSVZBVEUgS0VZLS0tLS0="))
            }
        };

        static void Init(string appId, Dictionary<string, JwtSecurityKey> securityKeys, string marketPlaceEndpoint)
        {
            AppId = appId;
            MarketPlaceEndpoint = marketPlaceEndpoint;
            MyAppJwtProvider = new JwtProvider(appId, SecurityKeys);
            JwtProviderBuilder = new JwtProviderBuilder(MyAppJwtProvider);

            DeviceInfo = new Information("KinCsharpClient", "KinMetricClient", "chancity", "Windows", "zomg");

            var marketPlaceClient = new MarketPlaceClient(MarketPlaceEndpoint, DeviceInfo, null);


            Config = marketPlaceClient.Config().Result;

            Dictionary<string, JwtSecurityKey> kinsKeys = new Dictionary<string, JwtSecurityKey>();

            foreach (KeyValuePair<string, JwtKey> configJwtKey in Config.JwtKeys)
            {
                kinsKeys.Add(configJwtKey.Key,
                    new JwtSecurityKey(configJwtKey.Value.Algorithm, configJwtKey.Value.Key));
            }

            MarketPlaceJwtProvider = new JwtProvider("kin", kinsKeys);
            BlockChainHandler = new BlockChainHandler(Config, appId);
        }
        static void Main(string[] args)
        {
            Init("test", SecurityKeys, "https://api.developers.kinecosystem.com/v1");

            var kinMetricClient = new KinMetricClient();

            kinMetricClient.OnNewMetricEvent += delegate(IMetric metric)
            {
                Console.WriteLine(metric.ToString());
                return Task.CompletedTask;
            };

            var wtf = new KinMarketPlaceUser(MyAppJwtProvider, JwtProviderBuilder, BlockChainHandler, DeviceInfo,
                MarketPlaceJwtProvider, Config, MarketPlaceEndpoint, AppId);


            kinMetricClient.AddGather(new KinNewUserMetricGather(wtf));


            kinMetricClient.StartAsync().Wait();
            Console.WriteLine("hmm");
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
