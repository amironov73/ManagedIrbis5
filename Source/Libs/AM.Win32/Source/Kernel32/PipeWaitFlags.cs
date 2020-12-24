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

/* PipeWaitFlags.cs -- wait intervals for named pipes
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Wait intervals for named pipes.
    /// </summary>
    [Flags]
    public enum PipeWaitFlags
    {
        /// <summary>
        /// Waits indefinitely.
        /// </summary>
        NMPWAIT_WAIT_FOREVER = unchecked((int)0xffffffff),

        /// <summary>
        /// Does not wait for the named pipe. If the named pipe
        /// is not available, the function returns an error.
        /// </summary>
        NMPWAIT_NOWAIT = 0x00000001,

        /// <summary>
        /// Uses the default time-out specified in a call to the
        /// CreateNamedPipe function.
        /// </summary>
        NMPWAIT_USE_DEFAULT_WAIT = 0x00000000

    } // enum PipeWaitFlags

} // namespace AM.Win32
