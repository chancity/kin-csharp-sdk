using Kin.Jwt.Models;

namespace Kin.Jwt.JwtPayloadBuilders
{
    public class RegisterBuilder : BaseBuilder
    {
        internal RegisterBuilder(JwtProvider provider) : base(provider, SubjectNames.Register, 1) { }

        public RegisterBuilder AddUserId(string userId)
        {
            KinJwtPayload payload = new KinJwtPayload(ClaimNames.UserId, userId);
            AddPayload(payload);

            return this;
        }
    }
}