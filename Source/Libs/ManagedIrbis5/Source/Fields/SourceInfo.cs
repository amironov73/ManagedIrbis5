// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* SourceInfo.cs -- издание, в котором опубликована статья
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
    /// Издание, в котором опубликована статья, поля 463 и 963.
    /// </summary>
    public sealed class SourceInfo
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        ///
        /// </summary>
        public const int Tag463 = 463;

        /// <summary>
        ///
        /// </summary>
        public const int Tag963 = 963;

        #endregion

        #region Properties

        /// <summary>
        /// Заглавие. 463^c.
        /// </summary>
        [Field(463, 'c')]
        [XmlElement("title")]
        [JsonPropertyName("title")]
        [Description("Заглавие")]
        [DisplayName("Заглавие")]
        public string? Title { get; set; }

        /// <summary>
        /// Год издания. 463^j.
        /// </summary>
        [Field(463, 'j')]
        [XmlElement("year")]
        [JsonPropertyName("year")]
        [Description("Год издания")]
        [DisplayName("Год издания")]
        public string? Year { get; set; }

        /// <summary>
        /// Издательство. 463^g.
        /// </summary>
        [Field(463, 'g')]
        [XmlElement("publisher")]
        [JsonPropertyName("publisher")]
        [Description("Издательство")]
        [DisplayName("Издательство")]
        public string? Publisher { get; set; }

        /// <summary>
        /// Город. 463^d.
        /// </summary>
        [Field(463, 'd')]
        [XmlElement("city")]
        [JsonPropertyName("city")]
        [Description("Город")]
        [DisplayName("Город")]
        public string? City { get; set; }

        /// <summary>
        /// Местоположение - единица измерения. 463^1.
        /// </summary>
        [Field(463, '1')]
        [XmlElement("unit")]
        [JsonPropertyName("unit")]
        [Description("Местоположение - единица измерения")]
        [DisplayName("Местоположение - единица измерения")]
        public string? Unit { get; set; }

        /// <summary>
        /// Местоположение - страницы. 463^s.
        /// </summary>
        [Field(463, 's')]
        [XmlElement("position")]
        [JsonPropertyName("position")]
        [Description("Местоположение - страницы")]
        [DisplayName("Местоположение - страницы")]
        public string? Position { get; set; }

        /// <summary>
        /// Номер статьи. 463^n.
        /// </summary>
        [Field(463, 'n')]
        [XmlElement("articleNumber")]
        [JsonPropertyName("articleNumber")]
        [Description("Номер статьи")]
        [DisplayName("Номер статьи")]
        public string? ArticleNumber { get; set; }

        /// <summary>
        /// Наличие иллюстраций. 463^0.
        /// </summary>
        [Field(463, '0')]
        [XmlElement("illustrations")]
        [JsonPropertyName("illustrations")]
        [Description("Наличие иллюстраций")]
        [DisplayName("Наличие иллюстраций")]
        public string? Illustrations { get; set; }

        /// <summary>
        /// Примечание. 463^p.
        /// </summary>
        [Field(463, 'p')]
        [XmlElement("comments")]
        [JsonPropertyName("comments")]
        [Description("Примечание")]
        [DisplayName("Примечание")]
        public string? Comments { get; set; }

        /// <summary>
        /// Обозначение и № 1-й единицы деления (том). 463^v.
        /// </summary>
        [Field(463, 'v')]
        [XmlElement("volumeNumber")]
        [JsonPropertyName("volumeNumber")]
        [Description("Обозначение и № 1-й единицы деления (том)")]
        [DisplayName("Обозначение и № 1-й единицы деления (том)")]
        public string? VolumeNumber { get; set; }

        /// <summary>
        /// Заглавие 1-й единицы деления (том). 463^a.
        /// </summary>
        [Field(463, 'a')]
        [XmlElement("volumeTitle")]
        [JsonPropertyName("volumeTitle")]
        [Description("Заглавие 1-й единицы деления (том)")]
        [DisplayName("Заглавие 1-й единицы деления (том)")]
        public string? VolumeTitle { get; set; }

        /// <summary>
        /// Параллельное заглавие 1-й единицы деления (том). 463^r.
        /// </summary>
        [Field(463, 'r')]
        [XmlElement("parallelVolumeTitle")]
        [JsonPropertyName("parallelVolumeTitle")]
        [Description("Параллельное заглавие 1-й единицы деления (том)")]
        [DisplayName("Параллельное заглавие 1-й единицы деления (том)")]
        public string? ParallelVolumeTitle { get; set; }

        /// <summary>
        /// Обозначение и № 2-й единицы деления (выпуск). 463^h.
        /// </summary>
        [Field(463, 'h')]
        [XmlElement("secondLevelNumber")]
        [JsonPropertyName("secondLevelNumber")]
        [Description("Обозначение и № 2-й единицы деления (выпуск)")]
        [DisplayName("Обозначение и № 2-й единицы деления (выпуск)")]
        public string? SecondLevelNumber { get; set; }

        /// <summary>
        /// Заглавие 2-й единицы деления (выпуск). 463^i.
        /// </summary>
        [Field(463, 'i')]
        [XmlElement("secondLevelTitle")]
        [JsonPropertyName("secondLevelTitle")]
        [Description("Заглавие 2-й единицы деления (выпуск)")]
        [DisplayName("Заглавие 2-й единицы деления (выпуск)")]
        public string? SecondLevelTitle { get; set; }

        /// <summary>
        /// Обозначение и № 3-й единицы деления (часть). 463^k.
        /// </summary>
        [Field(463, 'k')]
        [XmlElement("thirdLevelNumber")]
        [JsonPropertyName("thirdLevelNumber")]
        [Description("Обозначение и № 3-й единицы деления (часть)")]
        [DisplayName("Обозначение и № 3-й единицы деления (часть)")]
        public string? ThirdLevelNumber { get; set; }

        /// <summary>
        /// Заглавие 3-й единицы деления (часть). 463^l.
        /// </summary>
        [Field(463, 'l')]
        [XmlElement("thirdLevelTitle")]
        [JsonPropertyName("thirdLevelTitle")]
        [Description("Заглавие 3-й единицы деления (часть)")]
        [DisplayName("Заглавие 3-й единицы деления (часть)")]
        public string? ThirdLevelTitle { get; set; }

        /// <summary>
        /// Заглавие - сокращение по ГОСТ. 463^7.
        /// </summary>
        [Field(463, '7')]
        [XmlElement("abbreviation")]
        [JsonPropertyName("abbreviation")]
        [Description("Заглавие - сокращение по ГОСТ")]
        [DisplayName("Заглавие - сокращение по ГОСТ")]
        public string? Abbreviation { get; set; }

        /// <summary>
        /// Параллельное заглавие 1. 463^x.
        /// </summary>
        [Field(463, 'x')]
        [XmlElement("parallelTitle1")]
        [JsonPropertyName("parallelTitle1")]
        [Description("Параллельное заглавие 1")]
        [DisplayName("Параллельное заглавие 1")]
        public string? ParallelTitle1 { get; set; }

        /// <summary>
        /// Параллельное заглавие 2. 463^y.
        /// </summary>
        [Field(463, 'y')]
        [XmlElement("parallelTitle2")]
        [JsonPropertyName("parllelTitle2")]
        [Description("Параллельное заглавие 2")]
        [DisplayName("Параллельное заглавие 2")]
        public string? ParallelTitle2 { get; set; }

        /// <summary>
        /// Параллельное заглавие 3. 463^z.
        /// </summary>
        [Field(463, 'z')]
        [XmlElement("parallelTitle3")]
        [JsonPropertyName("parallelTitle3")]
        [Description("Параллельное заглавие 3")]
        [DisplayName("Параллельное заглавие 3")]
        public string? ParallelTitle3 { get; set; }

        /// <summary>
        /// Шифр документа в БД. 463^w.
        /// </summary>
        [Field(463, 'w')]
        [XmlElement("index")]
        [JsonPropertyName("index")]
        [Description("Шифр документа в БД")]
        [DisplayName("Шифр документа в БД")]
        public string? Index { get; set; }

        /// <summary>
        /// 1-й автор - Заголовок описания. 963^x.
        /// </summary>
        [Field(963, 'x')]
        [XmlElement("")]
        [JsonPropertyName("")]
        [Description("1-й автор - Заголовок описания")]
        [DisplayName("1-й автор - Заголовок описания")]
        public string? FirstAuthor { get; set; }

        /// <summary>
        /// Роль (Инвертирование ФИО допустимо?). 963^9.
        /// </summary>
        [Field(963, '9')]
        [XmlElement("cantBeInverted")]
        [JsonPropertyName("cantBeInverted")]
        [Description("Роль (Инвертирование ФИО допустимо?)")]
        [DisplayName("Роль (Инвертирование ФИО допустимо?)")]
        public string? CantBeInverted { get; set; }

        /// <summary>
        /// Коллектив или мероприятие - Заголовок описания. 963^b.
        /// </summary>
        [Field(963, 'b')]
        [XmlElement("collective")]
        [JsonPropertyName("collective")]
        [Description("Коллектив или мероприятие - Заголовок описания")]
        [DisplayName("Коллектив или мероприятие - Заголовок описания")]
        public string? Collective { get; set; }

        /// <summary>
        /// Сокращение коллектива по ГОСТ. 963^7.
        /// </summary>
        [Field(963, '7')]
        [XmlElement("collectiveAbbreviation")]
        [JsonPropertyName("collectiveAbbreviation")]
        [Description("Сокращение коллектива по ГОСТ")]
        [DisplayName("Сокращение коллектива по ГОСТ")]
        public string? CollectiveAbbreviation { get; set; }

        /// <summary>
        /// Сведения, относящиеся к заглавию. 963^e.
        /// </summary>
        [Field(963, 'e')]
        [XmlElement("subtitle")]
        [JsonPropertyName("subtitle")]
        [Description("Сведения, относящиеся к заглавию")]
        [DisplayName("Сведения, относящиеся к заглавию")]
        public string? Subtitle { get; set; }

        /// <summary>
        /// Сведения об ответственности. 963^f.
        /// </summary>
        [Field(963, 'f')]
        [XmlElement("responsibility")]
        [JsonPropertyName("responsibility")]
        [Description("Сведения об ответственности")]
        [DisplayName("Сведения об ответственности")]
        public string? Responsibility { get; set; }

        /// <summary>
        /// ISBN или ISSN. 963^i.
        /// </summary>
        [Field(963, 'i')]
        [XmlElement("isbn")]
        [JsonPropertyName("isbn")]
        [Description("ISBN или ISSN")]
        [DisplayName("ISBN или ISSN")]
        public string? Isbn { get; set; }

        /// <summary>
        /// Сведения об издании. 963^p.
        /// </summary>
        [Field(963, 'p')]
        [XmlElement("reprint")]
        [JsonPropertyName("reprint")]
        [Description("Сведения об издании")]
        [DisplayName("Сведения об издании")]
        public string? Reprint { get; set; }

        /// <summary>
        /// Обозначение и № в серии. 963^v.
        /// </summary>
        [Field(963, 'v')]
        [XmlElement("seriesNumber")]
        [JsonPropertyName("seriesNumber")]
        [Description("Обозначение и № в серии")]
        [DisplayName("Обозначение и № в серии")]
        public string? SeriesNumber { get; set; }

        /// <summary>
        /// Заглавие серии. 963^a.
        /// </summary>
        [Field(963, 'a')]
        [XmlElement("seriesTitle")]
        [JsonPropertyName("seriesTitle")]
        [Description("Заглавие серии")]
        [DisplayName("Заглавие серии")]
        public string? SeriesTitle { get; set; }

        /// <summary>
        /// Сведения об ответственности серии. 963^o.
        /// </summary>
        [Field(963, 'o')]
        [XmlElement("seriesResponsibility")]
        [JsonPropertyName("seriesResponsibility")]
        [Description("Сведения об ответственности серии")]
        [DisplayName("Сведения об ответственности серии")]
        public string? SeriesResponsibility { get; set; }

        /// <summary>
        /// Associated field 463.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Description("Поле 463")]
        [DisplayName("Поле 463")]
        public Field? Field463 { get; private set; }

        /// <summary>
        /// Associated field 963.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Description("Поле 963")]
        [DisplayName("Поле 963")]
        public Field? Field963 { get; private set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Description("Пользовательские данные")]
        [DisplayName("Пользовательские данные")]
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the record.
        /// </summary>
        public static SourceInfo[] ParseRecord
            (
                Record record
            )
        {
            List<SourceInfo> result = new List<SourceInfo>();
            for (int i = 0; ; i++)
            {
                var field463 = record.Fields.GetField(Tag463, i);
                var field963 = record.Fields.GetField(Tag963, i);
                if (ReferenceEquals(field463, field963))
                {
                    // This can happen only if both fields = null
                    break;
                }

                SourceInfo info = new SourceInfo();
                result.Add(info);

                if (!ReferenceEquals(field463, null))
                {
                    info.Title = field463.GetFirstSubFieldValue('c');
                    info.Year = field463.GetFirstSubFieldValue('j');
                    info.Publisher = field463.GetFirstSubFieldValue('g');
                    info.City = field463.GetFirstSubFieldValue('d');
                    info.Unit = field463.GetFirstSubFieldValue('1');
                    info.Position = field463.GetFirstSubFieldValue('s');
                    info.ArticleNumber = field463.GetFirstSubFieldValue('n');
                    info.Illustrations = field463.GetFirstSubFieldValue('0');
                    info.Comments = field463.GetFirstSubFieldValue('p');
                    info.VolumeNumber = field463.GetFirstSubFieldValue('v');
                    info.VolumeTitle = field463.GetFirstSubFieldValue('a');
                    info.ParallelVolumeTitle = field463.GetFirstSubFieldValue('r');
                    info.SecondLevelNumber = field463.GetFirstSubFieldValue('h');
                    info.SecondLevelTitle = field463.GetFirstSubFieldValue('i');
                    info.ThirdLevelNumber = field463.GetFirstSubFieldValue('k');
                    info.ThirdLevelTitle = field463.GetFirstSubFieldValue('l');
                    info.Abbreviation = field463.GetFirstSubFieldValue('7');
                    info.ParallelTitle1 = field463.GetFirstSubFieldValue('x');
                    info.ParallelTitle2 = field463.GetFirstSubFieldValue('y');
                    info.ParallelTitle3 = field463.GetFirstSubFieldValue('z');
                    info.Index = field463.GetFirstSubFieldValue('w');
                    info.Field463 = field463;
                }

                if (!ReferenceEquals(field963, null))
                {
                    info.FirstAuthor = field963.GetFirstSubFieldValue('x');
                    info.CantBeInverted = field963.GetFirstSubFieldValue('9');
                    info.Collective = field963.GetFirstSubFieldValue('b');
                    info.CollectiveAbbreviation = field963.GetFirstSubFieldValue('7');
                    info.Subtitle = field963.GetFirstSubFieldValue('e');
                    info.Responsibility = field963.GetFirstSubFieldValue('f');
                    info.Isbn = field963.GetFirstSubFieldValue('i');
                    info.Reprint = field963.GetFirstSubFieldValue('p');
                    info.SeriesNumber = field963.GetFirstSubFieldValue('v');
                    info.SeriesTitle = field963.GetFirstSubFieldValue('a');
                    info.SeriesResponsibility = field963.GetFirstSubFieldValue('o');
                    info.Field963 = field963;
                }
            }

            return result.ToArray();
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Title = reader.ReadNullableString();
            Year = reader.ReadNullableString();
            Publisher = reader.ReadNullableString();
            City = reader.ReadNullableString();
            Unit = reader.ReadNullableString();
            Position = reader.ReadNullableString();
            ArticleNumber = reader.ReadNullableString();
            Illustrations = reader.ReadNullableString();
            Comments = reader.ReadNullableString();
            VolumeNumber = reader.ReadNullableString();
            VolumeTitle = reader.ReadNullableString();
            ParallelVolumeTitle = reader.ReadNullableString();
            SecondLevelNumber = reader.ReadNullableString();
            SecondLevelTitle = reader.ReadNullableString();
            ThirdLevelNumber = reader.ReadNullableString();
            ThirdLevelTitle = reader.ReadNullableString();
            Abbreviation = reader.ReadNullableString();
            ParallelTitle1 = reader.ReadNullableString();
            ParallelTitle2 = reader.ReadNullableString();
            ParallelTitle3 = reader.ReadNullableString();
            Index = reader.ReadNullableString();
            FirstAuthor = reader.ReadNullableString();
            CantBeInverted = reader.ReadNullableString();
            Collective = reader.ReadNullableString();
            CollectiveAbbreviation = reader.ReadNullableString();
            Subtitle = reader.ReadNullableString();
            Responsibility = reader.ReadNullableString();
            Isbn = reader.ReadNullableString();
            Reprint = reader.ReadNullableString();
            SeriesNumber = reader.ReadNullableString();
            SeriesTitle = reader.ReadNullableString();
            SeriesResponsibility = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Title)
                .WriteNullable(Year)
                .WriteNullable(Publisher)
                .WriteNullable(City)
                .WriteNullable(Unit)
                .WriteNullable(Position)
                .WriteNullable(ArticleNumber)
                .WriteNullable(Illustrations)
                .WriteNullable(Comments)
                .WriteNullable(VolumeNumber)
                .WriteNullable(VolumeTitle)
                .WriteNullable(ParallelVolumeTitle)
                .WriteNullable(SecondLevelNumber)
                .WriteNullable(SecondLevelTitle)
                .WriteNullable(ThirdLevelNumber)
                .WriteNullable(ThirdLevelTitle)
                .WriteNullable(Abbreviation)
                .WriteNullable(ParallelTitle1)
                .WriteNullable(ParallelTitle2)
                .WriteNullable(ParallelTitle3)
                .WriteNullable(Index)
                .WriteNullable(FirstAuthor)
                .WriteNullable(CantBeInverted)
                .WriteNullable(Collective)
                .WriteNullable(CollectiveAbbreviation)
                .WriteNullable(Subtitle)
                .WriteNullable(Responsibility)
                .WriteNullable(Isbn)
                .WriteNullable(Reprint)
                .WriteNullable(SeriesNumber)
                .WriteNullable(SeriesTitle)
                .WriteNullable(SeriesResponsibility);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Title.ToVisibleString();
        }

        #endregion

    } // class SourceInfo

} // namespace ManagedIrbis.Fields
