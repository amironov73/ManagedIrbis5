// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ManifestProperty.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace ManagedIrbis.Epub.Schema;

/// <summary>
///
/// </summary>
public enum ManifestProperty
{
    /// <summary>
    ///
    /// </summary>
    COVER_IMAGE = 1,

    /// <summary>
    ///
    /// </summary>
    MATHML,

    /// <summary>
    ///
    /// </summary>
    NAV,

    /// <summary>
    ///
    /// </summary>
    REMOTE_RESOURCES,

    /// <summary>
    ///
    /// </summary>
    SCRIPTED,

    /// <summary>
    ///
    /// </summary>
    SVG,

    /// <summary>
    ///
    /// </summary>
    UNKNOWN
}

/// <summary>
///
/// </summary>
[SuppressMessage ("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name",
    Justification = "Enum and parser need to be close to each other to avoid issues when the enum was changed without changing the parser. The file needs to be named after enum.")]
internal static class ManifestPropertyParser
{
    public static ManifestProperty Parse (string stringValue)
    {
        return stringValue.ToLowerInvariant() switch
        {
            "cover-image" => ManifestProperty.COVER_IMAGE,
            "mathml" => ManifestProperty.MATHML,
            "nav" => ManifestProperty.NAV,
            "remote-resources" => ManifestProperty.REMOTE_RESOURCES,
            "scripted" => ManifestProperty.SCRIPTED,
            "svg" => ManifestProperty.SVG,
            _ => ManifestProperty.UNKNOWN
        };
    }
}
