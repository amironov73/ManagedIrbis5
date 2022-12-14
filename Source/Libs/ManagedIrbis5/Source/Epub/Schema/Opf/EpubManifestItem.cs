// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* EpubManifestItem.cs --
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
public class EpubManifestItem
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Href { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? MediaType { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? RequiredNamespace { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? RequiredModules { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Fallback { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? FallbackStyle { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<ManifestProperty>? Properties { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"Id: {Id}, Href = {Href}, MediaType = {MediaType}";
    }

    #endregion
}
