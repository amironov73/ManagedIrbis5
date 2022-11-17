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
using System.Reactive;
using System.Threading.Tasks;

using AM;
using AM.Avalonia;
using AM.Collections;

using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;

using ManagedIrbis;

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

    internal void LoginButtonClicked
        (
            object? sender,
            RoutedEventArgs eventArgs
        )
    {
        window.Close (true);
    }

    internal void CancelButtonClicked
        (
            object? sender,
            RoutedEventArgs eventArgs
        )
    {
        window.Close (false);
    }

    #endregion
}

