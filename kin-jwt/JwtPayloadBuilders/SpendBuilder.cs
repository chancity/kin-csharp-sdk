using Kin.Jwt.Models;

namespace Kin.Jwt.JwtPayloadBuilders
{
    public class SpendBuilder : BaseBuilder
    {
        internal SpendBuilder(JwtProvider provider) : base(provider, SubjectNames.Spend, 2) { }

        public SpendBuilder AddOffer(string offerId, string amount)
        {
            JwtBodyPartOffer offer = new JwtBodyPartOffer(offerId, amount);

            KinJwtPayload<JwtBodyPartOffer> offerPayload =
                new KinJwtPayload<JwtBodyPartOffer>(ClaimNames.Offer, offer);
            AddPayload(offerPayload);
            return this;
        }

        public SpendBuilder AddSender(string userId, string title, string description)
        {
            JwtBodyPartSender sender = new JwtBodyPartSender(userId, title, description);

            KinJwtPayload<JwtBodyPartSender> senderPayload =
                new KinJwtPayload<JwtBodyPartSender>(ClaimNames.Sender, sender);
            AddPayload(senderPayload);
            return this;
        }
    }
}