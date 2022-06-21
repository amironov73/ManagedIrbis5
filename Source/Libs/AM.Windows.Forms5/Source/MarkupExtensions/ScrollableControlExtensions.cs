// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ScrollableControlExtensions.cs -- методы расширения для ScrollableControl
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="ScrollableControl"/>.
/// </summary>
public static class ScrollableControlExtensions
{
    #region Public methods

    /// <summary>
    /// Включение режима автоматической прокрутки.
    /// </summary>
    public static TControl AutoScroll<TControl>
        (
            this TControl control,
            bool autoScroll = true
        )
        where TControl: ScrollableControl
    {
        Sure.NotNull (control);

        control.AutoScroll = autoScroll;

        return control;
    }


    #endregion
}
