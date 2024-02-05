// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ThreePartLayout.cs -- пользовательский интерфейс из трех частей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

using JetBrains.Annotations;

#endregion

namespace AM.Avalonia.Controls;

/// <summary>
/// Пользовательский интерфейс из трех частей: тулбар, основная часть и статус-бар.
/// Тулбар и статус-бар опциональны.
/// </summary>
[PublicAPI]
public sealed class ThreePartLayout
    : DockPanel
{
    #region Properties

    /// <summary>
    /// Тулбар или меню - опционально.
    /// </summary>
    public Control? Toolbar { get; }

    /// <summary>
    /// Основая часть интерфейса - обязательный элемент.
    /// </summary>
    public Control MainArea { get; }

    /// <summary>
    /// Статус-бар - опционально.
    /// </summary>
    public Control? StatusBar { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ThreePartLayout
        (
            Control? toolbar,
            Control mainArea,
            Control? statusBar
        )
    {
        Sure.NotNull (mainArea);

        Toolbar = toolbar;
        MainArea = mainArea;
        StatusBar = statusBar;

        HorizontalAlignment = HorizontalAlignment.Stretch;
        VerticalAlignment = VerticalAlignment.Stretch;
        LastChildFill = true;

        if (Toolbar is not null)
        {
            var border = new Border
            {
                Child = Toolbar,
                BorderThickness = new Thickness (1),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                BorderBrush = AvaloniaUtility.GetThemeForegroundBrush()
            }
            .DockTop();

            Toolbar.HorizontalAlignment = HorizontalAlignment.Stretch;
            Children.Add (border);
        }

        if (StatusBar is not null)
        {
            var border = new Border
            {
                Child = StatusBar,
                BorderThickness = new Thickness (1),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                BorderBrush = AvaloniaUtility.GetThemeForegroundBrush()
            }
            .DockBottom();

            StatusBar.HorizontalAlignment = HorizontalAlignment.Stretch;
            Children.Add (border);
        }

        MainArea.HorizontalAlignment = HorizontalAlignment.Stretch;
        MainArea.VerticalAlignment = VerticalAlignment.Stretch;
        Children.Add (MainArea);
    }

    #endregion
}
