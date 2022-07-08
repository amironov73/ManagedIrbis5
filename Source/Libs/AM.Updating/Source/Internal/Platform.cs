// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* Platform.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion


namespace AM.Updating.Internal;

internal static class Platform
{
    public static void EnsureWindows()
    {
        if (!RuntimeInformation.IsOSPlatform (OSPlatform.Windows))
            throw new PlatformNotSupportedException ("Only Windows platform supprted");
    }
}
