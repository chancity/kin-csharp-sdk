using System.Collections.Generic;
using Kin.Jwt.JwtPayloadBuilders;
using Kin.Jwt.Models;

namespace Kin.Jwt
{
    public class JwtProviderBuilder
    {
        private readonly JwtProvider _provider;

        public JwtProviderBuilder(JwtProvider provider)
        {
            _provider = provider;
        }

        public RegisterBuilder Register => new RegisterBuilder(_provider);


        public P2PBuilder P2P => new P2PBuilder(_provider);


        public SpendBuilder Spend => new SpendBuilder(_provider);


        public  EarnBuilder Earn => new EarnBuilder(_provider);

        public CustomBuilder Custom(string subject)
        {
            return new CustomBuilder(_provider, subject);
        }

    }
}