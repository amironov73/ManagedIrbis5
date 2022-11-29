// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

/* MainWindow.cs -- главное окно приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

using AM;
using AM.AOT;
using AM.Avalonia;
using AM.Avalonia.AppServices;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.ReactiveUI;

using Microsoft.Extensions.Logging;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

#nullable enable

namespace Rendition;

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
        /// Язык, с которого выполняется перевод.
        /// </summary>
        [Reactive]
        public string? FromLanguage { get; set; }

        /// <summary>
        /// Язык, на который выполняется перевод.
        /// </summary>
        [Reactive]
        public string? ToLanguage { get; set; }

        /// <summary>
        /// Текст для перевода.
        /// </summary>
        [Reactive]
        public string? SourceText { get; set; }

        /// <summary>
        /// Результат перевода.
        /// </summary>
        [Reactive]
        public string? TranslatedText { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Копирование переведенного текста в буфер обмена.
        /// </summary>
        public async Task Copy()
        {
            var translatedText = TranslatedText;
            if (Application.Current is { Clipboard: { } clipboard }
                && !string.IsNullOrEmpty (translatedText))
            {
                await clipboard.SetTextAsync (translatedText);
            }
        }

        /// <summary>
        /// Вставка текста из буфера обмена в редактор для перевода.
        /// </summary>
        public async Task Paste()
        {
            if (Application.Current is { Clipboard: { } clipboard })
            {
                var sourceText = await clipboard.GetTextAsync();
                if (!string.IsNullOrEmpty (sourceText))
                {
                    SourceText = sourceText;
                }
            }

        }

        /// <summary>
        /// Проверка возможности перевода.
        /// </summary>
        public IObservable<bool> CanTranslate => this.WhenAnyValue
            (
                x => x.FromLanguage,
                x => x.ToLanguage,
                x => x.SourceText,
                (fromLang, toLang, source) =>
                    !string.IsNullOrWhiteSpace (fromLang)
                    && !string.IsNullOrWhiteSpace (toLang)
                    && fromLang != toLang
                    && !string.IsNullOrWhiteSpace (source)
            );

        /// <summary>
        /// Перевод.
        /// </summary>
        public void Translate()
        {
            try
            {
                var translator = new SimplestGoogleTranslateClient();
                var fromLanguage = FromLanguage.SafeTrim()
                    .ThrowIfNullOrEmpty();
                var toLanguage = ToLanguage.SafeTrim()
                    .ThrowIfNullOrEmpty();
                var sourceText = SourceText.SafeTrim()
                    .ThrowIfNullOrEmpty();
                TranslatedText = translator.Translate
                    (
                        fromLanguage,
                        toLanguage,
                        sourceText
                    );

                Magna.Logger.LogInformation
                    (
                        "Translated '{FromLanguage}' '{Source}' to '{ToLanguage}' '{Translated}'",
                        fromLanguage,
                        sourceText,
                        toLanguage,
                        TranslatedText
                    );
            }
            catch (Exception exception)
            {
                TranslatedText = exception.ToString();

                Magna.Logger.LogInformation
                    (
                        "Translating '{FromLanguage}' '{Source}' to '{ToLanguage}'",
                        FromLanguage,
                        SourceText,
                        ToLanguage
                    );
                Magna.Logger.LogError
                    (
                        exception,
                        "Exception message {Message}",
                        exception.Message
                    );
            }
        }

        #endregion
    }

    #endregion

    #region Window members

    /// <summary>
    /// Вызывается, когда окно проинициализировано фреймворком.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Title = "Переводчик Avalonia";
        Width = MinWidth = 560;
        Height = MinHeight = 300;

        this.SetWindowIcon ("translate.ico");
        DataContext = new Model
        {
            FromLanguage = "en",
            ToLanguage = "ru",
            SourceText = "Quick brown fox jumps over the lazy dog",
            TranslatedText = "Нажмите кнопку \"Перевести\""
        };
        var model = ViewModel.ThrowIfNull();
        Content = new DockPanel
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Children =
            {
                new StackPanel
                    {
                        Spacing = 5,
                        Margin = new Thickness (5),
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = HorizontalAlignment.Center,

                        Children =
                        {
                            new Label
                            {
                                Content = "Перевод с",
                                VerticalContentAlignment = VerticalAlignment.Center
                            },
                            new ComboBox
                            {
                                Items = KnownLanguages(),
                                SelectedIndex = 0,
                                [!SelectingItemsControl.SelectedItemProperty]
                                    = new Binding (nameof (model.FromLanguage))
                            },
                            new Label
                            {
                                Content = "на",
                                VerticalContentAlignment = VerticalAlignment.Center
                            },
                            new ComboBox
                            {
                                Items = KnownLanguages(),
                                SelectedIndex = 1,
                                [!SelectingItemsControl.SelectedItemProperty]
                                    = new Binding (nameof (model.ToLanguage))
                            },
                            new Button
                            {
                                Content = "Вставить",
                                Command = ReactiveCommand.Create (model.Paste)
                            },
                            new Button
                            {
                                Content = "Копировать",
                                Command = ReactiveCommand.Create (model.Copy)
                            },
                            new Button
                            {
                                Content = "Перевести",
                                Command = ReactiveCommand.Create (model.Translate, model.CanTranslate)
                            }
                        }
                    }
                    .DockTop(),

                AvaloniaUtility.CreateGrid (2, GridLength.Star, 1, GridLength.Star)
                    .WithCell
                        (
                            0,
                            0,
                            new TextBox
                            {
                                [!TextBox.TextProperty] = new Binding (nameof (Model.SourceText))
                            }
                        )
                    .WithCell
                        (
                            1,
                            0,
                            new TextBox
                            {
                                IsReadOnly = true,
                                [!TextBox.TextProperty] = new Binding (nameof (Model.TranslatedText))
                            }
                        )
            }
        };

        string[] KnownLanguages() => new[] { "en", "ru" };
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
        DesktopApplication
            .Run<MainWindow> (args);
    }

    #endregion
}
