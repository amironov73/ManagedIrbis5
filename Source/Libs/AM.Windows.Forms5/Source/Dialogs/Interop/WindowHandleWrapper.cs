// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.Dialogs.Interop;

class WindowHandleWrapper : IWin32Window
{
    private IntPtr _handle;

    public WindowHandleWrapper (IntPtr handle)
    {
        _handle = handle;
    }

    #region IWin32Window Members

    public IntPtr Handle
    {
        get { return _handle; }
    }

    #endregion
}
