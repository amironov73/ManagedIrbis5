// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* InMemoryTerm.cs -- термин поискового словаря, хранящийся в оперативной памяти
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace ManagedIrbis.InMemory;

/// <summary>
/// Термин поискового словаря, хранящийся в оперативной памяти.
/// </summary>
[Serializable]
[XmlRoot ("term")]
public class InMemoryTerm
{
    #region Properties

    /// <summary>
    /// Текст.
    /// </summary>
    [XmlAttribute ("text")]
    [JsonPropertyName ("text")]
    [DisplayName ("Текст")]
    [Description ("Текст")]
    public string? Text { get; set; }

    /// <summary>
    /// Постинги термина.
    /// </summary>
    [XmlElement ("postings")]
    [JsonPropertyName ("postings")]
    [DisplayName ("Постинги")]
    [Description ("Постинги термина")]
    public List<InMemoryPosting>? Postings { get; set; }

    #endregion
}
