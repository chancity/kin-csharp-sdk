using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kin.Jwt;
using Kin.Jwt.Models;
using Kin.Marketplace;
using Kin.Shared.Models.Device;

namespace kin_csharp_sample_app
{
    class Program
    {
        static void Main(string[] args)
        {
            var deviceInfo = new Information();
            //Too lazy to fill with test key zzz
            var jwtProvider = new JwtProvider("test", new Dictionary<string, JwtSecurityKey>());
            var marketplace = new MarketPlaceClient("https://api.developers.kinecosystem.com/v1", deviceInfo, AuthorizationHeaderValueGetter);
        }

        private static Task<string> AuthorizationHeaderValueGetter()
        {
            throw new NotImplementedException();
        }
    }
}
