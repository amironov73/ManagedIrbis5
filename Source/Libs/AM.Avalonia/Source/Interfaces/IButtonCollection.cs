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
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedParameter.Local

/* IButtonCollection.cs -- интерфейс коллекции кнопок
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;

#endregion

#nullable enable

namespace AM.Avalonia.Interfaces;

/// <summary>
/// Интерфейс коллекции кнопок.
/// </summary>
public interface IButtonCollection
    : INotifyPropertyChanged, IReadOnlyCollection<IButton>
{
    /// <summary>
    /// Кнопка по умолчанию (срабатывает по <c>Enter</c>).
    /// </summary>
    IButton? DefaultButton { get; set; }

    /// <summary>
    /// Кнопка, срабатывающая по <c>Esc</c>.
    /// </summary>
    IButton? CancelButton { get; set; }

    /// <summary>
    ///
    /// </summary>
    IObservable<ButtonArgs> Clicked { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="button"></param>
    void AddButton (IButton button);

    /// <summary>
    ///
    /// </summary>
    /// <param name="buttonName"></param>
    /// <returns></returns>
    IButton AddButton (string buttonName);

    /// <summary>
    ///
    /// </summary>
    void AddOkCancel();

    /// <summary>
    ///
    /// </summary>
    void AddCancel();
}
