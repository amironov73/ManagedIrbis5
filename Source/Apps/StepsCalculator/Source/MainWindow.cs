// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable StringLiteralTypo

/* MainWindow.cs -- главное окно приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Reactive.Linq;

using AM.Avalonia;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.ReactiveUI;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

#nullable enable

namespace StepsCalculator;

/// <summary>
/// Главное окно приложения
/// </summary>
public sealed class MainWindow
    : ReactiveWindow<MainWindow.Model>
{
    #region View model

    /// <summary>
    /// Модель данных.
    /// </summary>
    public sealed class Model
        : ReactiveObject
    {
        #region Properties

        /// <summary>
        /// Количество изображений.
        /// </summary>
        [Reactive]
        public int ImageCount { get; set; }

        /// <summary>
        /// Количество повторений.
        /// </summary>
        [Reactive]
        public int Repeats { get; set; }

        /// <summary>
        /// Количество эпох.
        /// </summary>
        [Reactive]
        public int Epochs { get; set; }

        /// <summary>
        /// Размер пакета.
        /// </summary>
        [Reactive]
        public int BatchSize { get; set; }

        /// <summary>
        /// Вычисленное количество шагов.
        /// </summary>
        [ObservableAsProperty]
        public int Steps => 0;

        [ObservableAsProperty]
        public string Recommendation => "Начните работу с программой";

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Model()
        {
            // считаем шаги
            this.WhenAnyValue
                    (
                        first => first.ImageCount,
                        second => second.Repeats,
                        third => third.Epochs,
                        fourth => fourth.BatchSize
                    )
                .Select
                    (
                        data =>
                        {
                            var imageCount = Math.Max (data.Item1, 1);
                            var repeats = Math.Max (data.Item2, 1);
                            var epochs = Math.Max (data.Item3, 1);
                            var batchSize = Math.Max (data.Item4, 1);
                            var result = imageCount * repeats * epochs / batchSize;
                            return result;
                        })
                .ToPropertyEx (this, vm => vm.Steps);

            // выдаем рекомендации
            this.WhenAnyValue
                    (
                        first => first.ImageCount
                    )
                .Select
                    (
                        data =>
                        {
                            var imageCount = Math.Max (data, 1);
                            var repeats = imageCount switch
                            {
                                <= 20 => 10,
                                <= 100 => 3,
                                _ => 1
                            };
                            var epochs = imageCount switch
                            {
                                <= 10 => 20,
                                _ => 10
                            };

                            return $"повторений: {repeats}, эпох: {epochs}";
                        })
                .ToPropertyEx (this, vm => vm.Recommendation);
        }

        #endregion
    }

    #endregion

    #region Application class

    /// <summary>
    /// Класс приложения.
    /// </summary>
    private sealed class App
        : Application
    {
        /// <inheritdoc cref="Application.Initialize"/>
        public override void Initialize()
        {
            Styles.Add (AvaloniaUtility.CreateFluentTheme());
        }

        /// <inheritdoc cref="Application.OnFrameworkInitializationCompleted"/>
        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }

    #endregion

    #region Window members

    /// <summary>
    /// Вызывается, когда окно проинициализировано фреймворком.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        this.SetWindowIcon ("ladder.ico");
        Title = "Калькулятор шагов обучения LoRA";
        Width = MinWidth = MaxWidth = 380;
        Height = MinHeight = MaxHeight = 450;

        DataContext = new Model
        {
            ImageCount = 50,
            Repeats = 20,
            BatchSize = 1,
            Epochs = 1
        };

        Content = new StackPanel
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Spacing = 5,
            Children =
            {
                CreateLabel ("Изображений:"),
                CreateUpDown (nameof (Model.ImageCount)),

                CreateLabel ("Повторений:"),
                CreateUpDown (nameof (Model.Repeats)),

                CreateLabel ("Эпох:"),
                CreateUpDown (nameof (Model.Epochs)),

                CreateLabel ("Размер пакета:"),
                CreateUpDown (nameof (Model.BatchSize)),

                CreateLabel ("Шагов:"),
                CreateTextBox (nameof (Model.Steps), isReadOnly: true),

                CreateLabel ("Рекомендация:"),
                CreateTextBox (nameof (Model.Recommendation), isReadOnly: true)
            }
        };

        Label CreateLabel (string text) =>
            new ()
            {
                Width = 200,
                Content = text
            };

        NumericUpDown CreateUpDown (string propertyName, bool isReadOnly = false) =>
            new()
            {
                Name = propertyName,
                Width = 200,
                IsReadOnly = isReadOnly,
                Minimum = 1,
                FormatString = "0",
                HorizontalAlignment = HorizontalAlignment.Center,
                [!NumericUpDown.ValueProperty] = new Binding (propertyName)
            };

        TextBox CreateTextBox (string propertyName, bool isReadOnly = false) =>
            new()
            {
                Name = propertyName,
                Width = 200,
                IsReadOnly = isReadOnly,
                HorizontalAlignment = HorizontalAlignment.Center,
                [!TextBox.TextProperty] = new Binding (propertyName)
            };
    }

    #endregion

    #region Program entry point

    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    /// <param name="args">Аргументы командной строки</param>
    [STAThread]
    public static void Main
        (
            string[] args
        )
    {
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime (args);
        // DesktopApplication
        //     .Run<MainWindow> (args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    private static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .UseReactiveUI()
            .LogToTrace();
    }

    #endregion
}
