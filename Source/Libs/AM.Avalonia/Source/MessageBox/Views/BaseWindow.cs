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

/* BaseWindow.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia.Controls;
using Avalonia.Threading;

#endregion

#nullable enable

namespace AM.Avalonia.Views;

/// <summary>
///
/// </summary>
public class BaseWindow
    : Window

{
    /// <summary>
    ///
    /// </summary>
    public BaseWindow()
    {
        ShowInTaskbar = false;
        CanResize = false;
    }

    /// <summary>
    ///
    /// </summary>
    public async void CloseSafe()
    {
        await Dispatcher.UIThread.InvokeAsync (Close);
    }
}
