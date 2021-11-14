// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global

/* LanguageInfo.cs -- вид документа (дополнительные данные), поле 919
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
    /// Язык документа (дополнительные данные). Поле 919.
    /// </summary>
    [XmlRoot ("language-info")]
    public sealed class LanguageInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Метка поля.
        /// </summary>
        public const int Tag = 919;

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "abefgklnoz";

        #endregion

        #region Properties

        /// <summary>
        /// Язык каталогизации. Подполе A.
        /// </summary>
        [SubField ('a')]
        [XmlElement ("catalogingLanguage")]
        [JsonPropertyName ("catalogingLanguage")]
        [Description ("Язык каталогизации")]
        [DisplayName ("Язык каталогизации")]
        public string? CatalogingLanguage { get; set; }

        /// <summary>
        /// Правила каталогизации. Подполе K.
        /// </summary>
        /// <remarks>See
        /// <see cref="ManagedIrbis.Fields.CatalogingRules"/> class.
        /// </remarks>
        [SubField ('k')]
        [XmlElement ("cataloguingRules")]
        [JsonPropertyName ("cataloguingRules")]
        [Description ("Правила каталогизации")]
        [DisplayName ("Правила каталогизации")]
        public string? CatalogingRules { get; set; }

        /// <summary>
        /// Набор символов. Подполе N.
        /// </summary>
        /// <remarks>See <see cref="CharacterSetCode"/> class.
        /// </remarks>
        [SubField ('n')]
        [XmlElement ("characterSet")]
        [JsonPropertyName ("characterSet")]
        [Description ("Набор символов")]
        [DisplayName ("Набор символов")]
        public string? CharacterSet { get; set; }

        /// <summary>
        /// Графика заглавия. Подполе G.
        /// </summary>
        /// <remarks>See <see cref="ManagedIrbis.Fields.TitleCharacterSet"/> class.
        /// </remarks>
        [SubField ('g')]
        [XmlElement ("titleCharacterSet")]
        [JsonPropertyName ("titleCharacterSet")]
        [Description ("Графика заглавия")]
        [DisplayName ("Графика заглавия")]
        public string? TitleCharacterSet { get; set; }

        /// <summary>
        /// Язык промежуточного перевода. Подполе B.
        /// </summary>
        /// <remarks>See <see cref="LanguageCode"/> class.
        /// </remarks>
        [SubField ('b')]
        [XmlElement ("intermediateTranslationLanguage")]
        [JsonPropertyName ("intermediateTranslationLanguage")]
        [Description ("Язык промежуточного перевода")]
        [DisplayName ("Язык промежуточного перевода")]
        public string? IntermediateTranslationLanguage { get; set; }

        /// <summary>
        /// Язык оригинала. Подполе O.
        /// </summary>
        [SubField ('o')]
        [XmlElement ("originalLanguage")]
        [JsonPropertyName ("originalLanguage")]
        [Description ("Язык оригинала")]
        [DisplayName ("Язык оригинала")]
        public string? OriginalLanguage { get; set; }

        /// <summary>
        /// Язык оглавления. Подполе E.
        /// </summary>
        [SubField ('e')]
        [XmlElement ("tocLanguage")]
        [JsonPropertyName ("tocLanguage")]
        [Description ("Язык оглавления")]
        [DisplayName ("Язык оглавления")]
        public string? TocLanguage { get; set; }

        /// <summary>
        /// Язык титульного листа. Подполе F.
        /// </summary>
        [SubField ('f')]
        [XmlElement ("titlePageLanguage")]
        [JsonPropertyName ("titlePageLanguage")]
        [Description ("Язык титульного листа")]
        [DisplayName ("Язык титульного листа")]
        public string? TitlePageLanguage { get; set; }

        /// <summary>
        /// Язык основного заглавия. Подполе Z.
        /// </summary>
        [SubField ('z')]
        [XmlElement ("mainTitleLanguage")]
        [JsonPropertyName ("mainTitleLanguage")]
        [Description ("Язык основного заглавия")]
        [DisplayName ("Язык основного заглавия")]
        public string? MainTitleLanguage { get; set; }

        /// <summary>
        /// Язык сопроводительного материала. Подполе I.
        /// </summary>
        [SubField ('i')]
        [XmlElement ("accompanyingMaterialLanguage")]
        [JsonPropertyName ("accompanyingMaterialLanguage")]
        [Description ("Язык сопроводительного материала")]
        [DisplayName ("Язык сопроводительного материала")]
        public string? AccompanyingMaterialLanguage { get; set; }

        /// <summary>
        /// Неизвестные подполя.
        /// </summary>
        [XmlElement ("unknown")]
        [JsonPropertyName ("unknown")]
        [Browsable (false)]
        public SubField[]? UnknownSubFields { get; set; }

        /// <summary>
        /// Ассоциированное поле библиографической записи <see cref="Field"/>.
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
        /// Применение данных к указаннному полю библиографической записи <see cref="Field"/>.
        /// </summary>
        public Field ApplyToField (Field field) => field
            .SetSubFieldValue ('a', CatalogingLanguage)
            .SetSubFieldValue ('k', CatalogingRules)
            .SetSubFieldValue ('n', CharacterSet)
            .SetSubFieldValue ('g', TitleCharacterSet)
            .SetSubFieldValue ('b', IntermediateTranslationLanguage)
            .SetSubFieldValue ('o', OriginalLanguage)
            .SetSubFieldValue ('e', TocLanguage)
            .SetSubFieldValue ('f', TitlePageLanguage)
            .SetSubFieldValue ('z', MainTitleLanguage)
            .SetSubFieldValue ('i', AccompanyingMaterialLanguage);

        /// <summary>
        /// Разбор указанного поля библиографической записи.
        /// </summary>
        public static LanguageInfo ParseField
            (
                Field field
            )
        {
            Sure.NotNull (field);

            var result = new LanguageInfo
            {
                CatalogingLanguage = field.GetFirstSubFieldValue ('a'),
                CatalogingRules = field.GetFirstSubFieldValue ('k'),
                CharacterSet = field.GetFirstSubFieldValue ('n'),
                TitleCharacterSet = field.GetFirstSubFieldValue ('g'),
                IntermediateTranslationLanguage = field.GetFirstSubFieldValue ('b'),
                OriginalLanguage = field.GetFirstSubFieldValue ('o'),
                TocLanguage = field.GetFirstSubFieldValue ('e'),
                TitlePageLanguage = field.GetFirstSubFieldValue ('f'),
                MainTitleLanguage = field.GetFirstSubFieldValue ('z'),
                AccompanyingMaterialLanguage = field.GetFirstSubFieldValue ('i'),
                UnknownSubFields = field.Subfields.GetUnknownSubFields (KnownCodes),
                Field = field
            };

            return result;

        } // method Parse

        /// <summary>
        /// Разбор указанной библиографической записи <see cref="Record"/>.
        /// </summary>
        public static LanguageInfo[] ParseRecord
            (
                Record record,
                int tag = Tag
            )
        {
            Sure.NotNull (record);
            Sure.Positive (tag);

            var result = new List<LanguageInfo>();
            foreach (var field in record.Fields)
            {
                if (field.Tag == tag)
                {
                    var info = ParseField (field);
                    result.Add (info);
                }
            }

            return result.ToArray();

        } // method Parse

        /// <summary>
        /// Преобразование данных в поле библиографической записи <see cref="Field"/>.
        /// </summary>
        public Field ToField() => new Field (Tag)
                .AddNonEmpty ('a', CatalogingLanguage)
                .AddNonEmpty ('k', CatalogingRules)
                .AddNonEmpty ('n', CharacterSet)
                .AddNonEmpty ('g', TitleCharacterSet)
                .AddNonEmpty ('b', IntermediateTranslationLanguage)
                .AddNonEmpty ('o', OriginalLanguage)
                .AddNonEmpty ('e', TocLanguage)
                .AddNonEmpty ('f', TitlePageLanguage)
                .AddNonEmpty ('z', MainTitleLanguage)
                .AddNonEmpty ('i', AccompanyingMaterialLanguage);

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            CatalogingLanguage = reader.ReadNullableString();
            CatalogingRules = reader.ReadNullableString();
            CharacterSet = reader.ReadNullableString();
            TitleCharacterSet = reader.ReadNullableString();
            IntermediateTranslationLanguage = reader.ReadNullableString();
            OriginalLanguage = reader.ReadNullableString();
            TocLanguage = reader.ReadNullableString();
            TitlePageLanguage = reader.ReadNullableString();
            MainTitleLanguage = reader.ReadNullableString();
            AccompanyingMaterialLanguage = reader.ReadNullableString();
            UnknownSubFields = reader.ReadNullableArray<SubField>();

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer
                .WriteNullable (CatalogingLanguage)
                .WriteNullable (CatalogingRules)
                .WriteNullable (CharacterSet)
                .WriteNullable (TitleCharacterSet)
                .WriteNullable (IntermediateTranslationLanguage)
                .WriteNullable (OriginalLanguage)
                .WriteNullable (TocLanguage)
                .WriteNullable (TitlePageLanguage)
                .WriteNullable (MainTitleLanguage)
                .WriteNullable (AccompanyingMaterialLanguage)
                .WriteNullableArray (UnknownSubFields);

        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<LanguageInfo> (this, throwOnError);

            verifier.Assert
                (
                    !string.IsNullOrEmpty (CatalogingLanguage)
                    || !string.IsNullOrEmpty (CatalogingRules)
                    || !string.IsNullOrEmpty (CharacterSet)
                    || !string.IsNullOrEmpty (TitleCharacterSet)
                    || !string.IsNullOrEmpty (IntermediateTranslationLanguage)
                    || !string.IsNullOrEmpty (OriginalLanguage)
                    || !string.IsNullOrEmpty (TocLanguage)
                    || !string.IsNullOrEmpty (TitlePageLanguage)
                    || !string.IsNullOrEmpty (MainTitleLanguage)
                    || !string.IsNullOrEmpty (AccompanyingMaterialLanguage)
                );

            return verifier.Result;

        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => CatalogingLanguage.ToVisibleString();

        #endregion

    } // class LanguageInfo

} // namespace ManagedIrbis.Fields
