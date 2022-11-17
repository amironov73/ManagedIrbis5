// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local

/* LoginWindow.cs -- окно для ввода логина и пароля
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Avalonia;
using AM.Avalonia.Controls;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Threading;

using ReactiveUI;

#endregion

#nullable enable

namespace ManagedIrbis.Avalonia;

/// <summary>
/// Окно для ввода логина и пароля при подключении к серверу ИРБИС64.
/// </summary>
public sealed class LoginWindow
    : Window
{
    #region Properties

    /// <summary>
    /// Модель с данными.
    /// </summary>
    public LoginModel Model { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LoginWindow()
    {
        Model = new LoginModel { window = this };

        Title = "Подключение к серверу ИРБИС64";
        Width = MinWidth = 350;
        Height = MinHeight = 200;

        Content = new StackPanel
        {
            Orientation = Orientation.Vertical,
            Margin = new Thickness (10),
            Spacing = 10,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,

            Children =
            {
                new LabeledTextBox
                {
                    Label = "Логин",
                    MinWidth = 300,
                    [!LabeledTextBox.TextProperty] = new Binding (nameof (LoginModel.Username))
                },

                new LabeledTextBox
                {
                    Label = "Пароль",
                    MinWidth = 300,
                    [!LabeledTextBox.TextProperty] = new Binding (nameof (LoginModel.Password))
                },

                new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness (0, 10, 0, 0),
                    Spacing = 10,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Children =
                    {
                        new Button
                            {
                                Focusable = true,
                                Content = "Вход"
                            }
                            .OnClick (Model.LoginButtonClicked),

                        new Button
                            {
                                Content = "Отмена",
                            }
                            .OnClick (Model.CancelButtonClicked)
                    }
                }
            }
        };

        // loginButton.Click += Model.LoginButtonClicked;
        // cancelButton.Click += Model.CancelButtonClicked;

        DataContext = Model;
    }

    #endregion
}
