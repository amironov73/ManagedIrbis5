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

/* AvaloniaUtility.cs -- полезные расширения для Avalonia UI
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform;

#endregion

#nullable enable

namespace NativeAvaloniaApp;

/// <summary>
/// Полезные расширения для Avalonia UI.
/// </summary>
public static class AvaloniaUtility
{
    #region Public methods

    /// <summary>
    /// Выполнение произвольных побочных действий.
    /// </summary>
    public static TControl Also<TControl>
        (
            this TControl control,
            Action<TControl> action
        )
        where TControl: Control
    {
        Sure.NotNull (control);
        Sure.NotNull (action);

        action (control);

        return control;
    }

    /// <summary>
    /// Присваивание указанной переменной.
    /// </summary>
    public static TControl Assign<TControl>
        (
            this TControl control,
            out TControl variable
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        variable = control;

        return control;
    }

    /// <summary>
    /// Установка жирного начертания для текстового блока.
    /// </summary>
    public static T Bold<T>
        (
            this T block,
            bool bold = true
        )
        where T: TextBlock
    {
        Sure.NotNull (block);

        block.FontWeight = bold ? FontWeight.Bold : FontWeight.Regular;

        return block;
    }

    /// <summary>
    /// Центрирование содержимого контрола.
    /// </summary>
    public static T CenterContent<T>
        (
            this T control
        )
        where T: IContentControl
    {
        control.HorizontalContentAlignment = HorizontalAlignment.Center;
        control.VerticalContentAlignment = VerticalAlignment.Center;

        return control;
    }

    /// <summary>
    /// Центрирование самого контрола.
    /// </summary>
    public static T CenterControl<T>
        (
            this T control
        )
        where T: Control
    {
        Sure.NotNull (control);

        control.HorizontalAlignment = HorizontalAlignment.Center;
        control.VerticalAlignment = VerticalAlignment.Center;

        return control;
    }

    /// <summary>
    /// Центрирование контрола по горизонтали.
    /// </summary>
    public static T CenterHorizontally<T>
        (
            this T control
        )
        where T: Control
    {
        Sure.NotNull (control);

        control.HorizontalAlignment = HorizontalAlignment.Center;

        return control;
    }

    /// <summary>
    /// Центрирование контрола по вертикали.
    /// </summary>
    public static T CenterVertically<T>
        (
            this T control
        )
        where T: Control
    {
        Sure.NotNull (control);

        control.VerticalAlignment = VerticalAlignment.Center;

        return control;
    }

    /// <summary>
    /// Создание простого грида.
    /// </summary>
    /// <param name="rowCount">Количество строк.</param>
    /// <param name="rowLength">Высота каждой строки.</param>
    /// <param name="columnCount">Количество столбцов.</param>
    /// <param name="columnLength">Ширина каждого столбца.</param>
    /// <returns></returns>
    public static Grid CreateGrid
        (
            int rowCount,
            GridLength rowLength,
            int columnCount,
            GridLength columnLength
        )
    {
        Sure.Positive (rowCount);
        Sure.Positive (columnCount);

        var result = new Grid
        {
            RowDefinitions = CreateGridRows (rowCount, rowLength),
            ColumnDefinitions = CreateGridColumns (columnCount, columnLength),
        };

        return result;
    }

    /// <summary>
    /// Создание описания единообразных столбцов для грида.
    /// </summary>
    /// <param name="count">Необходимое количество столбцов.</param>
    /// <param name="length">Ширина каждого столбца.</param>
    /// <returns>Описания столбцов.</returns>
    public static ColumnDefinitions CreateGridColumns
        (
            int count,
            GridLength length
        )
    {
        Sure.Positive (count);

        var result = new ColumnDefinitions();
        for (var i = 0; i < count; i++)
        {
            result.Add (new ColumnDefinition (length));
        }

        return result;
    }

    /// <summary>
    /// Создание описания единообразных строк для грида.
    /// </summary>
    /// <param name="count">Необходимое количество строк.</param>
    /// <param name="length">Высота каждой строки.</param>
    /// <returns>Описания строк.</returns>
    public static RowDefinitions CreateGridRows
        (
            int count,
            GridLength length
        )
    {
        Sure.Positive (count);

        var result = new RowDefinitions();
        for (var i = 0; i < count; i++)
        {
            result.Add (new RowDefinition (length));
        }

        return result;
    }

    /// <summary>
    /// Размещение контрола внизу в <see cref="DockPanel"/>>.
    /// </summary>
    public static T DockBottom<T>
        (
            this T control

        )
        where T: Control
    {
        Sure.NotNull (control);

        control.SetValue (DockPanel.DockProperty, Dock.Bottom);

        return control;
    }

    /// <summary>
    /// Размещение контрола слева в <see cref="DockPanel"/>>.
    /// </summary>
    public static T DockLeft<T>
        (
            this T control

        )
        where T: Control
    {
        Sure.NotNull (control);

        control.SetValue (DockPanel.DockProperty, Dock.Left);

        return control;
    }

    /// <summary>
    /// Размещение контрола справа в <see cref="DockPanel"/>>.
    /// </summary>
    public static T DockRight<T>
        (
            this T control

        )
        where T: Control
    {
        Sure.NotNull (control);

        control.SetValue (DockPanel.DockProperty, Dock.Right);

        return control;
    }

    /// <summary>
    /// Размещение контрола наверху в <see cref="DockPanel"/>>.
    /// </summary>
    public static T DockTop<T>
        (
            this T control

        )
        where T: Control
    {
        Sure.NotNull (control);

        control.SetValue (DockPanel.DockProperty, Dock.Top);

        return control;
    }

    /// <summary>
    /// Получение главного окна приложения (если оно есть, конечно).
    /// </summary>
    public static Window? GetMainWindow()
    {
        if (Application.Current!.ApplicationLifetime is ClassicDesktopStyleApplicationLifetime classic)
        {
            return classic.MainWindow;
        }

        return null;
    }

    /// <summary>
    /// Установка наклонного начертания для текстового блока.
    /// </summary>
    public static T Italic<T>
        (
            this T block,
            bool italic = true
        )
        where T: TextBlock
    {
        Sure.NotNull (block);

        block.FontStyle = italic ? FontStyle.Italic : FontStyle.Normal;

        return block;
    }

    /// <summary>
    /// Прижатие содержимого контрола влево по центру.
    /// </summary>
    public static T LeftContent<T>
        (
            this T control
        )
        where T: IContentControl
    {
        control.HorizontalContentAlignment = HorizontalAlignment.Left;
        control.VerticalContentAlignment = VerticalAlignment.Center;

        return control;
    }

    /// <summary>
    /// Установка обработчика событий нажатия на кнопку.
    /// </summary>
    public static T OnClick<T>
        (
            this T button,
            EventHandler<RoutedEventArgs> handler
        )
        where T: Button
    {
        Sure.NotNull (button);
        Sure.NotNull (handler);

        button.Click += handler;

        return button;
    }

    /// <summary>
    /// Добыча ассетов из ресурсов Avalonia.
    /// </summary>
    public static Stream? OpenAssetStream
        (
            string assetName
        )
    {
        Sure.NotNullNorEmpty (assetName);

        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
        if (assets is not null)
        {
                var assembly = Assembly.GetEntryAssembly();
                if (assembly is not null)
                {
                    var name = assembly.GetName().Name;
                    if (!string.IsNullOrEmpty (name))
                    {
                        var uri = "avares://" + name + "/" + assetName;
                        return assets.Open (new Uri (uri));
                    }
                }
        }

        return null;
    }

    /// <summary>
    /// Заполнение ячейки грида.
    /// </summary>
    public static Control SetCellObject
        (
            this Grid grid,
            int row,
            int column,
            object? obj,
            HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment verticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment horizontalContentAlignment = HorizontalAlignment.Center,
            VerticalAlignment verticalContentAlignment = VerticalAlignment.Center
        )
    {
        Sure.NotNull (grid);
        Sure.Positive (row);
        Sure.Positive (column);

        var result = new Label
        {
            HorizontalAlignment = horizontalAlignment,
            VerticalAlignment = verticalAlignment,
            HorizontalContentAlignment = horizontalContentAlignment,
            VerticalContentAlignment = verticalContentAlignment,
            Content = obj
        };
        result.SetValue (Grid.RowProperty, row);
        result.SetValue (Grid.ColumnProperty, column);

        grid.Children.Add (result);

        return result;
    }

    /// <summary>
    /// Заполнение ячейки грида.
    /// </summary>
    public static Control SetCellText
        (
            this Grid grid,
            int row,
            int column,
            string? text,
            HorizontalAlignment horizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment verticalAlignment = VerticalAlignment.Stretch
        )
    {
        Sure.NotNull (grid);
        Sure.Positive (row);
        Sure.Positive (column);

        var result = new TextBlock
        {
            HorizontalAlignment = horizontalAlignment,
            VerticalAlignment = verticalAlignment,
            Text = text
        };
        result.SetValue (Grid.RowProperty, row);
        result.SetValue (Grid.ColumnProperty, column);

        grid.Children.Add (result);

        return result;
    }

    /// <summary>
    /// Установка номера столбца для объекта, помещаемого в грид.
    /// </summary>
    public static T SetColumn<T>
        (
            this T obj,
            int column
        )
        where T: AvaloniaObject
    {
        Sure.NotNull (obj);
        Sure.Positive (column);

        obj.SetValue (Grid.ColumnProperty, column);

        return obj;
    }

    /// <summary>
    /// Установка полей внутри контрола.
    /// </summary>
    public static T SetMargin<T>
        (
            this T control,
            Thickness thickness
        )
        where T: ContentControl
    {
        Sure.NotNull (control);

        control.Margin = thickness;

        return control;
    }

    /// <summary>
    /// Установка полей внутри контрола.
    /// </summary>
    public static T SetMargin<T>
        (
            this T control,
            double thickness
        )
        where T: ContentControl
    {
        Sure.NotNull (control);

        control.Margin = new Thickness (thickness);

        return control;
    }

    /// <summary>
    /// Установка полей внутри контрола.
    /// </summary>
    public static T SetMargin<T>
        (
            this T control,
            double horizontal,
            double vertical
        )
        where T: ContentControl
    {
        Sure.NotNull (control);

        control.Margin = new Thickness (horizontal, vertical);

        return control;
    }

    /// <summary>
    /// Установка полей внутри контрола.
    /// </summary>
    public static T SetMargin<T>
        (
            this T control,
            double left,
            double top,
            double right,
            double bottom
        )
        where T: ContentControl
    {
        Sure.NotNull (control);

        control.Margin = new Thickness (left, top, right, bottom);

        return control;
    }

    /// <summary>
    /// Установка полей вокруг текстового блока.
    /// </summary>
    public static T SetPadding<T>
        (
            this T block,
            Thickness thickness
        )
        where T: TextBlock
    {
        Sure.NotNull (block);

        block.Padding = thickness;

        return block;
    }

    /// <summary>
    /// Установка полей вокруг текстового блока.
    /// </summary>
    public static T SetPadding<T>
        (
            this T block,
            double thickness
        )
        where T: TextBlock
    {
        Sure.NotNull (block);

        block.Padding = new Thickness (thickness);

        return block;
    }

    /// <summary>
    /// Установка полей вокруг текстового блока.
    /// </summary>
    public static T SetPadding<T>
        (
            this T block,
            double horizontal,
            double vertical
        )
        where T: TextBlock
    {
        Sure.NotNull (block);

        block.Padding = new Thickness (horizontal, vertical);

        return block;
    }

    /// <summary>
    /// Установка полей вокруг текстового блока.
    /// </summary>
    public static T SetPadding<T>
        (
            this T block,
            double left,
            double top,
            double right,
            double bottom
        )
        where T: TextBlock
    {
        Sure.NotNull (block);

        block.Padding = new Thickness (left, top, right, bottom);

        return block;
    }

    /// <summary>
    /// Установка номера строки для объекта, помещаемого в грид.
    /// </summary>
    public static T SetRow<T>
        (
            this T obj,
            int row
        )
        where T: AvaloniaObject
    {
        Sure.NotNull (obj);
        Sure.Positive (row);

        obj.SetValue (Grid.RowProperty, row);

        return obj;
    }

    /// <summary>
    /// Установка номера строки для объекта, помещаемого в грид.
    /// </summary>
    public static T SetRowAndColumn<T>
        (
            this T obj,
            int row,
            int column
        )
        where T: AvaloniaObject
    {
        Sure.NotNull (obj);
        Sure.Positive (row);
        Sure.Positive (column);

        obj.SetValue (Grid.RowProperty, row);
        obj.SetValue (Grid.ColumnProperty, column);

        return obj;
    }

    /// <summary>
    /// Добавление в заголовок окна информации о версии сборки.
    /// </summary>
    public static void ShowVersionInfoInTitle
        (
            this Window window
        )
    {
        Sure.NotNull (window);

        var assembly = Assembly.GetEntryAssembly();
        var location = assembly?.Location;
        if (string.IsNullOrEmpty (location))
        {
            // TODO: в single-exe-application .Location возвращает string.Empty
            // consider using the AppContext.BaseDirectory
            return;
        }

        // TODO: в single-exe-application .Location возвращает string.Empty
        // consider using the AppContext.BaseDirectory
        var fvi = FileVersionInfo.GetVersionInfo (location);
        var fi = new FileInfo (location);

        window.Title += $": version {fvi.FileVersion} from {fi.LastWriteTime.ToShortDateString()}";
    }

    /// <summary>
    /// Растягивание контрола по горизонтали и по вертикали.
    /// </summary>
    public static T Stretch<T>
        (
            this T control
        )
        where T: Control
    {
        Sure.NotNull (control);

        control.HorizontalAlignment = HorizontalAlignment.Stretch;
        control.VerticalAlignment = VerticalAlignment.Stretch;

        return control;
    }

    /// <summary>
    /// Растягивание контрола по горизонтали.
    /// </summary>
    public static T StretchHorizontally<T>
        (
            this T control
        )
        where T: Control
    {
        Sure.NotNull (control);

        control.HorizontalAlignment = HorizontalAlignment.Stretch;

        return control;
    }

    /// <summary>
    /// Растягивание контрола по вертикали.
    /// </summary>
    public static T StretchVertically<T>
        (
            this T control
        )
        where T: Control
    {
        Sure.NotNull (control);

        control.VerticalAlignment = VerticalAlignment.Stretch;

        return control;
    }

    /// <summary>
    /// Добавление ячейки в грид.
    /// </summary>
    public static Grid WithCell
        (
            this Grid grid,
            int row,
            int column,
            Control control
        )
    {
        Sure.NotNull (grid);
        Sure.InRange (row, grid.RowDefinitions);
        Sure.InRange (column, grid.ColumnDefinitions);

        control.SetValue (Grid.RowProperty, row);
        control.SetValue (Grid.ColumnProperty, column);
        grid.Children.Add (control);

        return grid;
    }

    /// <summary>
    /// Добавление дочерних контролов в панель.
    /// </summary>
    public static T WithChildren<T>
        (
            this T panel,
            params Control[] children
        )
        where T: Panel
    {
        Sure.NotNull (panel);

        panel.Children.AddRange (children);

        return panel;
    }

    #endregion
}
