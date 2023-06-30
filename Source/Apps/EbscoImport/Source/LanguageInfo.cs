// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* LanguageInfo.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Xml.Serialization;

#endregion

#nullable enable

namespace EbscoImport;

[XmlRoot ("language")]
public sealed class LanguageInfo
{
    [XmlAttribute ("code")]
    public string? Code { get; set; }

    [XmlText]
    public string? Name { get; set; }
}
