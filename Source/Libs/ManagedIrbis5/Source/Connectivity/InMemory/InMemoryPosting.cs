// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* InMemoryPosting.cs -- постинг термина, хранящийся в оперативной памяти
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace ManagedIrbis.InMemory;

/// <summary>
/// Постинг термина, хранящийся в оперативной памяти.
/// </summary>
[Serializable]
[XmlRoot ("posting")]
public sealed class InMemoryPosting
{
    #region Properties

    /// <summary>
    /// MFN.
    /// </summary>
    [XmlAttribute ("mfn")]
    [JsonPropertyName ("mfn")]
    [DisplayName ("MFN")]
    [Description ("MFN")]
    public int Mfn { get; set; }

    /// <summary>
    /// Метка поля.
    /// </summary>
    [XmlAttribute ("tag")]
    [JsonPropertyName ("tag")]
    [DisplayName ("Метка")]
    [Description ("Метка поля")]
    public int Tag { get; set; }

    /// <summary>
    /// Номер повторения поля.
    /// </summary>
    [XmlAttribute ("occurrence")]
    [JsonPropertyName ("occurrence")]
    [DisplayName ("Повторение")]
    [Description ("Номер повторения поля")]
    public int Occurrence { get; set; }

    /// <summary>
    /// Позиция в поле.
    /// </summary>
    [XmlAttribute ("position")]
    [JsonPropertyName ("position")]
    [DisplayName ("Позиция")]
    [Description ("Позиция в поле")]
    public int Position { get; set; }

    #endregion
}
