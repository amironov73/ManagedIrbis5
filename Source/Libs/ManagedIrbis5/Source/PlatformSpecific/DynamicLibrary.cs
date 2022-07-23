// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* DynamicLibrary.cs -- поддержка DLL на Windows
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using AM;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.PlatformSpecific;

/// <summary>
/// Поддержка динамический библиотек (DLL) на Windows.
/// </summary>
[ExcludeFromCodeCoverage]
public class DynamicLibrary
    : IDisposable
{
    #region Constants

    private const string Kernel32 = "kernel32.dll";

    #endregion

    #region Events

    /// <summary>
    /// Raised when disposing.
    /// </summary>
    public event EventHandler? Disposing;

    #endregion

    #region Properties

    /// <summary>
    /// Library name.
    /// </summary>
    public string LibraryName { get; private set; }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public DynamicLibrary
        (
            string libraryName
        )
    {
        Sure.NotNullNorEmpty (libraryName);

        Magna.Logger.LogTrace
            (
                nameof (DynamicLibrary) + "::Constructor"
                                        + ": name={Name}",
                libraryName.ToVisibleString()
            );

        LibraryName = libraryName;

        _handle = LoadLibrary (libraryName);
        if (_handle == IntPtr.Zero)
        {
            var errorCode = Marshal.GetLastWin32Error();

            Magna.Logger.LogError
                (
                    nameof (DynamicLibrary) + "::Constructor"
                                            + ": error={Code}",
                    +errorCode
                );

            throw new ArsMagnaException
                (
                    $"Failed to load library (ErrorCode: {errorCode})"
                );
        }
    }

    #endregion

    #region Private members

    private readonly IntPtr _handle;

    #region Native methods

    /// <summary>
    /// Loads the specified module into the address space
    /// of the calling process. The specified module may
    /// cause other modules to be loaded.
    /// </summary>
    /// <param name="libraryName">The name of the module.
    /// This can be either a library module (a .dll file)
    /// or an executable module (an .exe file).</param>
    /// <returns>If the function succeeds, the return value
    /// is a handle to the module.
    /// If the function fails, the return value is NULL.
    /// To get extended error information, call GetLastError.
    /// </returns>
    /// <remarks>See https://msdn.microsoft.com/en-us/library/windows/desktop/ms684175(v=vs.85).aspx
    /// </remarks>
    [DllImport (Kernel32, CharSet = CharSet.Ansi,
        SetLastError = true)]
    private static extern IntPtr LoadLibrary
        (
            string libraryName
        );

    /// <summary>
    /// Frees the loaded dynamic-link library (DLL) module and,
    /// if necessary, decrements its reference count.
    /// When the reference count reaches zero, the module
    /// is unloaded from the address space of the calling
    /// process and the handle is no longer valid.
    /// </summary>
    /// <param name="hModule">A handle to the loaded library module.
    /// The LoadLibrary, LoadLibraryEx, GetModuleHandle,
    /// or GetModuleHandleEx function returns this handle.
    /// </param>
    /// <returns>If the function succeeds, the return value is nonzero.
    /// If the function fails, the return value is zero.
    /// To get extended error information, call the GetLastError function.
    /// </returns>
    /// <remarks>See https://msdn.microsoft.com/en-us/library/windows/desktop/ms683152(v=vs.85).aspx
    /// </remarks>
    [DllImport (Kernel32, CharSet = CharSet.Ansi)]
    private static extern bool FreeLibrary
        (
            IntPtr hModule
        );

    /// <summary>
    /// Retrieves the address of an exported function
    /// or variable from the specified dynamic-link library (DLL).
    /// </summary>
    /// <param name="hModule">A handle to the DLL module
    /// that contains the function or variable.
    /// The LoadLibrary, LoadLibraryEx, LoadPackagedLibrary,
    /// or GetModuleHandle function returns this handle.
    /// </param>
    /// <param name="lpProcName">The function or variable name,
    /// or the function's ordinal value. If this parameter
    /// is an ordinal value, it must be in the low-order word;
    /// the high-order word must be zero.</param>
    /// <returns>If the function succeeds, the return value
    /// is the address of the exported function or variable.
    /// If the function fails, the return value is NULL.
    /// To get extended error information, call GetLastError.
    /// </returns>
    /// <remarks>See https://msdn.microsoft.com/en-us/library/windows/desktop/ms683212(v=vs.85).aspx
    /// </remarks>
    [DllImport (Kernel32, CharSet = CharSet.Ansi)]
    private static extern IntPtr GetProcAddress
        (
            IntPtr hModule,
            string lpProcName
        );

    #endregion

    #endregion

    #region Public methods

    /// <summary>
    /// Create delegate for specified function.
    /// </summary>
    public Delegate CreateDelegate
        (
            string functionName,
            Type type
        )
    {
        Sure.NotNullNorEmpty (functionName);
        Sure.NotNull (type);

        Magna.Logger.LogTrace
            (
                nameof (DynamicLibrary) + "::" + nameof (CreateDelegate)
                + ": function {Name}",
                functionName.ToVisibleString()
            );

        var address = GetProcAddress
            (
                _handle,
                functionName
            );
        if (address == IntPtr.Zero)
        {
            Magna.Logger.LogError
                (
                    nameof (DynamicLibrary) + "::" + nameof (CreateDelegate)
                    + ": can't find function {Name}",
                    functionName.ToVisibleString()
                );

            throw new ArsMagnaException ($"Can't find function {functionName}");
        }

        var result = Marshal.GetDelegateForFunctionPointer
            (
                address,
                type
            );

        return result;
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        Magna.Logger.LogTrace
            (
                "DynamicLibrary::Dispose: "
                + "name={Name}",
                LibraryName
            );

        Disposing?.Invoke (this, EventArgs.Empty);
        FreeLibrary (_handle);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return LibraryName;
    }

    #endregion
}
