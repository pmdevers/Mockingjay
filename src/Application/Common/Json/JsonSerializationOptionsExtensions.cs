using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mockingjay.Common.Json
{
    public static class JsonSerializationOptionsExtensions
    {
        public static JsonSerializerOptions SetApplicationDefaultSettings(this JsonSerializerOptions options)
        {
            Guard.NotNull(options, nameof(options));
            options.Converters.Add(new ConventionBasedJsonConverter());

            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

            options.PropertyNameCaseInsensitive = true;
            options.IgnoreNullValues = true;

            return options;
        }
    }
}
