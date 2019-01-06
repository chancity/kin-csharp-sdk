using System;
using System.Linq;
using Kin.Stellar.Sdk.responses.effects;
using Kin.Stellar.Sdk.responses.operations;
using Kin.Stellar.Sdk.responses.page;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kin.Stellar.Sdk.responses
{
    public static class JsonSingleton
    {
        public static T GetInstance<T>(string content)
        {
            Type[] pageResponseConversions =
            {
                typeof(Page<AccountResponse>),
                typeof(Page<AssetResponse>),
                typeof(Page<EffectResponse>),
                typeof(Page<LedgerResponse>),
                typeof(Page<OfferResponse>),
                typeof(Page<OperationResponse>),
                typeof(Page<PathResponse>),
                typeof(Page<TransactionResponse>),
                typeof(Page<TradeResponse>),
                typeof(Page<TradeAggregationResponse>),
                typeof(Page<TransactionResponse>)
            };

            JsonConverter[] jsonConverters =
            {
                new AssetDeserializer(),
                new KeyPairTypeAdapter(),
                new OperationDeserializer(),
                new EffectDeserializer(),
                new TransactionDeserializer()
            };

            JsonConverter[] pageJsonConverters =
            {
                new AssetDeserializer(),
                new KeyPairTypeAdapter(),
                new OperationDeserializer(),
                new EffectDeserializer()
            };

            if (pageResponseConversions.Contains(typeof(T)))
            {
                content = PageAccountResponseConverter(content);
                return JsonConvert.DeserializeObject<T>(content, pageJsonConverters);
            }

            return JsonConvert.DeserializeObject<T>(content, jsonConverters);
        }

        private static string PageAccountResponseConverter(string content)
        {
            JObject json = JObject.Parse(content);
            JObject newJson = new JObject();
            newJson.Add("records", json.SelectToken("$._embedded.records"));
            newJson.Add("links", json.SelectToken("$._links"));

            return newJson.Root.ToString();
        }
    }
}