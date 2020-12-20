// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* COPYDATASTRUCT.cs -- содержит данные для сообщения WM_COPYDATA
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
    /// Содержит данные, которые будут переданы другому приложению
    /// с помощью сообщения WM_COPYDATA.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct COPYDATASTRUCT
    {
        /// <summary>
        /// The data to be passed to the receiving application.
        /// </summary>
        public int dwData;

        /// <summary>
        /// The size, in bytes, of the data pointed
        /// to by the lpData member.
        /// </summary>
        public int cbData;

        /// <summary>
        /// The data to be passed to the receiving application.
        /// This member can be NULL.
        /// </summary>
        public IntPtr lpData;

    } // struct COPYDATASTRUCT

} // namespace AM.Win32
