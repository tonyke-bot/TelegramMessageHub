using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramMessageHub
{
    class JsonSerializable
    {
        private static readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() },
            Formatting = Formatting.Indented,
        };

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this, serializerSettings);
        }
    }
}
