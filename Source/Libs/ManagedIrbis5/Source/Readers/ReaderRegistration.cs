// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* ReaderRegistration.cs -- информация о регистрации/перерегистрации читателя
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
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

#nullable enable

namespace ManagedIrbis.Readers
{
    /// <summary>
    /// Информация о регистрации/перерегистрации читателя.
    /// </summary>
    [XmlRoot("registration")]
    public sealed class ReaderRegistration
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Поле регистрация.
        /// </summary>
        public const int RegistrationTag = 51;

        /// <summary>
        /// Поле "перерегистрация".
        /// </summary>
        public const int ReregistrationTag = 52;

        #endregion

        #region Properties

        /// <summary>
        /// Дата. Подполе *.
        /// </summary>
        [XmlAttribute("date")]
        [JsonPropertyName("date")]
        public string? DateString { get; set; }

        /// <summary>
        /// Дата.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public DateTime Date
        {
            get => IrbisDate.ConvertStringToDate(DateString);
            set => DateString = IrbisDate.ConvertDateToString(value);
        }

        /// <summary>
        /// Место (кафедра обслуживания).
        /// Подполе c.
        /// </summary>
        [SubField('c')]
        [XmlAttribute("chair")]
        [JsonPropertyName("chair")]
        public string? Chair { get; set; }

        /// <summary>
        /// Номер приказа. Подполе a.
        /// </summary>
        [SubField('a')]
        [XmlAttribute("order-number")]
        [JsonPropertyName("order-number")]
        public string? OrderNumber { get; set; }

        /// <summary>
        /// Причина. Подполе b.
        /// </summary>
        [SubField('b')]
        [XmlAttribute("reason")]
        [JsonPropertyName("reason")]
        public string? Reason { get; set; }

        /// <summary>
        /// Ссылка на зарегистрированного читателя.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public ReaderInfo? Reader { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор поля.
        /// </summary>
        public static ReaderRegistration Parse
            (
                Field field
            )
        {
            // TODO Support for unknown subfields

            var result = new ReaderRegistration
            {
                DateString = field.Value,
                Chair = field.GetFirstSubFieldValue('c'),
                OrderNumber = field.GetFirstSubFieldValue('a'),
                Reason = field.GetFirstSubFieldValue('b')
            };

            return result;
        } // method Parse

        /// <summary>
        /// Разбор записи.
        /// </summary>
        public static ReaderRegistration[] Parse
            (
                Record record,
                int tag
            )
        {
            var result = record.Fields
                .GetField(tag)
                .Select(field => Parse(field))
                .ToArray();

            return result;
        } // method Parse

        /// <summary>
        /// Преобразование в поле.
        /// </summary>
        public Field ToField()
        {
            var result = new Field (RegistrationTag, DateString)
                .AddNonEmpty ('c', Chair)
                .AddNonEmpty ('a', OrderNumber)
                .AddNonEmpty ('b', Reason);

            return result;

        } // method ToField

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            DateString = reader.ReadNullableString();
            Chair = reader.ReadNullableString();
            OrderNumber = reader.ReadNullableString();
            Reason = reader.ReadNullableString();

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable(DateString);
            writer.WriteNullable(Chair);
            writer.WriteNullable(OrderNumber);
            writer.WriteNullable(Reason);

        } // method SaveToStream

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => $"{DateString.ToVisibleString()} - {Chair.ToVisibleString()}";

        #endregion

    } // class ReaderRegistration

} // namespace ManagedIrbis.Readers
