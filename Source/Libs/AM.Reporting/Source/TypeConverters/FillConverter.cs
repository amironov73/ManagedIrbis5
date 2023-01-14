// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* FillConverter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Globalization;

#endregion

#nullable enable

namespace AM.Reporting.TypeConverters;

internal class FillConverter
    : TypeConverter
{
    #region Public Methods

    /// <inheritdoc cref="TypeConverter.CanConvertFrom(System.ComponentModel.ITypeDescriptorContext?,System.Type)"/>
    public override bool CanConvertFrom
        (
            ITypeDescriptorContext? context,
            Type sourceType
        )
    {
        if (sourceType == typeof (string))
        {
            return true;
        }

        return base.CanConvertFrom (context, sourceType);
    }

    /// <inheritdoc cref="TypeConverter.CanConvertTo(System.ComponentModel.ITypeDescriptorContext?,System.Type?)"/>
    public override bool CanConvertTo
        (
            ITypeDescriptorContext? context,
            Type? destinationType
        )
    {
        if (destinationType == typeof (string))
        {
            return true;
        }

        return base.CanConvertTo (context, destinationType);
    }

    /// <inheritdoc cref="TypeConverter.ConvertFrom(System.ComponentModel.ITypeDescriptorContext?,System.Globalization.CultureInfo?,object)"/>
    public override object? ConvertFrom
        (
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            object value
        )
    {
        if (value is string className)
        {
            FillBase? result = className switch
            {
                "Solid" => new SolidFill(),
                "LinearGradient" => new LinearGradientFill(),
                "PathGradient" => new PathGradientFill(),
                "Hatch" => new HatchFill(),
                "Glass" => new GlassFill(),
                "Texture" => new TextureFill(),
                _ => null
            };

            return result;
        }

        return base.ConvertFrom (context, culture, value);
    }

    /// <inheritdoc cref="TypeConverter.ConvertTo(System.ComponentModel.ITypeDescriptorContext?,System.Globalization.CultureInfo?,object?,System.Type)"/>
    public override object? ConvertTo
        (
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            object? value,
            Type destinationType
        )
    {
        if (destinationType == typeof (string))
        {
            if (value is not FillBase fill)
            {
                return string.Empty;
            }

            return fill.Name;
        }

        return base.ConvertTo (context, culture, value, destinationType);
    }

    /// <inheritdoc cref="TypeConverter.GetProperties(System.ComponentModel.ITypeDescriptorContext?,object,System.Attribute[])"/>
    public override PropertyDescriptorCollection GetProperties
        (
            ITypeDescriptorContext? context,
            object value,
            Attribute[]? attributes
        )
    {
        return TypeDescriptor.GetProperties (value, attributes);
    }

    /// <inheritdoc cref="TypeConverter.GetPropertiesSupported(System.ComponentModel.ITypeDescriptorContext?)"/>
    public override bool GetPropertiesSupported
        (
            ITypeDescriptorContext? context
        )
    {
        return true;
    }

    #endregion Public Methods
}
