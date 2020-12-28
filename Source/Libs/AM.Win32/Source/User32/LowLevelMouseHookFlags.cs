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

/* LowLevelMouseHookFlags.cs -- event-injected flags
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Specifies the event-injected flag.
    /// </summary>
    [Flags]
    public enum LowLevelMouseHookFlags
    {
        /// <summary>
        /// Test the event-injected flag.
        /// </summary>
        LLMHF_INJECTED = 0x00000001

    } // enum LowLevelMouseHookFlags

} // namespace AM.Win32
