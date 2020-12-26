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
/* BroadcastSystemMessageRecipient.cs -- information about the recipients of the system message
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Contains information about the recipients of the
    /// system message.
    /// </summary>
    [Flags]
    public enum BroadcastSystemMessageRecipient
    {
        /// <summary>
        /// Broadcast to all system components.
        /// </summary>
        BSM_ALLCOMPONENTS = 0x00000000,

        /// <summary>
        /// Windows 95/98/Me: Broadcast to all system-level device drivers.
        /// </summary>
        BSM_VXDS = 0x00000001,

        /// <summary>
        /// Windows 95/98/Me: Broadcast to network drivers.
        /// </summary>
        BSM_NETDRIVER = 0x00000002,

        /// <summary>
        /// Windows 95/98/Me: Broadcast to installable drivers.
        /// </summary>
        BSM_INSTALLABLEDRIVERS = 0x00000004,

        /// <summary>
        /// Broadcast to applications.
        /// </summary>
        BSM_APPLICATIONS = 0x00000008,

        /// <summary>
        /// Windows NT/2000/XP: Broadcast to all desktops.
        /// Requires the SE_TCB_NAME privilege.
        /// </summary>
        BSM_ALLDESKTOPS = 0x00000010

    } // enum BroadcastSystemMessageRecipient

} // namespace AM.Win32
