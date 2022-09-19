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

using System.ComponentModel;

using Avalonia.ExtendedToolkit.Font;
using Avalonia.Media;

#endregion

#nullable enable

namespace Avalonia.ExtendedToolkit.Controls.PropertyGrid.Converters;

//
// ported from https://github.com/DenisVuyka/WPG
//

/// <summary>
/// Extended <see cref="FontWeightConverter"/> that provides standard values collection.
/// </summary>
public class FontWeightConverterDecorator
    : FontConverterDecorator
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="FontWeightConverterDecorator"/> class.
    /// </summary>
    public FontWeightConverterDecorator()
        : base (new FontWeightConverter())
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
        return new StandardValuesCollection (
            new[]
            {
                FontWeight.Thin,
                FontWeight.ExtraLight,
                FontWeight.Light,
                FontWeight.Normal,
                FontWeight.Medium,
                FontWeight.SemiBold,
                FontWeight.Bold,
                FontWeight.ExtraBold,
                FontWeight.Black,
                FontWeight.ExtraBlack
            });
    }
}
