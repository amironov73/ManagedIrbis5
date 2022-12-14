// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* EpubVersion.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace ManagedIrbis.Epub.Schema;

/// <summary>
///
/// </summary>
public enum EpubVersion
{
    /// <summary>
    ///
    /// </summary>
    [VersionString ("2")]
    EPUB_2 = 2,

    /// <summary>
    ///
    /// </summary>
    [VersionString ("3")]
    EPUB_3,

    /// <summary>
    ///
    /// </summary>
    [VersionString ("3.1")]
    EPUB_3_1
}

[SuppressMessage ("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name",
    Justification = "Enum and attribute need to be close to each other to indicate that attribute applies only to this enum. The file needs to be named after enum.")]
internal class VersionStringAttribute
    : Attribute
{
    #region Properties

    public string Version { get; }

    #endregion

    #region Construction

    public VersionStringAttribute
        (
            string version
        )
    {
        Version = version;
    }

    #endregion
}
