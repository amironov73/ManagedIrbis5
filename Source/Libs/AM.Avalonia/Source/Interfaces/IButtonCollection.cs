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

public interface IButtonCollection
    : INotifyPropertyChanged, IReadOnlyCollection<IButton>
{
    IButton DefaultButton { get; set; }

    IButton CancelButton { get; set; }

    IObservable<ButtonArgs> Clicked { get; }

    void AddButton(IButton button);

    IButton AddButton(string buttonName);

    void AddOkCancel();

    void AddCancel();
}
