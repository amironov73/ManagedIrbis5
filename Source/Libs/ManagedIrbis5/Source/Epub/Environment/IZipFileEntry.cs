// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IZipFileEntry.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

#endregion

#nullable enable

namespace ManagedIrbis.Epub.Environment;

/// <summary>
///
/// </summary>
public interface IZipFileEntry
{
    /// <summary>
    ///
    /// </summary>
    long Length { get; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    Stream Open();
}
