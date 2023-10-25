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
using System.Collections.ObjectModel;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.ReactiveUI;
using Avalonia.Threading;

using GptForMe.Source;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using OpenAI_API;
using OpenAI_API.Chat;

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
        public ObservableCollection<IChatMessage> History { get; } = new ();

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

        var model = new Model();
        DataContext = model;

        _historyBox = new ListBox
        {
            [Grid.RowProperty] = 1,
            [Grid.ColumnSpanProperty] = 3,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Margin = new Thickness (5),
            Padding = new Thickness (5),
            ItemsSource = model.History
        };

        Content = new Grid
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            ColumnDefinitions = ColumnDefinitions.Parse ("*,Auto,Auto"),
            RowDefinitions = RowDefinitions.Parse ("Auto,*"),
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
                    Content = "Новый",
                    Padding = new Thickness (5),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Command = ReactiveCommand.Create (NewConversation),
                },

                new Button
                {
                    [Grid.ColumnProperty] = 2,
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

    private ListBox _historyBox = null!;
    private OpenAIAPI? _api;
    private Conversation? _conversation;

    private void ScrollToEnd()
    {
        var last = _historyBox.Items.Count - 1;
        if (last >= 0)
        {
            _historyBox.ScrollIntoView (last);
        }
    }

    private void AddConversation
        (
            string prompt,
            string answer
        )
    {
        var model = (Model) DataContext.ThrowIfNull();

        model.History.Add (new OutgoingMessage
        {
            MessageContent = prompt
        });

        model.History.Add (new IncomingMessage
        {
            MessageContent = answer
        });
    }

    private void AddError
        (
            string message
        )
    {
        var model = (Model) DataContext.ThrowIfNull();

        model.History.Add (new ErrorMessage
        {
            MessageContent = message
        });
    }

    private void NewConversation()
    {
        var model = (Model) DataContext.ThrowIfNull();
        model.History.Clear();

        if (_api is not null)
        {
            _conversation = _api.Chat.CreateConversation();
            _conversation.AppendSystemMessage ("You are a helpful assistant.");
        }
    }

    private async void HandlePrompt()
    {
        var model = (Model) DataContext.ThrowIfNull();
        var prompt = model.Prompt;
        if (string.IsNullOrEmpty (prompt))
        {
            return;
        }

        if (_conversation is null)
        {
             var configuration = Program.Configuration;
             var apiKey = configuration["api-key"];
             _api = new OpenAIAPI (apiKey)
             {
                 ApiUrlFormat = configuration["api-url"]
             };
             NewConversation();
        }

        _conversation!.AppendUserInput (prompt);

        try
        {
            var answer = await _conversation.GetResponseFromChatbotAsync();
            if (!string.IsNullOrEmpty (answer))
            {
                model.Answer = answer;
                model.Prompt = null;
                _conversation.AppendExampleChatbotOutput (answer);
                AddConversation (prompt, answer);
            }
        }
        catch (Exception exception)
        {
            AddError (exception.Message);
        }

        DispatcherTimer.RunOnce (ScrollToEnd, TimeSpan.FromMilliseconds (100));
    }

    #endregion
}
