// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* IButton.cs -- интерфейс кнопки.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;

#endregion

#nullable enable

namespace AM.Avalonia.Interfaces;

/// <summary>
/// Интерфейс кнопки.
/// </summary>
public interface IButton
    : INotifyPropertyChanged
{
    /// <summary>
    /// Имя кнопки.
    /// </summary>
    string? Name { get; set; }

    /// <summary>
    /// Признак видимости.
    /// </summary>
    bool IsVisible { get; set; }

    /// <summary>
    /// Признак разрешения.
    /// </summary>
    bool IsEnabled { get; set; }

    /// <summary>
    /// Срабатывает по <c>Enter</c>?
    /// </summary>
    bool IsDefault { get; set; }

    /// <summary>
    /// Срабатывает по <c>Esc</c>?
    /// </summary>
    bool IsCancel { get; set; }

    /// <summary>
    /// Событие щелчка по кнопке.
    /// </summary>
    IObservable<ButtonArgs> OnClick { get; }

    /// <summary>
    /// Событие щелчка по кнопке.
    /// </summary>
    IObservable<ButtonArgs> Clicked { get; }
}
