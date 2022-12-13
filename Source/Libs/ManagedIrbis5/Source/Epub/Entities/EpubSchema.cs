// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* EpubSchema.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Epub.Schema;

#endregion

#nullable enable

namespace ManagedIrbis.Epub;

/// <summary>
///
/// </summary>
public sealed class EpubSchema
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public EpubPackage? Package { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Epub2Ncx? Epub2Ncx { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Epub3NavDocument? Epub3NavDocument { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? ContentDirectoryPath { get; set; }

    #endregion
}
