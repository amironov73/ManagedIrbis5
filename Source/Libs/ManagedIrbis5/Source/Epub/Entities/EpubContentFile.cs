// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* EpubContentFile.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Epub;

/// <summary>
///
/// </summary>
public abstract class EpubContentFile
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? FilePathInEpubArchive { get; set; }

    /// <summary>
    ///
    /// </summary>
    public EpubContentType ContentType { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? ContentMimeType { get; set; }

    #endregion
}
