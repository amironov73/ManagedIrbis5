﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* AvaloniaUtility.cs -- полезные расширения для Avalonia UI
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using ActiproSoftware.UI.Avalonia.Themes;

using AM.Text.Output;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Data.Core;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml.MarkupExtensions.CompiledBindings;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Media;
using Avalonia.Media.Fonts;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Skia;
using Avalonia.Styling;
using Avalonia.Themes.Fluent;
using Avalonia.Themes.Simple;

using JetBrains.Annotations;

using Material.Colors;
using Material.Styles.Themes;
using Material.Styles.Themes.Base;

using Microsoft.Extensions.Logging;

using SkiaSharp;

#endregion

namespace AM.Avalonia;

/// <summary>
/// Полезные расширения для Avalonia UI.
/// </summary>
[PublicAPI]
public static class AvaloniaUtility
{
    #region Public methods

    /// <summary>
    /// only target is Initialized
    /// </summary>
    /// <param name="visual">target</param>
    /// <returns>size</returns>
    public static double ActualWidth (this Visual visual) => visual.Bounds.Width;

    /// <summary>
    /// only target is Initialized
    /// </summary>
    /// <param name="visual">target</param>
    /// <returns>size</returns>
    public static double ActualHeight (this Visual visual) => visual.Bounds.Height;

    /// <summary>
    /// Выполнение произвольных побочных действий.
    /// </summary>
    public static TControl Also<TControl>
        (
            this TControl control,
            Action<TControl> action
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (action);

        action (control);

        return control;
    }

    /// <summary>
    /// Получение ресурса по указанному URL.
    /// </summary>
    public static ResourceInclude? AsResource
        (
            this string url
        )
    {
        Sure.NotNullNorEmpty (url);

        return new Uri (url).AsResource();
    }

    /// <summary>
    /// Получение ресурса по указанному URL.
    /// </summary>
    public static ResourceInclude? AsResource
        (
            this Uri uri
        )
    {
        Sure.NotNull (uri);

        try
        {
            // TODO разобраться с аргументом конструктора и Source
            return new ResourceInclude (uri) { Source = uri };
        }
        catch (Exception exception)
        {
            Magna.Logger.LogDebug (exception, "URI as resource");
        }

        return default;
    }

    /// <summary>
    /// Присваивание указанной переменной.
    /// </summary>
    public static TControl Assign<TControl>
        (
            this TControl control,
            out TControl variable
        )
        where TControl : Control
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
        where T : TextBlock
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
        where T : ContentControl
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
        where T : Control
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
        where T : Control
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
        where T : Control
    {
        Sure.NotNull (control);

        control.VerticalAlignment = VerticalAlignment.Center;

        return control;
    }

    /// <summary>
    /// Создание темы для контрола, основанной на теме для указанного типа.
    /// </summary>
    /// <returns></returns>
    public static ControlTheme? CreateControlTheme
        (
            Type baseControlType,
            Type actualControlType
        )
    {
        Sure.NotNull (baseControlType);
        Sure.NotNull (actualControlType);

        var basedOn = GetControlTheme (baseControlType);
        if (basedOn is null)
        {
            return null;
        }

        var result = new ControlTheme (actualControlType)
        {
            BasedOn = basedOn
        };

        return result;
    }

    /// <summary>
    /// Создание Fluent-темы.
    /// </summary>
    public static IStyle CreateFluentTheme
        (
            bool light = true
        )
    {
        // TODO разобраться с light
        return new FluentTheme();

        // return new FluentTheme (new Uri
        //     (
        //         light
        //         ? "avares://Avalonia.Themes.Fluent/FluentLight.xaml"
        //         : "avares://Avalonia.Themes.Fluent/FluentDark.xaml"
        //     ));
    }

    /// <summary>
    /// Создание Material-темы.
    /// </summary>
    public static IStyle CreateMaterialTheme
        (
            bool light = true
        )
    {
        var baseMode = light ? BaseThemeMode.Light : BaseThemeMode.Dark;

        // var uri = new Uri ("avares://Material.Styles/MaterialToolKit.xaml");
        var result = new MaterialTheme (Magna.Host.Services)
        {
            BaseTheme = baseMode,
            PrimaryColor = PrimaryColor.Blue,
            SecondaryColor = SecondaryColor.Teal
        };

        return result;
    }

    /// <summary>
    /// Создание Simple-темы.
    /// </summary>
    public static IStyle CreateSimpleTheme
        (
            bool light = true
        )
    {
        // TODO разобраться с light
        return new SimpleTheme();

        // var mode = light ? SimpleThemeMode.Light : SimpleThemeMode.Dark;
        // var uri = new Uri ("avares://Avalonia.Themes.Simple/SimpleTheme.xaml");
        // var result = new SimpleTheme (uri)
        // {
        //     Mode = mode
        // };

        // return result;
    }

