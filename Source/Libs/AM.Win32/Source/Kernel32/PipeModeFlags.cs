﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
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

/* PipeModeFlags.cs -- named pipe mode flags
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Named pipe mode flags.
    /// </summary>
    [Flags]
    public enum PipeModeFlags
    {
        /// <summary>
        /// Blocking mode is enabled. When the pipe handle is specified
        /// in the ReadFile, WriteFile, or ConnectNamedPipe function,
        /// the operations are not completed until there is data to read,
        /// all data is written, or a client is connected. Use of this
        /// mode can mean waiting indefinitely in some situations for a
        /// client process to perform an action.
        /// </summary>
        PIPE_WAIT = 0x00000000,

        /// <summary>
        /// <para>Nonblocking mode is enabled. In this mode, ReadFile,
        /// WriteFile, and ConnectNamedPipe always return immediately.
        /// </para>
        /// <para>Note that nonblocking mode is supported for
        /// compatibility with Microsoft LAN Manager version 2.0 and
        /// should not be used to achieve asynchronous I/O with named
        /// pipes.</para>
        /// </summary>
        PIPE_NOWAIT = 0x00000001,

        /// <summary>
        /// Data is read from the pipe as a stream of bytes. This mode
        /// can be used with either PIPE_TYPE_MESSAGE or PIPE_TYPE_BYTE.
        /// </summary>
        PIPE_READMODE_BYTE = 0x00000000,

        /// <summary>
        /// Data is read from the pipe as a stream of messages. This mode
        /// can be only used if PIPE_TYPE_MESSAGE is also specified.
        /// </summary>
        PIPE_READMODE_MESSAGE = 0x00000002,

        /// <summary>
        /// Data is written to the pipe as a stream of bytes. This mode
        /// cannot be used with PIPE_READMODE_MESSAGE.
        /// </summary>
        PIPE_TYPE_BYTE = 0x00000000,

        /// <summary>
        /// Data is written to the pipe as a stream of messages.
        /// This mode can be used with either PIPE_READMODE_MESSAGE
        /// or PIPE_READMODE_BYTE.
        /// </summary>
        PIPE_TYPE_MESSAGE = 0x00000004

    } // enum PipeModeFlags

} // namespace AM.Win32
