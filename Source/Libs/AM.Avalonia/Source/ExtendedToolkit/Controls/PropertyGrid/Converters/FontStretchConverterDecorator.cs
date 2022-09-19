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

#endregion

#nullable enable

namespace Avalonia.ExtendedToolkit.Controls.PropertyGrid.Converters;

//
// ported from https://github.com/DenisVuyka/WPG
//

// TODO update if avalonia has a front stretch type

/// <summary>
/// FontStretchConverter is missing
/// Extended  that provides standard values collection.
/// </summary>
public sealed class FontStretchConverterDecorator
    : FontConverterDecorator
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="FontStretchConverterDecorator"/> class.
    /// </summary>
    public FontStretchConverterDecorator()
        : base (null /*new FontStretchConverter()*/)
    {
        // пустое тело конструктора
    }

    #endregion

    /// <inheritdoc cref="TypeConverter.GetStandardValues(System.ComponentModel.ITypeDescriptorContext?)"/>
    public override StandardValuesCollection GetStandardValues
        (
            ITypeDescriptorContext? context
        )
    {
        throw new NotImplementedException();

        //  return new StandardValuesCollection(
        //    new[]
        //    {
        //FontStretches.Condensed,
        //FontStretches.Expanded,
        //FontStretches.ExtraCondensed,
        //FontStretches.ExtraExpanded,
        //FontStretches.Normal,
        //FontStretches.SemiCondensed,
        //FontStretches.SemiExpanded,
        //FontStretches.UltraCondensed,
        //FontStretches.UltraExpanded
        //    });
    }
}
