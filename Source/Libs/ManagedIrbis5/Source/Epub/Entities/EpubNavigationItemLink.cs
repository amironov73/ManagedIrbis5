// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* EpubNavigationItemLink.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using ManagedIrbis.Epub.Internal;

#endregion

#nullable enable

namespace ManagedIrbis.Epub;

/// <summary>
///
/// </summary>
public sealed class EpubNavigationItemLink
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public string ContentFileName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ContentFilePathInEpubArchive { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Anchor { get; set; }

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    public EpubNavigationItemLink
        (
            string contentFileUrl,
            string baseDirectoryPath
        )
    {
        var urlParser = new UrlParser (contentFileUrl);
        ContentFileName = urlParser.Path.ThrowIfNull();
        ContentFilePathInEpubArchive = ZipPathUtils.Combine (baseDirectoryPath, ContentFileName);
        Anchor = urlParser.Anchor;
    }

    #endregion
}
