// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* CreateDatabaseParameters.cs -- параметры создания базы данных на ИРБИС-сервере
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure;

/// <summary>
/// Параметры создания базы данных на ИРБИС-сервере
/// </summary>
[Serializable]
[XmlRoot ("parameters")]
public sealed class CreateDatabaseParameters
{
    #region Properties

    /// <summary>
    /// Имя создаваемой базы данных (обязательно).
    /// </summary>
    [XmlAttribute ("database")]
    [JsonPropertyName ("database")]
    [DisplayName ("База данных")]
    [Description ("Имя базы данных")]
    public string? Database { get; set; }

    /// <summary>
    /// Описание создаваемой базы данных в произвольной форме
    /// (опционально).
    /// </summary>
    [XmlAttribute ("description")]
    [JsonPropertyName ("description")]
    [DisplayName ("Описание")]
    [Description ("Описание в произвольной форме")]
    public string? Description { get; set; }

    /// <summary>
    /// Сделать базу данных видимой для АРМ "Читатель"?
    /// </summary>
    [XmlAttribute ("reader")]
    [JsonPropertyName ("reader")]
    [DisplayName ("Читатель имеет доступ?")]
    [Description ("Читатель имеет доступ к базе?")]
    public bool ReaderAccess { get; set; }

    /// <summary>
    /// Имя базы данных-шаблона (опционально).
    /// </summary>
    [XmlAttribute ("template")]
    [JsonPropertyName ("template")]
    [DisplayName ("Шаблон")]
    [Description ("Имя базы данных-шаблона")]
    public string? Template { get; set; }

    #endregion
}
