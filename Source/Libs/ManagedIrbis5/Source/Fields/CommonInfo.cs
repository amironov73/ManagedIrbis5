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
    /// Общая информация о многотомнике. Поля 461 и 46.
    /// </summary>
    [XmlRoot ("common")]
    public sealed class CommonInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Метка основного поля.
        /// </summary>
        public const int MainTag = 461;

        /// <summary>
        /// Метка дополнительного поля.
        /// </summary>
        public const int AdditionalTag = 46;

        #endregion

        #region Properties

        /// <summary>
        /// Заглавие. 461^c.
        /// </summary>
        [Field (MainTag, 'c')]
        [XmlElement ("title")]
        [JsonPropertyName ("title")]
        [Description ("Заглавие")]
        [DisplayName ("Заглавие")]
        public string? Title { get; set; }

        /// <summary>
        /// Роль (Нехарактерное заглавие, доб.карточка?). 461^u.
        /// </summary>
        [Field (MainTag, 'u')]
        [XmlElement ("specific")]
        [JsonPropertyName ("specific")]
        [Description ("Роль (Нехарактерное заглавие, доб.карточка?)")]
        [DisplayName ("Роль (Нехарактерное заглавие, доб.карточка?)")]
        public string? Specific { get; set; }

        /// <summary>
        /// Общее обозначение материала. 461^2.
        /// </summary>
        [Field (MainTag, '2')]
        [XmlElement ("general")]
        [JsonPropertyName ("general")]
        [Description ("Общее обозначение материала")]
        [DisplayName ("Общее обозначение материала")]
        public string? General { get; set; }

        /// <summary>
        /// Сведения, относящиеся к заглавию. 461^e.
        /// </summary>
        [Field (MainTag, 'e')]
        [XmlElement ("subtitle")]
        [JsonPropertyName ("subtitle")]
        [Description ("Сведения, относящиеся к заглавию")]
        [DisplayName ("Сведения, относящиеся к заглавию")]
        public string? Subtitle { get; set; }

        /// <summary>
        /// Сведения об ответственности. 461^f.
        /// </summary>
        [Field (MainTag, 'f')]
        [XmlElement ("responsibility")]
        [JsonPropertyName ("responsibility")]
        [Description ("Сведения об ответственности")]
        [DisplayName ("Сведения об ответственности")]
        public string? Responsibility { get; set; }

        /// <summary>
        /// Издательство. 461^g.
        /// </summary>
        [Field (MainTag, 'g')]
        [XmlElement ("publisher")]
        [JsonPropertyName ("publisher")]
        [Description ("Издательство")]
        [DisplayName ("Издательство")]
        public string? Publisher { get; set; }

        /// <summary>
        /// Город. 461^d.
        /// </summary>
        [Field (MainTag, 'd')]
        [XmlElement ("city")]
        [JsonPropertyName ("city")]
        [Description ("Город")]
        [DisplayName ("Город")]
        public string? City { get; set; }

        /// <summary>
        /// Год начала издания. 461^h.
        /// </summary>
        [Field (MainTag, 'h')]
        [XmlElement ("beginningYear")]
        [JsonPropertyName ("beginningYear")]
        [Description ("Год начала издания")]
        [DisplayName ("Год начала издания")]
        public string? BeginningYear { get; set; }

        /// <summary>
        /// Год окончания издания. 461^z.
        /// </summary>
        [Field (MainTag, 'z')]
        [XmlElement ("endingYear")]
        [JsonPropertyName ("endingYear")]
        [Description ("Год окончания издания")]
        [DisplayName ("Год окончания издания")]
        public string? EndingYear { get; set; }

        /// <summary>
        /// ISBN. 461^i.
        /// </summary>
        [Field (MainTag, 'i')]
        [XmlElement ("isbn")]
        [JsonPropertyName ("isbn")]
        [Description ("ISBN")]
        [DisplayName ("ISBN")]
        public string? Isbn { get; set; }

        /// <summary>
        /// ISSN. 461^j.
        /// </summary>
        [Field (MainTag, 'j')]
        [XmlElement ("issn")]
        [JsonPropertyName ("issn")]
        [Description ("ISSN")]
        [DisplayName ("ISSN")]
        public string? Issn { get; set; }

        /// <summary>
        /// Сведения об издании. 461^p.
        /// </summary>
        [Field (MainTag, 'p')]
        [XmlElement ("edition")]
        [JsonPropertyName ("edition")]
        [Description ("Сведения об издании")]
        [DisplayName ("Сведения об издании")]
        public string? Edition { get; set; }

        /// <summary>
        /// Перевод заглавия. 461^a.
        /// </summary>
        [Field (MainTag, 'a')]
        [XmlElement ("translation")]
        [JsonPropertyName ("translation")]
        [Description ("Перевод заглавия")]
        [DisplayName ("Перевод заглавия")]
        public string? Translation { get; set; }

        /// <summary>
        /// 1-й автор - Заголовок описания. 461^x.
        /// </summary>
        [Field (MainTag, 'x')]
        [XmlElement ("firstAuthor")]
        [JsonPropertyName ("firstAuthor")]
        [Description ("1-й автор - Заголовок описания")]
        [DisplayName ("1-й автор - Заголовок описания")]
        public string? FirstAuthor { get; set; }

        /// <summary>
        /// Коллектив или меропритие - Заголовок описания. 461^b.
        /// </summary>
        [Field (MainTag, 'b')]
        [XmlElement ("collective")]
        [JsonPropertyName ("collective")]
        [Description ("Коллектив или меропритие - Заголовок описания")]
        [DisplayName ("Коллектив или меропритие - Заголовок описания")]
        public string? Collective { get; set; }

        /// <summary>
        /// Вариант заглавия. 46^r.
        /// </summary>
        [Field (AdditionalTag, 'r')]
        [XmlElement ("titleVariant")]
        [JsonPropertyName ("titleVariant")]
        [Description ("Вариант заглавия")]
        [DisplayName ("Вариант заглавия")]
        public string? TitleVariant { get; set; }

        /// <summary>
        /// Обозначение и № 2-й единицы деления (серия). 46^h.
        /// </summary>
        [Field (AdditionalTag, 'h')]
        [XmlElement ("secondLevelNumber")]
        [JsonPropertyName ("secondLevelNumber")]
        [Description ("Обозначение и № 2-й единицы деления (серия)")]
        [DisplayName ("Обозначение и № 2-й единицы деления (серия)")]
        public string? SecondLevelNumber { get; set; }

        /// <summary>
        /// Заглавие 2-й единицы деления (серия). 46^i.
        /// </summary>
        [Field (AdditionalTag, 'i')]
        [XmlElement ("secondLevelTitle")]
        [JsonPropertyName ("secondLevelTitle")]
        [Description ("Заглавие 2-й единицы деления (серия)")]
        [DisplayName ("Заглавие 2-й единицы деления (серия)")]
        public string? SecondLevelTitle { get; set; }

        /// <summary>
        /// Обозначение и № 3-й единицы деления (подсерия). 46^k.
        /// </summary>
        [Field (AdditionalTag, 'k')]
        [XmlElement ("thirdLevelNumber")]
        [JsonPropertyName ("thirdLevelNumber")]
        [Description ("Обозначение и № 3-й единицы деления (подсерия)")]
        [DisplayName ("Обозначение и № 3-й единицы деления (подсерия)")]
        public string? ThirdLevelNumber { get; set; }

        /// <summary>
        /// Заглавие 3-й единицы деления (подсерия). 46^m.
        /// </summary>
        [Field (AdditionalTag, 'm')]
        [XmlElement ("thirdLevelTitle")]
        [JsonPropertyName ("thirdLevelTitle")]
        [Description ("Заглавие 3-й единицы деления (подсерия)")]
        [DisplayName ("Заглавие 3-й единицы деления (подсерия)")]
        public string? ThirdLevelTitle { get; set; }

        /// <summary>
        /// Параллельное заглавие. 46^l.
        /// </summary>
        [Field (AdditionalTag, 'l')]
        [XmlElement ("parallelTitle")]
        [JsonPropertyName ("parallelTitle")]
        [Description ("Параллельное заглавие")]
        [DisplayName ("")]
        public string? ParallelTitle { get; set; }

        /// <summary>
        /// Заглавие серии. 46^a.
        /// </summary>
        [Field (AdditionalTag, 'a')]
        [XmlElement ("seriesTitle")]
        [JsonPropertyName ("seriesTitle")]
        [Description ("Заглавие серии")]
        [DisplayName ("Заглавие серии")]
        public string? SeriesTitle { get; set; }

        /// <summary>
        /// Предыдущее заглавие издания. 46^c.
        /// </summary>
        [Field (AdditionalTag, 'c')]
        [XmlElement ("previousTitle")]
        [JsonPropertyName ("previousTitle")]
        [Description ("Предыдущее заглавие издания")]
        [DisplayName ("Предыдущее заглавие издания")]
        public string? PreviousTitle { get; set; }

        /// <summary>
        /// Ассоциированное поле 461.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public Field? Field461 { get; set; }

        /// <summary>
        /// Ассоциированное поле 46.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public Field? Field46 { get; set; }

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
        /// Применение данных к полям библиографической записи.
        /// </summary>
        public void ApplyTo
            (
                Field? field461,
                Field? field46
            )
        {
            if (field461 is not null)
            {
                field461.SetSubFieldValue ('c', Title);
                field461.SetSubFieldValue ('u', Specific);
                field461.SetSubFieldValue ('2', General);
                field461.SetSubFieldValue ('e', Subtitle);
                field461.SetSubFieldValue ('f', Responsibility);
                field461.SetSubFieldValue ('g', Publisher);
                field461.SetSubFieldValue ('d', City);
                field461.SetSubFieldValue ('h', BeginningYear);
                field461.SetSubFieldValue ('z', EndingYear);
                field461.SetSubFieldValue ('i', Isbn);
                field461.SetSubFieldValue ('j', Issn);
                field461.SetSubFieldValue ('p', Edition);
                field461.SetSubFieldValue ('a', Translation);
                field461.SetSubFieldValue ('x', FirstAuthor);
                field461.SetSubFieldValue ('b', Collective);

            } // if

            if (field46 is not null)
            {
                field46.SetSubFieldValue ('r', TitleVariant);
                field46.SetSubFieldValue ('h', SecondLevelNumber);
                field46.SetSubFieldValue ('i', SecondLevelTitle);
                field46.SetSubFieldValue ('k', ThirdLevelNumber);
                field46.SetSubFieldValue ('m', ThirdLevelTitle);
                field46.SetSubFieldValue ('l', ParallelTitle);
                field46.SetSubFieldValue ('a', SeriesTitle);
                field46.SetSubFieldValue ('c', PreviousTitle);

            } // if

        } // method ApplyTo

        /// <summary>
        /// Применение данных к библиографической записи.
        /// </summary>
        public void ApplyTo
            (
                Record record
            )
        {
            record.RemoveField (MainTag);
            record.RemoveField (AdditionalTag);

            var field461 = ToField461();
            if (!field461.IsEmpty)
            {
                record.Add (field461);
            }

            var field46 = ToField46();
            if (!field46.IsEmpty)
            {
                record.Add (field46);
            }

        } // method ApplyTo


        /// <summary>
        /// Разбор поля 461.
        /// </summary>
        public void ParseField461
            (
                Field field
            )
        {
            Sure.NotNull (field);

            Title = field.GetFirstSubFieldValue ('c');
            Specific = field.GetFirstSubFieldValue ('u');
            General = field.GetFirstSubFieldValue ('2');
            Subtitle = field.GetFirstSubFieldValue ('e');
            Responsibility = field.GetFirstSubFieldValue ('f');
            Publisher = field.GetFirstSubFieldValue ('g');
            City = field.GetFirstSubFieldValue ('d');
            BeginningYear = field.GetFirstSubFieldValue ('h');
            EndingYear = field.GetFirstSubFieldValue ('z');
            Isbn = field.GetFirstSubFieldValue ('i');
            Issn = field.GetFirstSubFieldValue ('j');
            Edition = field.GetFirstSubFieldValue ('p');
            Translation = field.GetFirstSubFieldValue ('a');
            FirstAuthor = field.GetFirstSubFieldValue ('x');
            Collective = field.GetFirstSubFieldValue ('b');
            Field461 = field;

        } // method ParseField461

        /// <summary>
        /// Разбор поля 46.
        /// </summary>
        public void ParseField46
            (
                Field field
            )
        {
            Sure.NotNull (field);

            TitleVariant = field.GetFirstSubFieldValue ('r');
            SecondLevelNumber = field.GetFirstSubFieldValue ('h');
            SecondLevelTitle = field.GetFirstSubFieldValue ('i');
            ThirdLevelNumber = field.GetFirstSubFieldValue ('k');
            ThirdLevelTitle = field.GetFirstSubFieldValue ('m');
            ParallelTitle = field.GetFirstSubFieldValue ('l');
            SeriesTitle = field.GetFirstSubFieldValue ('a');
            PreviousTitle = field.GetFirstSubFieldValue ('c');
            Field46 = field;

        } // method ParseField46

        /// <summary>
        /// Разбор библиографической записи.
        /// </summary>
        public static CommonInfo[] ParseRecord
            (
                Record record
            )
        {
            Sure.NotNull (record);

            var result = new List<CommonInfo>();
            for (var i = 0;; i++)
            {
                var field461 = record.Fields.GetField (MainTag, i);
                var field46 = record.Fields.GetField (AdditionalTag, i);
                if (ReferenceEquals (field461, field46))
                {
                    // This can happen only if both fields = null
                    break;
                }

                var info = new CommonInfo();
                result.Add (info);

                if (field461 is not null)
                {
                    info.ParseField461 (field461);
                }

                if (field46 is not null)
                {
                    info.ParseField46 (field46);

                }

            } // for

            return result.ToArray();

        } // method ParseRecord

        /// <summary>
        /// Преобразование в поле 461.
        /// </summary>
        public Field ToField461() => new Field (MainTag)
            .AddNonEmpty ('c', Title)
            .AddNonEmpty ('u', Specific)
            .AddNonEmpty ('2', General)
            .AddNonEmpty ('e', Subtitle)
            .AddNonEmpty ('f', Responsibility)
            .AddNonEmpty ('g', Publisher)
            .AddNonEmpty ('d', City)
            .AddNonEmpty ('h', BeginningYear)
            .AddNonEmpty ('z', EndingYear)
            .AddNonEmpty ('i', Isbn)
            .AddNonEmpty ('j', Issn)
            .AddNonEmpty ('p', Edition)
            .AddNonEmpty ('a', Translation)
            .AddNonEmpty ('x', FirstAuthor)
            .AddNonEmpty ('b', Collective);

        /// <summary>
        /// Преобразование в поле 46.
        /// </summary>
        /// <returns></returns>
        public Field ToField46() => new Field (AdditionalTag)
            .AddNonEmpty ('r', TitleVariant)
            .AddNonEmpty ('h', SecondLevelNumber)
            .AddNonEmpty ('i', SecondLevelTitle)
            .AddNonEmpty ('k', ThirdLevelNumber)
            .AddNonEmpty ('m', ThirdLevelTitle)
            .AddNonEmpty ('l', ParallelTitle)
            .AddNonEmpty ('a', SeriesTitle)
            .AddNonEmpty ('c', PreviousTitle);

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            Title = reader.ReadNullableString();
            Specific = reader.ReadNullableString();
            General = reader.ReadNullableString();
            Subtitle = reader.ReadNullableString();
            Responsibility = reader.ReadNullableString();
            Publisher = reader.ReadNullableString();
            City = reader.ReadNullableString();
            BeginningYear = reader.ReadNullableString();
            EndingYear = reader.ReadNullableString();
            Isbn = reader.ReadNullableString();
            Issn = reader.ReadNullableString();
            Edition = reader.ReadNullableString();
            Translation = reader.ReadNullableString();
            FirstAuthor = reader.ReadNullableString();
            Collective = reader.ReadNullableString();
            TitleVariant = reader.ReadNullableString();
            SecondLevelNumber = reader.ReadNullableString();
            SecondLevelTitle = reader.ReadNullableString();
            ThirdLevelNumber = reader.ReadNullableString();
            ThirdLevelTitle = reader.ReadNullableString();
            ParallelTitle = reader.ReadNullableString();
            SeriesTitle = reader.ReadNullableString();
            PreviousTitle = reader.ReadNullableString();

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer
                .WriteNullable (Title)
                .WriteNullable (Specific)
                .WriteNullable (General)
                .WriteNullable (Subtitle)
                .WriteNullable (Responsibility)
                .WriteNullable (Publisher)
                .WriteNullable (City)
                .WriteNullable (BeginningYear)
                .WriteNullable (EndingYear)
                .WriteNullable (Isbn)
                .WriteNullable (Issn)
                .WriteNullable (Edition)
                .WriteNullable (Translation)
                .WriteNullable (FirstAuthor)
                .WriteNullable (Collective)
                .WriteNullable (TitleVariant)
                .WriteNullable (SecondLevelNumber)
                .WriteNullable (SecondLevelTitle)
                .WriteNullable (ThirdLevelNumber)
                .WriteNullable (ThirdLevelTitle)
                .WriteNullable (ParallelTitle)
                .WriteNullable (SeriesTitle)
                .WriteNullable (PreviousTitle);

        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<CommonInfo> (this, throwOnError);

            verifier
                .NotNullNorEmpty (Title);

            return verifier.Result;

        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => Title.ToVisibleString();

        #endregion

    } // class CommonInfo

} // namespace ManagedIbris.Fields
