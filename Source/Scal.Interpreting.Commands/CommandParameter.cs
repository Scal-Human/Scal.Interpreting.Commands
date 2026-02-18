using System;
using System.ComponentModel;

namespace Scal.Interpreting.Commands;

/// Model the metadata concerning a command parameter.
/// <param name="Property">The descriptor of the property containing the parameter value.</param>
/// <param name="Name">The name of the parameter, i.e. the property name.</param>
/// <param name="Description">The help description text, if any, extracted for the property <see cref="DescriptionAttribute"/>.</param>
public record CommandParameter(
    PropertyDescriptor  Property,
    string              Name,
    string?             Description
) {

    /// The minimal name abbreviation to show in help text.
    public string NameAbbreviation { get; set; } = Name;

    /// Set the property value on the specified instance.
    /// <param name="instance">The command instance in which value must be set.</param>
    /// <param name="value">The string value to set in the property, using <see cref="TypeConverter"/>.</param>
    public void SetValue(object instance, string value)
    {
        var type                = Nullable.GetUnderlyingType(this.Property.PropertyType)
                                ?? this.Property.PropertyType;
        object? propertyValue   = (string.IsNullOrWhiteSpace(value) && (type == typeof(bool)))
                                ? true
                                : this.Property.Converter.ConvertFromInvariantString(value);
        if (propertyValue is not null) {
            this.Property.SetValue(instance, propertyValue);
        }
    }

};
