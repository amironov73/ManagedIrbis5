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

/* PipeOpenFlags.cs -- named pipe open flags
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Named pipe open flags.
    /// </summary>
    [Flags]
    public enum PipeOpenFlags
    {
        /// <summary>
        /// The flow of data in the pipe goes from client to server only.
        /// This mode gives the server the equivalent of GENERIC_READ
        /// access to the pipe. The client must specify GENERIC_WRITE
        /// access when connecting to the pipe.
        /// </summary>
        PIPE_ACCESS_INBOUND = 0x00000001,

        /// <summary>
        /// The flow of data in the pipe goes from server to client only.
        /// This mode gives the server the equivalent of GENERIC_WRITE
        /// access to the pipe. The client must specify GENERIC_READ
        /// access when connecting to the pipe.
        /// </summary>
        PIPE_ACCESS_OUTBOUND = 0x00000002,

        /// <summary>
        /// The pipe is bi-directional; both server and client processes
        /// can read from and write to the pipe. This mode gives the
        /// server the equivalent of GENERIC_READ | GENERIC_WRITE access
        /// to the pipe. The client can specify GENERIC_READ or
        /// GENERIC_WRITE, or both, when it connects to the pipe using
        /// the CreateFile function.
        /// </summary>
        PIPE_ACCESS_DUPLEX = 0x00000003,

        /// <summary>
        /// Write-through mode is enabled. This mode affects only write
        /// operations on byte-type pipes and, then, only when the
        /// client and server processes are on different computers.
        /// If this mode is enabled, functions writing to a named pipe
        /// do not return until the data written is transmitted across
        /// the network and is in the pipe's buffer on the remote
        /// computer. If this mode is not enabled, the system enhances
        /// the efficiency of network operations by buffering data until
        /// a minimum number of bytes accumulate or until a maximum time
        /// elapses.
        /// </summary>
        FILE_FLAG_WRITE_THROUGH = unchecked((int)0x80000000),

        /// <summary>
        /// Overlapped mode is enabled. If this mode is enabled,
        /// functions performing read, write, and connect operations
        /// that may take a significant time to be completed can return
        /// immediately. This mode enables the thread that started the
        /// operation to perform other operations while the
        /// time-consuming operation executes in the background.
        /// For example, in overlapped mode, a thread can handle
        /// simultaneous input and output (I/O) operations on multiple
        /// instances of a pipe or perform simultaneous read and write
        /// operations on the same pipe handle. If overlapped mode is
        /// not enabled, functions performing read, write, and connect
        /// operations on the pipe handle do not return until the
        /// operation is finished. The ReadFileEx and WriteFileEx
        /// functions can only be used with a pipe handle in overlapped
        /// mode. The ReadFile, WriteFile, ConnectNamedPipe, and
        /// TransactNamedPipe functions can execute either synchronously
        /// or as overlapped operations.
        /// </summary>
        FILE_FLAG_OVERLAPPED = 0x40000000,

        /// <summary>
        /// <para>If you attempt to create multiple instances of a pipe
        /// with this flag, creation of the first instance succeeds,
        /// but creation of the next instance fails with
        /// ERROR_ACCESS_DENIED.</para>
        /// <para>Windows 2000/NT: This flag is not supported until
        /// Windows 2000 SP2 and Windows XP.</para>
        /// </summary>
        FILE_FLAG_FIRST_PIPE_INSTANCE = 0x00080000

    } // enum PipeOpenFlags

} // namespace AM.Win32
