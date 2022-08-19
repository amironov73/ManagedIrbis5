// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* IProgressDialog.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Windows.Forms.Dialogs.Interop;

/// <summary>
///
/// </summary>
[ComImport]
[Guid (CLSIDGuid.ProgressDialog)]
internal class ProgressDialogRCW
{
    // пустое тело
}

/// <summary>
///
/// </summary>
[ComImport]
[Guid (IIDGuid.IProgressDialog)]
[CoClass (typeof (ProgressDialogRCW))]
internal interface ProgressDialog
    : IProgressDialog
{
    // пустое тело
}

/// <summary>
///
/// </summary>
[Flags]
internal enum ProgressDialogFlags
    : uint
{
    /// <summary>
    ///
    /// </summary>
    Normal = 0x00000000,

    /// <summary>
    ///
    /// </summary>
    Modal = 0x00000001,

    /// <summary>
    ///
    /// </summary>
    AutoTime = 0x00000002,

    /// <summary>
    ///
    /// </summary>
    NoTime = 0x00000004,

    /// <summary>
    ///
    /// </summary>
    NoMinimize = 0x00000008,

    /// <summary>
    ///
    /// </summary>
    NoProgressBar = 0x00000010,

    /// <summary>
    ///
    /// </summary>
    MarqueeProgress = 0x00000020,

    /// <summary>
    ///
    /// </summary>
    NoCancel = 0x00000040
}

/// <summary>
///
/// </summary>
[ComImport]
[Guid (IIDGuid.IProgressDialog)]
[InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
internal interface IProgressDialog
{
    /// <summary>
    ///
    /// </summary>
    [PreserveSig]
    void StartProgressDialog
        (
            IntPtr hwndParent,
            [MarshalAs (UnmanagedType.IUnknown)] object punkEnableModless,
            ProgressDialogFlags dwFlags,
            IntPtr pvResevered
        );

    /// <summary>
    ///
    /// </summary>
    [PreserveSig]
    void StopProgressDialog();

    /// <summary>
    ///
    /// </summary>
    [PreserveSig]
    void SetTitle
        (
            [MarshalAs (UnmanagedType.LPWStr)] string pwzTitle
        );

    /// <summary>
    ///
    /// </summary>
    [PreserveSig]
    void SetAnimation
        (
            SafeModuleHandle hInstAnimation,
            ushort idAnimation
        );

    /// <summary>
    ///
    /// </summary>
    [PreserveSig]
    [return: MarshalAs (UnmanagedType.Bool)]
    bool HasUserCancelled();

    /// <summary>
    ///
    /// </summary>
    [PreserveSig]
    void SetProgress
        (
            uint dwCompleted,
            uint dwTotal
        );

    /// <summary>
    ///
    /// </summary>
    [PreserveSig]
    void SetProgress64
        (
            ulong ullCompleted,
            ulong ullTotal
        );

    /// <summary>
    ///
    /// </summary>
    [PreserveSig]
    void SetLine
        (
            uint dwLineNum,
            [MarshalAs (UnmanagedType.LPWStr)] string pwzString,
            [MarshalAs (UnmanagedType.VariantBool)]
            bool fCompactPath,
            IntPtr pvResevered
        );

    /// <summary>
    ///
    /// </summary>
    [PreserveSig]
    void SetCancelMsg
        (
            [MarshalAs (UnmanagedType.LPWStr)] string pwzCancelMsg,
            object pvResevered
        );

    /// <summary>
    ///
    /// </summary>
    [PreserveSig]
    void Timer
        (
            uint dwTimerAction,
            object pvResevered
        );
}
