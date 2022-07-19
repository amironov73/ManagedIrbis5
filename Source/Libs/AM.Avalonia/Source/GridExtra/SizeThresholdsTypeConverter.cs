// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* SizeThresholdsTypeConverter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

#endregion

#nullable enable

namespace GridExtra.Avalonia;

/// <summary>
///
/// </summary>
public class SizeThresholdsTypeConverter
    : TypeConverter
{
    /// <inheritdoc cref="TypeConverter.CanConvertFrom(System.ComponentModel.ITypeDescriptorContext?,System.Type)"/>
    public override bool CanConvertFrom
        (
            ITypeDescriptorContext? context,
            Type sourceType
        )
    {
        return sourceType == typeof (string);
    }

    /// <inheritdoc cref="TypeConverter.ConvertFrom(System.ComponentModel.ITypeDescriptorContext?,System.Globalization.CultureInfo?,object)"/>
    public override object ConvertFrom
        (
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            object value
        )
    {
        var text = (string) value;
        var list = text.Split (',')
            .Select (o => o.Trim())
            .Select (int.Parse)
            .ToList();

        if (list.Count() != 3)
        {
            throw new ArgumentException ($"'{value}' Invalid value. BreakPoints must contains 3 items.");
        }

        return new SizeThresholds() { XS_SM = list[0], SM_MD = list[1], MD_LG = list[2] };
    }
}
