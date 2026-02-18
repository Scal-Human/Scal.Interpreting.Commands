using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace Scal.Interpreting.Commands.Tests.WithTypeConverter;

internal abstract class Program
{
}

internal class AddReference : Program
{
    [Required]
    [TypeConverter(typeof(ReferenceConverter))]
    public Reference? Ref { get; set; }
}

public record struct Reference(string Scope, string ContractId);

public class ReferenceConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string);

    public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType)
    {
        destinationType ??= typeof(Reference);
        return destinationType == typeof(Reference);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is null) return null;
        var parts = (value as string ?? value.GetType().Name).Split('-');
        if (parts.Length != 2) {
            throw new FormatException($"{nameof(Reference)} not well formed '{value}'");
        }
        return new Reference(
            parts.First(),
            parts.Skip(1).First()
        );
    }


    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        return (value is Reference someValue)
            ? (destinationType == typeof(string))
                ? someValue.Scope + '-' + someValue.ContractId
                : destinationType.Name
            : null;
    }
}
