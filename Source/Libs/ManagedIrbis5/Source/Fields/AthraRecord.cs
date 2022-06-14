// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* AthraRecord.cs -- запись в базе данных ATHRA
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Fields;

/// <summary>
/// Запись в базе данных ATHRA.
/// </summary>
public sealed class AthraRecord
{
    #region Properties

    /// <summary>
    /// Заголовок - Основное (принятое) имя лица.
    /// Поле 210.
    /// </summary>
    [Field (210)]
    [XmlElement ("mainTitle")]
    [JsonPropertyName ("mainTitle")]
    public AthraTitle? MainTitle { get; set; }

    /// <summary>
    /// Место работы автора.
    /// Поле 910.
    /// </summary>
    [Field (910)]
    [XmlElement ("workPlace")]
    [JsonPropertyName ("workPlaces")]
    [DisplayName ("Место работы автора")]
    public AthraWorkPlace[]? WorkPlaces { get; set; }

    /// <summary>
    /// Ссылки типа СМ. (вариантные (другие) принятые
    /// формы имени лица).
    /// Поле 410.
    /// </summary>
    [Field (410)]
    [XmlElement ("see")]
    [JsonPropertyName ("see")]
    [DisplayName ("Ссылки типа СМ.")]
    public AthraSee[]? See { get; set; }

    /// <summary>
    /// Ссылки типа СМ. ТАКЖЕ. (связанные принятые формы
    /// имени лица).
    /// Поле 510.
    /// </summary>
    [Field (510)]
    [XmlElement ("seeAlso")]
    [JsonPropertyName ("seeAlso")]
    [DisplayName ("Ссылки типа СМ. ТАКЖЕ")]
    public AthraSee[]? SeeAlso { get; set; }

    /// <summary>
    /// Связанные принятые формы имени лица на других языках.
    /// Поле 710.
    /// </summary>
    [Field (710)]
    [XmlElement ("linkedTitle")]
    [JsonPropertyName ("linkedTitles")]
    [DisplayName ("Связанные принятые формы имени лица на других языках")]
    public AthraLinkedTitle[]? LinkedTitles { get; set; }

    /// <summary>
    /// Информационное примечание.
    /// Поле 300.
    /// </summary>
    [Field (300)]
    [XmlElement ("note")]
    [JsonPropertyName ("notes")]
    [DisplayName ("Информационное примечание")]
    public string[]? Notes { get; set; }

    /// <summary>
    /// Текстовое ссылочное примечание "см. также".
    /// Поле 305.
    /// </summary>
    [Field (305)]
    [XmlElement ("seeAlsoNote")]
    [JsonPropertyName ("seeAlsoNotes")]
    [DisplayName ("Текстовое ссылочное примечание СМ. ТАКЖЕ")]
    public object[]? SeeAlsoNote { get; set; }

    /// <summary>
    /// Примечания об области применения.
    /// Поле 330.
    /// </summary>
    [Field (330)]
    [XmlElement ("usageNote")]
    [JsonPropertyName ("usageNotes")]
    [DisplayName ("Примечания об области применения")]
    public object[]? UsageNotes { get; set; }

    /// <summary>
    /// Источник составления записи.
    /// Поле 801.
    /// </summary>
    [Field (801)]
    [XmlElement ("informationSource")]
    [JsonPropertyName ("informaionSources")]
    [DisplayName ("Источник составления записи")]
    public object[]? InformationSources { get; set; }

    /// <summary>
    /// Источник, в котором выявлена информ. о заголовке.
    /// Поле 810.
    /// </summary>
    [Field (810)]
    [XmlElement ("identificationSource")]
    [JsonPropertyName ("identificationSources")]
    [DisplayName ("Источник, в котором выявлена информация")]
    public object[]? IdentificationSources { get; set; }

    /// <summary>
    /// Источник, в котором не выявлена информ. о заголовке.
    /// Поле 815.
    /// </summary>
    [Field (815)]
    [XmlElement ("nonIdentificationSource")]
    [JsonPropertyName ("nonIdentificationSources")]
    [DisplayName ("Источник, в котором не выявлена информация")]
    public object[]? NonIdentificationSources { get; set; }

    /// <summary>
    /// Информации об использовании заголовка в поле 200.
    /// Поле 820.
    /// </summary>
    [Field (820)]
    [XmlElement ("usageInformation")]
    [JsonPropertyName ("usageInformation")]
    [DisplayName ("Информация об использовании")]
    public object[]? UsageInformation { get; set; }

    /// <summary>
    /// Пример,приведенный в примечании.
    /// Поле 825.
    /// </summary>
    [Field (825)]
    [XmlElement ("example")]
    [JsonPropertyName ("examples")]
    [DisplayName ("Примеры")]
    public object[]? Examples { get; set; }

    /// <summary>
    /// Общее примечание каталогизатора.
    /// Поле 830.
    /// </summary>
    [Field (830)]
    [XmlElement ("cataloguerNote")]
    [JsonPropertyName ("cataloguerNotes")]
    [DisplayName ("Общие примечания")]
    public object[]? CataloguerNotes { get; set; }

    /// <summary>
    /// Информация об исключении принятого имени лица.
    /// Поле 835.
    /// </summary>
    [Field (835)]
    [XmlElement ("exclusionInformation")]
    [JsonPropertyName ("exclusionInformation")]
    [DisplayName ("Информация об исключении")]
    public object[]? ExclusionInformation { get; set; }

    /// <summary>
    /// Правила каталогизации и предметизации.
    /// Поле 152.
    /// </summary>
    [Field (152)]
    [XmlElement ("cataloguingRules")]
    [JsonPropertyName ("cataloguingRules")]
    [DisplayName ("Правила каталогизации и предметизации")]
    public object? CataloguingRules { get; set; }

    /// <summary>
    /// Каталогизатор, дата.
    /// Поле 907.
    /// </summary>
    [Field (907)]
    [XmlElement ("technology")]
    [JsonPropertyName ("technology")]
    [DisplayName ("Каталогизатор, дата")]
    public object[]? Technology { get; set; }

    /// <summary>
    /// Имя рабочего листа.
    /// Поле 920.
    /// </summary>
    [Field (920)]
    [XmlElement ("worksheet")]
    [JsonPropertyName ("worksheet")]
    [DisplayName ("Рабочий лист")]
    public string? Worksheet { get; set; }

    /// <summary>
    /// Ассоциированная запись <see cref="Record" />.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public Record? Record { get; set; }

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
    /// Разбор библиографической записи <see cref="Record"/>.
    /// </summary>
    public void ParseRecord
        (
            Record record
        )
    {
        Sure.NotNull (record);

        MainTitle = AthraTitle.ParseField (record.GetField (AthraTitle.Tag));
        WorkPlaces = AthraWorkPlace.ParseRecord (record);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return MainTitle.ToVisibleString();
    }

    #endregion
}
