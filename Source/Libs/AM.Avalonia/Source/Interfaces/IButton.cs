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

#endregion

#nullable enable

namespace AM.Avalonia.Interfaces;

/// <summary>
///
/// </summary>
public interface IButton
    : INotifyPropertyChanged
{
    /// <summary>
    ///
    /// </summary>
    string Name { get; set; }

    /// <summary>
    ///
    /// </summary>
    bool IsVisible { get; set; }

    /// <summary>
    ///
    /// </summary>
    bool IsEnabled { get; set; }

    /// <summary>
    ///
    /// </summary>
    bool IsDefault { get; set; }

    /// <summary>
    ///
    /// </summary>
    bool IsCancel { get; set; }

    /// <summary>
    ///
    /// </summary>
    IObservable<ButtonArgs> OnClick { get; }

    /// <summary>
    ///
    /// </summary>
    IObservable<ButtonArgs> Clicked { get; }
}
