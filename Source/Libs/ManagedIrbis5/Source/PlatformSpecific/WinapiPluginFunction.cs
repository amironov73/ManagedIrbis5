// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* WinapiPluginFunction.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Runtime.InteropServices;
using System.Text;

#endregion

#nullable enable

namespace ManagedIrbis.PlatformSpecific
{
    /// <summary>
    /// Plugin function with WinAPI call convention.
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    public delegate int WinapiPluginFunction
        (
            string buf1,
            StringBuilder buf2,
            int bufsize
        );
}

