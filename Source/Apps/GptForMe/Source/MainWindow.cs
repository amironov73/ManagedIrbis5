// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
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

using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.ReactiveUI;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using OpenAI_API;

#endregion

#nullable enable

namespace GptForMe;

/// <summary>
/// Главное окно приложения.
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
        /// Текст запроса.
        /// </summary>
        [Reactive]
        public string? Prompt { get; set; }

        /// <summary>
        /// Текст ответа.
        /// </summary>
        [Reactive]
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public string? Answer { get; set; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        /// <summary>
        /// История общения.
        /// </summary>
        [Reactive]
        public string? History { get; set; }

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

        this.AttachDevTools();
        this.SetWindowIcon ("Assets/ai.ico");

        Title = "GPT для меня";
        Width = MinWidth = 600;
        Height = MinHeight = 450;

        DataContext = new Model();

        _historyBox = new TextBox
        {
            [Grid.RowProperty] = 1,
            [Grid.ColumnSpanProperty] = 2,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Margin = new Thickness (5),
            Padding = new Thickness (5),
            Watermark = "История запросов",
            IsReadOnly = true,
            TextWrapping = TextWrapping.Wrap,
            [!TextBox.TextProperty] = new Binding (nameof (Model.History))
        };

        Content = new Grid
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            ColumnDefinitions = new ColumnDefinitions
            {
                new (1.0, GridUnitType.Star),
                new (GridLength.Auto)
            },
            RowDefinitions = new RowDefinitions
            {
                new (GridLength.Auto),
                new (1.0, GridUnitType.Star)
            },
            Margin = new Thickness (5),
            Children =
            {
                new TextBox
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Watermark = "Текст запроса",
                    Margin = new Thickness (5),
                    [!TextBox.TextProperty] = new Binding (nameof (Model.Prompt))
                },

                new Button
                {
                    [Grid.ColumnProperty] = 1,
                    Content = "Спросить",
                    Padding = new Thickness (5),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Command = ReactiveCommand.Create (HandlePrompt),
                    IsDefault = true
                },

                _historyBox
            }
        };
    }

    #endregion

    #region Private members

    private  TextBox _historyBox = null!;

    private async void HandlePrompt()
    {
        var model = (Model) DataContext.ThrowIfNull();
        var prompt = model.Prompt;
        if (string.IsNullOrEmpty (prompt))
        {
            return;
        }

        var configuration = Program.Configuration;
        var apiKey = configuration["api-key"];
        var api = new OpenAIAPI (apiKey)
        {
            ApiUrlFormat = configuration["api-url"]
        };
        var chat = api.Chat.CreateConversation();
        chat.AppendSystemMessage ("You are a helpful assistant.");
        chat.AppendUserInput (prompt);
        var answer = await chat.GetResponseFromChatbotAsync();
        model.Answer = answer;
        model.Prompt = null;

        var newLine = Environment.NewLine;
        model.History += $"> {prompt}" + newLine + answer + newLine + newLine;
        var length = model.History.Length;
        _historyBox.SelectionStart = length;
        _historyBox.SelectionEnd = length;
    }

    #endregion
}
