// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* MailingList.cs -- описание списка рассылки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace BulletinMaker;

/// <summary>
/// Описание списка рассылки.
/// </summary>
public sealed class MailingList
{
    [XmlAttribute ("id")]
    public string Id { get; set; }

    [XmlElement("address")]
    public List<string> Addresses { get; set; }

    public MailingList()
    {
        Addresses = new List<string>();
    }
}
