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

/* FulltextRecord.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Fulltext
{
    /// <summary>
    /// Запись во вложенной базе данных TEXT.
    /// </summary>
    public class FulltextRecord
    {
        #region Properties

        /// <summary>
        /// GUID.
        /// </summary>
        /// <remarks>
        /// See <see cref="IrbisGuid"/>.
        /// </remarks>
        [XmlAttribute("guid")]
        [JsonPropertyName("guid")]
        public string? Guid { get; set; }

        /// <summary>
        /// Число слов в тексте.
        /// Поле 20.
        /// </summary>
        [Field(20)]
        [XmlAttribute("wordCount")]
        [JsonPropertyName("wordCount")]
        public string? WordCount { get; set; }

        /// <summary>
        /// Индекс естественно-тематического рубрикатора.
        /// Поле 21.
        /// </summary>
        [Field(21)]
        [XmlAttribute("subject")]
        [JsonPropertyName("subject")]
        public string? Subject { get; set; }

        /// <summary>
        /// Первые строки текста.
        /// Поле 22.
        /// </summary>
        [Field(22)]
        [XmlAttribute("brief")]
        [JsonPropertyName("brief")]
        public string? BriefText { get; set; }

        /// <summary>
        /// Комментарий.
        /// </summary>
        [Field(23)]
        [XmlAttribute("remarks")]
        [JsonPropertyName("remarks")]
        public string? Remarks { get; set; }

        /// <summary>
        /// Дата ввода записи в базу данных.
        /// Поле 24.
        /// </summary>
        [Field(24)]
        [XmlAttribute("entryDate")]
        [JsonPropertyName("entryDate")]
        public string? EntryDate { get; set; }

        /// <summary>
        /// Размер файла полного текста в байтах.
        /// Поле 25.
        /// </summary>
        [Field(25)]
        [XmlAttribute("fileSize")]
        [JsonPropertyName("fileSize")]
        public string? FileSize { get; set; }

        /// <summary>
        /// Дата создания полного текста.
        /// Поле 26.
        /// </summary>
        [Field(26)]
        [XmlAttribute("fileDate")]
        [JsonPropertyName("fileDate")]
        public string? FileDate { get; set; }

        /// <summary>
        /// Строки текста.
        /// Поле 27.
        /// </summary>
        [Field(27)]
        [XmlElement("line")]
        [JsonPropertyName("lines")]
        public string[]? Lines { get; set; }

        /// <summary>
        /// Данные о переносе записи из ЭК.
        /// Поле 66.
        /// </summary>
        [Field(66)]
        [XmlAttribute("transfer")]
        [JsonPropertyName("transfer")]
        public string? Transfer { get; set; }

        /// <summary>
        /// Исходные данные из ЭК.
        /// Поле 951.
        /// </summary>
        [Field(951)]
        [XmlAttribute("initialData")]
        [JsonPropertyName("initialData")]
        public string? InitialData { get; set; }

        /// <summary>
        /// Ссылка на объект полнотекстового поиска.
        /// Поле 952.
        /// </summary>
        [Field(952)]
        [XmlElement("reference")]
        [JsonPropertyName("reference")]
        public FullTextReference? TextReference { get; set; }

        /// <summary>
        /// Ссылка на библиографическую запись.
        /// Поле 999.
        /// </summary>
        [Field(999)]
        [XmlAttribute("refGuid")]
        [JsonPropertyName("refGuid")]
        public string? ReferenceGuid { get; set; }

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
        public static FulltextRecord Parse
            (
                Record record
            )
        {
            // TODO: реализовать оптимально

            var result = new FulltextRecord
            {
                Guid = record.FM(IrbisGuid.Tag).ToString(),
                WordCount = record.FM(20).ToString(),
                Subject = record.FM(21).ToString(),
                BriefText = record.FM(22).ToString(),
                Remarks = record.FM(23).ToString(),
                EntryDate = record.FM(24).ToString(),
                FileSize = record.FM(25).ToString(),
                FileDate = record.FM(26).ToString(),
                Lines = record.FMA(27).Select(o=>o.ToString()).ToArray(),
                Transfer = record.FM(66).ToString(),
                InitialData = record.FM(951).ToString(),
                TextReference = FullTextReference.Parse(record.GetField(952)),
                ReferenceGuid = record.FM(999).ToString(),
                Record = record
            };

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => BriefText.ToVisibleString();

        #endregion
    }
}
