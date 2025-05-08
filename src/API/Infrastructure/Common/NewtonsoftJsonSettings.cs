using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GameService.Infrastructure.Common;

public static class NewtonsoftJsonSettings
{
    public static readonly JsonSerializerSettings CaseInsensitive = new()
    {
        ContractResolver = new DefaultContractResolver()
        {
            NamingStrategy = new CamelCaseNamingStrategy
            {
                ProcessDictionaryKeys = true,
                OverrideSpecifiedNames = false
            }
        },
        NullValueHandling = NullValueHandling.Ignore,
        DateTimeZoneHandling = DateTimeZoneHandling.Utc
    };
}