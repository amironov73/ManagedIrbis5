// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* LabelExtensions.cs -- методы расширения для Label
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="Label"/>.
/// </summary>
public static class LabelExtensions
{
    #region Public methods

    /// <summary>
    /// Установка выравнивания по центру слева для метки.
    /// </summary>
    public static TLabel AlignMiddleLeft<TLabel>
        (
            this TLabel label
        )
        where TLabel: Label
    {
        Sure.NotNull (label);

        label.TextAlign = ContentAlignment.MiddleLeft;

        return label;
    }

    /// <summary>
    /// Установка выравнивания по центру для метки.
    /// </summary>
    public static TLabel AlignMiddleCenter<TLabel>
        (
            this TLabel label
        )
        where TLabel: Label
    {
        Sure.NotNull (label);

        label.TextAlign = ContentAlignment.MiddleCenter;

        return label;
    }

    /// <summary>
    /// Установка выравнивания для метки.
    /// </summary>
    public static TLabel TextAlign<TLabel>
        (
            this TLabel label,
            ContentAlignment alignment
        )
        where TLabel: Label
    {
        Sure.NotNull (label);

        label.TextAlign = alignment;

        return label;
    }

    #endregion
}
