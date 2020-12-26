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

/* BootMode.cs -- operating system boot mode
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32
{
    /// <summary>
    /// Operating system boot mode.
    /// </summary>
    public enum BootMode
    {
        /// <summary>
        /// Unknown boot mode.
        /// </summary>
        UnknownBootMode = -1,

        /// <summary>
        /// Normal.
        /// </summary>
        NormalBoot = 0,

        /// <summary>
        /// Failsafe mode.
        /// </summary>
        FailSafeBoot = 1,

        /// <summary>
        /// Failsafe mode with network support.
        /// </summary>
        FailSafeWithNetworkSupportBoot = 2

    } // enum BootMode

} // namespace AM.Win32
