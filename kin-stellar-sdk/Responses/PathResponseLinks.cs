using Newtonsoft.Json;

namespace Kin.Stellar.Sdk.responses
{
    public class PathResponseLinks
    {
        [JsonProperty(PropertyName = "self")] public Link Self { get; }

        public PathResponseLinks(Link self)
        {
            Self = self;
        }
    }
}