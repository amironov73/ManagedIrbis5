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

/* IDialog.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace AM.Avalonia.Interfaces;

/// <summary>
///
/// </summary>
public interface IDialog
    : INotifyPropertyChanged
{
    /// <summary>
    ///
    /// </summary>
    string Title { get; set; }

    /// <summary>
    ///
    /// </summary>
    string Description { get; set; }

    /// <summary>
    ///
    /// </summary>
    IButtonCollection Buttons { get; }

    /// <summary>
    ///
    /// </summary>
    ICollection<IDialogControl> Controls { get; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    Task<IButton> ShowAsync();

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    Task CloseAsync();
}
