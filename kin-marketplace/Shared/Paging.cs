using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Kin.Marketplace.Models
{
    public class Paging
    {
        [JsonProperty("cursors")]
        public Cursors Cursors { get; private set; }

        [JsonProperty("previous")]
        public string Previous { get; private set; }

        [JsonProperty("next")]
        public string Next { get; private set; }
    }
}