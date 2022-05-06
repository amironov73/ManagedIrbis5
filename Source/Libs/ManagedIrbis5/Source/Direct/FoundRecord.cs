// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* FoundRecord.cs -- информация о найденной/отобранной записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace ManagedIrbis.Direct;

/// <summary>
/// Информация о найденной/отобранной записи.
/// </summary>
[Serializable]
public struct FoundRecord
{
    #region Properties

    /// <summary>
    /// MFN записи.
    /// </summary>
    [XmlAttribute ("mfn")]
    [JsonPropertyName ("mfn")]
    [Description ("MFN")]
    public int Mfn { get; set; }

    /// <summary>
    /// Смещение относительно начала файла, байты.
    /// </summary>
    [XmlAttribute ("position")]
    [JsonPropertyName ("position")]
    [Description ("Смещение")]
    public long Position { get; set; }

    /// <summary>
    /// Длина записи в байтах.
    /// </summary>
    [XmlAttribute ("length")]
    [JsonPropertyName ("length")]
    [Description ("Длина")]
    public int Length { get; set; }

    /// <summary>
    /// Количество полей в записи.
    /// </summary>
    [XmlAttribute ("fields")]
    [JsonPropertyName ("fields")]
    [Description ("Полей")]
    public int FieldCount { get; set; }

    /// <summary>
    /// Версия записи (нумерация от 1).
    /// </summary>
    [XmlAttribute ("version")]
    [JsonPropertyName ("version")]
    [Description ("Версия")]
    public int Version { get; set; }

    /// <summary>
    /// Флаги.
    /// </summary>
    [XmlAttribute ("flags")]
    [JsonPropertyName ("flags")]
    [Description ("Флаги")]
    public int Flags { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return $"[{Mfn}] v{Version}";
    }

    #endregion
}
