using System;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Kin.Marketplace.Models
{
    public enum OrderOriginEnum
    {
        Marketplace,
        External
    }

    public enum OrderStatusEnum
    {
        Completed,
        Failed,
        Pending,
        Opened
    }

    public class Order : BaseOrder
    {
        [JsonProperty("error")]
        public object Error { get; private set; }

        [JsonProperty("content")]
        public string Content { get; private set; }

        [JsonProperty("status")]
        public OrderStatusEnum Status { get; private set; }

        [JsonProperty("completion_date")]
        public DateTime CompletionDate { get; private set; }

        [JsonProperty("result")]
        public Result Result { get; private set; }

        [JsonProperty("call_to_action")]
        public string CallToAction { get; private set; }

        [JsonProperty("origin")]
        public OrderOriginEnum Origin { get; private set; }
    }
}