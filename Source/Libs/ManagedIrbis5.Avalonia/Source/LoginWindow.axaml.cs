// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* LoginWindow.cs -- окно для ввода логина и пароля
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

#endregion

#nullable enable

namespace ManagedIrbis.Avalonia;

/// <summary>
/// Окно для ввода логина и пароля.
/// </summary>
public partial class LoginWindow
    : Window
{
    #region Properties

    /// <summary>
    /// Введенный логин.
    /// </summary>
    public string? Login { get; set; }

    /// <summary>
    /// Введенный пароль.
    /// </summary>
    public string? Password { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public LoginWindow()
    {
        AvaloniaXamlLoader.Load (this);
        DataContext = this;

#if DEBUG
        this.AttachDevTools();
#endif
    }

    #endregion

    #region Private members

    private void LoginButtonClicked()
    {
        // TODO: implement
        Close (true);
    }

    private void CancelButtonClicked()
    {
        // TODO: implement
        Close (false);
    }

    #endregion
}
