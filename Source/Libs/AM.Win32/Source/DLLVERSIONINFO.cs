// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* DLLVERSIONINFO.cs -- используется в DllGetVesion
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Win32
{
    /// <summary>
    /// Содержит информацию о версии конкретной DLL.
    /// Используется в функции DllGetVersion.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class DLLVERSIONINFO
    {
        /// <summary>
        /// The size of the structure, in bytes.
        /// This member must be filled in before calling the function.
        /// </summary>
        public uint cbSize;

        /// <summary>
        /// The major version of the DLL.
        /// For instance, if the DLL's version is 4.0.950,
        /// this value will be 4.
        /// </summary>
        public uint dwMajorVersion;

        /// <summary>
        /// The minor version of the DLL.
        /// For instance, if the DLL's version is 4.0.950,
        /// this value will be 0.
        /// </summary>
        public uint dwMinorVersion;

        /// <summary>
        /// The build number of the DLL.
        /// For instance, if the DLL's version is 4.0.950,
        /// this value will be 950.
        /// </summary>
        public uint dwBuildNumber;

        /// <summary>
        /// Identifies the platform for which the DLL was built.
        /// </summary>
        public uint dwPlatformID;

    } // struct DLLVERSIONINFO

} // namespace AM.Win32
