// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* EnumTypeConverter.cs -- конвертер для типа перечисления
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Globalization;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Конвертер для типа перечисления.
/// </summary>
internal sealed class EnumTypeConverter
    : TypeConverter
{
    /// <inheritdoc cref="TypeConverter.CanConvertFrom(System.ComponentModel.ITypeDescriptorContext?,System.Type)"/>
    public override bool CanConvertFrom
        (
            ITypeDescriptorContext? context,
            Type sourceType
        )
    {
        if ((sourceType == typeof (string))
            || (sourceType == typeof (Type)))
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
        if ((destinationType == typeof (string))
            || (destinationType == typeof (Type)))
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
        var s = value as string;
        if (!string.IsNullOrEmpty (s))
        {
            return Type.GetType (s);
        }

        if (value is Type t)
        {
            return t.FullName;
        }

        return base.ConvertFrom
            (
                context,
                culture,
                value
            );
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
        var s = value as string;
        if (!string.IsNullOrEmpty (s)
            && (destinationType == typeof (Type)))
        {
            return Type.GetType (s);
        }

        if ((value is Type t)
            && (destinationType == typeof (string)))
        {
            return t.FullName;
        }

        return base.ConvertTo
            (
                context,
                culture,
                value,
                destinationType
            );
    }
}
