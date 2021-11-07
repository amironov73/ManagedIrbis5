// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* RecordConfiguration.cs -- конфигурация стандартной библиографической записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Json;

using ManagedIrbis.Fields;

#endregion

#nullable enable

namespace ManagedIrbis.Records
{
    /// <summary>
    /// Конфигурация стандартной библиографической записи.
    /// </summary>
    [XmlRoot ("record-configuration")]
    public sealed class RecordConfiguration
    {
        #region Constants

        private const string IrbisRecord = "ИРБИС64: библиографическая запись";

        #endregion

        #region Properties

        /// <summary>
        /// Имя полного (по полям) формата записи.
        /// </summary>
        [XmlAttribute ("all")]
        [Category (IrbisRecord)]
        [DefaultValue ("@all")]
        [JsonPropertyName ("all")]
        public string AllFormat { get; set; } = "@all";

        /// <summary>
        /// Имя краткого (в одну строку) формата показа.
        /// </summary>
        [XmlAttribute ("brief")]
        [Category (IrbisRecord)]
        [DefaultValue ("@brief")]
        [JsonPropertyName ("brief")]
        public string BriefFormat { get; set; } = "@brief";

        /// <summary>
        /// Метка поля для кода страны.
        /// </summary>
        [XmlAttribute ("country")]
        [Category (IrbisRecord)]
        [DefaultValue (102)]
        [JsonPropertyName ("country")]
        public int CountryTag { get; set; } = 102;

        /// <summary>
        /// Метка поля для настройки библиографической записи.
        /// </summary>
        [XmlAttribute ("customization")]
        [Category (IrbisRecord)]
        [DefaultValue (905)]
        [JsonPropertyName ("customization")]
        public int CustomizationTag { get; set; } = 905;

        /// <summary>
        /// Метка поля для экземпляров.
        /// </summary>
        [XmlAttribute ("exemplar")]
        [Category (IrbisRecord)]
        [DefaultValue (910)]
        [JsonPropertyName ("exemplar")]
        public int ExemplarTag { get; set; } = 910;

        /// <summary>
        /// Метка поля для полных текстов.
        /// </summary>
        [XmlAttribute ("fulltext")]
        [Category (IrbisRecord)]
        [DefaultValue (951)]
        [JsonPropertyName ("fulltext")]
        public int FullTextTag { get; set; } = 951;

        /// <summary>
        /// Метка поля для держателя документа.
        /// </summary>
        [XmlAttribute ("holder")]
        [Category (IrbisRecord)]
        [DefaultValue (902)]
        [JsonPropertyName ("holder")]
        public int HolderTag { get; set; } = 902;

        /// <summary>
        /// Метка поля для графических данных.
        /// </summary>
        [XmlAttribute ("image")]
        [Category (IrbisRecord)]
        [DefaultValue (950)]
        [JsonPropertyName ("image")]
        public int ImageTag { get; set; } = 950;

        /// <summary>
        /// Метка поля для шифра документа.
        /// </summary>
        [XmlAttribute ("index")]
        [Category (IrbisRecord)]
        [DefaultValue (903)]
        [JsonPropertyName ("index")]
        public int IndexTag { get; set; } = 903;

        /// <summary>
        /// Метка поля для ISBN и цены.
        /// </summary>
        [XmlAttribute ("isbn")]
        [Category (IrbisRecord)]
        [DefaultValue (10)]
        [JsonPropertyName ("isbn")]
        public int IsbnTag { get; set; } = 10;

        /// <summary>
        /// Метка поля для ISSN.
        /// </summary>
        [XmlAttribute ("issn")]
        [Category (IrbisRecord)]
        [DefaultValue (11)]
        [JsonPropertyName ("issn")]
        public int IssnTag { get; set; } = 11;

        /// <summary>
        /// Метка поля для раздела знаний.
        /// </summary>
        [XmlAttribute ("knowledge")]
        [Category (IrbisRecord)]
        [DefaultValue (60)]
        [JsonPropertyName ("knowledge")]
        public int KnowledgeTag { get; set; } = 60;

        /// <summary>
        /// Метка поля для кода языка основного текста.
        /// </summary>
        [XmlAttribute ("language")]
        [Category (IrbisRecord)]
        [DefaultValue (101)]
        [JsonPropertyName ("language")]
        public int LanguageTag { get; set; } = 101;

        /// <summary>
        /// Метка поля с информацией о каталогизаторе и дате обработки.
        /// </summary>
        [XmlAttribute ("operator")]
        [Category (IrbisRecord)]
        [DefaultValue (907)]
        [JsonPropertyName ("operator")]
        public int OperatorTag { get; set; } = 907;

        /// <summary>
        /// Метка поля для количества выдач документа.
        /// </summary>
        [XmlAttribute ("rental")]
        [Category (IrbisRecord)]
        [DefaultValue (999)]
        [JsonPropertyName ("rental")]
        public int RentalTag { get; set; } = 999;

        /// <summary>
        /// Метка поля для внутреннего двоичного ресурса.
        /// </summary>
        [XmlAttribute ("resource")]
        [Category (IrbisRecord)]
        [DefaultValue (953)]
        [JsonPropertyName ("resource")]
        public int ResourceTag { get; set; } = 953;

