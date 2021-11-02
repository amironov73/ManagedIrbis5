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
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Язык документа (дополнительные данные). Поле 919.
    /// </summary>
    public sealed class LanguageInfo
    {
        #region Constants

        /// <summary>
        /// Known codes.
        /// </summary>
        public const string KnownCodes = "abefgklnoz";

        /// <summary>
        /// Tag.
        /// </summary>
        public const int Tag = 919;

        #endregion

        #region Properties

        /// <summary>
        /// Язык каталогизации. Подполе a.
        /// </summary>
        [SubField('a')]
        [XmlElement("catalogingLanguage")]
        [JsonPropertyName("catalogingLanguage")]
        [Description("Язык каталогизации")]
        [DisplayName("Язык каталогизации")]
        public string? CatalogingLanguage { get; set; }

        /// <summary>
        /// Правила каталогизации. Подполе k.
        /// </summary>
        /// <remarks>See
        /// <see cref="ManagedIrbis.Fields.CatalogingRules"/> class.
        /// </remarks>
        [SubField('k')]
        [XmlElement("cataloguingRules")]
        [JsonPropertyName("cataloguingRules")]
        [Description("Правила каталогизации")]
        [DisplayName("Правила каталогизации")]
        public string? CatalogingRules { get; set; }

        /// <summary>
        /// Набор символов. Подполе n.
        /// </summary>
        /// <remarks>See <see cref="CharacterSetCode"/> class.
        /// </remarks>
        [SubField('n')]
        [XmlElement("characterSet")]
        [JsonPropertyName("characterSet")]
        [Description("Набор символов")]
        [DisplayName("Набор символов")]
        public string? CharacterSet { get; set; }

        /// <summary>
        /// Графика заглавия. Подполе g.
        /// </summary>
        /// <remarks>See <see cref="ManagedIrbis.Fields.TitleCharacterSet"/> class.
        /// </remarks>
        [SubField('g')]
        [XmlElement("titleCharacterSet")]
        [JsonPropertyName("titleCharacterSet")]
        [Description("Графика заглавия")]
        [DisplayName("Графика заглавия")]
        public string? TitleCharacterSet { get; set; }

        /// <summary>
        /// Язык промежуточного перевода. Подполе b.
        /// </summary>
        /// <remarks>See <see cref="LanguageCode"/> class.
        /// </remarks>
        [SubField('b')]
        [XmlElement("intermediateTranslationLanguage")]
        [JsonPropertyName("intermediateTranslationLanguage")]
        [Description("Язык промежуточного перевода")]
        [DisplayName("Язык промежуточного перевода")]
        public string? IntermediateTranslationLanguage { get; set; }

        /// <summary>
        /// Язык оригинала. Подполе o.
        /// </summary>
        [SubField('o')]
        [XmlElement("originalLanguage")]
        [JsonPropertyName("originalLanguage")]
        [Description("Язык оригинала")]
        [DisplayName("Язык оригинала")]
        public string? OriginalLanguage { get; set; }

        /// <summary>
        /// Язык оглавления. Подполе e.
        /// </summary>
        [SubField('e')]
        [XmlElement("tocLanguage")]
        [JsonPropertyName("tocLanguage")]
        [Description("Язык оглавления")]
        [DisplayName("Язык оглавления")]
        public string? TocLanguage { get; set; }

        /// <summary>
        /// Язык титульного листа. Подполе f.
        /// </summary>
        [SubField('f')]
        [XmlElement("titlePageLanguage")]
        [JsonPropertyName("titlePageLanguage")]
        [Description("Язык титульного листа")]
        [DisplayName("Язык титульного листа")]
        public string? TitlePageLanguage { get; set; }

        /// <summary>
        /// Язык основного заглавия. Подполе z.
        /// </summary>
        [SubField('z')]
        [XmlElement("mainTitleLanguage")]
        [JsonPropertyName("mainTitleLanguage")]
        [Description("Язык основного заглавия")]
        [DisplayName("Язык основного заглавия")]
        public string? MainTitleLanguage { get; set; }

        /// <summary>
        /// Язык сопроводительного материала. Подполе i.
        /// </summary>
        [SubField('i')]
        [XmlElement("accompanyingMaterialLanguage")]
        [JsonPropertyName("accompanyingMaterialLanguage")]
        [Description("Язык сопроводительного материала")]
        [DisplayName("Язык сопроводительного материала")]
        public string? AccompanyingMaterialLanguage { get; set; }

        /// <summary>
        /// Associated field.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [DisplayName("Поле с подполями")]
        public Field? Field { get; private set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Apply the <see cref="LanguageInfo"/>
        /// to the <see cref="Field"/>.
        /// </summary>
        public Field ApplyToField (Field field) => field
            .SetSubFieldValue ('a', CatalogingLanguage)
            .SetSubFieldValue ('k', CatalogingRules)
            .SetSubFieldValue ('n', CharacterSet)
            .SetSubFieldValue ('g', TitleCharacterSet)
            .SetSubFieldValue ('b', IntermediateTranslationLanguage)
            .SetSubFieldValue ('e', OriginalLanguage)
            .SetSubFieldValue ('o', TocLanguage)
            .SetSubFieldValue ('f', TitlePageLanguage)
            .SetSubFieldValue ('z', MainTitleLanguage)
            .SetSubFieldValue ('i', AccompanyingMaterialLanguage);

        /// <summary>
        /// Parse the field.
        /// </summary>
        public static LanguageInfo Parse
            (
                Field field
            )
        {
            var result = new LanguageInfo
            {
                CatalogingLanguage = field.GetFirstSubFieldValue('a'),
                CatalogingRules = field.GetFirstSubFieldValue('k'),
                CharacterSet = field.GetFirstSubFieldValue('n'),
                TitleCharacterSet = field.GetFirstSubFieldValue('g'),
                IntermediateTranslationLanguage = field.GetFirstSubFieldValue('b'),
                OriginalLanguage = field.GetFirstSubFieldValue('e'),
                TocLanguage = field.GetFirstSubFieldValue('o'),
                TitlePageLanguage = field.GetFirstSubFieldValue('f'),
                MainTitleLanguage = field.GetFirstSubFieldValue('z'),
                AccompanyingMaterialLanguage = field.GetFirstSubFieldValue('i'),
                Field = field
            };

            return result;

        } // method Parse

        /// <summary>
        /// Parse the <see cref="Record"/>.
        /// </summary>
        public static LanguageInfo[] Parse
            (
                Record record,
                int tag = Tag
            )
        {
            var result = new List<LanguageInfo>();
            foreach (var field in record.Fields)
            {
                if (field.Tag == tag)
                {
                    var info = Parse(field);
                    result.Add(info);
                }
            }

            return result.ToArray();

        } // method Parse

        /// <summary>
        /// Convert <see cref="LanguageInfo"/>
        /// to <see cref="Field"/>.
        /// </summary>
        public Field ToField()
        {
            var result = new Field(Tag);
            result
                .AddNonEmpty ('a', CatalogingLanguage)
                .AddNonEmpty ('k', CatalogingRules)
                .AddNonEmpty ('n', CharacterSet)
                .AddNonEmpty ('g', TitleCharacterSet)
                .AddNonEmpty ('b', IntermediateTranslationLanguage)
                .AddNonEmpty ('e', OriginalLanguage)
                .AddNonEmpty ('o', TocLanguage)
                .AddNonEmpty ('f', TitlePageLanguage)
                .AddNonEmpty ('z', MainTitleLanguage)
                .AddNonEmpty ('i', AccompanyingMaterialLanguage);

            return result;

        } // method ToField

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => CatalogingLanguage.ToVisibleString();

        #endregion

    } // class LanguageInfo

} // namespace ManagedIrbis.Fields
