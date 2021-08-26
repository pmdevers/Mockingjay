using System.Text.Json;

namespace Mockingjay.Common.Json
{
    public static class JsonSerializationOptionsExtensions
    {
        public static JsonSerializerOptions SetApplicationDefaultSettings(this JsonSerializerOptions options)
        {
            Guard.NotNull(options, nameof(options));
            options.Converters.Add(new ConventionBasedJsonConverter());
            return options;
        }
    }
}