        /// <summary>
        /// Метка поля для рабочего листа (вида документа).
        /// </summary>
        [XmlAttribute ("worksheet")]
        [Category (IrbisRecord)]
        [DefaultValue (920)]
        [JsonPropertyName ("worksheet")]
        public int WorksheetTag { get; set; } = 920;

        #endregion

        #region Public methods

        /// <summary>
        /// Получение кода страны.
        /// </summary>
        [Pure]
        public string? GetCountryCode (Record record, string? defaultValue = null) => record.FM (CountryTag) ?? defaultValue;

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
            var result = record.FMA (CountryTag);
            if (result.Length == 0 && !string.IsNullOrEmpty (defaultValue))
            {
                return new[] { defaultValue };
            }

            return result;

        } // method GetCountryCodes

        /// <summary>
        /// Получение поля для настройки библиографической записи.
        /// </summary>
        [Pure]
        public Field? GetCustomizationField (Record record) => record.GetFirstField (CustomizationTag);

        /// <summary>
        /// Получение конфигурации по умолчанию.
        /// </summary>
        [Pure]
        public static RecordConfiguration GetDefault() => new ();

        /// <summary>
        /// Получение полей с экземплярами.
        /// </summary>
        public Field[] GetExemplarFields (Record record) => record.Fields.GetField (ExemplarTag);

        /// <summary>
        /// Получение экземпляров документа.
        /// </summary>
        public ExemplarInfo[] GetExemplars (Record record) => ExemplarInfo.Parse (record, ExemplarTag);

        /// <summary>
        /// Получение поля с держателем документа.
        /// </summary>
        [Pure]
        public Field? GetHolderField (Record record) => record.GetFirstField (HolderTag);

        /// <summary>
        /// Получение поля с держателем документа.
        /// </summary>
        public Field[] GetHolderFields (Record record) => record.Fields.GetField (HolderTag);

        /// <summary>
        /// Получение поля с графическими данными.
        /// </summary>
        [Pure]
        public Field? GetImageField (Record record) => record.GetFirstField (ImageTag);

        /// <summary>
        /// Получение поля с графическими данными.
        /// </summary>
        public Field[] GetImageFields (Record record) => record.Fields.GetField (ImageTag);

        /// <summary>
        /// Получение поля с шифром документа.
        /// </summary>
        [Pure]
        public string? GetIndex (Record record) => record.FM (IndexTag);

        /// <summary>
        /// Получение поля с ISBN.
        /// </summary>
        [Pure]
        public Field? GetIsbnField (Record record) => record.GetFirstField (IsbnTag);

        /// <summary>
        /// Получение поля с ISSN.
        /// </summary>
        [Pure]
        public Field? GetIssnField (Record record) => record.GetFirstField (IssnTag);

        /// <summary>
        /// Получение раздела знаний.
        /// </summary>
        [Pure]
        public string? GetKnowledgeSection (Record record) => record.FM (KnowledgeTag);

        /// <summary>
        /// Получение кода языка основного текста.
        /// </summary>
        [Pure]
        public string? GetLanguageCode (Record record, string? defaultValue = null) => record.FM (LanguageTag) ?? defaultValue;

        /// <summary>
        /// Получение кодов языка основного текста.
        /// </summary>
        public string[] GetLanguageCodes
            (
                Record record,
                string? defaultValue = null
            )
        {
            var result = record.FMA (LanguageTag);
            if (result.Length == 0 && !string.IsNullOrEmpty (defaultValue))
            {
                return new[] { defaultValue };
            }

            return result;

        } // method GetLanguageCodes

        /// <summary>
        /// Получение полей с информацией о каталогизаторе и дате обработки.
        /// </summary>
        public Field[] GetOperatorFields (Record record) => record.Fields.GetField (OperatorTag);

        /// <summary>
        /// Получение информации о редактировании записи.
        /// </summary>
        public Technology[] GetTechnology (Record record) => Technology.Parse (record, OperatorTag);

        /// <summary>
        /// Получение количества выдач документа.
        /// </summary>
        [Pure]
        public int GetRentalCount (Record record) => record.FM (RentalTag).SafeToInt32();

        /// <summary>
        /// Получение поля с внутренним двоичном ресурсом.
        /// </summary>
        [Pure]
        public Field? GetResourceField (Record record) => record.GetFirstField (ResourceTag);

        /// <summary>
        /// Получение поля с внутренним двоичном ресурсом.
        /// </summary>
        public Field[] GetResourceFields (Record record) => record.Fields.GetField (ResourceTag);

        /// <summary>
        /// Получение кода рабочего листа (вида документа).
        /// </summary>
        [Pure]
        public string? GetWorksheet (Record record, string? defaultValue = null) => record.FM (WorksheetTag) ?? defaultValue;

        /// <summary>
        /// Чтение конфигурации из указанного файла.
        /// </summary>
        public static RecordConfiguration LoadConfiguration (string fileName) =>
            JsonUtility.ReadObjectFromFile<RecordConfiguration> (fileName);

        /// <summary>
        /// Запись конфигурации в указанный файл.
        /// </summary>
        public void SaveConfiguration (string fileName) => JsonUtility.SaveObjectToFile (this, fileName);

        #endregion

    } // class RecordConfiguration

} // namespace ManagedIrbis.Records
