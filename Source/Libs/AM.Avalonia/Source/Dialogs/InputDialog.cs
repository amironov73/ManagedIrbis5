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
// ReSharper disable UnusedParameter.Local

/* InputDialog.cs -- простой диалог ввода строкового значения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;

using ReactiveUI;

#endregion

#nullable enable


namespace AM.Avalonia.Dialogs;

/// <summary>
/// Простой диалог ввода строкового значения.
/// </summary>
public sealed class InputDialog
    : Window
{
    #region Properties

    /// <summary>
    /// Описание свойства "Prompt".
    /// </summary>
    public static readonly StyledProperty<string?> PromptProperty
        = AvaloniaProperty.Register<InputDialog, string?> (nameof (Prompt));

    /// <summary>
    /// Описание свойства "Value".
    /// </summary>
    public static readonly StyledProperty<string?> ValueProperty
        = AvaloniaProperty.Register<InputDialog, string?> (nameof (Value));

    /// <summary>
    /// Объяснение, что требуется ввести.
    /// </summary>
    public string? Prompt
    {
        get => GetValue (PromptProperty);
        set => SetValue (PromptProperty, value);
    }

    /// <summary>
    /// Введенное значение.
    /// </summary>
    public string? Value
    {
        get => GetValue (ValueProperty);
        set => SetValue (ValueProperty, value);
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public InputDialog()
    {
        this.AttachDevTools();
        DataContext = this;

        Content = new StackPanel
        {
            Margin = new Thickness (10),
            Orientation = Orientation.Vertical,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Center,
            Children =
            {
                new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    [!ContentProperty] = new Binding (nameof (Prompt))
                },

                new TextBox
                {
                    Margin = new Thickness (0, 10, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    [!TextBox.TextProperty] = new Binding (nameof (Value))
                },

                new StackPanel
                {
                    Margin = new Thickness (10),
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Children =
                    {
                        new Button
                        {
                            Content = "OK",
                            Command = ReactiveCommand.Create (() => Close (true))
                        },

                        new Button
                        {
                            Content = "Отмена",
                            Margin = new Thickness (3, 0, 0, 0),
                            Command = ReactiveCommand.Create (() => Close (false))
                        }
                    }
                }
            }
        };
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Запрос у пользователя строкового значения
    /// с предложением предлагая значение по умолчанию
    /// (оно же -- начальное значение).
    /// </summary>
    /// <param name="owner">Окно-владелец.</param>
    /// <param name="data">Описание вводимых данных.</param>
    /// <returns>Результат отработки диалогового окна.</returns>
    public static async Task<bool> Query
        (
            Window? owner,
            InputData data
        )
    {
        var window = new InputDialog
        {
            Title = data.Title,
            Prompt = data.Prompt,
            Value = data.Value
        };

        var result = await window.ShowDialog<bool> (owner!);
        if (result)
        {
            data.Value = window.Value;
        }

        return result;
    }

    #endregion
}