    /// <summary>
    /// Создание Citrus-темы.
    /// </summary>
    public static IStyle CreateCitrusTheme
        (
            string variant = "Citrus.axaml"
        )
    {
        var simple = CreateSimpleTheme();
        var uri = new Uri ($"avares://AM.Avalonia/Styles/Citrus/{variant}");
        var citrus = new StyleInclude (uri)
        {
            Source = uri
        };

        var result = new Styles
        {
            simple,
            citrus
        };

        return result;
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
    /// Создание Actipro-Modern-темы.
    /// </summary>
    public static IStyle CreateModernTheme()
    {
        return new ModernTheme();
    }

    /// <summary>
    /// Создание стиля для контрола.
    /// </summary>
    public static Style CreateStyle<TControl>()
        where TControl : Control
    {
        return new Style (x => x.OfType<TControl>());
    }

    /// <summary>
    /// Создание стиля с сеттером.
    /// </summary>
    public static Style CreateStyle<TControl>
        (
            AvaloniaProperty property,
            object value
        )
        where TControl : Control
    {
        Sure.NotNull (property);

        return new Style (x => x.OfType<TControl>())
        {
            Setters =
            {
                new Setter (property, value)
            }
        };
    }

    /// <summary>
    /// Размещение контрола внизу в <see cref="DockPanel"/>>.
    /// </summary>
    public static T DockBottom<T>
        (
            this T control
        )
        where T : Control
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
        where T : Control
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
        where T : Control
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
        where T : Control
    {
        Sure.NotNull (control);

        control.SetValue (DockPanel.DockProperty, Dock.Top);

        return control;
    }

    /// <summary>
    /// Получение цвета как динамического ресурса темы.
    /// </summary>
    public static IBinding DynamicColor
        (
            [CallerMemberName] string? key = null
        )
    {
        Sure.NotNullNorEmpty (key);

        return new DynamicResourceExtension (key!);
    }

    /// <summary>
    /// Получение кисти как динамического ресурса темы.
    /// </summary>
    public static IBinding DynamicBrush
        (
            [CallerMemberName] string? key = null
        )
    {
        Sure.NotNullNorEmpty (key);

        return new DynamicResourceExtension (key!);
    }

    /// <summary>
    /// Поиск первого дочернего элемента с контекстом данных указанного типа.
    /// </summary>
    public static IDataContextProvider? FindChildWithDataContext<TDataContext>
        (
            this ILogical control
        )
    {
        Sure.NotNull (control);

        foreach (var child in control.LogicalChildren)
        {
            if (child is IDataContextProvider { DataContext: TDataContext } found)
            {
                return found;
            }

            if (child.FindChildWithDataContext<TDataContext>() is { } inDepth)
            {
                return inDepth;
            }
        }

        return null;
    }

    /// <summary>
    /// Получение кисти как статического ресурса темы.
    /// </summary>
    public static IBrush FindBrush
        (
            this IResourceHost host,
            [CallerMemberName] string? key = null
        )
    {
        Sure.NotNullNorEmpty (key);

        var found = host.FindResource (key!);
        if (found is null)
        {
            return Brushes.Black;
        }

        return (IBrush)found;
    }

    /// <summary>
    /// Получение цвета как статического ресурса темы.
    /// </summary>
    /// <example>
    /// Предполагаемый сценарий использования
    /// <code>
    /// public static Color SystemAccentColor (this IResourceHost host) =&gt; host.FindColor();
    /// </code>
    /// </example>
    public static Color FindColor
        (
            this IResourceHost host,
            [CallerMemberName] string? key = null
        )
    {
        Sure.NotNullNorEmpty (key);

        var found = host.FindResource (key!);
        if (found is null)
        {
            return Colors.Black;
        }

        return (Color)found;
    }

    /// <summary>
    /// Получение скругления углов как статического ресурса темы.
    /// </summary>
    public static CornerRadius FindCornerRadius
        (
            this IResourceHost host,
            [CallerMemberName] string? key = null
        )
    {
        Sure.NotNullNorEmpty (key);

        var found = host.FindResource (key!);
        if (found is null)
        {
            return default;
        }

        return (CornerRadius)found;
    }

    /// <summary>
    /// Получение числа с плавающей точкой как статического ресурса темы.
    /// </summary>
    public static double FindDouble
        (
            this IResourceHost host,
            [CallerMemberName] string? key = null
        )
    {
        Sure.NotNullNorEmpty (key);

        var found = host.FindResource (key!);
        if (found is null)
        {
            return 0;
        }

        return Convert.ToDouble (found);
    }

    /// <summary>
    /// Получение размера как статического ресурса темы.
    /// </summary>
    public static Size FindSize
        (
            this IResourceHost host,
            [CallerMemberName] string? key = null
        )
    {
        Sure.NotNullNorEmpty (key);

        var found = host.FindResource (key!);
        if (found is null)
        {
            return default;
        }

        return (Size)found;
    }

    /// <summary>
    /// Получение толщины как статического ресурса темы.
    /// </summary>
    public static Thickness FindThickness
        (
            this IResourceHost host,
            [CallerMemberName] string? key = null
        )
    {
        Sure.NotNullNorEmpty (key);

        var found = host.FindResource (key!);
        if (found is null)
        {
            return default;
        }

        return (Thickness)found;
    }

    /// <summary>
    /// Пытается вписать внутренний прямоугольник во внешний.
    /// </summary>
    public static Rect Fit
        (
            Rect outer,
            Rect inner
        )
    {
        var outerAspect = outer.Width / outer.Height;
        var innerAspect = inner.Width / inner.Height;
        var fitInHeight = outerAspect < innerAspect;
        var ratio = fitInHeight
            ? outer.Width / inner.Width
            : outer.Height / inner.Height;
        var width = inner.Width * ratio;
        var height = inner.Height * ratio;
        var left = (outer.Width - width) / 2;
        var top = (outer.Height - height) / 2;
        return new Rect (left, top, width, height);
    }

    /// <summary>
    /// Получение темы для контрола указанного типа.
    /// </summary>
    public static ControlTheme? GetControlTheme
        (
            Type controlType
        )
    {
        Sure.NotNull (controlType);

        while (controlType.IsAssignableTo (typeof (Control)))
        {
            // TODO разобраться с аргументом ThemeVariant
            if (Application.Current!.Styles.TryGetResource (controlType, null, out var result))
            {
                return result as ControlTheme;
            }

            var baseType = controlType.BaseType;
            if (baseType is null)
            {
                break;
            }

            controlType = baseType;
        }

        return null;
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
    /// Получение родительского контекста указанного типа.
    /// </summary>
    public static TDataContext? GetParentDataContext<TDataContext>
        (
            this Control control
        )
        where TDataContext : class
    {
        Sure.NotNull (control);

        var parent = control.Parent;
        while (parent is not null)
        {
            if (parent.DataContext is TDataContext found)
            {
                return found;
            }

            parent = parent.Parent;
        }

        return null;
    }

    /// <summary>
    /// Поиск родительского контрола с контекстом данных
    /// указанного типа.
    /// </summary>
    public static Control? GetParentWithDataContext<TDataContext>
        (
            this Control control
        )
        where TDataContext : class
    {
        Sure.NotNull (control);

        var parent = control.Parent;
        while (parent is not null)
        {
            if (parent.DataContext is TDataContext
                && parent is Control parentControl)
            {
                return parentControl;
            }

            parent = parent.Parent;
        }

        return null;
    }

    /// <summary>
    /// Поиск родительского контрола указанного типа.
    /// </summary>
    public static TParent? GetParentOfType<TParent>
        (
            this Control control
        )
        where TParent : Control
    {
        Sure.NotNull (control);

        var parent = control.Parent;
        while (parent is not null)
        {
            if (parent is TParent found)
            {
                return found;
            }

            parent = parent.Parent;
        }

        return null;
    }

    /// <summary>
    /// Кисть с цветом фона текущей темы.
    /// </summary>
    public static Brush GetThemeBackgroundBrush() =>
        GetThemeResources (Application.Current!).ThemeBackgroundBrush;

    /// <summary>
    /// Кисть с цветом текста текущей темы.
    /// </summary>
    public static Brush GetThemeForegroundBrush() =>
        GetThemeResources (Application.Current!).ThemeForegroundBrush;

    /// <summary>
    /// Получение доступа к ресурсам темы.
    /// </summary>
    public static IThemeResources GetThemeResources
        (
            IResourceHost resourceHost
        )
    {
        Sure.NotNull (resourceHost);

        foreach (var style in Application.Current!.Styles)
        {
            if (style is FluentTheme)
            {
                return new FluentThemeResources (resourceHost);
            }

            if (style is SimpleTheme)
            {
                return new SimpleThemeResources (resourceHost);
            }
        }

        return new FallbackThemeResources();
    }

    /// <summary>
    /// Получение окна, которому принадлежит указанный контрол.
    /// </summary>
    public static Window GetWindow
        (
            this Control control
        )
    {
        Sure.NotNull (control);

        var parent = control.Parent;
        while (parent is not null)
        {
            if (parent is Window found)
            {
                return found;
            }

            parent = parent.Parent;
        }

        throw new ArsMagnaException ($"Can't find window for {control}");
    }

    /// <summary>
    /// Простая группировка элементов по горизонтали.
    /// </summary>
    public static StackPanel HorizontalGroup
        (
            params Control[] controls
        )
    {
        var result = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        result.Children.AddRange (controls);

        return result;
    }

    /// <summary>
    /// Включение ссылки на наши стили.
    /// </summary>
    public static IStyle IncludeArsMagnaStyles()
    {
        var uri = new Uri ("avares://AM.Avalonia/Styles.axaml");
        var result = new StyleInclude (uri)
        {
            Source = uri
        };

        return result;
    }

    /// <summary>
    /// Включение ссылки на стили ColorPicker.
    /// </summary>
    public static IStyle IncludeColorPickerStyles()
    {
        var uri = new Uri ("avares://Avalonia.Controls.ColorPicker/Themes/Fluent/Fluent.xaml");
        var result = new StyleInclude (uri)
        {
            Source = uri
        };

        return result;
    }

    /// <summary>
    /// Включение ссылки на стили DataGrid.
    /// </summary>
    public static IStyle IncludeDataGridStyles
        (
            string theme = "Fluent"
        )
    {
        var gridUri = new Uri ($"avares://Avalonia.Controls.DataGrid/Themes/{theme}.xaml");
        var result = new StyleInclude (gridUri)
        {
            Source = gridUri
        };

        return result;
    }

    /// <summary>
    /// Включение ссылки на стили DataGrid.
    /// </summary>
    public static IStyle IncludeLabsStyles()
    {
        var labsUri = new Uri ($"avares://Avalonia.Labs.Controls/Themes/ControlThemes.xaml");
        var result = new StyleInclude (labsUri)
        {
            Source = labsUri
        };

        return result;
    }

    /// <summary>
    /// Включение ссылки на стили TreeDataGrid.
    /// </summary>
    public static IStyle IncludeTreeDataGridStyles()
    {
        var treeDataGridUri = new Uri ("avares://Avalonia.Controls.TreeDataGrid/Themes/Fluent.axaml");
        var result = new StyleInclude (treeDataGridUri)
        {
            Source = treeDataGridUri
        };

        return result;
    }

    /// <summary>
    /// Релизная или отладочная версия сборки?
    /// </summary>
    public static bool IsProduction()
    {
#if DEBUG
        return false;
#else
        return true;
#endif
    }

    /// <summary>
    /// Установка наклонного начертания для текстового блока.
    /// </summary>
    public static T Italic<T>
        (
            this T block,
            bool italic = true
        )
        where T : TextBlock
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
        where T : ContentControl
    {
        control.HorizontalContentAlignment = HorizontalAlignment.Left;
        control.VerticalContentAlignment = VerticalAlignment.Center;

        return control;
    }

    /// <summary>
    /// Загрузка картинки из ассетов.
    /// </summary>
    public static Bitmap? LoadBitmapFromAssets
        (
            this Control control,
            string assetName
        )
    {
        Sure.NotNull (control);
        Sure.NotNullNorEmpty (assetName);

        using var stream = OpenAssetStream (control.GetType(), assetName);
        return stream is not null
            ? new Bitmap (stream)
            : null;
    }

    /// <summary>
    /// Создание рамки.
    /// </summary>
    public static Border MakeBorder (Control control)
        => new () { Child = control };

    /// <summary>
    /// Создание рамки с контролами, ориентированных вертикально.
    /// </summary>
    public static Border MakeBorder (params Control[] children) => new ()
    {
        Child = new StackPanel
        {
            Orientation = Orientation.Vertical
        }.WithChildren (children)
    };

    /// <summary>
    /// Создание <c>CompiledBinding</c> из кода.
    /// </summary>
    /// <param name="source">Объект, к которому осуществляется привязка.</param>
    /// <param name="format">Формат.</param>
    /// <param name="propertyName">Имя свойства.</param>
    /// <param name="getter">Геттер.</param>
    /// <param name="setter">Сеттер.</param>
    /// <typeparam name="TProperty">Тип свойства.</typeparam>
    /// <returns>Созданная <c>CompiledBinding</c></returns>
    public static CompiledBindingExtension MakeBinding<TProperty>
        (
            object source,
            string? format,
            string propertyName,
            Func<object, object?> getter,
            Action<object, object?> setter
        )
    {
        Sure.NotNull (source);
        Sure.NotNullNorEmpty (propertyName);
        Sure.NotNull (getter);

        var result = MakeBinding<TProperty> (propertyName, getter, setter);
        result.Source = source;
        result.StringFormat = format;

        return result;
    }

    /// <summary>
    /// Создание <c>CompiledBinding</c> из кода.
    /// </summary>
    /// <param name="propertyName">Имя свойства.</param>
    /// <param name="getter">Геттер.</param>
    /// <param name="setter">Сеттер.</param>
    /// <typeparam name="TProperty">Тип свойства.</typeparam>
    /// <returns>Созданная <c>CompiledBinding</c></returns>
    public static CompiledBindingExtension MakeBinding<TProperty>
        (
            string propertyName,
            Func<object, object?> getter,
            Action<object, object?> setter
        )
    {
        Sure.NotNullNorEmpty (propertyName);
        Sure.NotNull (getter);

        // setter может быть null, если очень нужно

        var propertyInfo = new ClrPropertyInfo
            (
                propertyName,
                getter,
                setter,
                typeof (TProperty)
            );

        var path = new CompiledBindingPathBuilder()
            .Property
                (
                    propertyInfo,
                    PropertyInfoAccessorFactory.CreateInpcPropertyAccessor
                )
            .Build();

        return new CompiledBindingExtension (path);
    }

    /// <summary>
    /// Создание кнопки с указанным текстом.
    /// </summary>
    public static Button MakeButton (object text) => new () { Content = text };

    /// <summary>
    /// Создание метки с указанным текстом.
    /// </summary>
    public static Label MakeLabel (object text) => new () { Content = text };

    /// <summary>
    /// Измерение строки.
    /// </summary>
    public static Size MeasureString
        (
            this string text,
            double fontSize,
            SKTypeface typeface
        )
    {
        Sure.Positive (fontSize);
        Sure.NotNull (typeface);

        if (string.IsNullOrEmpty (text))
        {
            return default;
        }

        try
        {
            using var paint = new SKPaint();
            paint.Typeface = typeface;
            paint.Style = SKPaintStyle.Fill;
            paint.TextSize = Convert.ToSingle (fontSize);
            var result = new SKRect();
            paint.MeasureText (text, ref result);
            var width = Convert.ToSingle (Math.Ceiling (result.Size.Width));
            var height = Convert.ToSingle (Math.Ceiling (result.Size.Height));

            return new Size (width, height);
        }
        catch (Exception exception)
        {
            Magna.Logger.LogDebug (exception, nameof (MeasureString));
        }

        return default;
    }

    /// <summary>
    /// Установка обработчика событий нажатия на кнопку.
    /// </summary>
    public static T OnClick<T>
        (
            this T button,
            EventHandler<RoutedEventArgs> handler
        )
        where T : Button
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
            Type type,
            string assetName
        )
    {
        Sure.NotNull (type);
        Sure.NotNullNorEmpty (assetName);

        var assembly = type.Assembly;
        var name = assembly.GetName().Name;
        if (!string.IsNullOrEmpty (name))
        {
            var uri = "avares://" + name + "/" + assetName;
            return AssetLoader.Open (new Uri (uri));
        }

        return null;
    }

    /// <summary>
    /// Print system information in abstract output.
    /// </summary>
    public static void PrintSystemInformation
        (
            this AbstractOutput? output
        )
    {
        if (output is not null)
        {
            output.WriteLine
                (
                    "OS version: {0}",
                    Environment.OSVersion
                );
            output.WriteLine
                (
                    "Framework version: {0}",
                    Environment.Version
                );
            var assembly = Assembly.GetEntryAssembly();
            var vi = assembly?.GetName().Version;
            if (assembly?.Location is null)
            {
                // TODO: в single-exe-application .Location возвращает string.Empty
                // consider using the AppContext.BaseDirectory
                return;
            }

            // TODO: в single-exe-application .Location возвращает string.Empty
            // consider using the AppContext.BaseDirectory
            var fi = new FileInfo (assembly.Location);
            output.WriteLine
                (
                    "Application version: {0} ({1})",
                    vi.ToVisibleString(),
                    fi.LastWriteTime.ToShortDateString()
                );
            output.WriteLine
                (
                    "Memory: {0} Mb",
                    GC.GetTotalMemory (false) / 1024
                );
        }
    }

    /// <summary>
    /// Установка выравнивания по горизонтали.
    /// </summary>
    public static Setter SetAlignment
        (
            HorizontalAlignment alignment
        )
        => new ()
        {
            Property = Layoutable.HorizontalAlignmentProperty,
            Value = alignment
        };

    /// <summary>
    /// Установка выравнивания по вертикали.
    /// </summary>
    public static Setter SetAlignment
        (
            VerticalAlignment alignment
        )
        => new ()
        {
            Property = Layoutable.VerticalAlignmentProperty,
            Value = alignment
        };

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
            int column,
            int span = 0
        )
        where T : AvaloniaObject
    {
        Sure.NotNull (obj);

        obj.SetValue (Grid.ColumnProperty, column);
        if (span > 0)
        {
            obj.SetValue (Grid.ColumnSpanProperty, span);
        }

        return obj;
    }

