// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* PublicationInfo.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Xml.Serialization;

#endregion

#nullable enable

namespace EbscoImport;

[XmlRoot ("pubInfo")]
public sealed class PublicationInfo
{
    [XmlElement ("dt")]
    public PublicationDate? Date { get; set; }

    [XmlElement ("vid")]
    public string? Id { get; set; }

    [XmlElement ("pub")]
    public string? Publisher { get; set; }

    [XmlElement ("place")]
    public string? Place { get; set; }
}
