using System;

namespace Application.Common.Formatting
{
    public static class StringFormatter
    {
        public static bool TryApplyCustomFormatter(string format, object obj, IFormatProvider formatProvider, out string formatted)
        {
            var customFormatter = formatProvider?.GetFormat<ICustomFormatter>();
            if (customFormatter is null)
            {
                formatted = null;
                return false;
            }
            else
            {
                formatted = customFormatter.Format(format, obj, formatProvider);
                return true;
            }
        }

        public static TFormat GetFormat<TFormat>(this IFormatProvider provider)
        {
            Guard.NotNull(provider, nameof(provider));
            return (TFormat)provider.GetFormat(typeof(TFormat));
        }
    }
}
