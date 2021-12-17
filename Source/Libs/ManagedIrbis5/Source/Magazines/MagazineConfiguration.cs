// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* MagazineConfiguration.cs -- конфигурация для работы с журналами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Json;
using AM.Runtime;

using ManagedIrbis.Fields;

#endregion

#nullable enable

namespace ManagedIrbis.Magazines
{
    /// <summary>
    /// Конфигурация для работы с журналами/газетами и статьями.
    /// </summary>
    [XmlRoot ("magazine-configuration")]
    public sealed class MagazineConfiguration
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        private const string Magazine = "ИРБИС64: журналы";

        #endregion

        #region Properties

        /// <summary>
        /// Метка поля для шифра СИ или журнала.
        /// </summary>
        [XmlAttribute ("code")]
        [Category (Magazine)]
        [DefaultValue (933)]
        [JsonPropertyName ("code")]
        [Description ("Шифр СИ или журнала")]
        public int MagazineTag { get; set; } = 933;

        /// <summary>
        /// Метка поля для шифра выпуска.
        /// </summary>
        [XmlAttribute ("index")]
        [Category (Magazine)]
        [DefaultValue (903)]
        [JsonPropertyName ("index")]
        [Description ("Шифр выпуска")]
        public int IndexTag { get; set; } = 903;

        /// <summary>
        /// Год.
        /// </summary>
        [XmlAttribute ("year")]
        [Category (Magazine)]
        [DefaultValue (934)]
        [JsonPropertyName ("year")]
        [Description ("Год")]
        public int YearTag { get; set; } = 934;

        /// <summary>
        /// Том (если есть).
        /// </summary>
        [XmlAttribute ("volume")]
        [Category (Magazine)]
        [DefaultValue (935)]
        [JsonPropertyName ("volume")]
        [Description ("Том")]
        public int VolumeTag { get; set; } = 935;

        /// <summary>
        /// Номер, часть.
        /// </summary>
        [XmlAttribute ("number")]
        [Category (Magazine)]
        [DefaultValue (936)]
        [JsonPropertyName ("number")]
        [Description ("Номер, часть")]
        public int NumberTag { get; set; } = 936;

        /// <summary>
        /// Дополнение к номеру.
        /// </summary>
        [XmlAttribute ("addition")]
        [Category (Magazine)]
        [DefaultValue (931)]
        [JsonPropertyName ("addition")]
        [Description ("Дополнение к номеру")]
        public int AdditionTag { get; set; } = 931;

        /// <summary>
        /// Сведения об экземплярах.
        /// </summary>
        [XmlAttribute ("exemplar")]
        [Category (Magazine)]
        [DefaultValue (910)]
        [JsonPropertyName ("exemplar")]
        [Description ("Сведения об экземплярах")]
        public int ExemplarTag { get; set; } = 910;

        /// <summary>
        /// Статьи из журнала.
        /// </summary>
        [XmlAttribute ("article")]
        [Category (Magazine)]
        [DefaultValue (922)]
        [JsonPropertyName ("article")]
        [Description ("Статьи из журнала")]
        public int ArticleTag { get; set; } = 922;

        /// <summary>
        /// Кодированная информация.
        /// </summary>
        [XmlAttribute ("info")]
        [Category (Magazine)]
        [DefaultValue (110)]
        [JsonPropertyName ("info")]
        [Description ("Кодированная информация")]
        public int InfoTag { get; set; } = 110;

        /// <summary>
        /// ISSN.
        /// </summary>
        [XmlAttribute ("issn")]
        [Category (Magazine)]
        [DefaultValue (11)]
        [JsonPropertyName ("issn")]
        [Description ("ISSN")]
        public int IssnTag { get; set; } = 11;

        /// <summary>
        /// Зарегистрированные поступления (кумуляция).
        /// </summary>
        [XmlAttribute ("cumulation")]
        [Category (Magazine)]
        [DefaultValue (909)]
        [JsonPropertyName ("cumulation")]
        [Description ("Зарегистрированные поступления")]
        public int CumulationTag { get; set; } = 909;

