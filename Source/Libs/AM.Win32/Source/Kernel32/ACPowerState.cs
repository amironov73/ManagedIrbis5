// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ACPowerState.cs -- AC power status.
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32
{
    /// <summary>
    /// AC power status.
    /// </summary>
    public enum ACPowerStatus
        : byte
    {
        /// <summary>
        /// Offline.
        /// </summary>
        Offline = 0,

        /// <summary>
        /// Online.
        /// </summary>
        Online = 1,

        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown = 255

    } // enum ACPowerStatus

} // namespace AM.Win32
