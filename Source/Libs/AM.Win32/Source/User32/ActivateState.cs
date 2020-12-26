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

/* ActivateState.cs -- flags for WM_ACTIVATE message
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Flags for WM_ACTIVATE message (low word of wparam).
    /// </summary>
    public enum ActivateState
        : ushort
    {
        /// <summary>
        /// Window has been deactivated.
        /// </summary>
        WA_INACTIVE = 0,

        /// <summary>
        /// Window activated by other than a mouse click,
        /// like call to SetActiveWindow.
        /// </summary>
        WA_ACTIVE = 1,

        /// <summary>
        /// Window activated by a mouse click.
        /// </summary>
        WA_CLICKACTIVE = 2

    } // enum ActivateState

} // namespace AM.Win32
