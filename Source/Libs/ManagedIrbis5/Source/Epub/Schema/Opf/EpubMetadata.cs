// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* EpubMetadata.cs --
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
public class EpubMetadata
{
    #region Using directives

    /// <summary>
    ///
    /// </summary>
    public List<string>? Titles { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<EpubMetadataCreator>? Creators { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<string>? Subjects { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<string>? Publishers { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<EpubMetadataContributor>? Contributors { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<EpubMetadataDate>? Dates { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<string>? Types { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<string>? Formats { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<EpubMetadataIdentifier>? Identifiers { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<string>? Sources { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<string>? Languages { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<string>? Relations { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<string>? Coverages { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<string>? Rights { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<EpubMetadataMeta>? MetaItems { get; set; }

    #endregion
}
