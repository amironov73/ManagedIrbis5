// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* CommonInfo.cs -- общая информация о многотомнике
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
    /// Общая информация о многотомнике. Поля 461 и 46.
    /// </summary>
    public sealed class CommonInfo
    {
        #region Constants

        /// <summary>
        ///
        /// </summary>
        public const int MainTag = 461;

        /// <summary>
        ///
        /// </summary>
        public const int AdditionalTag = 46;

        #endregion

        #region Properties

        /// <summary>
        /// Заглавие. 461^c.
        /// </summary>
        [Field(461, 'c')]
        [XmlElement("title")]
        [JsonPropertyName("title")]
        [Description("Заглавие")]
        [DisplayName("Заглавие")]
        public string? Title { get; set; }

        /// <summary>
        /// Роль (Нехарактерное заглавие, доб.карточка?). 461^u.
        /// </summary>
        [Field(461, 'u')]
        [XmlElement("specific")]
        [JsonPropertyName("specific")]
        [Description("Роль (Нехарактерное заглавие, доб.карточка?)")]
        [DisplayName("Роль (Нехарактерное заглавие, доб.карточка?)")]
        public string? Specific { get; set; }

        /// <summary>
        /// Общее обозначение материала. 461^2.
        /// </summary>
        [Field(461, '2')]
        [XmlElement("general")]
        [JsonPropertyName("general")]
        [Description("Общее обозначение материала")]
        [DisplayName("Общее обозначение материала")]
        public string? General { get; set; }

        /// <summary>
        /// Сведения, относящиеся к заглавию. 461^e.
        /// </summary>
        [Field(461, 'e')]
        [XmlElement("subtitle")]
        [JsonPropertyName("subtitle")]
        [Description("Сведения, относящиеся к заглавию")]
        [DisplayName("Сведения, относящиеся к заглавию")]
        public string? Subtitle { get; set; }

        /// <summary>
        /// Сведения об ответственности. 461^f.
        /// </summary>
        [Field(461, 'f')]
        [XmlElement("responsibility")]
        [JsonPropertyName("responsibility")]
        [Description("Сведения об ответственности")]
        [DisplayName("Сведения об ответственности")]
        public string? Responsibility { get; set; }

        /// <summary>
        /// Издательство. 461^g.
        /// </summary>
        [Field(461, 'g')]
        [XmlElement("publisher")]
        [JsonPropertyName("publisher")]
        [Description("Издательство")]
        [DisplayName("Издательство")]
        public string? Publisher { get; set; }

        /// <summary>
        /// Город. 461^d.
        /// </summary>
        [Field(461, 'd')]
        [XmlElement("city")]
        [JsonPropertyName("city")]
        [Description("Город")]
        [DisplayName("Город")]
        public string? City { get; set; }

        /// <summary>
        /// Год начала издания. 461^h.
        /// </summary>
        [Field(461, 'h')]
        [XmlElement("beginningYear")]
        [JsonPropertyName("beginningYear")]
        [Description("Год начала издания")]
        [DisplayName("Год начала издания")]
        public string? BeginningYear { get; set; }

        /// <summary>
        /// Год окончания издания. 461^z.
        /// </summary>
        [Field(461, 'z')]
        [XmlElement("endingYear")]
        [JsonPropertyName("endingYear")]
        [Description("Год окончания издания")]
        [DisplayName("Год окончания издания")]
        public string? EndingYear { get; set; }

        /// <summary>
        /// ISBN. 461^i.
        /// </summary>
        [Field(461, 'i')]
        [XmlElement("isbn")]
        [JsonPropertyName("isbn")]
        [Description("ISBN")]
        [DisplayName("ISBN")]
        public string? Isbn { get; set; }

        /// <summary>
        /// ISSN. 461^j.
        /// </summary>
        [Field(461, 'j')]
        [XmlElement("issn")]
        [JsonPropertyName("issn")]
        [Description("ISSN")]
        [DisplayName("ISSN")]
        public string? Issn { get; set; }

        /// <summary>
        /// Сведения об издании. 461^p.
        /// </summary>
        [Field(461, 'p')]
        [XmlElement("reprint")]
        [JsonPropertyName("reprint")]
        [Description("Сведения об издании")]
        [DisplayName("Сведения об издании")]
        public string? Reprint { get; set; }

        /// <summary>
        /// Перевод заглавия. 461^a.
        /// </summary>
        [Field(461, 'a')]
        [XmlElement("translation")]
        [JsonPropertyName("translation")]
        [Description("Перевод заглавия")]
        [DisplayName("Перевод заглавия")]
        public string? Translation { get; set; }

        /// <summary>
        /// 1-й автор - Заголовок описания. 461^x.
        /// </summary>
        [Field(461, 'x')]
        [XmlElement("firstAuthor")]
        [JsonPropertyName("firstAuthor")]
        [Description("1-й автор - Заголовок описания")]
        [DisplayName("1-й автор - Заголовок описания")]
        public string? FirstAuthor { get; set; }

        /// <summary>
        /// Коллектив или меропритие - Заголовок описания. 461^b.
        /// </summary>
        [Field(461, 'b')]
        [XmlElement("collective")]
        [JsonPropertyName("collective")]
        [Description("Коллектив или меропритие - Заголовок описания")]
        [DisplayName("Коллектив или меропритие - Заголовок описания")]
        public string? Collective { get; set; }

        /// <summary>
        /// Вариант заглавия. 46^r.
        /// </summary>
        [Field(46, 'r')]
        [XmlElement("titleVariant")]
        [JsonPropertyName("titleVariant")]
        [Description("Вариант заглавия")]
        [DisplayName("Вариант заглавия")]
        public string? TitleVariant { get; set; }

        /// <summary>
        /// Обозначение и № 2-й единицы деления (серия). 46^h.
        /// </summary>
        [Field(46, 'h')]
        [XmlElement("secondLevelNumber")]
        [JsonPropertyName("secondLevelNumber")]
        [Description("Обозначение и № 2-й единицы деления (серия)")]
        [DisplayName("Обозначение и № 2-й единицы деления (серия)")]
        public string? SecondLevelNumber { get; set; }

        /// <summary>
        /// Заглавие 2-й единицы деления (серия). 46^i.
        /// </summary>
        [Field(46, 'i')]
        [XmlElement("secondLevelTitle")]
        [JsonPropertyName("secondLevelTitle")]
        [Description("Заглавие 2-й единицы деления (серия)")]
        [DisplayName("Заглавие 2-й единицы деления (серия)")]
        public string? SecondLevelTitle { get; set; }

        /// <summary>
        /// Обозначение и № 3-й единицы деления (подсерия). 46^k.
        /// </summary>
        [Field(46, 'k')]
        [XmlElement("thirdLevelNumber")]
        [JsonPropertyName("thirdLevelNumber")]
        [Description("Обозначение и № 3-й единицы деления (подсерия)")]
        [DisplayName("Обозначение и № 3-й единицы деления (подсерия)")]
        public string? ThirdLevelNumber { get; set; }

        /// <summary>
        /// Заглавие 3-й единицы деления (подсерия). 46^m.
        /// </summary>
        [Field(46, 'm')]
        [XmlElement("thirdLevelTitle")]
        [JsonPropertyName("thirdLevelTitle")]
        [Description("Заглавие 3-й единицы деления (подсерия)")]
        [DisplayName("Заглавие 3-й единицы деления (подсерия)")]
        public string? ThirdLevelTitle { get; set; }

        /// <summary>
        /// Параллельное заглавие. 46^l.
        /// </summary>
        [Field(46, 'l')]
        [XmlElement("parallelTitle")]
        [JsonPropertyName("parallelTitle")]
        [Description("Параллельное заглавие")]
        [DisplayName("")]
        public string? ParallelTitle { get; set; }

        /// <summary>
        /// Заглавие серии. 46^a.
        /// </summary>
        [Field(46, 'a')]
        [XmlElement("seriesTitle")]
        [JsonPropertyName("seriesTitle")]
        [Description("Заглавие серии")]
        [DisplayName("Заглавие серии")]
        public string? SeriesTitle { get; set; }

        /// <summary>
        /// Предыдущее заглавие издания. 46^c.
        /// </summary>
        [Field(46, 'c')]
        [XmlElement("previousTitle")]
        [JsonPropertyName("previousTitle")]
        [Description("Предыдущее заглавие издания")]
        [DisplayName("Предыдущее заглавие издания")]
        public string? PreviousTitle { get; set; }

        /// <summary>
        /// Поле 461.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public Field? Field461 { get; private set; }

        /// <summary>
        /// Поле 46.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public Field? Field46 { get; private set; }

        /// <summary>
        /// Пользовательские данные.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the record.
        /// </summary>
        public static CommonInfo[] ParseRecord
            (
                Record record
            )
        {
            List<CommonInfo> result = new List<CommonInfo>();
            for (int i = 0;; i++)
            {
                var field461 = record.Fields
                    .GetField(MainTag, i);
                var field46 = record.Fields
                    .GetField(AdditionalTag, i);
                if (ReferenceEquals(field461, field46))
                {
                    // This can happen only if both fields = null
                    break;
                }

                CommonInfo info = new CommonInfo();
                result.Add(info);

                if (!ReferenceEquals(field461, null))
                {
                    info.Title = field461.GetFirstSubFieldValue('c').ToString();
                    info.Specific = field461.GetFirstSubFieldValue('u').ToString();
                    info.General = field461.GetFirstSubFieldValue('2').ToString();
                    info.Subtitle = field461.GetFirstSubFieldValue('e').ToString();
                    info.Responsibility = field461.GetFirstSubFieldValue('f').ToString();
                    info.Publisher = field461.GetFirstSubFieldValue('g').ToString();
                    info.City = field461.GetFirstSubFieldValue('d').ToString();
                    info.BeginningYear = field461.GetFirstSubFieldValue('h').ToString();
                    info.EndingYear = field461.GetFirstSubFieldValue('z').ToString();
                    info.Isbn = field461.GetFirstSubFieldValue('i').ToString();
                    info.Issn = field461.GetFirstSubFieldValue('j').ToString();
                    info.Reprint = field461.GetFirstSubFieldValue('p').ToString();
                    info.Translation = field461.GetFirstSubFieldValue('a').ToString();
                    info.FirstAuthor = field461.GetFirstSubFieldValue('x').ToString();
                    info.Collective = field461.GetFirstSubFieldValue('b').ToString();
                    info.Field461 = field461;
                }

                if (!ReferenceEquals(field46, null))
                {
                    info.TitleVariant = field46.GetFirstSubFieldValue('r').ToString();
                    info.SecondLevelNumber = field46.GetFirstSubFieldValue('h').ToString();
                    info.SecondLevelTitle = field46.GetFirstSubFieldValue('i').ToString();
                    info.ThirdLevelNumber = field46.GetFirstSubFieldValue('k').ToString();
                    info.ThirdLevelTitle = field46.GetFirstSubFieldValue('m').ToString();
                    info.ParallelTitle = field46.GetFirstSubFieldValue('l').ToString();
                    info.SeriesTitle = field46.GetFirstSubFieldValue('a').ToString();
                    info.PreviousTitle = field46.GetFirstSubFieldValue('c').ToString();
                    info.Field46 = field46;
                }
            }

            return result.ToArray();
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => Title.ToVisibleString();

        #endregion

    } // class CommonInfo

} // namespace ManagedIbris.Fields
