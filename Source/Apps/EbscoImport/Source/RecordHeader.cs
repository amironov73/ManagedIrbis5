// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* RecordHeader.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Xml.Serialization;

#endregion

#nullable enable

namespace EbscoImport;

[XmlRoot ("header")]
public sealed class RecordHeader
{
    [XmlAttribute ("shortDbName")]
    public string? ShortDatabaseName { get; set; }

    [XmlAttribute ("longDbName")]
    public string? LongDatabaseName { get; set; }

    [XmlAttribute ("uiTerm")]
    public string? Term { get; set; }

    [XmlElement ("controlInfo")]
    public RecordControl? Control { get; set; }
}
