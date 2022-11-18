// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable StringLiteralTypo

/* LoginModel.cs -- модель для диалога подключения к серверу ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Avalonia.Controls;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

#nullable enable

namespace ManagedIrbis.Avalonia;

/// <summary>
/// Модель для диалога подключения к серверу ИРБИС64.
/// </summary>
public sealed class LoginModel
    : ReactiveObject
{
    #region Properties

    /// <summary>
    /// Окно.
    /// </summary>
    internal Window window = null!;

    /// <summary>
    /// Искомое понятие.
    /// </summary>
    [Reactive]
    public string? Username { get; set; }

    /// <summary>
    /// Сообщение об ошибке.
    /// </summary>
    [Reactive]
    public string? Password { get; set; }

    #endregion

    #region Private members

    internal void LoginClicked()
    {
        window.Close (true);
    }

    internal IObservable<bool> CanLogin()
    {
        return this.WhenAnyValue
            (
                x => x.Username,
                y => y.Password,
                (x, y) => !string.IsNullOrEmpty (Username)
                          && !string.IsNullOrEmpty (Password)
            );
    }

    internal void CancelClicked()
    {
        window.Close (false);
    }

    #endregion
}

