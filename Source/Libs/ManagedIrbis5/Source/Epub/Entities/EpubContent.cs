// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* EpubContent.cs --
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
public class EpubContent
{
    /// <summary>
    ///
    /// </summary>
    public EpubByteContentFile Cover { get; set; }

    /// <summary>
    ///
    /// </summary>
    public EpubTextContentFile NavigationHtmlFile { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Dictionary<string, EpubTextContentFile> Html { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Dictionary<string, EpubTextContentFile> Css { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Dictionary<string, EpubByteContentFile> Images { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Dictionary<string, EpubByteContentFile> Fonts { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Dictionary<string, EpubContentFile> AllFiles { get; set; }
}
