// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* ComfortableWindow.cs -- окно со строкой логов внизу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

using JetBrains.Annotations;

using NLog;

#endregion

namespace AM.Avalonia.Controls;

/// <summary>
/// Окно со строкой логов внизу.
/// </summary>
[PublicAPI]
public class ComfortableWindow
    : Window
{
    #region Properties

    /// <summary>
    /// Грид, в котором размещается контент окна.
    /// </summary>
    public Grid ContentGrid { get; }

    /// <summary>
    /// Строка с логами.
    /// </summary>
    public LogTextBox LogBox { get; }

    /// <summary>
    /// Основной контент окна.
    /// </summary>
    public object? MainContent
    {
        get => MainControl.Content;
        set => MainControl.Content = value;
    }

    /// <summary>
    /// Контрол, в котором размещается основной контент окна.
    /// </summary>
    public ContentControl MainControl { get; }

    /// <summary>
    /// Строка статуса.
    /// </summary>
    public StatusBar StatusBar { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ComfortableWindow()
    {
        this.AttachDevTools();

        var splitter = new GridSplitter
        {
            [Grid.RowProperty] = 1,
            ResizeDirection = GridResizeDirection.Rows,
            Background = Brushes.Black
        };

        MainControl = new ContentControl
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch
        };

        LogBox = new LogTextBox
        {
            [Grid.RowProperty] = 2,
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        StatusBar = new StatusBar
        {
            [Grid.RowProperty] = 3
        };

        ContentGrid = new Grid
        {
            RowDefinitions = RowDefinitions.Parse ("*,4,100,Auto"),
            Children =
            {
                MainControl,
                splitter,
                LogBox,
                StatusBar
            }
        };

        Content = ContentGrid;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Вывод в лог строки текста.
    /// </summary>
    public void WriteLog
        (
            LogLevel level,
            string message
        )
    {
        LogBox.WriteLine (level, message);
    }

    #endregion
}
