// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Epub.Internal;

internal static class ZipPathUtils
{
    private const string DIRECTORY_UP = "../";

    public static string GetDirectoryPath(string filePath)
    {
        var lastSlashIndex = filePath.LastIndexOf('/');
        if (lastSlashIndex == -1)
        {
            return String.Empty;
        }
        else
        {
            return filePath.Substring(0, lastSlashIndex);
        }
    }

    public static string Combine(string directory, string fileName)
    {
        if (String.IsNullOrEmpty(directory))
        {
            return fileName;
        }
        else
        {
            while (fileName.StartsWith(DIRECTORY_UP))
            {
                var lastDirectorySlashIndex = directory.LastIndexOf('/');
                directory = lastDirectorySlashIndex != -1 ? directory.Substring(0, lastDirectorySlashIndex) : String.Empty;
                fileName = fileName.Substring(DIRECTORY_UP.Length);
            }

            return String.IsNullOrEmpty(directory) ? fileName : String.Concat(directory, '/', fileName);
        }
    }
}