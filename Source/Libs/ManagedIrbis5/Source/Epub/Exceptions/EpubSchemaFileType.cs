// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* EpubSchemaFileType.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis.Epub;

/// <summary>
///
/// </summary>
public enum EpubSchemaFileType
{
    /// <summary>
    ///
    /// </summary>
    UNKNOWN = 0,

    /// <summary>
    ///
    /// </summary>
    META_INF_CONTAINER,

    /// <summary>
    ///
    /// </summary>
    OPF_PACKAGE,

    /// <summary>
    ///
    /// </summary>
    EPUB2_NCX,

    /// <summary>
    ///
    /// </summary>
    EPUB3_NAV_DOCUMENT
}
