// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* IZipFile.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Epub.Environment;

/// <summary>
///
/// </summary>
public interface IZipFile
    : IDisposable
{
    /// <summary>
    ///
    /// </summary>
    IZipFileEntry GetEntry (string entryName);
}
