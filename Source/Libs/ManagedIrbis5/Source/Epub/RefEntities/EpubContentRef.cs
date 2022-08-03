// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.Epub;

public class EpubContentRef
{
    public EpubByteContentFileRef Cover { get; set; }
    public EpubTextContentFileRef NavigationHtmlFile { get; set; }
    public Dictionary<string, EpubTextContentFileRef> Html { get; set; }
    public Dictionary<string, EpubTextContentFileRef> Css { get; set; }
    public Dictionary<string, EpubByteContentFileRef> Images { get; set; }
    public Dictionary<string, EpubByteContentFileRef> Fonts { get; set; }
    public Dictionary<string, EpubContentFileRef> AllFiles { get; set; }
}