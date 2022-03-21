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

/* IButton.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Windows.Input;

#endregion

#nullable enable

namespace AM.Avalonia.Interfaces;

public interface IButton
    : INotifyPropertyChanged
{
    string Name { get; set; }

    bool IsVisible { get; set; }

    bool IsEnabled { get; set; }

    bool IsDefault { get; set; }

    bool IsCancel { get; set; }

    IObservable<ButtonArgs> OnClick { get; }

    IObservable<ButtonArgs> Clicked { get; }
}
