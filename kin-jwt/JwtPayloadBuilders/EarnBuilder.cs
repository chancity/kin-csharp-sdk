using Kin.Jwt.Models;

namespace Kin.Jwt.JwtPayloadBuilders
{
    public class EarnBuilder : BaseBuilder
    {
        internal EarnBuilder(JwtProvider provider) : base(provider, SubjectNames.Earn, 2) { }

        public EarnBuilder AddOffer(string offerId, int amount)
        {
            JwtBodyPartOffer offer = new JwtBodyPartOffer(offerId, amount);

            KinJwtPayload<JwtBodyPartOffer> offerPayload =
                new KinJwtPayload<JwtBodyPartOffer>(ClaimNames.Offer, offer);
            AddPayload(offerPayload);
            return this;
        }

        public EarnBuilder AddRecipient(string userId, string title, string description)
        {
            JwtBodyPartRecipient recipient = new JwtBodyPartRecipient(userId, title, description);

            KinJwtPayload<JwtBodyPartRecipient> payload =
                new KinJwtPayload<JwtBodyPartRecipient>(ClaimNames.Recipient, recipient);
            AddPayload(payload);
            return this;
        }
    }
}