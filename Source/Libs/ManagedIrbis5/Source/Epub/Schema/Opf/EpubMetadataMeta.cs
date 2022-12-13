// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* EpubMetadataMeta.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Epub.Schema;

/// <summary>
///
/// </summary>
public class EpubMetadataMeta
{
    /// <summary>
    ///
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Refines { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Property { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Scheme { get; set; }
}
