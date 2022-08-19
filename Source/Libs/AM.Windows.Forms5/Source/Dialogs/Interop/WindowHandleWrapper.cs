// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* WindowHandleWrapper.cs -- оебртка над дескриптором Win32-окна
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.Dialogs.Interop;

/// <summary>
/// Обертка над дескриптором Win32-окна.
/// </summary>
internal sealed class WindowHandleWrapper
    : IWin32Window
{
    #region Construction

    public WindowHandleWrapper
        (
            IntPtr handle
        )
    {
        Handle = handle;
    }

    #endregion

    #region IWin32Window Members

    public IntPtr Handle { get; }

    #endregion
}
