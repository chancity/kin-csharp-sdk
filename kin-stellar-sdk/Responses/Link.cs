using System;
using Newtonsoft.Json;

namespace Kin.Stellar.Sdk.responses
{
    public class Link
    {
        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }

        [JsonProperty(PropertyName = "templated")]
        public bool Templated { get; set; }

        public Uri Uri => new Uri(Href);

        public Link(string href, bool templated)
        {
            Href = href;
            Templated = templated;
        }

        public bool IsTemplated()
        {
            return Templated;
        }
    }
}