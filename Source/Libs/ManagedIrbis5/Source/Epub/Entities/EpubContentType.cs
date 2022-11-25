// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* EpubContentType.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis.Epub;

/// <summary>
///
/// </summary>
public enum EpubContentType
{
    /// <summary>
    ///
    /// </summary>
    XHTML_1_1 = 1,

    /// <summary>
    ///
    /// </summary>
    DTBOOK,

    /// <summary>
    ///
    /// </summary>
    DTBOOK_NCX,

    /// <summary>
    ///
    /// </summary>
    OEB1_DOCUMENT,

    /// <summary>
    ///
    /// </summary>
    XML,

    /// <summary>
    ///
    /// </summary>
    CSS,

    /// <summary>
    ///
    /// </summary>
    OEB1_CSS,

    /// <summary>
    ///
    /// </summary>
    IMAGE_GIF,

    /// <summary>
    ///
    /// </summary>
    IMAGE_JPEG,

    /// <summary>
    ///
    /// </summary>
    IMAGE_PNG,

    /// <summary>
    ///
    /// </summary>
    IMAGE_SVG,

    /// <summary>
    ///
    /// </summary>
    FONT_TRUETYPE,

    /// <summary>
    ///
    /// </summary>
    FONT_OPENTYPE,

    /// <summary>
    ///
    /// </summary>
    OTHER
}