    /// <summary>
    /// Установка полей снаружи контрола.
    /// </summary>
    public static T SetMargin<T>
        (
            this T control,
            Thickness thickness
        )
        where T : ContentControl
    {
        Sure.NotNull (control);

        control.Margin = thickness;

        return control;
    }

    /// <summary>
    /// Установка полей снаружи контрола.
    /// </summary>
    public static T SetMargin<T>
        (
            this T control,
            double thickness
        )
        where T : ContentControl
    {
        Sure.NotNull (control);

        control.Margin = new Thickness (thickness);

        return control;
    }

    /// <summary>
    /// Установка полей снаружи контрола.
    /// </summary>
    public static Setter SetMargin
        (
            double thickness
        )
        => new ()
        {
            Property = Layoutable.MarginProperty,
            Value = new Thickness (thickness)
        };

    /// <summary>
    /// Установка полей снаружи контрола.
    /// </summary>
    public static T SetMargin<T>
        (
            this T control,
            double horizontal,
            double vertical
        )
        where T : ContentControl
    {
        Sure.NotNull (control);

        control.Margin = new Thickness (horizontal, vertical);

        return control;
    }

    /// <summary>
    /// Установка полей снаружи контрола.
    /// </summary>
    public static Setter SetMargin
        (
            double horizontal,
            double vertical
        )
        => new ()
        {
            Property = Layoutable.MarginProperty,
            Value = new Thickness (horizontal, vertical)
        };

