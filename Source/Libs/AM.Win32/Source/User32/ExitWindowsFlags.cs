// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ExitWindowsFlags.cs -- shutdown type
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Shutdown type.
/// </summary>
[Flags]
public enum ExitWindwowsFlags
{
    /// <summary>
    /// Shuts down all processes running in the security context
    /// of the process that called the ExitWindowsEx function.
    /// Then it logs the user off.
    /// </summary>
    EWX_LOGOFF = 0x0,

    /// <summary>
    /// <para>Shuts down the system to a point at which it is safe
    /// to turn off the power. All file buffers have been flushed
    /// to disk, and all running processes have stopped.</para>
    /// <para>The calling process must have the SE_SHUTDOWN_NAME
    /// privilege.</para>
    /// <para>Specifying this flag will not shut down the power
    /// to the system even if it supports the power-off feature.
    /// You must specify EWX_POWEROFF to do this.</para>
    /// </summary>
    EWX_SHUTDOWN = 0x1,

    /// <summary>
    /// <para>Shuts down the system and then restarts the system.
    /// </para>
    /// <para>The calling process must have the SE_SHUTDOWN_NAME
    /// privilege.</para>
    /// </summary>
    EWX_REBOOT = 0x2,

    /// <summary>
    /// <para>Forces processes to terminate. When this flag is set,
    /// the system does not send the WM_QUERYENDSESSION and
    /// WM_ENDSESSION messages. This can cause the applications
    /// to lose data. Therefore, you should only use this flag in
    /// an emergency.</para>
    /// <para>Windows XP: If the computer is locked and this flag
    /// is not specified, the shutdown process will fail.</para>
    /// </summary>
    EWX_FORCE = 0x4,

    /// <summary>
    /// <para>Shuts down the system and turns off the power.
    /// The system must support the power-off feature.</para>
    /// <para>The calling process must have the SE_SHUTDOWN_NAME
    /// privilege.</para>
    /// </summary>
    EWX_POWEROFF = 0x8,

    /// <summary>
    /// <para>Forces processes to terminate if they do not respond to
    /// the WM_QUERYENDSESSION or WM_ENDSESSION message. This flag is
    /// ignored if EWX_FORCE is used.</para>
    /// <para>Windows NT and Windows Me/98/95:  This value is not
    /// supported.</para>
    /// </summary>
    EWX_FORCEIFHUNG = 0x10
}
