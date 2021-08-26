using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mockingjay.Common.Json
{
    /// <summary>A JSON converter that converts Single Value Objects based on naming conventions.</summary>
    public class ConventionBasedJsonConverter : JsonConverterFactory
    {
        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert != null
                   && !TypeHelper.GetNotNullableType(typeToConvert).IsPrimitive
                   && CreateConverter(typeToConvert, null) != null;
        }

        /// <inheritdoc />
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var type = TypeHelper.GetNotNullableType(typeToConvert);

            if (_notSupported.Contains(type))
            {
                return null;
            }

            if (!_converters.TryGetValue(typeToConvert, out JsonConverter converter))
            {
                lock (_locker)
                {
                    if (!_converters.TryGetValue(typeToConvert, out converter))
                    {
                        var converterType = typeof(ConventionBasedSerializer<>).MakeGenericType(typeToConvert);
                        converter = (JsonConverter)Activator.CreateInstance(converterType);

                        if (converter.CanConvert(typeToConvert))
                        {
                            _converters[typeToConvert] = converter;
                        }
                        else
                        {
                            _notSupported.Add(type);
                            return null;
                        }
                    }
                }
            }

            return converter;
        }

        private readonly object _locker = new ();
        private readonly Dictionary<Type, JsonConverter> _converters = new ();
        private readonly HashSet<Type> _notSupported = new ();
    }
}
