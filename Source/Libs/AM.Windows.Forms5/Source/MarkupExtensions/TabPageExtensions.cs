// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* TabPageExtensions.cs -- методы расширения для TabPage
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="TabPage"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public static class TabPageExtensions
{
    #region Public methods

    /// <summary>
    /// Разрешение/запрет таба.
    /// </summary>
    public static TTabPage Enabled<TTabPage>
        (
            this TTabPage tabPage,
            bool enabled
        )
        where TTabPage: TabPage
    {
        Sure.NotNull (tabPage);

        tabPage.Enabled = enabled;

        return tabPage;
    }

    /// <summary>
    /// Задание индекса картинки для таба.
    /// </summary>
    public static TTabPage ImageIndex<TTabPage>
        (
            this TTabPage tabPage,
            int imageIndex
        )
        where TTabPage: TabPage
    {
        Sure.NotNull (tabPage);
        Sure.NonNegative (imageIndex);

        tabPage.ImageIndex = imageIndex;

        return tabPage;
    }

    /// <summary>
    /// Остановка по клавише <code>Tab</code>.
    /// </summary>
    public static TTabPage TabStop<TTabPage>
        (
            this TTabPage tabPage,
            bool tabStop
        )
        where TTabPage: TabPage
    {
        Sure.NotNull (tabPage);

        tabPage.TabStop = tabStop;

        return tabPage;
    }

    /// <summary>
    /// Текст для тултипа.
    /// </summary>
    public static TTabPage ToolTipText<TTabPage>
        (
            this TTabPage tabPage,
            string text
        )
        where TTabPage: TabPage
    {
        Sure.NotNull (tabPage);
        Sure.NotNullNorEmpty (text);

        tabPage.ToolTipText = text;

        return tabPage;
    }

    #endregion
}
