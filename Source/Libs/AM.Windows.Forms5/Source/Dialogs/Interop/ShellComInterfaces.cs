// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* ShellComInterfaces.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#endregion

#nullable enable

// Disable warning CS0108: 'x' hides inherited member 'y'. Use the new keyword if hiding was intended.
#pragma warning disable 0108

namespace AM.Windows.Forms.Dialogs.Interop;

/// <summary>
///
/// </summary>
[ComImport]
[Guid (IIDGuid.IModalWindow)]
[InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
internal interface IModalWindow
{
    /// <summary>
    ///
    /// </summary>
    [PreserveSig]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int Show ([In] IntPtr parent);
}

/// <summary>
///
/// </summary>
[ComImport,
 Guid (IIDGuid.IFileDialog),
 InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
internal interface IFileDialog : IModalWindow
{
    // Defined on IModalWindow - repeated here due to requirements of COM interop layer
    // --------------------------------------------------------------------------------
    /// <summary>
    ///
    /// </summary>
    [PreserveSig]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int Show ([In] IntPtr parent);

    // IFileDialog-Specific interface members
    // --------------------------------------------------------------------------------
    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetFileTypes ([In] uint cFileTypes,
        [In, MarshalAs (UnmanagedType.LPArray)] NativeMethods.COMDLG_FILTERSPEC[] rgFilterSpec);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetFileTypeIndex ([In] uint iFileType);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetFileTypeIndex (out uint piFileType);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Advise ([In, MarshalAs (UnmanagedType.Interface)] IFileDialogEvents pfde, out uint pdwCookie);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Unadvise ([In] uint dwCookie);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetOptions ([In] NativeMethods.FOS fos);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetOptions (out NativeMethods.FOS pfos);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetDefaultFolder ([In, MarshalAs (UnmanagedType.Interface)] IShellItem psi);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetFolder ([In, MarshalAs (UnmanagedType.Interface)] IShellItem psi);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetFolder ([MarshalAs (UnmanagedType.Interface)] out IShellItem ppsi);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetCurrentSelection ([MarshalAs (UnmanagedType.Interface)] out IShellItem ppsi);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetFileName ([In, MarshalAs (UnmanagedType.LPWStr)] string pszName);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetFileName ([MarshalAs (UnmanagedType.LPWStr)] out string pszName);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetTitle ([In, MarshalAs (UnmanagedType.LPWStr)] string pszTitle);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetOkButtonLabel ([In, MarshalAs (UnmanagedType.LPWStr)] string pszText);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetFileNameLabel ([In, MarshalAs (UnmanagedType.LPWStr)] string pszLabel);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetResult ([MarshalAs (UnmanagedType.Interface)] out IShellItem ppsi);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void AddPlace ([In, MarshalAs (UnmanagedType.Interface)] IShellItem psi, NativeMethods.FDAP fdap);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetDefaultExtension ([In, MarshalAs (UnmanagedType.LPWStr)] string pszDefaultExtension);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Close ([MarshalAs (UnmanagedType.Error)] int hr);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetClientGuid ([In] ref Guid guid);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ClearClientData();

    // Not supported:  IShellItemFilter is not defined, converting to IntPtr
    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetFilter ([MarshalAs (UnmanagedType.Interface)] IntPtr pFilter);
}

[ComImport]
[Guid (IIDGuid.IFileOpenDialog)]
[InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
internal interface IFileOpenDialog : IFileDialog
{
    // Defined on IModalWindow - repeated here due to requirements of COM interop layer
    // --------------------------------------------------------------------------------
    /// <summary>
    ///
    /// </summary>
    [PreserveSig]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int Show ([In] IntPtr parent);

    // Defined on IFileDialog - repeated here due to requirements of COM interop layer
    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetFileTypes ([In] uint cFileTypes, [In] ref NativeMethods.COMDLG_FILTERSPEC rgFilterSpec);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetFileTypeIndex ([In] uint iFileType);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetFileTypeIndex (out uint piFileType);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Advise ([In, MarshalAs (UnmanagedType.Interface)] IFileDialogEvents pfde, out uint pdwCookie);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Unadvise ([In] uint dwCookie);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetOptions ([In] NativeMethods.FOS fos);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetOptions (out NativeMethods.FOS pfos);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetDefaultFolder ([In, MarshalAs (UnmanagedType.Interface)] IShellItem psi);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetFolder ([In, MarshalAs (UnmanagedType.Interface)] IShellItem psi);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetFolder ([MarshalAs (UnmanagedType.Interface)] out IShellItem ppsi);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetCurrentSelection ([MarshalAs (UnmanagedType.Interface)] out IShellItem ppsi);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetFileName ([In, MarshalAs (UnmanagedType.LPWStr)] string pszName);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetFileName ([MarshalAs (UnmanagedType.LPWStr)] out string pszName);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetTitle ([In, MarshalAs (UnmanagedType.LPWStr)] string pszTitle);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetOkButtonLabel ([In, MarshalAs (UnmanagedType.LPWStr)] string pszText);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetFileNameLabel ([In, MarshalAs (UnmanagedType.LPWStr)] string pszLabel);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetResult ([MarshalAs (UnmanagedType.Interface)] out IShellItem ppsi);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void AddPlace ([In, MarshalAs (UnmanagedType.Interface)] IShellItem psi, NativeMethods.FDAP fdap);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetDefaultExtension ([In, MarshalAs (UnmanagedType.LPWStr)] string pszDefaultExtension);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Close ([MarshalAs (UnmanagedType.Error)] int hr);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetClientGuid ([In] ref Guid guid);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ClearClientData();

    // Not supported:  IShellItemFilter is not defined, converting to IntPtr
    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetFilter ([MarshalAs (UnmanagedType.Interface)] IntPtr pFilter);

    // Defined by IFileOpenDialog
    // ---------------------------------------------------------------------------------
    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetResults ([MarshalAs (UnmanagedType.Interface)] out IShellItemArray ppenum);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetSelectedItems ([MarshalAs (UnmanagedType.Interface)] out IShellItemArray ppsai);
}

/// <summary>
///
/// </summary>
[ComImport]
[Guid (IIDGuid.IFileSaveDialog)]
[InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
internal interface IFileSaveDialog : IFileDialog
{
    // Defined on IModalWindow - repeated here due to requirements of COM interop layer
    // --------------------------------------------------------------------------------
    /// <summary>
    ///
    /// </summary>
    [PreserveSig]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int Show ([In] IntPtr parent);

    // Defined on IFileDialog - repeated here due to requirements of COM interop layer
    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetFileTypes ([In] uint cFileTypes, [In] ref NativeMethods.COMDLG_FILTERSPEC rgFilterSpec);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetFileTypeIndex ([In] uint iFileType);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetFileTypeIndex (out uint piFileType);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Advise ([In, MarshalAs (UnmanagedType.Interface)] IFileDialogEvents pfde, out uint pdwCookie);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Unadvise ([In] uint dwCookie);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetOptions ([In] NativeMethods.FOS fos);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetOptions (out NativeMethods.FOS pfos);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetDefaultFolder ([In, MarshalAs (UnmanagedType.Interface)] IShellItem psi);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetFolder ([In, MarshalAs (UnmanagedType.Interface)] IShellItem psi);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetFolder ([MarshalAs (UnmanagedType.Interface)] out IShellItem ppsi);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetCurrentSelection ([MarshalAs (UnmanagedType.Interface)] out IShellItem ppsi);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetFileName ([In, MarshalAs (UnmanagedType.LPWStr)] string pszName);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetFileName ([MarshalAs (UnmanagedType.LPWStr)] out string pszName);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetTitle ([In, MarshalAs (UnmanagedType.LPWStr)] string pszTitle);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetOkButtonLabel ([In, MarshalAs (UnmanagedType.LPWStr)] string pszText);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetFileNameLabel ([In, MarshalAs (UnmanagedType.LPWStr)] string pszLabel);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetResult ([MarshalAs (UnmanagedType.Interface)] out IShellItem ppsi);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void AddPlace ([In, MarshalAs (UnmanagedType.Interface)] IShellItem psi, NativeMethods.FDAP fdap);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetDefaultExtension ([In, MarshalAs (UnmanagedType.LPWStr)] string pszDefaultExtension);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Close ([MarshalAs (UnmanagedType.Error)] int hr);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetClientGuid ([In] ref Guid guid);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ClearClientData();

    // Not supported:  IShellItemFilter is not defined, converting to IntPtr
    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetFilter ([MarshalAs (UnmanagedType.Interface)] IntPtr pFilter);

    // Defined by IFileSaveDialog interface
    // -----------------------------------------------------------------------------------
    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetSaveAsItem ([In, MarshalAs (UnmanagedType.Interface)] IShellItem psi);

    // Not currently supported: IPropertyStore
    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetProperties ([In, MarshalAs (UnmanagedType.Interface)] IntPtr pStore);

    // Not currently supported: IPropertyDescriptionList
    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetCollectedProperties ([In, MarshalAs (UnmanagedType.Interface)] IntPtr pList, [In] int fAppendDefault);

    // Not currently supported: IPropertyStore
    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetProperties ([MarshalAs (UnmanagedType.Interface)] out IntPtr ppStore);

    // Not currently supported: IPropertyStore, IFileOperationProgressSink
    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ApplyProperties ([In, MarshalAs (UnmanagedType.Interface)] IShellItem psi,
        [In, MarshalAs (UnmanagedType.Interface)] IntPtr pStore,
        [In, ComAliasName ("Interop.wireHWND")] ref IntPtr hwnd,
        [In, MarshalAs (UnmanagedType.Interface)] IntPtr pSink);
}

[ComImport]
[Guid (IIDGuid.IFileDialogEvents)]
[InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
internal interface IFileDialogEvents
{
    // NOTE: some of these callbacks are cancelable - returning S_FALSE means that
    // the dialog should not proceed (e.g. with closing, changing folder); to
    // support this, we need to use the PreserveSig attribute to enable us to return
    // the proper HRESULT
    /// <summary>
    ///
    /// </summary>
    [PreserveSig]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    HRESULT OnFileOk ([In, MarshalAs (UnmanagedType.Interface)] IFileDialog pfd);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
     PreserveSig]
    HRESULT OnFolderChanging ([In, MarshalAs (UnmanagedType.Interface)] IFileDialog pfd,
        [In, MarshalAs (UnmanagedType.Interface)] IShellItem psiFolder);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void OnFolderChange ([In, MarshalAs (UnmanagedType.Interface)] IFileDialog pfd);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void OnSelectionChange ([In, MarshalAs (UnmanagedType.Interface)] IFileDialog pfd);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void OnShareViolation ([In, MarshalAs (UnmanagedType.Interface)] IFileDialog pfd,
        [In, MarshalAs (UnmanagedType.Interface)] IShellItem psi,
        out NativeMethods.FDE_SHAREVIOLATION_RESPONSE pResponse);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void OnTypeChange ([In, MarshalAs (UnmanagedType.Interface)] IFileDialog pfd);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void OnOverwrite ([In, MarshalAs (UnmanagedType.Interface)] IFileDialog pfd,
        [In, MarshalAs (UnmanagedType.Interface)] IShellItem psi,
        out NativeMethods.FDE_OVERWRITE_RESPONSE pResponse);
}

/// <summary>
///
/// </summary>
[ComImport]
[Guid (IIDGuid.IShellItem)]
[InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
internal interface IShellItem
{
    // Not supported: IBindCtx
    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void BindToHandler ([In, MarshalAs (UnmanagedType.Interface)] IntPtr pbc, [In] ref Guid bhid,
        [In] ref Guid riid, out IntPtr ppv);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetParent ([MarshalAs (UnmanagedType.Interface)] out IShellItem ppsi);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetDisplayName ([In] NativeMethods.SIGDN sigdnName,
        [MarshalAs (UnmanagedType.LPWStr)] out string ppszName);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetAttributes ([In] uint sfgaoMask, out uint psfgaoAttribs);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Compare ([In, MarshalAs (UnmanagedType.Interface)] IShellItem psi, [In] uint hint, out int piOrder);
}

/// <summary>
///
/// </summary>
[ComImport]
[Guid (IIDGuid.IShellItemArray)]
[InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
internal interface IShellItemArray
{
    // Not supported: IBindCtx
    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void BindToHandler ([In, MarshalAs (UnmanagedType.Interface)] IntPtr pbc, [In] ref Guid rbhid,
        [In] ref Guid riid, out IntPtr ppvOut);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetPropertyStore ([In] int Flags, [In] ref Guid riid, out IntPtr ppv);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetPropertyDescriptionList ([In] ref NativeMethods.PROPERTYKEY keyType, [In] ref Guid riid,
        out IntPtr ppv);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetAttributes ([In] NativeMethods.SIATTRIBFLAGS dwAttribFlags, [In] uint sfgaoMask,
        out uint psfgaoAttribs);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetCount (out uint pdwNumItems);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetItemAt ([In] uint dwIndex, [MarshalAs (UnmanagedType.Interface)] out IShellItem ppsi);

    // Not supported: IEnumShellItems (will use GetCount and GetItemAt instead)
    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void EnumItems ([MarshalAs (UnmanagedType.Interface)] out IntPtr ppenumShellItems);
}

/// <summary>
///
/// </summary>
[ComImport]
[Guid (IIDGuid.IKnownFolder)]
[InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
internal interface IKnownFolder
{
    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetId (out Guid pkfid);

    // Not yet supported - adding to fill slot in vtable
    /// <summary>
    ///
    /// </summary>
    void spacer1();

    //[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    //void GetCategory(out mbtagKF_CATEGORY pCategory);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetShellItem ([In] uint dwFlags, ref Guid riid, out IShellItem ppv);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetPath ([In] uint dwFlags, [MarshalAs (UnmanagedType.LPWStr)] out string ppszPath);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetPath ([In] uint dwFlags, [In, MarshalAs (UnmanagedType.LPWStr)] string pszPath);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetLocation ([In] uint dwFlags, [Out, ComAliasName ("Interop.wirePIDL")] IntPtr ppidl);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetFolderType (out Guid pftid);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetRedirectionCapabilities (out uint pCapabilities);

    // Not yet supported - adding to fill slot in vtable
    /// <summary>
    ///
    /// </summary>
    void spacer2();

    //[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    //void GetFolderDefinition(out tagKNOWNFOLDER_DEFINITION pKFD);
}

/// <summary>
///
/// </summary>
[ComImport]
[Guid (IIDGuid.IKnownFolderManager)]
[InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
internal interface IKnownFolderManager
{
    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void FolderIdFromCsidl ([In] int nCsidl, out Guid pfid);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void FolderIdToCsidl ([In] ref Guid rfid, out int pnCsidl);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetFolderIds ([Out] IntPtr ppKFId, [In, Out] ref uint pCount);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetFolder ([In] ref Guid rfid, [MarshalAs (UnmanagedType.Interface)] out IKnownFolder ppkf);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetFolderByName ([In, MarshalAs (UnmanagedType.LPWStr)] string pszCanonicalName,
        [MarshalAs (UnmanagedType.Interface)] out IKnownFolder ppkf);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void RegisterFolder ([In] ref Guid rfid, [In] ref NativeMethods.KNOWNFOLDER_DEFINITION pKFD);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void UnregisterFolder ([In] ref Guid rfid);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void FindFolderFromPath ([In, MarshalAs (UnmanagedType.LPWStr)] string pszPath,
        [In] NativeMethods.FFFP_MODE mode, [MarshalAs (UnmanagedType.Interface)] out IKnownFolder ppkf);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void FindFolderFromIDList ([In] IntPtr pidl, [MarshalAs (UnmanagedType.Interface)] out IKnownFolder ppkf);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Redirect ([In] ref Guid rfid, [In] IntPtr hwnd, [In] uint Flags,
        [In, MarshalAs (UnmanagedType.LPWStr)] string pszTargetPath, [In] uint cFolders, [In] ref Guid pExclusion,
        [MarshalAs (UnmanagedType.LPWStr)] out string ppszError);
}

/// <summary>
///
/// </summary>
[ComImport]
[Guid (IIDGuid.IFileDialogCustomize)]
[InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
internal interface IFileDialogCustomize
{
    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void EnableOpenDropDown ([In] int dwIDCtl);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void AddMenu ([In] int dwIDCtl, [In, MarshalAs (UnmanagedType.LPWStr)] string pszLabel);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void AddPushButton ([In] int dwIDCtl, [In, MarshalAs (UnmanagedType.LPWStr)] string pszLabel);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void AddComboBox ([In] int dwIDCtl);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void AddRadioButtonList ([In] int dwIDCtl);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void AddCheckButton ([In] int dwIDCtl, [In, MarshalAs (UnmanagedType.LPWStr)] string pszLabel,
        [In] bool bChecked);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void AddEditBox ([In] int dwIDCtl, [In, MarshalAs (UnmanagedType.LPWStr)] string pszText);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void AddSeparator ([In] int dwIDCtl);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void AddText ([In] int dwIDCtl, [In, MarshalAs (UnmanagedType.LPWStr)] string pszText);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetControlLabel ([In] int dwIDCtl, [In, MarshalAs (UnmanagedType.LPWStr)] string pszLabel);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetControlState ([In] int dwIDCtl, [Out] out NativeMethods.CDCONTROLSTATE pdwState);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetControlState ([In] int dwIDCtl, [In] NativeMethods.CDCONTROLSTATE dwState);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetEditBoxText ([In] int dwIDCtl, [Out] IntPtr ppszText);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetEditBoxText ([In] int dwIDCtl, [In, MarshalAs (UnmanagedType.LPWStr)] string pszText);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetCheckButtonState ([In] int dwIDCtl, [Out] out bool pbChecked);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetCheckButtonState ([In] int dwIDCtl, [In] bool bChecked);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void AddControlItem ([In] int dwIDCtl, [In] int dwIDItem,
        [In, MarshalAs (UnmanagedType.LPWStr)] string pszLabel);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void RemoveControlItem ([In] int dwIDCtl, [In] int dwIDItem);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void RemoveAllControlItems ([In] int dwIDCtl);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetControlItemState ([In] int dwIDCtl, [In] int dwIDItem, [Out] out NativeMethods.CDCONTROLSTATE pdwState);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetControlItemState ([In] int dwIDCtl, [In] int dwIDItem, [In] NativeMethods.CDCONTROLSTATE dwState);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetSelectedControlItem ([In] int dwIDCtl, [Out] out int pdwIDItem);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetSelectedControlItem ([In] int dwIDCtl, [In] int dwIDItem); // Not valid for OpenDropDown

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void StartVisualGroup ([In] int dwIDCtl, [In, MarshalAs (UnmanagedType.LPWStr)] string pszLabel);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void EndVisualGroup();

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void MakeProminent ([In] int dwIDCtl);
}

/// <summary>
///
/// </summary>
[ComImport]
[Guid (IIDGuid.IFileDialogControlEvents)]
[InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
internal interface IFileDialogControlEvents
{
    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void OnItemSelected ([In, MarshalAs (UnmanagedType.Interface)] IFileDialogCustomize pfdc, [In] int dwIDCtl,
        [In] int dwIDItem);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void OnButtonClicked ([In, MarshalAs (UnmanagedType.Interface)] IFileDialogCustomize pfdc, [In] int dwIDCtl);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void OnCheckButtonToggled ([In, MarshalAs (UnmanagedType.Interface)] IFileDialogCustomize pfdc,
        [In] int dwIDCtl, [In] bool bChecked);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void OnControlActivating ([In, MarshalAs (UnmanagedType.Interface)] IFileDialogCustomize pfdc,
        [In] int dwIDCtl);
}

/// <summary>
///
/// </summary>
[ComImport]
[Guid (IIDGuid.IPropertyStore)]
[InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
internal interface IPropertyStore
{
    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetCount ([Out] out uint cProps);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetAt ([In] uint iProp, out NativeMethods.PROPERTYKEY pkey);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetValue ([In] ref NativeMethods.PROPERTYKEY key, out object pv);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetValue ([In] ref NativeMethods.PROPERTYKEY key, [In] ref object pv);

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Commit();
}
