// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Globalization;

#endregion

namespace Avalonia.ExtendedToolkit.Controls.PropertyGrid.Converters;

//
// ported from https://github.com/DenisVuyka/WPG
//

/// <summary>
/// Specifies a generic font type converter that provides standard values collection.
/// </summary>
public abstract class FontConverterDecorator : TypeConverter
{
    private readonly TypeConverter? _converter;

    /// <summary>
    /// Initializes a new instance of the <see cref="FontConverterDecorator"/> class.
    /// </summary>
    /// <param name="converter">The original converter.</param>
    protected FontConverterDecorator
        (
            TypeConverter? converter
        )
    {
        _converter = converter;
    }

    /// <inheritdoc cref="TypeConverter.CanConvertFrom(System.ComponentModel.ITypeDescriptorContext?,System.Type)"/>
    public override bool CanConvertFrom
        (
            ITypeDescriptorContext? context,
            Type sourceType
        )
    {
        return _converter?.CanConvertFrom(context, sourceType) ?? false;
    }

    /// <inheritdoc cref="TypeConverter.CanConvertTo(System.ComponentModel.ITypeDescriptorContext?,System.Type?)"/>
    public override bool CanConvertTo
        (
            ITypeDescriptorContext? context,
            Type? destinationType
        )
    {
        return _converter?.CanConvertTo(context, destinationType) ?? false;
    }

    /// <inheritdoc cref="TypeConverter.ConvertFrom(System.ComponentModel.ITypeDescriptorContext?,System.Globalization.CultureInfo?,object)"/>
    public override object? ConvertFrom
        (
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            object? value
        )
    {
        return _converter?.ConvertFrom (context, culture, value);
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
        return _converter?.ConvertTo (context, culture, value, destinationType);
    }

    /// <inheritdoc cref="TypeConverter.GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext?)"/>
    public override bool GetStandardValuesSupported
        (
            ITypeDescriptorContext? context
        )
    {
        return true;
    }
}
