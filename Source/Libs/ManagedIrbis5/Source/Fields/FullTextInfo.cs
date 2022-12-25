// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement

/* FullTextInfo.cs -- сведения о полном тексте документа, поле 955
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Fields;

//
// Начиная с версии 2018.1
//

//
// В структуру БД ЭК ИРБИС64+ по сравнению с ИРБИС64
// введено одно новое поле, предназначенное для описания
// ПОЛНОГО ТЕКСТА исходного документа (не путать
// с полем 951 – ВНЕШНИЙ ОБЪЕКТ, которое сохраняется
// в структуре БД ЭК в прежнем виде).
// Метка поля – 955.
// Поле – НЕПОВТОРЯЮЩЕЕСЯ.
// Включает в себя следующие подполя:
// A – имя файла полного текста с расширением PDF
// (расширение указывается обязательно);
// B – идентификатор записи права доступа (см. ниже);
// N – количество физических страниц полного текста
// (формируется системой автоматически).
//
// Файл полного текста должен быть распознанным
// (т.е. иметь текстовый слой) PDF-файлом, поддающимся
// разбиению на страницы. Имя файла не может содержать
// символы «запятая», «кавычки», двойные подчеркивания,
// квадратные и фигурные скобки. Максимальная длина
// имени файла - 64 символа. Не рекомендуется использовать
// в именах файлов кириллические символы. Файл полного
// текста должен находиться по пути, который указан в
// 11 строке параметрического файла <имя_БД>.par или
// в подпапках по этому пути – в последнем случае
// в подполе А 955 поля необходимо указывать относительный
// путь, начинающийся со слэша (все слэши в относительном
// пути должны быть ОБРАТНЫМИ). Имя файла должно быть
// УНИКАЛЬНЫМ в рамках одной БД.
//

/// <summary>
/// Сведения о полном тексте документа (поле 955).
/// </summary>
[XmlRoot ("fulltext")]
public sealed class FullTextInfo
    : IHandmadeSerializable,
    IVerifiable
{
    #region Constants

    /// <summary>
    /// Метка поля.
    /// </summary>
    public const int Tag = 955;

    /// <summary>
    /// Известные коды подполей.
    /// </summary>
    public const string KnownCodes = "abnt";

    #endregion

    #region Properties

    /// <summary>
    /// Текст для ссылки. Подполе T.
    /// </summary>
    [SubField ('t')]
    [XmlAttribute ("display-text")]
    [JsonPropertyName ("displayText")]
    [Description ("Текст для ссылки")]
    [DisplayName ("Текст для ссылки")]
    public string? DisplayText { get; set; }

    /// <summary>
    /// Имя файла (PDF). Подполе A.
    /// </summary>
    [SubField ('a')]
    [XmlAttribute ("filename")]
    [JsonPropertyName ("filename")]
    [Description ("Имя файла")]
    [DisplayName ("Имя файла")]
    public string? FileName { get; set; }

    /// <summary>
    /// Количество страниц. Подполе N.
    /// Формируется автоматически.
    /// </summary>
    [SubField ('n')]
    [XmlAttribute ("page-count")]
    [JsonPropertyName ("pageCount")]
    [Description ("Количество страниц")]
    [DisplayName ("Количество страниц")]
    public string? PageCount { get; set; }

    /// <summary>
    /// Идентификатор записи права доступа. Подполе B.
    /// Запись расположена в базе данных RIGHT,
    /// отыскивается по поисковому префиксу "I=".
    /// </summary>
    [SubField ('b')]
    [XmlAttribute ("access-rights")]
    [JsonPropertyName ("accessRights")]
    [Description ("Идентификатор записи права доступа")]
    [DisplayName ("Идентификатор записи права доступа")]
    public string? AccessRights { get; set; }

    /// <summary>
    /// Неизвестные подполя.
    /// </summary>
    [XmlElement ("unknown")]
    [JsonPropertyName ("unknown")]
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
    /// Применение данных к указанному полю библиографической записи.
    /// </summary>
    public void ApplyTo
        (
            Field field
        )
    {
        Sure.NotNull (field);

        field
            .SetSubFieldValue ('t', DisplayText)
            .SetSubFieldValue ('a', FileName)
            .SetSubFieldValue ('n', PageCount)
            .SetSubFieldValue ('b', AccessRights);
    }

    /// <summary>
    /// Разбор указанного поля библиографической записи.
    /// </summary>
    public static FullTextInfo ParseField
        (
            Field field
        )
    {
        Sure.NotNull (field);

        return new FullTextInfo
        {
            DisplayText = field.GetFirstSubFieldValue ('t'),
            FileName = field.GetFirstSubFieldValue ('a'),
            PageCount = field.GetFirstSubFieldValue ('n'),
            AccessRights = field.GetFirstSubFieldValue ('b'),
            Field = field
        };
    }

    /// <summary>
    /// Разбор указанной библиографической записи.
    /// </summary>
    public static FullTextInfo[] ParseRecord
        (
            Record record,
            int tag = Tag
        )
    {
        Sure.NotNull (record);
        Sure.Positive (tag);

        return record
            .EnumerateField (tag)
            .Select (ParseField)
            .ToArray();
    }

    /// <summary>
    /// Преобразование данных в поле библиографической записи.
    /// </summary>
    public Field ToField()
    {
        var result = new Field (Tag)
            .AddNonEmpty ('t', DisplayText)
            .AddNonEmpty ('a', FileName)
            .AddNonEmpty ('n', PageCount)
            .AddNonEmpty ('b', AccessRights);

        return result;
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

        DisplayText = reader.ReadNullableString();
        FileName = reader.ReadNullableString();
        AccessRights = reader.ReadNullableString();
        PageCount = reader.ReadNullableString();
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
            .WriteNullable (DisplayText)
            .WriteNullable (FileName)
            .WriteNullable (AccessRights)
            .WriteNullable (PageCount)
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
        var verifier = new Verifier<FullTextInfo> (this, throwOnError);

        verifier
            .NotNullNorEmpty (FileName);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString() => FileName.ToVisibleString();

    #endregion
}
