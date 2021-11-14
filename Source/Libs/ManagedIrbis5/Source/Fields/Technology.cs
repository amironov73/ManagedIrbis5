// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Technology.cs -- информация о создании и внесении модификаций в запись
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Информация о создании и внесении изменений в библиографическую запись.
    /// Поле 907.
    /// </summary>
    [XmlRoot ("technology")]
    public sealed class Technology
        : IHandmadeSerializable,
            IVerifiable
    {
        #region Constants

        /// <summary>
        /// Метка поля.
        /// </summary>
        public const int Tag = 907;

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "abc";

        #endregion

        #region Properties

        /// <summary>
        /// Этап работы, подполе C. См. <see cref="WorkPhase"/>.
        /// </summary>
        [SubField ('c')]
        [XmlElement ("phase")]
        [JsonPropertyName ("phase")]
        [Description ("Этап работы")]
        [DisplayName ("Этап работы")]
        public string? Phase { get; set; }

        /// <summary>
        /// Дата, подполе A.
        /// </summary>
        [SubField ('a')]
        [XmlElement ("date")]
        [JsonPropertyName ("date")]
        [Description ("Дата")]
        [DisplayName ("Дата")]
        public string? Date { get; set; }

        /// <summary>
        /// Ответственное лицо, ФИО, подполе B.
        /// </summary>
        [SubField ('b')]
        [XmlElement ("responsible")]
        [JsonPropertyName( ("responsible"))]
        [Description ("Ответственное лицо")]
        [DisplayName ("Ответственное лицо")]
        public string? Responsible { get; set; }

        /// <summary>
        /// Неизвестные подполя.
        /// </summary>
        [XmlElement ("unknown")]
        [JsonPropertyName ("unknown")]
        [Browsable (false)]
        public SubField[]? UnknownSubFields { get; set; }

        /// <summary>
        /// Связанное поле библиографической записи <see cref="Field"/>.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public Field? Field { get; set; }

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
        /// Применение информации к полю.
        /// </summary>
        public Field ApplyToField (Field field) => field
            .ThrowIfNull ()
            .SetSubFieldValue ('a', Date)
            .SetSubFieldValue ('b', Responsible)
            .SetSubFieldValue ('c', Phase);

        /// <summary>
        /// Получение даты первой модификации (создания) записи.
        /// </summary>
        public static string? GetFirstDate
            (
                Record record,
                int tag = Tag
            )
        {
            Sure.NotNull (record);
            Sure.Positive (tag);

            string? result = null;
            foreach (var field in record.EnumerateField (tag))
            {
                var candidate = field.GetFirstSubFieldValue ('a');
                if (!string.IsNullOrEmpty (candidate))
                {
                    if (string.IsNullOrEmpty (result))
                    {
                        result = candidate;
                    }
                    else
                    {
                        result = string.CompareOrdinal (result, candidate) > 0
                            ? candidate
                            : result;
                    }

                } // if

            } // foreach

            return result;

        } // method GetFirstDate

        /// <summary>
        /// Получение даты последней модификации записи.
        /// </summary>
        public static string? GetLatestDate
            (
                Record record,
                int tag = Tag
            )
        {
            Sure.NotNull (record);
            Sure.Positive (tag);

            string? result = null;
            foreach (var field in record.EnumerateField (tag))
            {
                var candidate = field.GetFirstSubFieldValue ('a');
                if (!string.IsNullOrEmpty (candidate))
                {
                    if (string.IsNullOrEmpty (result))
                    {
                        result = candidate;
                    }
                    else
                    {
                        result = string.CompareOrdinal (result, candidate) < 0
                            ? candidate
                            : result;
                    }

                } // if

            } // foreach

            return result;

        } // method GetLatestDate

        /// <summary>
        /// Разбор библиографической записи <see cref="Record"/>.
        /// </summary>
        public static Technology[] ParseRecord
            (
                Record record,
                int tag = Tag
            )
        {
            Sure.NotNull (record);
            Sure.Positive (tag);

            var result = new List<Technology>();
            foreach (var field in record.Fields)
            {
                if (field.Tag == tag)
                {
                    var tech = ParseField (field);
                    result.Add (tech);
                }
            }

            return result.ToArray();

        } // method Parse

        /// <summary>
        /// Разбор указанного поля библиографической записи <see cref="Field"/>.
        /// </summary>
        public static Technology ParseField
            (
                Field field
            )
        {
            Sure.NotNull (field);

            var result = new Technology
            {
                Date = field.GetFirstSubFieldValue ('a'),
                Responsible = field.GetFirstSubFieldValue ('b'),
                Phase = field.GetFirstSubFieldValue ('c'),
                UnknownSubFields = field.Subfields.GetUnknownSubFields (KnownCodes),
                Field = field
            };

            return result;

        } // method Parse

        /// <summary>
        /// Преобразование информации в поле библиографической записи.
        /// </summary>
        public Field ToField() =>  new Field (Tag)
                .AddNonEmpty ('a', Date)
                .AddNonEmpty ('b', Responsible)
                .AddNonEmpty ('c', Phase);  // method ToField

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            Date = reader.ReadNullableString();
            Responsible = reader.ReadNullableString();
            Phase = reader.ReadNullableString();

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer
                .WriteNullable (Date)
                .WriteNullable (Responsible)
                .WriteNullable (Phase);

        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<Technology> (this, throwOnError);

            verifier
                .NotNullNorEmpty (Phase)
                .NotNullNorEmpty (Responsible)
                .NotNullNorEmpty (Date);

            return verifier.Result;

        } // method Verify

        #endregion


        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() =>
            $"{Phase.ToVisibleString()}: {Date.ToVisibleString()}: {Responsible.ToVisibleString()}";

        #endregion

    } // class Technology

} // namespace ManagedIrbis.Fields