    /// <summary>
    /// Установка полей снаружи контрола.
    /// </summary>
    public static T SetMargin<T>
        (
            this T control,
            double left,
            double top,
            double right,
            double bottom
        )
        where T : ContentControl
    {
        Sure.NotNull (control);

        control.Margin = new Thickness (left, top, right, bottom);

        return control;
    }

    /// <summary>
    /// Установка полей для стиля.
    /// </summary>
    public static Style SetMargin
        (
            this Style style,
            double value
        )
    {
        Sure.NotNull (style);

        style.Setters.Add (new Setter
        {
            Property = Layoutable.MarginProperty,
            Value = new Thickness (value)
        });

        return style;
    }

    /// <summary>
    /// Установка полей снаружи контрола.
    /// </summary>
    public static Setter SetMargin
        (
            double left,
            double top,
            double right,
            double bottom
        )
        => new ()
        {
            Property = Layoutable.MarginProperty,
            Value = new Thickness (left, top, right, bottom)
        };

    /// <summary>
    /// Установка полей внутри текстового блока.
    /// </summary>
    public static T SetPadding<T>
        (
            this T block,
            Thickness thickness
        )
        where T : TextBlock
    {
        Sure.NotNull (block);

        block.Padding = thickness;

        return block;
    }

    /// <summary>
    /// Установка полей внутри текстового блока.
    /// </summary>
    public static T SetPadding<T>
        (
            this T block,
            double thickness
        )
        where T : TextBlock
    {
        Sure.NotNull (block);

        block.Padding = new Thickness (thickness);

        return block;
    }

