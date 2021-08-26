using Mockingjay.Common.Identifiers.Behaviors;
using System;
using System.ComponentModel;
using System.Globalization;

namespace Mockingjay.Common.Converters
{
    public class GuidTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            => value is null || value is string
                ? FromString((string)value)
                : base.ConvertFrom(context, culture, value);

        private static Guid FromString(string str)
        {
            if (GuidBehavior.Instance.TryParse(str, out object id))
            {
                return id is Guid guid ? guid : Guid.Empty;
            }
            throw new FormatException($"'{str}' is not a valid guid format.");
        }
    }
}
