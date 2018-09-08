using System.Collections.Generic;
using System.Threading.Tasks;
using Kin.Jwt;
using Kin.Jwt.Models;
using Kin.Shared.Models.Device;

namespace Kin.Client
{
    public class Class1
    {
        public void weee()
        {
            var jwtProvider = new JwtProvider("dsds", new Dictionary<string, JwtSecurityKey>());
            var jwtProviderBuilder = new JwtProviderBuilder(jwtProvider);

            var earn = jwtProviderBuilder.Earn.AddOffer("ds", "ds").AddRecipient("d", "sd", "ds");

            var market = new Marketplace.MarketPlaceClient("", new Information(), AuthorizationHeaderValueGetter );
  


        }


        private Task<string> AuthorizationHeaderValueGetter()
        {
            throw new System.NotImplementedException();
        }
    }
 
}
