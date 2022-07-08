// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* DirectoryEx.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

#endregion

#nullable enable

namespace AM.Updating.Internal;

internal static class DirectoryEx
{
    public static bool CheckWriteAccess (string dirPath)
    {
        var testFilePath = Path.Combine (dirPath, $"{Guid.NewGuid()}");

        try
        {
            File.WriteAllText (testFilePath, "");
            File.Delete (testFilePath);

            return true;
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }
    }

    public static void Reset (string dirPath)
    {
        if (Directory.Exists (dirPath))
            Directory.Delete (dirPath, true);

        Directory.CreateDirectory (dirPath);
    }
}
