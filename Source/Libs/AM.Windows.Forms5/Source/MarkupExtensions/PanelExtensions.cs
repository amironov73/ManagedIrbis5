// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PanelExtensions.cs -- методы расширения для Panel
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="Panel"/>.
/// </summary>
public static class PanelExtensions
{
    #region Public methods

    /// <summary>
    /// Задание стиля рамки.
    /// </summary>
    public static TPanel BorderStyle<TPanel>
        (
            this TPanel panel,
            BorderStyle borderStyle
        )
        where TPanel : Panel
    {
        Sure.NotNull (panel);
        Sure.Defined (borderStyle);

        panel.BorderStyle = borderStyle;

        return panel;
    }

    /// <summary>
    /// Задание стиля рамки.
    /// </summary>
    public static TPanel BorderStyleNone<TPanel>
        (
            this TPanel panel
        )
        where TPanel : Panel
    {
        Sure.NotNull (panel);

        panel.BorderStyle = System.Windows.Forms.BorderStyle.None;

        return panel;
    }

    #endregion
}
