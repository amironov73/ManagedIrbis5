// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PointConverter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;

#endregion

#nullable enable

namespace AM.Windows.Forms.Animation;

/// <summary>
/// PointFConverter
/// Thanks for Jay Riggs
/// </summary>
public sealed class PointFConverter
    : ExpandableObjectConverter
{
    /// <inheritdoc cref="TypeConverter.CanConvertFrom(System.ComponentModel.ITypeDescriptorContext?,System.Type)"/>
    public override bool CanConvertFrom
        (
            ITypeDescriptorContext? context,
            Type sourceType
        )
    {
        return sourceType == typeof (string) || base.CanConvertFrom (context, sourceType);
    }

    /// <inheritdoc cref="TypeConverter.ConvertFrom(System.ComponentModel.ITypeDescriptorContext?,System.Globalization.CultureInfo?,object)"/>
    public override object? ConvertFrom
        (
            ITypeDescriptorContext? context,
            System.Globalization.CultureInfo? culture,
            object value
        )
    {
        if (value is string text)
        {
            try
            {
                var converterParts = text.Split (',');
                float x;
                float y;
                if (converterParts.Length > 1)
                {
                    x = float.Parse (converterParts[0].Trim().Trim ('{', 'X', 'x', '='));
                    y = float.Parse (converterParts[1].Trim().Trim ('}', 'Y', 'y', '='));
                }
                else if (converterParts.Length == 1)
                {
                    x = float.Parse (converterParts[0].Trim());
                    y = 0;
                }
                else
                {
                    x = 0F;
                    y = 0F;
                }

                return new PointF (x, y);
            }
            catch
            {
                throw new ArgumentException ("Cannot convert [" + text + "] to pointF");
            }
        }

        return base.ConvertFrom (context, culture, value);
    }

    /// <inheritdoc cref="TypeConverter.ConvertTo(System.ComponentModel.ITypeDescriptorContext?,System.Globalization.CultureInfo?,object?,System.Type)"/>
    public override object? ConvertTo
        (
            ITypeDescriptorContext? context,
            System.Globalization.CultureInfo? culture,
            object? value,
            Type destinationType
        )
    {
        if (destinationType == typeof (string))
        {
            if (value is PointF pt)
            {
                return $"{{X={pt.X}, Y={pt.Y}}}";
            }
        }

        return base.ConvertTo (context, culture, value, destinationType);
    }
}
