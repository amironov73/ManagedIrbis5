// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* ActualizeRecordParameters.cs -- параметры актуализации записи на ИРБИС-сервере
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
/// Параметры актуализации записи на ИРБИС-сервере.
/// </summary>
[Serializable]
[XmlRoot ("parameters")]
public sealed class ActualizeRecordParameters
{
    #region Properties

    /// <summary>
    /// Имя базы данных (опционально).
    /// Если не указано, используется текущая база данных.
    /// </summary>
    [XmlAttribute ("database")]
    [JsonPropertyName ("database")]
    [DisplayName ("База данных")]
    [Description ("Имя базы данных")]
    public string? Database { get; set; }

    /// <summary>
    /// MFN актуализируемой записи (обязательно).
    /// 0 означает "актуализировать всю базу данных".
    /// </summary>
    [XmlAttribute ("mfn")]
    [JsonPropertyName ("mfn")]
    [DisplayName ("MFN")]
    [Description ("MFN актуализируемой записи")]
    public int Mfn { get; set; }

    #endregion
}
