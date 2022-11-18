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

using System;

using AM.Avalonia.Controls;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.ReactiveUI;

using ReactiveUI;

#endregion

#nullable enable

namespace ManagedIrbis.Avalonia;

/// <summary>
/// Окно для ввода логина и пароля при подключении к серверу ИРБИС64.
/// </summary>
public sealed class LoginWindow
    : ReactiveWindow<LoginModel>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LoginWindow()
    {
        DataContext = new LoginModel { window = this };
        var model = ViewModel!;

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
                                Content = "Вход",
                                Command = ReactiveCommand.Create (OkClicked, OkEnabled())
                            },

                        new Button
                            {
                                Content = "Отмена",
                                Command = ReactiveCommand.Create (model.CancelClicked)
                            }
                    }
                }
            }
        };
    }

    #endregion

    #region Private methods

    private void OkClicked()
    {
        Close (true);
    }

    private IObservable<bool> OkEnabled()
    {
        return ViewModel!.WhenAnyValue
            (
                x => x.Username,
                y => y.Password,
                (x, y) => !string.IsNullOrEmpty (x)
                          && !string.IsNullOrEmpty (y)
            );
    }


    #endregion
}
