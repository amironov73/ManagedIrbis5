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

/* IButtonCollection.cs --
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
///
/// </summary>
public interface IButtonCollection
    : INotifyPropertyChanged, IReadOnlyCollection<IButton>
{
    /// <summary>
    ///
    /// </summary>
    IButton DefaultButton { get; set; }

    /// <summary>
    ///
    /// </summary>
    IButton CancelButton { get; set; }

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
