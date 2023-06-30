// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ArticleInfo.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Xml.Serialization;

#endregion

#nullable enable

namespace EbscoImport;

[XmlRoot ("artInfo")]
public sealed class ArticleInfo
{
    [XmlArray ("tig")]
    [XmlArrayItem ("atl")]
    public string[]? Titles { get; set; }

    [XmlArray ("aug")]
    [XmlArrayItem ("au")]
    public string[]? Authors { get; set; }

    [XmlArray ("sug")]
    [XmlArrayItem ("subj")]
    public SubjectInfo[]? Subjects { get; set; }

    [XmlElement ("ab")]
    public string? Abstract { get; set; }

    [XmlElement ("pubtype")]
    public string? PublicationType { get; set; }

    [XmlElement ("doctype")]
    public string? DocumentType { get; set; }
}
