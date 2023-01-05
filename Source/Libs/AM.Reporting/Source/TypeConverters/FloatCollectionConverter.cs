// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* FloatCollectionConverter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Reporting.Utils;

using System;
using System.ComponentModel;
using System.Globalization;

#endregion

#nullable enable

namespace AM.Reporting.TypeConverters;

internal class FloatCollectionConverter
    : TypeConverter
{
    #region Public Methods

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

    public override object? ConvertFrom
        (
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            object value
        )
    {
        if (value is string s1)
        {
            var result = new FloatCollection();
            var values = s1.Split (',');
            foreach (var s in values)
            {
                result.Add ((float)Converter.FromString (typeof (float), s));
            }

            return result;
        }

        return base.ConvertFrom (context, culture, value);
    }

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
            if (value is null)
            {
                return string.Empty;
            }

            var builder = new FastString();
            var list = (FloatCollection) value;
            foreach (float f in list)
            {
                builder.Append (Converter.ToString (f)).Append (",");
            }

            if (builder.Length > 0)
            {
                builder.Remove (builder.Length - 1, 1);
            }

            return builder.ToString();
        }

        return base.ConvertTo (context, culture, value, destinationType);
    }

    #endregion Public Methods
}
