// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* PublicationDate.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Xml.Serialization;

#endregion

#nullable enable

namespace EbscoImport;

[XmlRoot ("dt")]
public sealed class PublicationDate
{
    [XmlAttribute ("year")]
    public string? Year { get; set; }

    [XmlAttribute ("month")]
    public string? Month { get; set; }

    [XmlAttribute ("day")]
    public string? Day { get; set; }
}
