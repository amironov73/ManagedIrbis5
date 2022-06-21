// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FlowLayoutPanelExtensions.cs -- методы расширения для FlowLayoutPanel
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="FlowLayoutPanel"/>.
/// </summary>
public static class FlowLayoutPanelExtensions
{
    #region Public methods

    /// <summary>
    /// Настройка направления размещения контента.
    /// </summary>
    public static TPanel FlowDirection<TPanel>
        (
            this TPanel panel,
            FlowDirection direction
        )
        where TPanel: FlowLayoutPanel
    {
        Sure.NotNull (panel);

        panel.FlowDirection = direction;

        return panel;
    }

    /// <summary>
    /// Настройка направления размещения "слева направо".
    /// </summary>
    public static TPanel LeftToRight<TPanel>
        (
            this TPanel panel
        )
        where TPanel: FlowLayoutPanel
    {
        Sure.NotNull (panel);

        panel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;

        return panel;
    }
    /// <summary>
    /// Настройка направления размещения "сверху вниз".
    /// </summary>
    public static TPanel TopDown<TPanel>
        (
            this TPanel panel
        )
        where TPanel: FlowLayoutPanel
    {
        Sure.NotNull (panel);

        panel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;

        return panel;
    }

    /// <summary>
    /// Включение "заворачивания" не помещающегося контента.
    /// </summary>
    public static TPanel WrapContents<TPanel>
        (
            this TPanel panel,
            bool wrap = true
        )
        where TPanel: FlowLayoutPanel
    {
        Sure.NotNull (panel);

        panel.WrapContents = wrap;

        return panel;
    }


    #endregion
}
