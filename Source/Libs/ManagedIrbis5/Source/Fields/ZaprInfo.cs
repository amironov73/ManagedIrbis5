// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

/* ZaprInfo.cs -- информация о постоянном запросе, поле 2
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

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Информация о постоянном запросе в базе данных ZAPR.
    /// Поле 2.
    /// </summary>
    [XmlRoot ("zapr")]
    public sealed class ZaprInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Метка поля.
        /// </summary>
        public const int Tag = 2;

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "ab";

        #endregion

        #region Properties

        /// <summary>
        /// Формулировка запроса на естественном языке.
        /// Подполе A.
        /// </summary>
        [SubField ('a')]
        [XmlElement ("natural")]
        [JsonPropertyName ("natual")]
        [Description ("Формулировка запроса на естественном языке")]
        [DisplayName ("Формулировка запроса на естественном языке")]
        public string? NaturalLanguage { get; set; }

        /// <summary>
        /// Полнотекстовая часть запроса.
        /// Подполе B.
        /// </summary>
        [SubField ('b')]
        [XmlElement ("fulltext")]
        [JsonPropertyName ("fulltext")]
        [Description ("Полнотекстовая часть запроса")]
        [DisplayName ("Полнотекстовая часть запроса")]
        public string? FullTextQuery { get; set; }

        /// <summary>
        /// Библиографическая часть запроса.
        /// Подполе C.
        /// </summary>
        [SubField ('c')]
        [XmlElement ("search")]
        [JsonPropertyName ("search")]
        [Description ("Библиографическая часть запроса")]
        [DisplayName ("Библиографическая часть запроса")]
        public string? SearchQuery { get; set; }

        /// <summary>
        /// Дата создания запроса.
        /// Подполе D.
        /// </summary>
        [SubField ('d')]
        [XmlElement ("date")]
        [JsonPropertyName ("date")]
        [Description ("Дата создания запроса")]
        [DisplayName ("Дата создания запроса")]
        public string? Date { get; set; } = IrbisDate.TodayText;

        /// <summary>
        /// Неизвестные подполя.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public SubField[]? UnknownSubFields { get; set; }

        /// <summary>
        /// Связанное поле в нераскодированном виде.
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
        /// Применение данных к указанному полю библиографической записи <see cref="Field"/>.
        /// </summary>
        public Field ApplyTo
            (
                Field field
            )
        {
            Sure.NotNull (field);

            return field
                .SetSubFieldValue ('a', NaturalLanguage)
                .SetSubFieldValue ('b', FullTextQuery)
                .SetSubFieldValue ('c', SearchQuery)
                .SetSubFieldValue ('d', Date);
        }

        /// <summary>
        /// Разбор указанного поля библиографической записи.
        /// </summary>
        public static ZaprInfo ParseField
            (
                Field field
            )
        {
            Sure.NotNull (field);

            var result = new ZaprInfo
            {
                NaturalLanguage = field.GetFirstSubFieldValue ('a'),
                FullTextQuery = field.GetFirstSubFieldValue ('b'),
                SearchQuery = field.GetFirstSubFieldValue ('c'),
                Date = field.GetFirstSubFieldValue ('d'),
                UnknownSubFields = field.Subfields.GetUnknownSubFields (KnownCodes),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Разбор библиографической записи.
        /// </summary>
        public static ZaprInfo[] ParseRecord
            (
                Record record,
                int tag = Tag
            )
        {
            Sure.NotNull (record);
            Sure.Positive (tag);

            var result = new List<ZaprInfo>();
            foreach (var field in record.Fields)
            {
                if (field.Tag == tag)
                {
                    var one = ParseField (field);
                    result.Add (one);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Преобразование данных в поле библиографической записи <see cref="Field"/>.
        /// </summary>
        public Field ToField()
        {
            return new Field (Tag)
                .AddNonEmpty ('a', NaturalLanguage)
                .AddNonEmpty ('b', FullTextQuery)
                .AddNonEmpty ('c', SearchQuery)
                .AddNonEmpty ('d', Date)
                .AddRange (UnknownSubFields);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            NaturalLanguage = reader.ReadNullableString();
            FullTextQuery = reader.ReadNullableString();
            SearchQuery = reader.ReadNullableString();
            Date = reader.ReadNullableString();
            UnknownSubFields = reader.ReadNullableArray<SubField>();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer
                .WriteNullable (NaturalLanguage)
                .WriteNullable (FullTextQuery)
                .WriteNullable (SearchQuery)
                .WriteNullable (Date)
                .WriteNullableArray (UnknownSubFields);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<ZaprInfo>(this, throwOnError);

            verifier
                .AnyNotNullNorEmpty (NaturalLanguage, FullTextQuery, SearchQuery);

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Utility.JoinNonEmpty
                (
                    " -- ",
                    NaturalLanguage,
                    FullTextQuery,
                    SearchQuery,
                    Date
                )
                .EmptyToNull()
                .ToVisibleString();
        }

        #endregion

    } // class ZaprInfo

} // namespace ManagedIrbis.Fields
