// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* BookInfo.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Xml.Serialization;

#endregion

#nullable enable

namespace EbscoImport;

[XmlRoot ("bkInfo")]
public sealed class BookInfo
{
    [XmlElement ("btl")]
    public string? Title { get; set; }

    [XmlArray ("aug")]
    [XmlArrayItem ("au")]
    public string[]? Authors { get; set; }

    [XmlElement ("sertl")]
    public string? Series { get; set; }

    [XmlElement ("isbn")]
    public IsbnInfo[]? Isbn { get; set; }
}
