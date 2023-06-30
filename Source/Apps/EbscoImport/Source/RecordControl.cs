// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* RecordControl.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Xml.Serialization;

#endregion

#nullable enable

namespace EbscoImport;

[XmlRoot ("controlInfo")]
public sealed class RecordControl
{
    [XmlElement ("bkinfo")]
    public BookInfo? Book { get; set; }

    [XmlElement ("pubinfo")]
    public PublicationInfo? Publication { get; set; }

    [XmlElement ("artinfo")]
    public ArticleInfo? Article { get; set; }

    [XmlElement ("language")]
    public LanguageInfo? Language { get; set; }

}
