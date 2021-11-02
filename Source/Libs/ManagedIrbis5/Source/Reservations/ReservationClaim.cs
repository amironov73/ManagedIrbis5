// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ReservationClaim.cs -- заявка на резервирование компьютера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Mapping;
using ManagedIrbis.Readers;

#endregion

#nullable enable

namespace ManagedIrbis.Reservations
{
    /// <summary>
    /// Заявка на резревирование компьютера.
    /// </summary>
    [XmlRoot ("claim")]
    public sealed class ReservationClaim
        : IHandmadeSerializable,
            IVerifiable
    {
        #region Constants

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "abcdz";

        /// <summary>
        /// Метка поля.
        /// </summary>
        public const int Tag = 20;

        #endregion

        #region Properties

        /// <summary>
        /// Номер читательского билета.
        /// </summary>
        [SubField ('a')]
        [XmlAttribute ("ticket")]
        [JsonPropertyName ("ticket")]
        [Description ("Читательский билет")]
        [DisplayName ("Читательский билет")]
        public string? Ticket { get; set; }

        /// <summary>
        /// Дата в виде строки.
        /// </summary>
        [SubField ('b')]
        [XmlAttribute ("date")]
        [JsonPropertyName ("date")]
        [Description ("Дата")]
        [DisplayName ("Дата")]
        public string? DateString { get; set; }

        /// <summary>
        /// Дата.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public DateTime Date
        {
            get => IrbisDate.ConvertStringToDate (DateString);
            set => DateString = IrbisDate.ConvertDateToString (value);
        }

        /// <summary>
        /// Время в виде строки.
        /// </summary>
        [SubField ('c')]
        [XmlAttribute ("time")]
        [JsonPropertyName ("time")]
        [Description ("Время")]
        [DisplayName ("Время")]
        public string? TimeString { get; set; }

        /// <summary>
        /// Время.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public TimeSpan Time
        {
            get => IrbisDate.ConvertStringToTime (TimeString);
            set => TimeString = IrbisDate.ConvertTimeToString (value);
        }

        /// <summary>
        /// Начальные время и дата.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public DateTime BeginDateTime
        {
            get => Date + Time;
            set
            {
                DateTime date = value.Date;
                Date = date;
                Time = value - date;
            }

        } // property BeginDateTime

        /// <summary>
        /// Продолжительность в виде строки.
        /// </summary>
        [SubField ('d')]
        [XmlAttribute ("duration")]
        [JsonPropertyName ("duration")]
        [Description ("Продолжительность")]
        [DisplayName ("Продолжительность")]
        public string? DurationString { get; set; }

        /// <summary>
        /// Продолжительность.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public TimeSpan Duration
        {
            get => IrbisDate.ConvertStringToTime (DurationString);
            set => DurationString = IrbisDate.ConvertTimeToString (value);
        }

        /// <summary>
        /// Конечные время и дата.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public DateTime EndDateTime
        {
            get => Date + Time + Duration;
            set
            {
                DateTime date = value.Date;
                Date = date;
                Duration = value - BeginDateTime;
            }

        } // property EndDateTime

        /// <summary>
        /// Запрос все еще активен?
        /// </summary>
        /// <remarks>
        /// Непустая строка означает "запрос не активен".
        /// </remarks>
        [SubField ('z')]
        [XmlAttribute ("status")]
        [JsonPropertyName ("status")]
        [Description ("Статус")]
        [DisplayName ("Статус")]
        public string? Status { get; set; }

        /// <summary>
        /// Ассоциированное поле записи.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        [Description ("Поле с подполями")]
        [DisplayName ("Поле с подполями")]
        public Field? Field { get; set; }

        /// <summary>
        /// Ассоциированный читатель.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        [Description ("Читатель")]
        [DisplayName ("Читатель")]
        public ReaderInfo? Reader { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        [Description ("Пользовательские данные")]
        [DisplayName ("Пользовательские данные")]
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Применение запроса к полю записи.
        /// </summary>
        public Field ApplyToField (Field field) => field
            .SetSubFieldValue ('a', Ticket)
            .SetSubFieldValue ('b', DateString)
            .SetSubFieldValue ('c', TimeString)
            .SetSubFieldValue ('d', DurationString)
            .SetSubFieldValue ('z', Status);

        /// <summary>
        /// Разбор поля записи.
        /// </summary>
        public static ReservationClaim ParseField
            (
                Field field
            )
        {
            Sure.NotNull (field);

            var result = new ReservationClaim
            {
                Ticket = field.GetFirstSubFieldValue ('a'),
                DateString = field.GetFirstSubFieldValue ('b'),
                TimeString = field.GetFirstSubFieldValue ('c'),
                DurationString = field.GetFirstSubFieldValue ('d'),
                Status = field.GetFirstSubFieldValue ('z'),
                Field = field
            };

            return result;

        } // method ParseField

        /// <summary>
        /// Разбор библиографической записи.
        /// </summary>
        public static NonNullCollection<ReservationClaim> ParseRecord
            (
                Record record
            )
        {
            Sure.NotNull (record);

            var result = new NonNullCollection<ReservationClaim>();
            foreach (var field in record.Fields)
            {
                if (field.Tag == Tag)
                {
                    ReservationClaim claim = ParseField (field);
                    result.Add (claim);
                }
            }

            return result;

        } // method ParseRecord

        /// <summary>
        /// Нужно ли сериализовать свойство <see cref="Status"/>?
        /// </summary>
        public bool ShouldSerializeStatus() => !string.IsNullOrEmpty (Status);

        /// <summary>
        /// Превращение запроса в поле записи.
        /// </summary>
        public Field ToField() => new Field (Tag)
                .AddNonEmpty ('a', Ticket)
                .AddNonEmpty ('b', DateString)
                .AddNonEmpty ('c', TimeString)
                .AddNonEmpty ('d', DurationString)
                .AddNonEmpty ('z', Status);

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            Ticket = reader.ReadNullableString();
            DateString = reader.ReadNullableString();
            TimeString = reader.ReadNullableString();
            DurationString = reader.ReadNullableString();
            Status = reader.ReadNullableString();

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer
                .WriteNullable (Ticket)
                .WriteNullable (DateString)
                .WriteNullable (TimeString)
                .WriteNullable (DurationString)
                .WriteNullable (Status);

        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<ReservationClaim> (this, throwOnError);

            verifier
                .NotNullNorEmpty (Ticket)
                .NotNullNorEmpty (DateString)
                .NotNullNorEmpty (TimeString)
                .NotNullNorEmpty (DurationString);

            return verifier.Result;

        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() =>
            $"{DateString.ToVisibleString()}: {TimeString.ToVisibleString()} {DurationString.ToVisibleString()} [{Ticket.ToVisibleString()}]";

        #endregion

    } // class ReservationClain

} // namespace ManagedIrbis.Reservations
