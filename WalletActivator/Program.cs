using System;
using System.Collections.Generic;
using System.IO;
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
using Kin.Stellar.Sdk;
using Kin.Stellar.Sdk.responses;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace WalletActivator
{
    class Program
    {
        private static readonly JwtProvider MyAppJwtProvider;
        private static readonly JwtProviderBuilder JwtProviderBuilder;
        private readonly BlockChainHandler _blockChainHandler;
        private readonly Information _deviceInfo;

        private readonly KeyPair _keyPair;
        private readonly MarketPlaceClient _marketPlaceClient;
        private readonly JwtProvider _marketPlaceJwtProvider;
        private AuthToken _authToken;

        static void Main(string[] args)
        {
            _deviceInfo = new Information("KinCsharpClient", "Samsung9+", "Samsung", "Android");

            _marketPlaceClient = new MarketPlaceClient("https://api-prod.developers.kinecosystem.com/v1", _deviceInfo, AuthorizationHeaderValueGetter);

            Config config = _marketPlaceClient.Config().Result;
        }
    }
}
