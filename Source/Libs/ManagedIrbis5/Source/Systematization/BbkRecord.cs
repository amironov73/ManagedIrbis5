// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* BbkRecord.cs -- запись в базе данных RSBBK
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Systematization;

/// <summary>
/// Запись в базе данных RSBBK.
/// </summary>
[XmlRoot ("record")]
public sealed class BbkRecord
    : IVerifiable
{
    #region Properties

    /// <summary>
    /// Вид записи.
    /// Поле 1.
    /// </summary>
    [Field (1)]
    [XmlElement ("kind")]
    [JsonPropertyName ("kind")]
    [Description ("Вид записи")]
    public string? RecordKind { get; set; }

    /// <summary>
    /// Надрубрика.
    /// Поле 2.
    /// </summary>
    [Field (2)]
    [XmlElement ("super")]
    [JsonPropertyName ("super")]
    [Description ("Надрубрика")]
    public string? SuperHeading { get; set; }

    /// <summary>
    /// Заглавный индекс.
    /// Поле 3.
    /// </summary>
    [Field (3)]
    [XmlElement ("main-index")]
    [JsonPropertyName ("mainIndex")]
    [Description ("Заглавный индекс")]
    public string? MainIndex { get; set; }

    /// <summary>
    /// Заглавная рубрика.
    /// Поле 4.
    /// </summary>
    [Field (4)]
    [XmlElement ("main-heading")]
    [JsonPropertyName ("mainHeading")]
    [Description ("Заглавная рубрика")]
    public string? MainHeading { get; set; }

    /// <summary>
    /// Расширение заглавной рубрики.
    /// Поле 5.
    /// </summary>
    [Field (5)]
    [XmlElement ("main-extension")]
    [JsonPropertyName ("mainExtension")]
    [Description ("Расширение заглавной рубрики")]
    public string? MainExtension { get; set; }

    /// <summary>
    /// Поисковая форма заглавного индекса.
    /// Поле 6.
    /// </summary>
    [Field (6)]
    [XmlElement ("search-query")]
    [JsonPropertyName ("searchQuery")]
    [Description ("Поисковая формат заглавного индекса")]
    public string? SearchQuery { get; set; }

    /// <summary>
    /// Дата и основание исключения.
    /// Поле 7.
    /// </summary>
    [Field (7)]
    [XmlElement ("exclusion-date")]
    [JsonPropertyName ("exclusionDate")]
    [Description ("Дата и основание исключения")]
    public string? ExclusionDate { get; set; }

    /// <summary>
    /// Методические указания.
    /// Поле 8.
    /// </summary>
    [Field (8)]
    [XmlElement ("methodical-instruction")]
    [JsonPropertyName ("methodicalInstructions")]
    [Description ("Методические указания")]
    public string[]? MethodicalInstructions { get; set; }

    /// <summary>
    /// Фасетно-методические указания.
    /// Поле 9.
    /// </summary>
    [Field (9)]
    [XmlElement ("faceted-instruction")]
    [JsonPropertyName ("facetedInstructions")]
    [Description ("Фасетно-методические указания")]
    public string[]? FacetedInstructions { get; set; }

    /// <summary>
    /// Отсылки "Смотри".
    /// Поле 10.
    /// </summary>
    [Field (10)]
    [XmlElement ("see")]
    [JsonPropertyName ("see")]
    [Description ("Отсылка 'смотри'")]
    public BbkReference[]? SeeReferences { get; set; }

    /// <summary>
    /// Ссылки "Смотри также".
    /// Поле 11.
    /// </summary>
    [Field (11)]
    [XmlElement ("also")]
    [JsonPropertyName ("also")]
    [Description ("Ссылка 'смотри также")]
    public BbkReference[]? SeeAlsoReferences { get; set; }

    /// <summary>
    /// Раскрытие.
    /// Поле 12.
    /// </summary>
    [Field (12)]
    [XmlElement ("expansion")]
    [JsonPropertyName ("expansion")]
    [Description ("Раскрытие")]
    public string[]? Expansion { get; set; }

    /// <summary>
    /// Смежные области.
    /// Поле 13.
    /// </summary>
    [Field (13)]
    [XmlElement ("adjacent")]
    [JsonPropertyName ("adjacent")]
    [Description ("Смежные области")]
    public string[]? AdjacentAreas { get; set; }

    /// <summary>
    /// Области применения.
    /// Поле 14.
    /// </summary>
    [Field (14)]
    [XmlElement ("application")]
    [JsonPropertyName ("application")]
    [Description ("Области применения")]
    public string[]? ApplicationAreas { get; set; }

    /// <summary>
    /// Заменяющий индекс.
    /// Поле 15.
    /// </summary>
    [Field (15)]
    [XmlElement ("substitute")]
    [JsonPropertyName ("substitute")]
    [Description ("Заменяющий индекс")]
    public string[]? SubstituteIndex { get; set; }

    /// <summary>
    /// Номер продолжающей записи.
    /// Поле 16.
    /// </summary>
    [Field (16)]
    [XmlElement ("continuing")]
    [JsonPropertyName ("continuing")]
    [Description ("Номер продолжающей записи")]
    public string? ContinuingRecordNumber { get; set; }

    /// <summary>
    /// Поисковый образ на ИЯ ГРНТИ.
    /// Поле 17.
    /// </summary>
    [Field (17)]
    [XmlElement ("grnti")]
    [JsonPropertyName ("grnti")]
    [Description ("ГРНТИ")]
    public string[]? Grnti { get; set; }

    /// <summary>
    /// Поисковый образ на ИЯ Дьюи.
    /// Поле 18.
    /// </summary>
    [Field (18)]
    [XmlElement ("dewey")]
    [JsonPropertyName ("dewey")]
    [Description ("Дьюи")]
    public string[]? Dewey { get; set; }

    /// <summary>
    /// Поисковый образ на ИЯ УДК.
    /// Поле 19.
    /// </summary>
    [Field (19)]
    [XmlElement ("udc")]
    [JsonPropertyName ("udc")]
    [Description ("УДК")]
    public string[]? Udc { get; set; }

    /// <summary>
    /// Поисковый образ на ИЯ МТ.
    /// Поле 20.
    /// </summary>
    [Field (20)]
    [XmlElement ("mt")]
    [JsonPropertyName ("mt")]
    [Description ("Поисковый образ на ИЯ МТ")]
    public string[]? MT { get; set; }

    /// <summary>
    /// Дата и составитель записи.
    /// Поле 21.
    /// </summary>
    [Field (21)]
    [XmlElement ("composition")]
    [JsonPropertyName ("composition")]
    [Description ("Дата и составитель записи")]
    public string? Composition { get; set; }

    /// <summary>
    /// Дата, оператор, вид корректуры.
    /// Поле 22.
    /// </summary>
    [Field (22)]
    [XmlElement ("correcture")]
    [JsonPropertyName ("correcture")]
    [Description ("Дата, оператор, вид корректуры")]
    public string[]? Correcture { get; set; }

    /// <summary>
    /// Рабочая схема.
    /// Поле 23.
    /// </summary>
    [Field (23)]
    [XmlElement ("working-scheme")]
    [JsonPropertyName ("workingScheme")]
    [Description ("Рабочая схема")]
    public string[]? WorkingScheme { get; set; }

    /// <summary>
    /// Обратные отсылки.
    /// Поле 24.
    /// </summary>
    [Field (24)]
    [XmlElement ("back-reference")]
    [JsonPropertyName ("backReferences")]
    [Description ("Обратные отсылки")]
    public BbkReference[]? BackReferences { get; set; }

    /// <summary>
    /// Дефисные конструкции.
    /// Поле 505.
    /// </summary>
    [Field (505)]
    [XmlElement ("hyphens")]
    [JsonPropertyName ("hyphens")]
    [Description ("Дефисные конструкции")]
    public string? Hyphens { get; set; }

    /// <summary>
    /// Ассоциированная библиографическая запись.
    /// </summary>
    public Record? Record { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Parse the record.
    /// </summary>
    public static BbkRecord Parse
        (
            Record record
        )
    {
        // TODO: реализовать оптимально

        var result = new BbkRecord
        {
            RecordKind = record.FM (1),
            SuperHeading = record.FM (2),
            MainIndex = record.FM (3),
            MainHeading = record.FM (4),
            MainExtension = record.FM (5),
            SearchQuery = record.FM (6),
            ExclusionDate = record.FM (7),
            MethodicalInstructions = record.FMA (8),
            FacetedInstructions = record.FMA (9),
            SeeReferences = record.Fields
                .GetField (10)
                .Select (field => BbkReference.ParseField (field))
                .ToArray(),
            SeeAlsoReferences = record.Fields
                .GetField (11)
                .Select (field => BbkReference.ParseField (field))
                .ToArray(),
            Expansion = record.FMA (12),
            AdjacentAreas = record.FMA (13),
            ApplicationAreas = record.FMA (14),
            SubstituteIndex = record.FMA (15),
            ContinuingRecordNumber = record.FM (16),
            Grnti = record.FMA (17),
            Dewey = record.FMA (18),
            Udc = record.FMA (19),
            MT = record.FMA (20),
            Composition = record.FM (21),
            Correcture = record.FMA (22),
            WorkingScheme = record.FMA (23),
            BackReferences = record.Fields
                .GetField (24)
                .Select (field => BbkReference.ParseField (field))
                .ToArray(),
            Hyphens = record.FM (505)
        };

        return result;
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<BbkRecord> (this, throwOnError);

        verifier
            .NotNullNorEmpty (MainHeading);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return MainHeading.ToVisibleString();
    }

    #endregion
}
