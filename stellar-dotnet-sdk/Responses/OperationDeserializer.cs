﻿using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using stellar_dotnet_sdk.responses.operations;

namespace stellar_dotnet_sdk.responses
{
    public class OperationDeserializer : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var type = jsonObject.GetValue("type_i").ToObject<int>();

            switch (type)
            {
                case 0:
                    return JsonSingleton.GetInstance<CreateAccountOperationResponse>(jsonObject.Root.ToString());
                case 1:
                    return JsonSingleton.GetInstance<PaymentOperationResponse>(jsonObject.Root.ToString());
                case 2:
                    return JsonSingleton.GetInstance<PathPaymentOperationResponse>(jsonObject.Root.ToString());
                case 3:
                    return JsonSingleton.GetInstance<ManageOfferOperationResponse>(jsonObject.Root.ToString());
                case 4:
                    return JsonSingleton.GetInstance<CreatePassiveOfferOperationResponse>(jsonObject.Root.ToString());
                case 5:
                    return JsonSingleton.GetInstance<SetOptionsOperationResponse>(jsonObject.Root.ToString());
                case 6:
                    return JsonSingleton.GetInstance<ChangeTrustOperationResponse>(jsonObject.Root.ToString());
                case 7:
                    return JsonSingleton.GetInstance<AllowTrustOperationResponse>(jsonObject.Root.ToString());
                case 8:
                    return JsonSingleton.GetInstance<AccountMergeOperationResponse>(jsonObject.Root.ToString());
                case 9:
                    return JsonSingleton.GetInstance<InflationOperationResponse>(jsonObject.Root.ToString());
                case 10:
                    return JsonSingleton.GetInstance<ManageDataOperationResponse>(jsonObject.Root.ToString());
                default:
                    throw new Exception("Invalid operation type");
            }
}

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(OperationResponse);
        }
    }
}