// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* EpubContentRef.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.Epub;

/// <summary>
///
/// </summary>
public class EpubContentRef
{
    /// <summary>
    ///
    /// </summary>
    public EpubByteContentFileRef? Cover { get; set; }

    /// <summary>
    ///
    /// </summary>
    public EpubTextContentFileRef? NavigationHtmlFile { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Dictionary<string, EpubTextContentFileRef>? Html { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Dictionary<string, EpubTextContentFileRef>? Css { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Dictionary<string, EpubByteContentFileRef>? Images { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Dictionary<string, EpubByteContentFileRef>? Fonts { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Dictionary<string, EpubContentFileRef>? AllFiles { get; set; }
}
