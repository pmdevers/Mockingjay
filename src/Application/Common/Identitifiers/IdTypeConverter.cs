using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Application.Common.Identifiers
{
    /// <summary>Provides a conversion for strongly typed identifiers.</summary>
    public sealed class IdTypeConverter : TypeConverter
    {
        /// <summary>Accessor to the underlying value.</summary>
        private readonly FieldInfo _value;

        /// <summary>Accessor to the private constructor.</summary>
        private readonly ConstructorInfo _ctor;

        /// <summary>The base type according to the <see cref="IIdentifierBehavior"/>.</summary>
        private readonly Type _baseType;

        /// <summary>The <see cref="TypeConverter"/> of the underlying value.</summary>
        private readonly TypeConverter _baseConverter;

        /// <summary>Creates a new instance of the <see cref="IdTypeConverter"/> class.</summary>
        /// <param name="type">
        /// The type to convert for.
        /// </param>
        public IdTypeConverter(Type type)
        {
            Guard.NotNull(type, nameof(type));

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Id<>) && type.GetGenericArguments().Length == 1)
            {
                _value = type.GetField(nameof(_value), BindingFlags.Instance | BindingFlags.NonPublic);
                var ctors = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                _ctor = ctors.FirstOrDefault(ctor => ctor.GetParameters().Length == 1);
                var behavior = (IIdentifierBehavior)Activator.CreateInstance(type.GetGenericArguments()[0]);
                _baseType = behavior.BaseType;
                _baseConverter = behavior.Converter;
            }
            else
            {
                throw new ArgumentException("Incompatible type", nameof(type));
            }
        }

        /// <inheritdoc />
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == _baseType || _baseConverter.CanConvertFrom(context, sourceType);
        }

        /// <inheritdoc />
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == _baseType || _baseConverter.CanConvertTo(context, destinationType);
        }

        /// <inheritdoc />
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (string.Empty.Equals(value))
            {
                value = null;
            }
            if (value is null || value.GetType() == _baseType)
            {
                return _ctor.Invoke(new[] { value });
            }
            var id = _baseConverter.ConvertFrom(context, culture, value);
            return _ctor.Invoke(new[] { id });
        }

        /// <inheritdoc />
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var id = _value.GetValue(value);
            return destinationType == _baseType
                ? id
                : _baseConverter.ConvertTo(context, culture, id, destinationType);
        }
    }
}
