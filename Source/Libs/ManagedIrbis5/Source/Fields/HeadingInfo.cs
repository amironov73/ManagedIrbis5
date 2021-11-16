// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* HeadingInfo.cs -- предметная рубрика, поле 606
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
    /// Предметная рубрика, поле 606.
    /// </summary>
    [XmlRoot ("heading")]
    public sealed class HeadingInfo
        : IHandmadeSerializable,
            IVerifiable
    {
        #region Constants

        /// <summary>
        /// Tag.
        /// </summary>
        public const int Tag = 606;

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "abcdegho9";

        #endregion

        #region Properties

        /// <summary>
        /// Предметный заголовок. Подполе A.
        /// </summary>
        [SubField ('a')]
        [XmlElement ("title")]
        [JsonPropertyName ("title")]
        [Description ("Предметный заголовок")]
        [DisplayName ("Предметный заголовок")]
        public string? Title { get; set; }

        /// <summary>
        /// Первый подзаголовок. Подполе B.
        /// </summary>
        [SubField ('b')]
        [XmlElement ("subtitle1")]
        [JsonPropertyName ("subtitle1")]
        [Description ("Первый подзаголовок")]
        [DisplayName ("Первый подзаголовок")]
        public string? Subtitle1 { get; set; }

        /// <summary>
        /// Второй подзаголовок. Подполе C.
        /// </summary>
        [SubField ('c')]
        [XmlElement ("subtitle2")]
        [JsonPropertyName ("subtitle2")]
        [Description ("Второй подзаголовок")]
        [DisplayName ("Второй подзаголовок")]
        public string? Subtitle2 { get; set; }

        /// <summary>
        /// Третий подзаголовок. Подполе D.
        /// </summary>
        [SubField ('d')]
        [XmlElement ("subtitle3")]
        [JsonPropertyName ("subtitle3")]
        [Description ("Третий подзаголовок")]
        [DisplayName ("Третий подзаголовок")]
        public string? Subtitle3 { get; set; }

        /// <summary>
        /// Географический подзаголовок. Подполе G.
        /// </summary>
        [SubField ('g')]
        [XmlElement ("geoSubtitle1")]
        [JsonPropertyName ("geoSubtitle1")]
        [Description ("Первый географический подзаголовок")]
        [DisplayName ("Первый географический подзаголовок")]
        public string? GeographicalSubtitle1 { get; set; }

        /// <summary>
        /// Географический подзаголовок. Подполе E.
        /// </summary>
        [SubField ('e')]
        [XmlElement ("geoSubtitle2")]
        [JsonPropertyName ("geoSubtitle2")]
        [Description ("Второй географический подзаголовок")]
        [DisplayName ("Второй географический подзаголовок")]
        public string? GeographicalSubtitle2 { get; set; }

        /// <summary>
        /// Географический подзаголовок. Подполе O.
        /// </summary>
        [SubField ('o')]
        [XmlElement ("geoSubtitle3")]
        [JsonPropertyName ("geoSubtitle3")]
        [Description ("Третий географический подзаголовок")]
        [DisplayName ("Третий географический подзаголовок")]
        public string? GeographicalSubtitle3 { get; set; }

        /// <summary>
        /// Хронологический подзаголовок. Подполе H.
        /// </summary>
        [SubField ('h')]
        [XmlElement ("chronoSubtitle")]
        [JsonPropertyName ("chronoSubtitle")]
        [Description ("Хронологический подзаголовок")]
        [DisplayName ("Хронологический подзаголовок")]
        public string? ChronologicalSubtitle { get; set; }

        /// <summary>
        /// Формальный подзаголовок (аспект). Подполе 9.
        /// </summary>
        [SubField ('9')]
        [XmlElement ("aspect")]
        [JsonPropertyName ("aspect")]
        [Description ("Формальный подзаголовок (аспект)")]
        [DisplayName ("Формальный подзаголовок (аспект)")]
        public string? Aspect { get; set; }

        /// <summary>
        /// Неизвестные подполя.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public SubField[]? UnknownSubFields { get; set; }

        /// <summary>
        /// Ассоциированное поле библиографической записи.
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
        public Field ApplyToField
            (
                Field field
            )
        {
            Sure.NotNull (field);

            return field
                .SetSubFieldValue ('a', Title)
                .SetSubFieldValue ('b', Subtitle1)
                .SetSubFieldValue ('c', Subtitle2)
                .SetSubFieldValue ('d', Subtitle3)
                .SetSubFieldValue ('g', GeographicalSubtitle1)
                .SetSubFieldValue ('e', GeographicalSubtitle2)
                .SetSubFieldValue ('o', GeographicalSubtitle3)
                .SetSubFieldValue ('h', ChronologicalSubtitle)
                .SetSubFieldValue ('9', Aspect);
        }

        /// <summary>
        /// Разбор указанного поля библиографической записи.
        /// </summary>
        public static HeadingInfo ParseField
            (
                Field field
            )
        {
            Sure.NotNull (field);

            return new ()
            {
                Title = field.GetFirstSubFieldValue ('a'),
                Subtitle1 = field.GetFirstSubFieldValue ('b'),
                Subtitle2 = field.GetFirstSubFieldValue ('c'),
                Subtitle3 = field.GetFirstSubFieldValue ('d'),
                GeographicalSubtitle1 = field.GetFirstSubFieldValue ('g'),
                GeographicalSubtitle2 = field.GetFirstSubFieldValue ('e'),
                GeographicalSubtitle3 = field.GetFirstSubFieldValue ('o'),
                ChronologicalSubtitle = field.GetFirstSubFieldValue ('h'),
                Aspect = field.GetFirstSubFieldValue ('9'),
                UnknownSubFields = field.Subfields.GetUnknownSubFields (KnownCodes),
                Field = field
            };
        }

        /// <summary>
        /// Разбор библиографической записи <see cref="Record"/>.
        /// </summary>
        public static HeadingInfo[] ParseRecord
            (
                Record record,
                int tag = Tag
            )
        {
            Sure.NotNull (record);
            Sure.Positive (tag);

            var result = new List<HeadingInfo>();
            foreach (var field in record.Fields)
            {
                if (field.Tag == tag)
                {
                    var heading = ParseField (field);
                    result.Add (heading);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Преобразование данных в поле библиографической записи. <see cref="Field"/>.
        /// </summary>
        public Field ToField()
        {
            return new Field (Tag)
                .AddNonEmpty ('a', Title)
                .AddNonEmpty ('b', Subtitle1)
                .AddNonEmpty ('c', Subtitle2)
                .AddNonEmpty ('d', Subtitle3)
                .AddNonEmpty ('g', GeographicalSubtitle1)
                .AddNonEmpty ('e', GeographicalSubtitle2)
                .AddNonEmpty ('o', GeographicalSubtitle3)
                .AddNonEmpty ('h', ChronologicalSubtitle)
                .AddNonEmpty ('9', Aspect)
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

            Title = reader.ReadNullableString();
            Subtitle1 = reader.ReadNullableString();
            Subtitle2 = reader.ReadNullableString();
            Subtitle3 = reader.ReadNullableString();
            GeographicalSubtitle1 = reader.ReadNullableString();
            GeographicalSubtitle2 = reader.ReadNullableString();
            GeographicalSubtitle3 = reader.ReadNullableString();
            ChronologicalSubtitle = reader.ReadNullableString();
            Aspect = reader.ReadNullableString();
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
                .WriteNullable (Title)
                .WriteNullable (Subtitle1)
                .WriteNullable (Subtitle2)
                .WriteNullable (Subtitle3)
                .WriteNullable (GeographicalSubtitle1)
                .WriteNullable (GeographicalSubtitle2)
                .WriteNullable (GeographicalSubtitle3)
                .WriteNullable (ChronologicalSubtitle)
                .WriteNullable (Aspect)
                .WriteNullableArray (UnknownSubFields);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<HeadingInfo> (this, throwOnError);

            verifier
                .NotNullNorEmpty (Title);

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
                    Title,
                    Subtitle1,
                    Subtitle2,
                    Subtitle3,
                    GeographicalSubtitle1,
                    GeographicalSubtitle2,
                    GeographicalSubtitle3,
                    ChronologicalSubtitle,
                    Aspect
                )
                .EmptyToNull()
                .ToVisibleString();

        }

        #endregion

    } // class HeadingInfo

} // namespace ManagedIrbis.Fields
