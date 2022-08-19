// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* Win32Resources.cs -- работа с Win32-ресурсами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

using AM.Text;

#endregion

#nullable enable

namespace AM.Windows.Forms.Dialogs.Interop;

/// <summary>
/// Работа с Win32-ресурсами.
/// </summary>
internal sealed class Win32Resources
    : IDisposable
{
    #region Constants

    private const int BufferSize = 500;

    #endregion

    #region Construction

    public Win32Resources
        (
            string module
        )
    {
        _moduleHandle = NativeMethods.LoadLibraryEx
            (
                module,
                IntPtr.Zero,
                NativeMethods.LoadLibraryExFlags.LoadLibraryAsDatafile
            );
        if (_moduleHandle.IsInvalid)
        {
            throw new Win32Exception (Marshal.GetLastWin32Error());
        }
    }

    #endregion

    #region Private members

    private readonly SafeModuleHandle _moduleHandle;

    private void CheckDisposed()
    {
        if (_moduleHandle.IsClosed)
        {
            throw new ObjectDisposedException ("Win32Resources");
        }
    }

    #endregion

    #region Public methods

    public string LoadString (uint id)
    {
        CheckDisposed();

        var builder = StringBuilderPool.Shared.Get();
        builder.EnsureCapacity (BufferSize);
        var code = NativeMethods.LoadString (_moduleHandle, id, builder, builder.Capacity + 1);
        var result = builder.ReturnShared();

        return code == 0
            ? throw new Win32Exception (Marshal.GetLastWin32Error())
            : result;
    }

    public string? FormatString
        (
            uint id,
            params string[] args
        )
    {
        CheckDisposed();

        var buffer = IntPtr.Zero;
        var source = LoadString (id);

        // For some reason FORMAT_MESSAGE_FROM_HMODULE doesn't work so we use this way.
        const NativeMethods.FormatMessageFlags flags =
            NativeMethods.FormatMessageFlags.FORMAT_MESSAGE_ALLOCATE_BUFFER |
            NativeMethods.FormatMessageFlags.FORMAT_MESSAGE_ARGUMENT_ARRAY |
            NativeMethods.FormatMessageFlags.FORMAT_MESSAGE_FROM_STRING;

        var sourcePtr = Marshal.StringToHGlobalAuto (source);
        try
        {
            if (NativeMethods.FormatMessage (flags, sourcePtr, id, 0, ref buffer, 0, args) == 0)
            {
                throw new Win32Exception (Marshal.GetLastWin32Error());
            }
        }
        finally
        {
            Marshal.FreeHGlobal (sourcePtr);
        }

        var result = Marshal.PtrToStringAuto (buffer);

        // FreeHGlobal calls LocalFree
        Marshal.FreeHGlobal (buffer);

        return result;
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        _moduleHandle.Dispose();
    }

    #endregion
}