        /// <summary>
        /// Сведения о заказанных экземплярах.
        /// </summary>
        [XmlAttribute ("ordered")]
        [Category (Magazine)]
        [DefaultValue (901)]
        [JsonPropertyName ("ordered")]
        [Description ("Заказанные экземпляры")]
        public int OrderedTag { get; set; } = 901;

        /// <summary>
        /// Метка для поля 203 ("текст: непосредственный").
        /// </summary>
        [XmlAttribute ("field-203")]
        [Category (Magazine)]
        [DefaultValue (203)]
        [JsonPropertyName ("field203")]
        [Description ("Вид содержания")]
        public int Tag203 { get; set; } = 203;

        /// <summary>
        /// Метка поля с информацией о каталогизаторе и дате обработки.
        /// </summary>
        [XmlAttribute ("operator")]
        [Category (Magazine)]
        [DefaultValue (907)]
        [JsonPropertyName ("operator")]
        [Description ("Технология")]
        public int OperatorTag { get; set; } = 907;

        /// <summary>
        /// Метка поля для кода страны.
        /// </summary>
        [XmlAttribute ("country")]
        [Category (Magazine)]
        [DefaultValue (102)]
        [JsonPropertyName ("country")]
        [Description ("Код страны")]
        public int CountryTag { get; set; } = 102;

        /// <summary>
        /// Метка поля для кода языка основного текста.
        /// </summary>
        [XmlAttribute ("language")]
        [Category (Magazine)]
        [DefaultValue (101)]
        [JsonPropertyName ("language")]
        [Description ("Язык основного текста")]
        public int LanguageTag { get; set; } = 101;

        /// <summary>
        /// Метка поля для рабочего листа (вида документа).
        /// </summary>
        [XmlAttribute ("worksheet")]
        [Category (Magazine)]
        [DefaultValue (920)]
        [JsonPropertyName ("worksheet")]
        [Description ("Рабочий лист")]
        public int WorksheetTag { get; set; } = 920;

        /// <summary>
        /// Метка поля для количества выдач документа.
        /// </summary>
        [XmlAttribute ("rental")]
        [Category (Magazine)]
        [DefaultValue (999)]
        [JsonPropertyName ("rental")]
        [Description ("Количество выдач")]
        public int RentalTag { get; set; } = 999;

        #endregion

        #region Public methods

        /// <summary>
        /// Получение конфигурации по умолчанию.
        /// </summary>
        [Pure]
        public static MagazineConfiguration GetDefault()
        {
            return new ();
        }

        /// <summary>
        /// Получение кода СИ или журнала.
        /// </summary>
        [Pure]
        public string? GetMagazineCode
            (
                Record record
            )
        {
            Sure.NotNull (record);

            return record.FM (MagazineTag);
        }

        /// <summary>
        /// Получение шифра выпуска.
        /// </summary>
        [Pure]
        public string? GetIndex
            (
                Record record
            )
        {
            Sure.NotNull (record);

            return record.FM (IndexTag);
        }

        /// <summary>
        /// Получение года.
        /// </summary>
        [Pure]
        public string? GetYear
            (
                Record record
            )
        {
            Sure.NotNull (record);

            return record.FM (YearTag);
        }

        /// <summary>
        /// Получение тома.
        /// </summary>
        [Pure]
        public string? GetVolume
            (
                Record record
            )
        {
            Sure.NotNull (record);

            return record.FM (VolumeTag);
        }

        /// <summary>
        /// Получение номера выпуска.
        /// </summary>
        [Pure]
        public string? GetNumber
            (
                Record record
            )
        {
            Sure.NotNull (record);

            return record.FM (NumberTag);
        }

        /// <summary>
        /// Получение поля с дополнением к номеру.
        /// </summary>
        [Pure]
        public Field[] GetAdditionFields
            (
                Record record
            )
        {
            Sure.NotNull (record);

            return record.Fields.GetField (AdditionTag);
        }