    /// <summary>
    /// Установка полей внутри контрола.
    /// </summary>
    public static Setter SetPadding
        (
            double thickness
        )
        => new ()
        {
            Property = TemplatedControl.PaddingProperty,
            Value = new Thickness (thickness)
        };

    /// <summary>
    /// Установка полей внутри текстового блока.
    /// </summary>
    public static T SetPadding<T>
        (
            this T block,
            double horizontal,
            double vertical
        )
        where T : TextBlock
    {
        Sure.NotNull (block);

        block.Padding = new Thickness (horizontal, vertical);

        return block;
    }

    /// <summary>
    /// Установка полей внутри контрола.
    /// </summary>
    public static Setter SetPadding
        (
            double horizontal,
            double vertical
        )
        => new ()
        {
            Property = TemplatedControl.PaddingProperty,
            Value = new Thickness (horizontal, vertical)
        };

    /// <summary>
    /// Установка полей внутри текстового блока.
    /// </summary>
    public static T SetPadding<T>
        (
            this T block,
            double left,
            double top,
            double right,
            double bottom
        )
        where T : TextBlock
    {
        Sure.NotNull (block);

        block.Padding = new Thickness (left, top, right, bottom);

        return block;
    }

    /// <summary>
    /// Установка полей внутри контрола.
    /// </summary>
    public static Setter SetPadding
        (
            double left,
            double top,
            double right,
            double bottom
        )
        => new ()
        {
            Property = TemplatedControl.PaddingProperty,
            Value = new Thickness (left, top, right, bottom)
        };

