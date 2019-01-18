using System;
using System.Threading.Tasks;
using Kin.BlockChain;
using Kin.Jwt;
using Kin.Marketplace;
using Kin.Marketplace.Models;
using Kin.Shared.Models.Device;
using Kin.Shared.Models.MarketPlace;
using Kin.Stellar.Sdk;

namespace kin_tooling_impl {
    class KinMarketPlaceUser
    {
        public  MarketPlaceClient MarketPlaceClient;
        public  JwtProvider MyAppJwtProvider;
        public  JwtProviderBuilder JwtProviderBuilder;
        public  BlockChainHandler BlockChainHandler;
        public  Information DeviceInfo;
        public  JwtProvider MarketPlaceJwtProvider;
        public  Config Config;
        public  KeyPair KeyPair;
        public  AuthToken AuthToken;
        public  string UserId;
        public  string MarketPlaceEndpoint;
        public  string AppId;


        public KinMarketPlaceUser(JwtProvider myAppJwtProvider, JwtProviderBuilder jwtProviderBuilder, BlockChainHandler blockChainHandler, Information deviceInfo, JwtProvider marketPlaceJwtProvider, Config config, string marketPlaceEndpoint, string appId)
        {
            MyAppJwtProvider = myAppJwtProvider;
            JwtProviderBuilder = jwtProviderBuilder;
            BlockChainHandler = blockChainHandler;
            DeviceInfo = deviceInfo;
            MarketPlaceJwtProvider = marketPlaceJwtProvider;
            Config = config;
            MarketPlaceEndpoint = marketPlaceEndpoint;
            AppId = appId;
            MarketPlaceClient = new MarketPlaceClient(marketPlaceEndpoint, deviceInfo, AuthorizationHeaderValueGetter);

            UserId = Guid.NewGuid().ToString().ToLower();
            KeyPair = KeyPair.Random();
        }

        public async Task<AuthToken> SignInAndActivate()
        {
            AuthToken = await MarketPlaceClient.Users(GetSignInData()).ConfigureAwait(false);
            AuthToken = await MarketPlaceClient.UsersMeActivate().ConfigureAwait(false);

            //Trusting the KIN asset
            await BlockChainHandler.TryUntilActivated(KeyPair).ConfigureAwait(false);

            return AuthToken;
        }
        private JwtSignInData GetSignInData()
        {
            string registerJwt = JwtProviderBuilder.Register.AddUserId(UserId).Jwt;
            JwtSignInData signInData = new JwtSignInData(DeviceInfo.XDeviceId, KeyPair.Address, registerJwt);
            return signInData;
        }

        private async Task<string> AuthorizationHeaderValueGetter()
        {
            if (AuthToken == null || DateTime.UtcNow > AuthToken.ExpirationDate)
            {
                AuthToken = await MarketPlaceClient.Users(GetSignInData()).ConfigureAwait(false);
            }

            if (string.IsNullOrEmpty(AuthToken.Token))
            {
                throw new Exception("oh nooooooooo");
            }


            return AuthToken.Token;
        }
    }
}