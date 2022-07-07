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
using System.Text.RegularExpressions;
using System.Xml.Serialization;

using AM;
using AM.Json;

using ManagedIrbis.Fields;

#endregion

#nullable enable

namespace ManagedIrbis.Records;

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
    [Description ("Формат полного просмотра")]
    public string AllFormat { get; set; } = "@all";

    /// <summary>
    /// Имя краткого (в одну строку) формата показа.
    /// </summary>
    [XmlAttribute ("brief")]
    [Category (IrbisRecord)]
    [DefaultValue ("@brief")]
    [JsonPropertyName ("brief")]
    [Description ("Формат краткого просмотра")]
    public string BriefFormat { get; set; } = "@brief";

    /// <summary>
    /// Вид содержания.
    /// </summary>
    [XmlAttribute ("content-type")]
    [Category (IrbisRecord)]
    [DefaultValue (181)]
    [JsonPropertyName ("contentType")]
    [Description ("Вид содержания")]
    public int ContentTypeTag { get; set; } = 181;

    /// <summary>
    /// Метка поля для кода страны.
    /// </summary>
    [XmlAttribute ("country")]
    [Category (IrbisRecord)]
    [DefaultValue (102)]
    [JsonPropertyName ("country")]
    [Description ("Код страны")]
    public int CountryTag { get; set; } = 102;

    /// <summary>
    /// Метка поля для настройки библиографической записи.
    /// </summary>
    [XmlAttribute ("customization")]
    [Category (IrbisRecord)]
    [DefaultValue (905)]
    [JsonPropertyName ("customization")]
    [Description ("Настройка записи")]
    public int CustomizationTag { get; set; } = 905;

    /// <summary>
    /// Метка поля для экземпляров.
    /// </summary>
    [XmlAttribute ("exemplar")]
    [Category (IrbisRecord)]
    [DefaultValue (910)]
    [JsonPropertyName ("exemplar")]
    [Description ("Экземпляр")]
    public int ExemplarTag { get; set; } = 910;

    /// <summary>
    /// Метка поля для полных текстов.
    /// </summary>
    [XmlAttribute ("fulltext")]
    [Category (IrbisRecord)]
    [DefaultValue (951)]
    [JsonPropertyName ("fulltext")]
    [Description ("Полный текст")]
    public int FullTextTag { get; set; } = 951;

    /// <summary>
    /// Метка поля для держателя документа.
    /// </summary>
    [XmlAttribute ("holder")]
    [Category (IrbisRecord)]
    [DefaultValue (902)]
    [JsonPropertyName ("holder")]
    [Description ("Держатель")]
    public int HolderTag { get; set; } = 902;

    /// <summary>
    /// Метка поля для графических данных.
    /// </summary>
    [XmlAttribute ("image")]
    [Category (IrbisRecord)]
    [DefaultValue (950)]
    [JsonPropertyName ("image")]
    [Description ("Графические данные")]
    public int ImageTag { get; set; } = 950;

    /// <summary>
    /// Метка поля для шифра документа.
    /// </summary>
    [XmlAttribute ("index")]
    [Category (IrbisRecord)]
    [DefaultValue (903)]
    [JsonPropertyName ("index")]
    [Description ("Шифр документа")]
    public int IndexTag { get; set; } = 903;

    /// <summary>
    /// Метка поля для ISBN и цены.
    /// </summary>
    [XmlAttribute ("isbn")]
    [Category (IrbisRecord)]
    [DefaultValue (10)]
    [JsonPropertyName ("isbn")]
    [Description ("ISBN")]
    public int IsbnTag { get; set; } = 10;

    /// <summary>
    /// Метка поля для ISSN.
    /// </summary>
    [XmlAttribute ("issn")]
    [Category (IrbisRecord)]
    [DefaultValue (11)]
    [JsonPropertyName ("issn")]
    [Description ("ISSN")]
    public int IssnTag { get; set; } = 11;

    /// <summary>
    /// Метка поля для раздела знаний.
    /// </summary>
    [XmlAttribute ("knowledge")]
    [Category (IrbisRecord)]
    [DefaultValue (60)]
    [JsonPropertyName ("knowledge")]
    [Description ("Раздел знаний")]
    public int KnowledgeTag { get; set; } = 60;

    /// <summary>
    /// Метка поля для кода языка основного текста.
    /// </summary>
    [XmlAttribute ("language")]
    [Category (IrbisRecord)]
    [DefaultValue (101)]
    [JsonPropertyName ("language")]
    [Description ("Язык основного текста")]
    public int LanguageTag { get; set; } = 101;

    /// <summary>
    /// Метка поля с информацией о каталогизаторе и дате обработки.
    /// </summary>
    [XmlAttribute ("operator")]
    [Category (IrbisRecord)]
    [DefaultValue (907)]
    [JsonPropertyName ("operator")]
    [Description ("Технология")]
    public int OperatorTag { get; set; } = 907;

    /// <summary>
    /// Метка поля для количества выдач документа.
    /// </summary>
    [XmlAttribute ("rental")]
    [Category (IrbisRecord)]
    [DefaultValue (999)]
    [JsonPropertyName ("rental")]
    [Description ("Количество выдач")]
    public int RentalTag { get; set; } = 999;

    /// <summary>
    /// Метка поля для внутреннего двоичного ресурса.
    /// </summary>
    [XmlAttribute ("resource")]
    [Category (IrbisRecord)]
    [DefaultValue (953)]
    [JsonPropertyName ("resource")]
    [Description ("Двоичный ресурс")]
    public int ResourceTag { get; set; } = 953;

    /// <summary>
    /// Метка для поля 203 ("текст: непосредственный").
    /// </summary>
    [XmlAttribute ("field-203")]
    [Category (IrbisRecord)]
    [DefaultValue (203)]
    [JsonPropertyName ("field203")]
    [Description ("Вид содержания")]
    public int Tag203 { get; set; } = 203;

    /// <summary>
    /// Метка поля для рабочего листа (вида документа).
    /// </summary>
    [XmlAttribute ("worksheet")]
    [Category (IrbisRecord)]
    [DefaultValue (920)]
    [JsonPropertyName ("worksheet")]
    [Description ("Рабочий лист")]
    public int WorksheetTag { get; set; } = 920;

    #endregion

    #region Public methods

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

        return field is null
            ? null
            : Field203.ParseField (field);
    }

    /// <summary>
    /// Получение типа контента (поле 181).
    /// </summary>
    public ContentType? GetContentType
        (
            Record record
        )
    {
        Sure.NotNull (record);

        var field = record.GetField (ContentTypeTag);

        return field is null
            ? null
            : ContentType.ParseField (field);
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
    /// Получение поля для настройки библиографической записи.
    /// </summary>
    [Pure]
    public Field? GetCustomizationField
        (
            Record record
        )
    {
        Sure.NotNull (record);

        return record.GetFirstField (CustomizationTag);
    }

    /// <summary>
    /// Получение конфигурации по умолчанию.
    /// </summary>
    [Pure]
    public static RecordConfiguration GetDefault()
    {
        return new ();
    }

    /// <summary>
    /// Получение полей с экземплярами.
    /// </summary>
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
    public ExemplarInfo[] GetExemplars
        (
            Record record
        )
    {
        Sure.NotNull (record);

        return ExemplarInfo.ParseRecord (record, ExemplarTag);
    }

    /// <summary>
    /// Получение поля с держателем документа.
    /// </summary>
    [Pure]
    public Field? GetHolderField
        (
            Record record
        )
    {
        Sure.NotNull (record);

        return record.GetFirstField (HolderTag);
    }

    /// <summary>
    /// Получение поля с держателем документа.
    /// </summary>
    public Field[] GetHolderFields
        (
            Record record
        )
    {
        Sure.NotNull (record);

        return record.Fields.GetField (HolderTag);
    }

    /// <summary>
    /// Получение поля с графическими данными.
    /// </summary>
    [Pure]
    public Field? GetImageField
        (
            Record record
        )
    {
        Sure.NotNull (record);

        return record.GetFirstField (ImageTag);
    }

    /// <summary>
    /// Получение поля с графическими данными.
    /// </summary>
    public Field[] GetImageFields
        (
            Record record
        )
    {
        Sure.NotNull (record);

        return record.Fields.GetField (ImageTag);
    }

    /// <summary>
    /// Получение поля с шифром документа.
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
    /// Получение массива полей с ISBN.
    /// </summary>
    [Pure]
    public Field[] GetIsbnFields
        (
            Record record
        )
    {
        Sure.NotNull (record);

        return record.Fields.GetField (IsbnTag);
    }

    /// <summary>
    /// Получение массива ISBN.
    /// </summary>
    public IsbnInfo[] GetIsbn
        (
            Record record
        )
    {
        Sure.NotNull (record);

        return IsbnInfo.ParseRecord (record, IsbnTag);
    }

    /// <summary>
    /// Получение массива полей с ISSN.
    /// </summary>
    [Pure]
    public Field[] GetIssnFields
        (
            Record record
        )
    {
        Sure.NotNull (record);

        return record.Fields.GetField (IssnTag);
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
    /// Получение раздела знаний.
    /// </summary>
    [Pure]
    public string? GetKnowledgeSection
        (
            Record record
        )
    {
        Sure.NotNull (record);

        return record.FM (KnowledgeTag);
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
    /// Получение цены, общей для всех экземпляров.
    /// </summary>
    public string? GetPrice
        (
            Record record
        )
    {
        Sure.NotNull (record);

        return record.FM (10, 'd');
    }

    /// <summary>
    /// Получение цены для указанного экземпляра.
    /// Если таковой нет, то выдается общая цена.
    /// </summary>
    public string? GetPrice
        (
            Record record,
            ExemplarInfo exemplar
        )
    {
        Sure.NotNull (record);
        Sure.NotNull (exemplar);

        if (!exemplar.Price.IsEmpty())
        {
            return exemplar.Price;
        }

        return GetPrice (record);
    }

    /// <summary>
    /// Получение цены для указанного экземпляра.
    /// Если таковой нет, то выдается общая цена.
    /// </summary>
    public string? GetPrice
        (
            Record record,
            Field field
        )
    {
        Sure.NotNull (record);
        Sure.NotNull (field);

        var exemplar = ExemplarInfo.ParseField (field);

        return GetPrice (record, exemplar);
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
    /// Получение поля с внутренним двоичном ресурсом.
    /// </summary>
    [Pure]
    public Field? GetResourceField
        (
            Record record
        )
    {
        Sure.NotNull (record);

        return record.GetFirstField (ResourceTag);
    }

    /// <summary>
    /// Получение поля с внутренним двоичном ресурсом.
    /// </summary>
    public Field[] GetResourceFields
        (
            Record record
        )
    {
        Sure.NotNull (record);

        return record.Fields.GetField (ResourceTag);
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
    /// Проверка, всё ли нормально с рабочим листом?
    /// </summary>
    public bool WorksheetOK
        (
            Record record
        )
    {
        Sure.NotNull (record);

        if (record.GetFieldCount (WorksheetTag) != 1)
        {
            // должно быть только одно повторение
            return false;
        }

        var worksheet = GetWorksheet (record);

        return !string.IsNullOrWhiteSpace (worksheet);
    }

    /// <summary>
    /// Получение года издания (выхода из печати).
    /// </summary>
    public string? GetYear
        (
            Record record
        )
    {
        Sure.NotNull (record);

        var result = record.FM (210, 'd');
        if (result.IsEmpty())
        {
            result = record.FM (461, 'h');
        }

        if (result.IsEmpty())
        {
            result = record.FM (461, 'z');
        }

        if (result.IsEmpty())
        {
            var worksheet = GetWorksheet (record);
            if (worksheet.SameString ("NJ") // отдельный номер журнала
                || worksheet.SameString ("NJK") // подшивка
                || worksheet.SameString ("NJP")) // номер, входящий в подшивку
            {
                result = record.FM (934);
            }
        }

        if (result.IsEmpty())
        {
            return result;
        }

        var match = Regex.Match (result, @"\d{4}");
        if (match.Success)
        {
            result = match.Value;
        }

        return result;
    }

    /// <summary>
    /// Чтение конфигурации из указанного файла.
    /// </summary>
    public static RecordConfiguration LoadConfiguration
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        return JsonUtility.ReadObjectFromFile<RecordConfiguration> (fileName);
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
}