        /// <summary>
        /// Получение полей с экземплярами.
        /// </summary>
        [Pure]
        public Field[] GetExemplarFields
            (
                Record record
            )
        {
            Sure.NotNull (record);

            return record.Fields.GetField (ExemplarTag);
        }

        /// <summary>
        /// Получение экземпляров документа.
        /// </summary>
        [Pure]
        public ExemplarInfo[] GetExemplars
            (
                Record record
            )
        {
            Sure.NotNull (record);

            return ExemplarInfo.ParseRecord (record, ExemplarTag);
        }

        /// <summary>
        /// Получение полей со статьями.
        /// </summary>
        [Pure]
        public Field[] GetArticleFields
            (
                Record record
            )
        {
            Sure.NotNull (record);

            return record.Fields.GetField (ArticleTag);
        }

        /// <summary>
        /// Получение массива статей.
        /// </summary>
        public MagazineArticleInfo[] GetArticles
            (
                Record record
            )
        {
            Sure.NotNull (record);

            return MagazineArticleInfo.ParseIssue (record, ArticleTag);
        }

        /// <summary>
        /// Получение ISSN.
        /// </summary>
        public IssnInfo[] GetIssn
            (
                Record record
            )
        {
            Sure.NotNull (record);

            return IssnInfo.ParseRecord (record, IssnTag);
        }

        /// <summary>
        /// Получение зарегистрированных поступлений (кумуляции).
        /// </summary>
        public MagazineCumulation[] GetCumulation
            (
                Record record
            )
        {
            Sure.NotNull (record);

            return MagazineCumulation.ParseRecord (record, CumulationTag);
        }

        /// <summary>
        /// Получение заказанных экземпляров.
        /// </summary>
        public Field[] GetOrdered
            (
                Record record
            )
        {
            Sure.NotNull (record);

            return record.Fields.GetField (OrderedTag);
        }

        /// <summary>
        /// Получение вида содержания (поле 203).
        /// </summary>
        public Field203? Get203
            (
                Record record
            )
        {
            Sure.NotNull (record);

            var field = record.GetField (Tag203);

            return field is null ? null : Field203.ParseField (field);
        }

        /// <summary>
        /// Получение полей с информацией о каталогизаторе и дате обработки.
        /// </summary>
        public Field[] GetOperatorFields
            (
                Record record
            )
        {
            Sure.NotNull (record);

            return record.Fields.GetField (OperatorTag);
        }

        /// <summary>
        /// Получение информации о редактировании записи.
        /// </summary>
        public Technology[] GetTechnology
            (
                Record record
            )
        {
            Sure.NotNull (record);

            return Technology.ParseRecord (record, OperatorTag);
        }

        /// <summary>
        /// Получение кода страны.
        /// </summary>
        [Pure]
        public string? GetCountryCode
            (
                Record record,
                string? defaultValue = null
            )
        {
            Sure.NotNull (record);

            return record.FM (CountryTag) ?? defaultValue;
        }

        /// <summary>
        /// Получение кодов стран.
        /// </summary>
        [Pure]
        public string[] GetCountryCodes
            (
                Record record,
                string? defaultValue = null
            )
        {
            Sure.NotNull (record);

            var result = record.FMA (CountryTag);
            if (result.Length == 0 && !string.IsNullOrEmpty (defaultValue))
            {
                return new[] { defaultValue };
            }

            return result;
        }

        /// <summary>
        /// Получение кода языка основного текста.
        /// </summary>
        [Pure]
        public string? GetLanguageCode
            (
                Record record,
                string? defaultValue = null
            )
        {
            Sure.NotNull (record);

            return record.FM (LanguageTag) ?? defaultValue;
        }

        /// <summary>
        /// Получение кодов языка основного текста.
        /// </summary>
        [Pure]
        public string[] GetLanguageCodes
            (
                Record record,
                string? defaultValue = null
            )
        {
            Sure.NotNull (record);

            var result = record.FMA (LanguageTag);
            if (result.Length == 0 && !string.IsNullOrEmpty (defaultValue))
            {
                return new[] { defaultValue };
            }

            return result;
        }

