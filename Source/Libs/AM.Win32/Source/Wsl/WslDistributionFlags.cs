// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* WslDistributionFlags.cs -- behavior of a distribution in WSL
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Win32;

/// <summary>
/// The WSL_DISTRIBUTION_FLAGS enumeration specifies
/// the behavior of a distribution in the Windows Subsystem for Linux (WSL).
/// </summary>
[Flags]
public enum WslDistributionFlags
{
    /// <summary>
    /// No flags are being supplied.
    /// </summary>
    None = 0,

    /// <summary>
    /// Allow the distribution to interoperate with Windows processes
    /// (for example, the user can invoke "cmd.exe" or "notepad.exe"
    /// from within a WSL session).
    /// </summary>
    EnableInterop = 1,

    /// <summary>
    /// Add the Windows %PATH% environment variable values to WSL sessions.
    /// </summary>
    AppendNtPath = 2,

    /// <summary>
    /// Automatically mount Windows drives inside of WSL sessions
    /// (for example, "C:" will be available under "/mnt/c").
    /// </summary>
    EnableDriveMounting = 4

}
