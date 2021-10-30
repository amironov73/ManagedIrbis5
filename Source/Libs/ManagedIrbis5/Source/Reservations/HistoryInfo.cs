// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* HistoryInfo.cs -- информация о посещении
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Mapping;

#endregion

namespace ManagedIrbis.Reservations
{
    /// <summary>
    /// Информация о посещении. Поле 30.
    /// </summary>
    [XmlRoot ("history")]
    public sealed class HistoryInfo
        : IHandmadeSerializable,
            IVerifiable
    {
        #region Constants

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "abcde";

        /// <summary>
        /// Метка поля.
        /// </summary>
        public const int Tag = 30;

        #endregion

        #region Properties

        /// <summary>
        /// Дата в виде строки.
        /// </summary>
        [SubField ('a')]
        [XmlElement ("date")]
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
        /// Дата начала (полностью).
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public DateTime BeginDate
        {
            get => Date.Add (BeginTime);
            set
            {
                DateTime date = value.Date;
                TimeSpan time = value - date;
                Date = date;
                BeginTime = time;
            }
        }

        /// <summary>
        /// Время начала.
        /// </summary>
        [SubField ('b')]
        [XmlElement ("beginTime")]
        [JsonPropertyName ("beginTime")]
        [Description ("Время начала")]
        [DisplayName ("Время начала")]
        public string? BeginTimeString { get; set; }

        /// <summary>
        /// Время начала.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public TimeSpan BeginTime
        {
            get => IrbisDate.ConvertStringToTime (BeginTimeString);
            set => BeginTimeString = IrbisDate.ConvertTimeToString (value);
        }

        /// <summary>
        /// Дата окончания (полностью).
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public DateTime EndDate
        {
            get => Date.Add (EndTime);
            set
            {
                DateTime date = value.Date;
                TimeSpan time = value - date;
                Date = date;
                EndTime = time;
            }

        } // property EndDate

        /// <summary>
        /// Время окончания.
        /// </summary>
        [SubField ('c')]
        [XmlElement ("endTime")]
        [JsonPropertyName ("endTime")]
        [Description ("Время окончания")]
        [DisplayName ("Время окончания")]
        public string? EndTimeString { get; set; }

        /// <summary>
        /// Время окончания.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public TimeSpan EndTime
        {
            get => IrbisDate.ConvertStringToTime (EndTimeString);
            set => EndTimeString = IrbisDate.ConvertTimeToString (value);
        }

        /// <summary>
        /// Продолжительность.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public TimeSpan Duration
        {
            get => EndTime - BeginTime;
            set => EndTime = BeginTime + value;
        }

        /// <summary>
        /// Номер билета.
        /// </summary>
        [SubField ('d')]
        [XmlElement ("ticket")]
        [JsonPropertyName ("ticket")]
        [Description ("Номер билета")]
        [DisplayName ("Номер билета")]
        public string? Ticket { get; set; }

        /// <summary>
        /// ФИО читателя.
        /// </summary>
        [SubField ('e')]
        [XmlElement ("name")]
        [JsonPropertyName ("name")]
        [Description ("ФИО читателя")]
        [DisplayName ("ФИО читателя")]
        public string? Name { get; set; }

        /// <summary>
        /// Ассоциированное поле записи.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Description ("Поле")]
        [DisplayName ("Поле")]
        public Field? Field { get; private set; }

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
        /// Применение данных к полю записи.
        /// </summary>
        public void ApplyToField
            (
                Field field
            )
        {
            Sure.NotNull (field);

            field
                .ApplySubField ('a', DateString)
                .ApplySubField ('b', BeginTimeString)
                .ApplySubField ('c', EndTimeString)
                .ApplySubField ('d', Ticket)
                .ApplySubField ('e', Name);

        } // method ApplyToField

        /// <summary>
        /// Разбор поля записи.
        /// </summary>
        public static HistoryInfo ParseField
            (
                Field field
            )
        {
            Sure.NotNull (field);

            var result = new HistoryInfo
            {
                DateString = field.GetFirstSubFieldValue ('a'),
                BeginTimeString = field.GetFirstSubFieldValue ('b'),
                EndTimeString = field.GetFirstSubFieldValue ('c'),
                Ticket = field.GetFirstSubFieldValue ('d'),
                Name = field.GetFirstSubFieldValue ('e'),
                Field = field
            };

            return result;

        } // method ParseField

        /// <summary>
        /// Разбор библиографической записи.
        /// </summary>
        public static HistoryInfo[] ParseRecord
            (
                Record record
            )
        {
            Sure.NotNull (record);

            return record.Fields
                .GetField (Tag)
                .Select (field => ParseField (field))
                .ToArray();

        } // method ParseRecord

        /// <summary>
        /// Конвертирование информации в поле записи.
        /// </summary>
        public Field ToField()
        {
            var result = new Field (Tag);
            result
                .AddNonEmptySubField ('a', DateString)
                .AddNonEmptySubField ('b', BeginTimeString)
                .AddNonEmptySubField ('c', EndTimeString)
                .AddNonEmptySubField ('d', Ticket)
                .AddNonEmptySubField ('e', Name);

            return result;

        } // method ToField

        /// <summary>
        /// Нужно ли сериализовать свойство <see cref="EndTimeString"/>?
        /// </summary>
        public bool ShouldSerializeEndTimeString() => !string.IsNullOrEmpty (EndTimeString);

        /// <summary>
        /// Нужно ли сериализовать свойство <see cref="Name"/>?
        /// </summary>
        public bool ShouldSerializeName() => !string.IsNullOrEmpty (Name);

        /// <summary>
        /// Нужно ли сериализовать свойство <see cref="Ticket"/>?
        /// </summary>
        public bool ShouldSerializeTicket() => !string.IsNullOrEmpty (Ticket);

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            DateString = reader.ReadNullableString();
            BeginTimeString = reader.ReadNullableString();
            EndTimeString = reader.ReadNullableString();
            Ticket = reader.ReadNullableString();
            Name = reader.ReadNullableString();

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer
                .WriteNullable (DateString)
                .WriteNullable (BeginTimeString)
                .WriteNullable (EndTimeString)
                .WriteNullable (Ticket)
                .WriteNullable (Name);

        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<HistoryInfo> (this, throwOnError);

            verifier
                .NotNullNorEmpty (DateString)
                .NotNullNorEmpty (BeginTimeString)
                .NotNullNorEmpty (Ticket);

            if (!string.IsNullOrEmpty (BeginTimeString)
                && !string.IsNullOrEmpty (EndTimeString))
            {
                verifier.Assert
                    (
                        string.CompareOrdinal
                            (
                                BeginTimeString,
                                EndTimeString
                            ) < 0,
                        "BeginTimeString < EndTimeString"
                    );
            }

            return verifier.Result;

        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() =>
            $"{DateString.ToVisibleString()}: {BeginTimeString.ToVisibleString()}-{EndTimeString.ToVisibleString()} [{Ticket.ToVisibleString()}] ({Name.ToVisibleString()})";

        #endregion

    } // class HistoryInfo

} // namespace ManagedIrbis.Reservations
