// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SplitContainerxtensions.cs -- методы расширения для SplitContainer
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="SplitContainer"/>.
/// </summary>
public static class SplitContainerExtensions
{
    #region Public methods

    /// <summary>
    /// Коллапс первой панели.
    /// </summary>
    public static TSplitContainer CollapsePanel1<TSplitContainer>
        (
            this TSplitContainer splitContainer,
            bool collapsed = true
        )
        where TSplitContainer: SplitContainer
    {
        Sure.NotNull (splitContainer);

        splitContainer.Panel1Collapsed = collapsed;

        return splitContainer;
    }

    /// <summary>
    /// Коллапс второй панели.
    /// </summary>
    public static TSplitContainer CollapsePanel2<TSplitContainer>
        (
            this TSplitContainer splitContainer,
            bool collapsed = true
        )
        where TSplitContainer: SplitContainer
    {
        Sure.NotNull (splitContainer);

        splitContainer.Panel2Collapsed = collapsed;

        return splitContainer;
    }

    /// <summary>
    /// Фиксация первой панели.
    /// </summary>
    public static TSplitContainer FixPanel1<TSplitContainer>
        (
            this TSplitContainer splitContainer
        )
        where TSplitContainer: SplitContainer
    {
        Sure.NotNull (splitContainer);

        splitContainer.FixedPanel = FixedPanel.Panel1;

        return splitContainer;
    }

    /// <summary>
    /// Фиксация второй панели.
    /// </summary>
    public static TSplitContainer FixPanel2<TSplitContainer>
        (
            this TSplitContainer splitContainer
        )
        where TSplitContainer: SplitContainer
    {
        Sure.NotNull (splitContainer);

        splitContainer.FixedPanel = FixedPanel.Panel2;

        return splitContainer;
    }

    /// <summary>
    /// Фиксация сплиттера.
    /// </summary>
    public static TSplitContainer FixSplitter<TSplitContainer>
        (
            this TSplitContainer splitContainer,
            bool isFixed = true
        )
        where TSplitContainer: SplitContainer
    {
        Sure.NotNull (splitContainer);

        splitContainer.IsSplitterFixed = isFixed;

        return splitContainer;
    }

    /// <summary>
    /// Переключение в горизонтальный режим.
    /// </summary>
    public static TSplitContainer Horizontal<TSplitContainer>
        (
            this TSplitContainer splitContainer
        )
        where TSplitContainer: SplitContainer
    {
        Sure.NotNull (splitContainer);

        splitContainer.Orientation = Orientation.Horizontal;

        return splitContainer;
    }

    /// <summary>
    /// Добавление контролов в первую панель.
    /// </summary>
    public static TSplitContainer Panel1<TSplitContainer>
        (
            this TSplitContainer splitContainer,
            params Control[] controls
        )
        where TSplitContainer: SplitContainer
    {
        Sure.NotNull (splitContainer);
        Sure.NotNull (controls);

        splitContainer.Panel1.Controls (controls);

        return splitContainer;
    }

    /// <summary>
    /// Установка минимального размера первой панели.
    /// </summary>
    public static TSplitContainer Panel1MinSize<TSplitContainer>
        (
            this TSplitContainer splitContainer,
            int minSize
        )
        where TSplitContainer: SplitContainer
    {
        Sure.NotNull (splitContainer);
        Sure.Positive (minSize);

        splitContainer.Panel1MinSize = minSize;

        return splitContainer;
    }

    /// <summary>
    /// Добавление контролов во вторую панель.
    /// </summary>
    public static TSplitContainer Panel2<TSplitContainer>
        (
            this TSplitContainer splitContainer,
            params Control[] controls
        )
        where TSplitContainer: SplitContainer
    {
        Sure.NotNull (splitContainer);
        Sure.NotNull (controls);

        splitContainer.Panel2.Controls (controls);

        return splitContainer;
    }

    /// <summary>
    /// Установка минимального размера второй панели.
    /// </summary>
    public static TSplitContainer Panel2MinSize<TSplitContainer>
        (
            this TSplitContainer splitContainer,
            int minSize
        )
        where TSplitContainer: SplitContainer
    {
        Sure.NotNull (splitContainer);
        Sure.Positive (minSize);

        splitContainer.Panel2MinSize = minSize;

        return splitContainer;
    }

    /// <summary>
    /// Установка дистанции для сплиттера.
    /// </summary>
    public static TSplitContainer SplitterDistance<TSplitContainer>
        (
            this TSplitContainer splitContainer,
            int distance
        )
        where TSplitContainer: SplitContainer
    {
        Sure.NotNull (splitContainer);
        Sure.NonNegative (distance);

        splitContainer.SplitterDistance = distance;

        return splitContainer;
    }

    /// <summary>
    /// Установка ширины сплиттера.
    /// </summary>
    public static TSplitContainer SplitterWidth<TSplitContainer>
        (
            this TSplitContainer splitContainer,
            int width
        )
        where TSplitContainer: SplitContainer
    {
        Sure.NotNull (splitContainer);
        Sure.Positive (width);

        splitContainer.SplitterWidth = width;

        return splitContainer;
    }

    #endregion
}
