// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* EventDescription.cs -- информация о мерориятии
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.EventDb;

/// <summary>
/// Информация о мероприятии.
/// </summary>
public sealed class EventDescription
{
    #region Properties

    /// <summary>
    /// Название мероприятия. Поле 972.
    /// </summary>
    [XmlElement ("title")]
    [JsonPropertyName ("title")]
    [DisplayName ("Название")]
    public EventTitle? Title { get; set; }

    /// <summary>
    /// Коды. Поле 900.
    /// </summary>
    [XmlElement ("codes")]
    [JsonPropertyName ("codes")]
    [DisplayName ("Коды")]
    public EventCodes? Codes { get; set; }

    /// <summary>
    /// Даты. Поле 30.
    /// </summary>
    [XmlElement ("dates")]
    [JsonPropertyName ("dates")]
    [DisplayName ("Даты")]
    public EventDates? Dates { get; set; }

    /// <summary>
    /// Время. Поле 31.
    /// </summary>
    [XmlElement ("times")]
    [JsonPropertyName ("times")]
    [DisplayName ("Время")]
    public EventTimes? Times { get; set; }

    /// <summary>
    /// Место проведения мероприятия. Поле 210.
    /// </summary>
    [XmlElement ("location")]
    [JsonPropertyName ("location")]
    [DisplayName ("Место")]
    public EventLocation? Location { get; set; }

    /// <summary>
    /// Статус мероприятия. Поле 997.
    /// </summary>
    [XmlElement ("status")]
    [JsonPropertyName ("status")]
    [DisplayName ("Статус")]
    public EventStatus? Status { get; set; }

    /// <summary>
    /// Ассоциированная библиографическая запись <see cref="Record"/>.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public Record? Record { get; set; }

    /// <summary>
    /// Произвольные пользовательские данные.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public object? UserData { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор библиографической записи <see cref="Record"/>.
    /// </summary>
    public static EventDescription Parse
        (
            Record record
        )
    {
        Sure.NotNull (record);

        var result = new EventDescription
        {
            Title = EventTitle.Parse (record.Fields.GetFirstField (972)),
            Codes = EventCodes.ParseField (record.Fields.GetFirstField (900)),
            Dates = EventDates.Parse (record.Fields.GetFirstField (30)),
            Times = EventTimes.Parse (record.Fields.GetFirstField (31)),
            Location = EventLocation.Parse (record.Fields.GetFirstField (210)),
            Status = EventStatus.Parse (record.Fields.GetFirstField (997)),
            Record = record
        };

        return result;
    }

    #endregion
}
