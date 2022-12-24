// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

#endregion

#nullable enable

namespace HtmlAgilityPack;

internal struct IOLibrary
{
    #region Internal Methods

    internal static void CopyAlways (string source, string target)
    {
        if (!File.Exists (source))
        {
            return;
        }

        Directory.CreateDirectory (Path.GetDirectoryName (target));
        MakeWritable (target);
        File.Copy (source, target, true);
    }
#if !PocketPC && !WINDOWS_PHONE
    internal static void MakeWritable (string path)
    {
        if (!File.Exists (path))
        {
            return;
        }

        File.SetAttributes (path, File.GetAttributes (path) & ~FileAttributes.ReadOnly);
    }
#else
        internal static void MakeWritable(string path)
        {
        }
#endif

    #endregion
}
