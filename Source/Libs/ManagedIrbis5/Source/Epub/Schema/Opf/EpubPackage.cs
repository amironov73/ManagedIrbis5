// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* EpubPackage.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Epub.Internal;

#endregion

#nullable enable

namespace ManagedIrbis.Epub.Schema;

/// <summary>
///
/// </summary>
public class EpubPackage
{
    /// <summary>
    ///
    /// </summary>
    public EpubVersion EpubVersion { get; set; }

    /// <summary>
    ///
    /// </summary>
    public EpubMetadata? Metadata { get; set; }

    /// <summary>
    ///
    /// </summary>
    public EpubManifest? Manifest { get; set; }

    /// <summary>
    ///
    /// </summary>
    public EpubSpine? Spine { get; set; }

    /// <summary>
    ///
    /// </summary>
    public EpubGuide? Guide { get; set; }

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public string GetVersionString()
    {
        return VersionUtils.GetVersionString (EpubVersion);
    }

    #endregion
}
