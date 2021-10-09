// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* EventDescription.cs -- информация о мерориятии
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.EventDb
{
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
        public EventTitle? Title { get; set; }

        /// <summary>
        /// Коды. Поле 900.
        /// </summary>
        [XmlElement ("codes")]
        [JsonPropertyName ("codes")]
        public EventCodes? Codes { get; set; }

        /// <summary>
        /// Даты. Поле 30.
        /// </summary>
        [XmlElement ("dates")]
        [JsonPropertyName ("dates")]
        public EventDates? Dates { get; set; }

        /// <summary>
        /// Время. Поле 31.
        /// </summary>
        [XmlElement ("times")]
        [JsonPropertyName ("times")]
        public EventTimes? Times { get; set; }

        /// <summary>
        /// Место проведения мероприятия. Поле 210.
        /// </summary>
        [XmlElement ("location")]
        [JsonPropertyName ("location")]
        public EventLocation? Location { get; set; }

        /// <summary>
        /// Статус мероприятия. Поле 997.
        /// </summary>
        [XmlElement ("status")]
        [JsonPropertyName ("status")]
        public EventStatus? Status { get; set; }

        /// <summary>
        /// Associated <see cref="Record"/>.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public Record? Record { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the record.
        /// </summary>
        public static EventDescription Parse
            (
                Record record
            )
        {
            Sure.NotNull (record, nameof (record));

            var result = new EventDescription
            {
                Title = EventTitle.Parse (record.Fields.GetFirstField (972)),
                Codes = EventCodes.Parse (record.Fields.GetFirstField (900)),
                Dates = EventDates.Parse (record.Fields.GetFirstField (30)),
                Times = EventTimes.Parse (record.Fields.GetFirstField (31)),
                Location = EventLocation.Parse (record.Fields.GetFirstField (210)),
                Status = EventStatus.Parse (record.Fields.GetFirstField (997)),
                Record = record
            };

            return result;

        } // method Parse

        #endregion

    } // class EventDescription

} // namespace ManagedIrbis.EventDb