    /// <summary>
    /// Установка полей для стиля.
    /// </summary>
    /// <returns></returns>
    public static Style SetPadding
        (
            this Style style,
            double value
        )
    {
        Sure.NotNull (style);

        style.Setters.Add (new Setter
        {
            Property = TemplatedControl.PaddingProperty,
            Value = value
        });

        return style;
    }

    /// <summary>
    /// Установка полей снаружи панели.
    /// </summary>
    public static T SetPanelMargin<T>
        (
            this T control,
            Thickness thickness
        )
        where T : Panel
    {
        Sure.NotNull (control);

        control.Margin = thickness;

        return control;
    }

    /// <summary>
    /// Установка полей снаружи панели.
    /// </summary>
    public static T SetPanelMargin<T>
        (
            this T control,
            double uniform
        )
        where T : Panel
    {
        Sure.NotNull (control);

        control.Margin = new Thickness (uniform);

        return control;
    }

    /// <summary>
    /// Установка полей снаружи панели.
    /// </summary>
    public static T SetPanelMargin<T>
        (
            this T control,
            double horizontal,
            double vertical
        )
        where T : Panel
    {
        Sure.NotNull (control);

        control.Margin = new Thickness (horizontal, vertical);

        return control;
    }

    /// <summary>
    /// Установка номера строки для объекта, помещаемого в грид.
    /// </summary>
    public static T SetRow<T>
        (
            this T obj,
            int row,
            int span = 0
        )
        where T : AvaloniaObject
    {
        Sure.NotNull (obj);

        if (row >= 0)
        {
            obj.SetValue (Grid.RowProperty, row);
        }

        if (span > 0)
        {
            obj.SetValue (Grid.RowSpanProperty, span);
        }

        return obj;
    }

