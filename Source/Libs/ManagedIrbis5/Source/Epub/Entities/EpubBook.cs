// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* EpubBook.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.Epub;

/// <summary>
///
/// </summary>
public class EpubBook
{
    /// <summary>
    ///
    /// </summary>
    public string? FilePath { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Author { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<string>? AuthorList { get; set; }

    /// <summary>
    ///
    /// </summary>
    public byte[]? CoverImage { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<EpubTextContentFile>? ReadingOrder { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<EpubNavigationItem>? Navigation { get; set; }

    /// <summary>
    ///
    /// </summary>
    public EpubContent? Content { get; set; }

    /// <summary>
    ///
    /// </summary>
    public EpubSchema? Schema { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Description { get; set; }
}
