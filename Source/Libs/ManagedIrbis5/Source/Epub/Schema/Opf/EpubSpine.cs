// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* EpubSpine.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.Epub.Schema;

/// <summary>
///
/// </summary>
public class EpubSpine
    : List<EpubSpineItemRef>
{
    /// <summary>
    ///
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    public PageProgressionDirection? PageProgressionDirection { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Toc { get; set; }
}
