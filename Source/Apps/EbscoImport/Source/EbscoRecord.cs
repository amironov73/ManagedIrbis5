// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* EbscoRecord.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Xml.Serialization;

#endregion

#nullable enable

namespace EbscoImport;

[XmlRoot ("rec")]
public sealed class EbscoRecord
{
    [XmlAttribute ("resultID")]
    public string? ResultId { get; set; }

    [XmlElement ("header")]
    public RecordHeader? Header { get; set; }
}