        /// <summary>
        /// Получение кода рабочего листа (вида документа).
        /// </summary>
        [Pure]
        public string? GetWorksheet
            (
                Record record,
                string? defaultValue = null
            )
        {
            Sure.NotNull (record);

            return record.FM (WorksheetTag) ?? defaultValue;
        }

        /// <summary>
        /// Получение количества выдач документа.
        /// </summary>
        [Pure]
        public int GetRentalCount
            (
                Record record
            )
        {
            Sure.NotNull (record);

            return record.FM (RentalTag).SafeToInt32();
        }

        /// <summary>
        /// Чтение конфигурации из указанного файла.
        /// </summary>
        public static MagazineConfiguration LoadConfiguration
            (
                string fileName
            )
        {
            Sure.FileExists (fileName);

            return JsonUtility.ReadObjectFromFile<MagazineConfiguration> (fileName);
        }

        /// <summary>
        /// Запись конфигурации в указанный файл.
        /// </summary>
        public void SaveConfiguration
            (
                string fileName
            )
        {
            Sure.NotNullNorEmpty (fileName);

            JsonUtility.SaveObjectToFile (this, fileName);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            MagazineTag = reader.Read7BitEncodedInt();
            IndexTag = reader.Read7BitEncodedInt();
            YearTag = reader.Read7BitEncodedInt();
            VolumeTag = reader.Read7BitEncodedInt();
            NumberTag = reader.Read7BitEncodedInt();
            AdditionTag = reader.Read7BitEncodedInt();
            ExemplarTag = reader.Read7BitEncodedInt();
            ArticleTag = reader.Read7BitEncodedInt();
            InfoTag = reader.Read7BitEncodedInt();
            IssnTag = reader.Read7BitEncodedInt();
            CumulationTag = reader.Read7BitEncodedInt();
            OrderedTag = reader.Read7BitEncodedInt();
            Tag203 = reader.Read7BitEncodedInt();
            OperatorTag = reader.Read7BitEncodedInt();
            CountryTag = reader.Read7BitEncodedInt();
            LanguageTag = reader.Read7BitEncodedInt();
            WorksheetTag = reader.Read7BitEncodedInt();
            RentalTag = reader.Read7BitEncodedInt();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer.Write7BitEncodedInt (MagazineTag);
            writer.Write7BitEncodedInt (IndexTag);
            writer.Write7BitEncodedInt (YearTag);
            writer.Write7BitEncodedInt (VolumeTag);
            writer.Write7BitEncodedInt (NumberTag);
            writer.Write7BitEncodedInt (AdditionTag);
            writer.Write7BitEncodedInt (ExemplarTag);
            writer.Write7BitEncodedInt (ArticleTag);
            writer.Write7BitEncodedInt (InfoTag);
            writer.Write7BitEncodedInt (IssnTag);
            writer.Write7BitEncodedInt (CumulationTag);
            writer.Write7BitEncodedInt (OrderedTag);
            writer.Write7BitEncodedInt (Tag203);
            writer.Write7BitEncodedInt (OperatorTag);
            writer.Write7BitEncodedInt (CountryTag);
            writer.Write7BitEncodedInt (LanguageTag);
            writer.Write7BitEncodedInt (WorksheetTag);
            writer.Write7BitEncodedInt (RentalTag);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<MagazineConfiguration> (this, throwOnError);

            verifier
                .Positive (MagazineTag)
                .Positive (IndexTag)
                .Positive (YearTag)
                .Positive (VolumeTag)
                .Positive (NumberTag)
                .Positive (AdditionTag)
                .Positive (ExemplarTag)
                .Positive (ArticleTag)
                .Positive (InfoTag)
                .Positive (IssnTag)
                .Positive (CumulationTag)
                .Positive (OrderedTag)
                .Positive (Tag203)
                .Positive (CountryTag)
                .Positive (LanguageTag)
                .Positive (WorksheetTag)
                .Positive (RentalTag);

            return verifier.Result;
        }

        #endregion
    }
}
