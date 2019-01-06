using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kin.Stellar.Sdk.responses
{
    public class AssetDeserializer : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);

            string type = jsonObject.GetValue("asset_type").ToObject<string>();

            if (type == "native")
            {
                return new AssetTypeNative();
            }

            string code = jsonObject.GetValue("asset_code").ToObject<string>();
            string issuer = jsonObject.GetValue("asset_issuer").ToObject<string>();
            return Asset.CreateNonNativeAsset(code, KeyPair.FromAccountId(issuer));
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Asset);
        }
    }
}