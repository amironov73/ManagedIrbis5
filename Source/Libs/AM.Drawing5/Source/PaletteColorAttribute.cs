// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* PaletteColorAttribute.cs -- задание палитры для элемента пользовательского интерфейса
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Drawing;

/// <summary>
/// Задание палитры для элемента пользовательского интерфейса.
/// </summary>
public sealed class PaletteColorAttribute
    : Attribute
{
    #region Properties

    /// <summary>
    /// Имя или значение цвета.
    /// </summary>
    public string Color { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PaletteColorAttribute
        (
            string color
        )
    {
        Sure.NotNullNorEmpty (color);

        Color = color;
    }

    #endregion
}