    /// <summary>
    /// Установка номера строки для объекта, помещаемого в грид.
    /// </summary>
    public static T SetRowAndColumn<T>
        (
            this T obj,
            int row,
            int column,
            int rowSpan = 0,
            int colSpan = 0
        )
        where T : AvaloniaObject
    {
        Sure.NotNull (obj);
        Sure.Positive (row);
        Sure.Positive (column);

        if (row >= 0)
        {
            obj.SetValue (Grid.RowProperty, row);
        }

        if (column >= 0)
        {
            obj.SetValue (Grid.ColumnProperty, column);
        }

        if (rowSpan > 0)
        {
            obj.SetValue (Grid.RowSpanProperty, rowSpan);
        }

        if (colSpan > 0)
        {
            obj.SetValue (Grid.ColumnSpanProperty, colSpan);
        }

        return obj;
    }

    /// <summary>
    /// Установка промежутка между вложенными контролами.
    /// </summary>
    public static Setter SetSpacing
        (
            double spacing
        )
        => new ()
        {
            Property = StackPanel.SpacingProperty,
            Value = spacing
        };

    /// <summary>
    /// Установка иконки для окна.
    /// </summary>
    public static void SetWindowIcon
        (
            this Window window,
            string iconName
        )
    {
        Sure.NotNull (window);
        Sure.NotNullNorEmpty (iconName);

        using var stream = OpenAssetStream (window.GetType(), iconName);
        if (stream is not null)
        {
            window.Icon = new WindowIcon (stream);
        }
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
    /// Завершение приложения.
    /// </summary>
    /// <param name="exitCode">Код завершения приложения.
    /// Ненулевой код, как правило, свидетельствует об ошибке.
    /// </param>
    public static void Shutdown
        (
            int exitCode = 0
        )
    {
        if (Application.Current is { } application)
        {
            if (application.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
            {
                lifetime.Shutdown (exitCode);
            }
            else
            {
                Environment.Exit (exitCode);
            }
        }
    }

    /// <summary>
    /// Растягивание контрола по горизонтали и по вертикали.
    /// </summary>
    public static T Stretch<T>
        (
            this T control
        )
        where T : Control
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
        where T : Control
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
        where T : Control
    {
        Sure.NotNull (control);

        control.VerticalAlignment = VerticalAlignment.Stretch;

        return control;
    }

    /// <summary>
    /// Попытка завершения приложения.
    /// </summary>
    /// <param name="exitCode">Код завершения приложения.
    /// Ненулевой код, как правило, свидетельствует об ошибке.</param>
    public static void TryShutdown
        (
            int exitCode = 0
        )
    {
        if (Application.Current is { } application)
        {
            if (application.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
            {
                lifetime.TryShutdown (exitCode);
            }
            else
            {
                Environment.Exit (exitCode);
            }
        }
    }

    /// <summary>
    /// Простая группировка элементов по вертикали.
    /// </summary>
    public static StackPanel VerticalGroup
        (
            params Control[] controls
        )
    {
        var result = new StackPanel
        {
            Orientation = Orientation.Vertical
        };
        result.Children.AddRange (controls);

        return result;
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
        where T : Panel
    {
        Sure.NotNull (panel);

        panel.Children.AddRange (children);

        return panel;
    }

    // До новой Авалонии, в 11.1 должен появиться нормальный метод
    //==================

    private static MethodInfo? _renderMethod;
    private static Type? _platformDrawingContext;

    /// <summary>
    /// Отрисовка визуального элемента.
    /// </summary>
    public static void Render
        (
            DrawingContext context,
            Visual visual,
            Rect clipRect
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (visual);

        if (_renderMethod is null)
        {
            var assembly = typeof (AvaloniaObject).Assembly;
            var type = assembly.GetType
                    (
                        "Avalonia.Rendering.ImmediateRenderer",
                        true
                    )
                .ThrowIfNull();
            const BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public;
            var types = new[]
            {
                typeof (DrawingContext),
                typeof (Visual),
                typeof (Rect)
            };
            _renderMethod = type.GetMethod ("Render", bindingFlags, types)
                .ThrowIfNull();
        }

        _renderMethod.Invoke (null, new object[] { context, visual, clipRect });
    }

    /// <summary>
    /// Отрисовка контрола на картинке.
    /// </summary>
    public static Task Render
        (
            SKCanvas canvas,
            Visual visual
        )
    {
        // TODO посмотреть, не изменилось ли чего, вдруг, теперь можно рендерить по-человечески

        if (_platformDrawingContext is null)
        {
            var assembly = typeof (AvaloniaObject).Assembly;
            _platformDrawingContext = assembly.GetType
                    (
                        "Avalonia.Media.PlatformDrawingContext",
                        true
                    )
                .ThrowIfNull();
        }

        using var drawingContextImpl = global::Avalonia.Skia.Helpers.DrawingContextHelper.WrapSkiaCanvas
            (
                canvas,
                SkiaPlatform.DefaultDpi
            );
        using var drawingContext = (DrawingContext)Activator.CreateInstance
                (
                    _platformDrawingContext,
                    drawingContextImpl,
                    false
                )
            .ThrowIfNull();

        Render (drawingContext, visual, visual.Bounds);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Получение шрифта Fira Mono.
    /// </summary>
    public static FontFamily GetFiraMono() => new ("fonts:Fira#Fira Mono");

    /// <summary>
    /// Добавление коллекции шрифтов Fira Mono.
    /// </summary>
    public static AppBuilder WithFiraMono
        (
            this AppBuilder builder
        )
    {
        Sure.NotNull (builder);

        builder.ConfigureFonts (fontManager =>
        {
            fontManager.AddFontCollection
                (
                    new EmbeddedFontCollection
                        (
                            new Uri ("fonts:Fira", UriKind.Absolute),
                            new Uri ("avares://AM.Avalonia/Assets/Fonts/Fira", UriKind.Absolute)
                        )
                );
        });

        return builder;
    }

    /// <summary>
    /// Отрисовка последовательности символов.
    /// </summary>
    public static void DrawGlyphRun
        (
            DrawingContext context,
            IGlyphTypeface typeface,
            double emSize,
            ReadOnlyMemory<char> text,
            Point point,
            IBrush brush
        )
    {
        var length = text.Length;
        var glyphs = new ushort[length];
        for (var i = 0; i < length; i++)
        {
            glyphs[i] = typeface.GetGlyph (text.Span[i]);
        }

        var run = new GlyphRun
            (
                typeface,
                emSize,
                text,
                glyphs,
                point
            );
        context.DrawGlyphRun (brush, run);
    }

    #endregion
}
