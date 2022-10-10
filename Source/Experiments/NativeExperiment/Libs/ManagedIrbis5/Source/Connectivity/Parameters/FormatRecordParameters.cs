// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* FormatRecordParameters.cs -- параметры форматирования записи на ИРБИС-сервере
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Collections;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure;

/// <summary>
/// Параметры форматирования записи на ИРБИС-сервере.
/// </summary>
[Serializable]
[XmlRoot ("parameters")]
public sealed class FormatRecordParameters
    : IVerifiable
{
    #region Properties

    /// <summary>
    /// Сюда помещается результат.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public SomeValues<string> Result { get; set; }

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
    /// Спецификация формата (обязательно).
    /// </summary>
    [XmlAttribute ("format")]
    [JsonPropertyName ("format")]
    [DisplayName ("Формат")]
    [Description ("Спецификация формата")]
    public string? Format { get; set; }

    /// <summary>
    /// MFN одной записи, подлежащей расформатированию.
    /// </summary>
    [XmlAttribute ("mfn")]
    [JsonPropertyName ("mfn")]
    [DisplayName ("MFN - один")]
    [Description ("MFN одной записи, подлежащей расформатированию")]
    public int Mfn { get; set; }

    /// <summary>
    /// MFN нескольких записей, подлежащих расформатированию.
    /// </summary>
    [XmlElement ("mfns")]
    [JsonPropertyName ("mfns")]
    [DisplayName ("MFN - несколько")]
    [Description ("MFN нескольких записей, подлежащих расформатированию")]
    public int[]? Mfns { get; set; }

    /// <summary>
    /// Запись, подлежащая расформатированию.
    /// </summary>
    [XmlElement ("record")]
    [JsonPropertyName ("record")]
    [DisplayName ("Запись")]
    [Description ("Запись, подлежащая расформатированию")]
    public Record? Record { get; set; }

    /// <summary>
    /// Записи подлежащие расформатированию.
    /// </summary>
    [XmlElement ("records")]
    [JsonPropertyName ("records")]
    [DisplayName ("Записи")]
    [Description ("Записи, подлежащие расформатированию")]
    public Record[]? Records { get; set; }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<FormatRecordParameters> (this, throwOnError);

        verifier.NotNullNorEmpty (Format);
        verifier.Assert
            (
                Mfn != 0
                || !Mfns.IsNullOrEmpty()
                || Record is not null
                || Records.IsNullOrEmpty()
            );

        return verifier.Result;
    }

    #endregion
}
