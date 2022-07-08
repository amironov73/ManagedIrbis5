// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* EnvironmentEx.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

#endregion

#nullable enable

namespace AM.Updating.Internal;

internal static class EnvironmentEx
{
    public static string GetCommandLineWithoutExecutable()
    {
        // Get the executable name
        var exeName = Environment.GetCommandLineArgs().First();
        var quotedExeName = $"\"{exeName}\"";

        // Remove the quoted executable name from command line and return it
        if (Environment.CommandLine.StartsWith (quotedExeName, StringComparison.OrdinalIgnoreCase))
            return Environment.CommandLine.Substring (quotedExeName.Length).Trim();

        // Remove the unquoted executable name from command line and return it
        if (Environment.CommandLine.StartsWith (exeName, StringComparison.OrdinalIgnoreCase))
            return Environment.CommandLine.Substring (exeName.Length).Trim();

        // Safe guard, shouldn't reach here
        return Environment.CommandLine;
    }
}
