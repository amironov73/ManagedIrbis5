// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* EpubMetadataContributor.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Epub.Schema;

/// <summary>
///
/// </summary>
public class EpubMetadataContributor
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Contributor { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? FileAs { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Role { get; set; }

    #endregion
}
