using Kin.Jwt.Extensions;
using Kin.Shared.Models.MarketPlace;
using Microsoft.IdentityModel.Tokens;

namespace Kin.Jwt.Models
{
    public class JwtSecurityKey : JwtKey
    {
        public JwtSecurityKey(string algorithm, string key) : base(algorithm, key)
        {}

        private SecurityKey _securityKey;
        public SecurityKey SecurityKey => _securityKey ?? (_securityKey = this.TryConvertPemToSecurityKey());
    }
}