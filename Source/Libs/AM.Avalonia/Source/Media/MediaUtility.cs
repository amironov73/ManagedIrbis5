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
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable VirtualMemberCallInConstructor

/* MediaUtility.cs -- полезные методы для отрисовки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia;
using Avalonia.Media;

#endregion

#nullable enable

namespace AM.Avalonia.Media;

/// <summary>
/// Полезные методы для отрисовки.
/// </summary>
public static class MediaUtility
{
    #region Public methods

    /// <summary>
    /// Отрисовка форматированного текста строго по центру прямоугольника.
    /// </summary>
    public static void DrawCentered
        (
            this FormattedText formatted,
            DrawingContext context,
            Rect bounds
        )
    {
        Sure.NotNull (formatted);
        Sure.NotNull (context);

        var lineHeight = formatted.Extent;
        var lineWidth = formatted.Width;
        var center = new Point
            (
                (bounds.Width - lineWidth) / 2,
                (bounds.Height - lineHeight) / 2
            );
        context.DrawText (formatted, center);

    }

    #endregion
}
