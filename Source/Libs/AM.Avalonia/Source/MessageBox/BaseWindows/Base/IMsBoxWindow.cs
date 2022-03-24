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

/* IMsBoxWindow.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Threading.Tasks;

using Avalonia.Controls;

#endregion

#nullable enable

namespace AM.Avalonia.BaseWindows.Base;

/// <summary>
///
/// </summary>
public interface IMsBoxWindow<T>
{
    /// <summary>
    /// Open message box window as dialog window under owner
    /// </summary>
    /// <param name="ownerWindow"></param>
    /// <returns></returns>
    Task<T> ShowDialog (Window ownerWindow);

    /// <summary>
    /// Open message box window
    /// </summary>
    /// <returns></returns>
    Task<T> Show();

    /// <summary>
    /// Open message box window under owner window
    /// </summary>
    /// <param name="ownerWindow"></param>
    /// <returns></returns>
    Task<T> Show (Window ownerWindow);
}
