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

/* MsBoxWindowBase.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Threading.Tasks;

using Avalonia.Controls;

#endregion

namespace AM.Avalonia.BaseWindows.Base;

internal class MsBoxWindowBase<U, T>
    : IMsBoxWindow<T>
    where U : Window, IWindowGetResult<T>
{
    private readonly U _window;

    public MsBoxWindowBase(U window)
    {
        _window = window;
    }

    /// <inheritdoc cref="IMsBoxWindow"/>
    public Task<T> Show()
    {
        var tcs = new TaskCompletionSource<T>();
        _window.Closed += delegate { tcs.TrySetResult(_window.GetResult()); };
        _window.Show();
        return tcs.Task;
    }

    /// <inheritdoc cref="IMsBoxWindow"/>
    public Task<T> Show(Window ownerWindow)
    {
        var tcs = new TaskCompletionSource<T>();
        _window.Closed += delegate { tcs.TrySetResult(_window.GetResult()); };
        _window.Show(ownerWindow);
        return tcs.Task;
    }

    /// <inheritdoc cref="IMsBoxWindow"/>
    public Task<T> ShowDialog(Window ownerWindow)
    {
        var tcs = new TaskCompletionSource<T>();
        _window.Closed += delegate { tcs.TrySetResult(_window.GetResult()); };
        _window.ShowDialog(ownerWindow);
        return tcs.Task;
    }
}
