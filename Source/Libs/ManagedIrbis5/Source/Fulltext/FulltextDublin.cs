// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* FulltextDublin.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Fulltext
{
    /// <summary>
    /// Dublin Core для полнотекстовой записи.
    /// </summary>
    public class FulltextDublin
    {
        #region Properties

        /// <summary>
        /// Заглавие.
        /// Поле 1.
        /// </summary>
        [Field(1)]
        [XmlAttribute("title")]
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        /// <summary>
        /// Создатель (автор).
        /// Поле 2.
        /// </summary>
        [Field(2)]
        [XmlAttribute("creator")]
        [JsonPropertyName("creator")]
        public string? Creator { get; set; }

        /// <summary>
        /// Тема.
        /// Поле 3.
        /// </summary>
        [Field(3)]
        [XmlAttribute("subject")]
        [JsonPropertyName("subject")]
        public string? Subject { get; set; }

        /// <summary>
        /// Описание.
        /// Поле 4.
        /// </summary>
        [Field(4)]
        [XmlAttribute("description")]
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Издатель.
        /// Поле 5.
        /// </summary>
        [Field(5)]
        [XmlAttribute("publisher")]
        [JsonPropertyName("publisher")]
        public string? Publisher { get; set; }

        /// <summary>
        /// Внесший вклад.
        /// Поле 6.
        /// </summary>
        [Field(6)]
        [XmlAttribute("contributor")]
        [JsonPropertyName("contributor")]
        public string? Contributor { get; set; }

        /// <summary>
        /// Дата.
        /// Поле 7.
        /// </summary>
        [Field(7)]
        [XmlAttribute("date")]
        [JsonPropertyName("date")]
        public string? Date { get; set; }

        /// <summary>
        /// Тип.
        /// Поле 8.
        /// </summary>
        [Field(8)]
        [XmlAttribute("type")]
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Формат документа.
        /// Поле 9.
        /// </summary>
        [Field(9)]
        [XmlAttribute("format")]
        [JsonPropertyName("format")]
        public string? Format { get; set; }

        /// <summary>
        /// Идентификатор.
        /// Поле 10.
        /// </summary>
        [Field(10)]
        [XmlAttribute("identifier")]
        [JsonPropertyName("identifier")]
        public string? Identifier { get; set; }

        /// <summary>
        /// Источник.
        /// Поле 11.
        /// </summary>
        [Field(11)]
        [XmlAttribute("source")]
        [JsonPropertyName("source")]
        public string? Source { get; set; }

        /// <summary>
        /// Язык.
        /// Поле 12.
        /// </summary>
        [Field(12)]
        [XmlAttribute("language")]
        [JsonPropertyName("language")]
        public string? Language { get; set; }

        /// <summary>
        /// Отношения.
        /// Поле 13.
        /// </summary>
        [Field(13)]
        [XmlAttribute("relation")]
        [JsonPropertyName("relation")]
        public string? Relation { get; set; }

        /// <summary>
        /// Покрытие
        /// Поле 14.
        /// </summary>
        [Field(14)]
        [XmlAttribute("coverage")]
        [JsonPropertyName("coverage")]
        public string? Coverage { get; set; }

        /// <summary>
        /// Авторские права.
        /// Поле 15.
        /// </summary>
        [Field(15)]
        [XmlAttribute("rights")]
        [JsonPropertyName("rights")]
        public string? Rights { get; set; }

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
        /// Parse the <see cref="Record"/>.
        /// </summary>
        public static FulltextDublin Parse
            (
                Record record
            )
        {
            // TODO: реализовать оптимально

            var result = new FulltextDublin
            {
                Title = record.FM(1),
                Creator = record.FM(2),
                Subject = record.FM(3),
                Description = record.FM(4),
                Publisher = record.FM(5),
                Contributor = record.FM(6),
                Date = record.FM(7),
                Type = record.FM(8),
                Format = record.FM(9),
                Identifier = record.FM(10),
                Source = record.FM(11),
                Language = record.FM(12),
                Relation = record.FM(13),
                Coverage = record.FM(14),
                Rights = record.FM(15),
                Record = record
            };

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => Title.ToVisibleString();

        #endregion
    }
}
