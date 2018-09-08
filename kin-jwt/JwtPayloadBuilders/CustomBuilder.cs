using Kin.Jwt.Models;

namespace Kin.Jwt.JwtPayloadBuilders
{
    public class CustomBuilder : BaseBuilder
    {
        internal CustomBuilder(JwtProvider provider, string subject) : base(provider, subject, 2) { }

        protected void Add(KinJwtPayload payload)
        {
            AddPayload(payload);
        }
    }
}