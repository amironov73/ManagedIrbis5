// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace

/* CubeSourceConverter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Reporting.Data;

using System;
using System.ComponentModel;
using System.Globalization;

#endregion

#nullable enable

namespace AM.Reporting.TypeConverters;

internal class CubeSourceConverter
    : TypeConverter
{
    #region TypeConverter members

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
        if (value is string s)
        {
            if (context is { Instance: { } } && !string.IsNullOrEmpty (s))
            {
                var c = (context.Instance as ComponentBase)!;
                var report = c.Report;
                if (report != null)
                {
                    return CubeHelper.GetCubeSource (report.Dictionary!, s);
                }
            }

            return null;
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
            if (value == null)
            {
                return string.Empty;
            }

            return (value as CubeSourceBase)?.Alias;
        }

        return base.ConvertTo (context, culture, value, destinationType);
    }

    #endregion
}
