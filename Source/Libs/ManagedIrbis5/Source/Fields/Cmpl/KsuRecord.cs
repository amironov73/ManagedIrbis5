// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* KsuRecord.cs -- запись о поступлении изданий в Книге Суммарного Учета в базе CMPL
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
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

namespace ManagedIrbis.Fields;

/// <summary>
/// Запись о поступлении изданий в Книге Суммарного Учета в базе CMPL.
/// </summary>
[XmlRoot ("ksu")]
public sealed class KsuRecord
    : IHandmadeSerializable,
    IVerifiable
{
    #region Properties

    /// <summary>
    /// Сведения о поступлении книг в библиотеку. Поле 88.
    /// </summary>
    [Field (88)]
    public object? ReceiptInfo { get; set; }


    /// <summary>
    /// Сведения о замене утерянных книг.
    /// </summary>
    [Field (80)]
    public object? LostBooksReplacement { get; set; }

    /// <summary>
    /// Этап работы, дата, ФИО.
    /// </summary>
    [Field (907)]
    public string? Phase { get; set; }

    /// <summary>
    /// Число наименований, поступивших впервые (баланс, не баланс, учебники).
    /// Поле 17.
    /// </summary>
    [Field (17)]
    public object? FirstReceived1 { get; set; }

    /// <summary>
    /// Число наименований, поступивших впервые.
    /// Поле 18.
    /// </summary>
    [Field (18)]
    public object? FirstReceived2 { get; set; }

    /// <summary>
    /// Число наименований, поступивших повторно.
    /// Поле 19.
    /// </summary>
    [Field (18)]
    public object? ReReceived { get; set; }

    /// <summary>
    /// Распределение по типу документов и языку текста. Поле 45.
    /// </summary>
    [Field (45)]
    public object? DocumentTypes { get; set; }

    /// <summary>
    /// Распределение по разделам знаний. Поле 151.
    /// </summary>
    [Field (151)]
    public object? KnowledgeSections { get; set; }

    /// <summary>
    /// По разделам знаний УДК. Поле 48.
    /// </summary>
    [Field (48)]
    public object? UdkSections { get; set; }

    /// <summary>
    /// По разделам знаний ББК. Поле 49.
    /// </summary>
    [Field (49)]
    public object? BbkSections { get; set; }

    /// <summary>
    /// По направлениям - начало. Поле 44.
    /// </summary>
    [Field (44)]
    public object? Directions1 { get; set; }

    /// <summary>
    /// По направлениям - окончание. Поле 744.
    /// </summary>
    [Field (744)]
    public object? Directions2 { get; set; }

    /// <summary>
    /// КСУ-распределение - общие данные. Поле 145.
    /// </summary>
    [Field (145)]
    public object? GeneralData { get; set; }

    /// <summary>
    /// Печатные издания по видам. Поле 146.
    /// </summary>
    [Field (146)]
    public object? Printing { get; set; }

    /// <summary>
    /// Непечатные издания по типам и носителям. Поле 147.
    /// </summary>
    [Field (147)]
    public object? NonPrinting { get; set; }

    /// <summary>
    /// Распределение по характеру документа. Поле 148.
    /// </summary>
    [Field (148)]
    public object? Characteristics { get; set; }

    /// <summary>
    /// Распределение "отечественные-иностранные" и по языкам.
    /// Поле 149.
    /// </summary>
    [Field (149)]
    public object? Foreign { get; set; }

    /// <summary>
    /// Труды сотрудников, фонды. Поле 150.
    /// </summary>
    [Field (150)]
    public object? Works { get; set; }

    /// <summary>
    /// Число наименований периодических изданий. Поле 155.
    /// </summary>
    [Field (155)]
    public object? Periodical { get; set; }

    /// <summary>
    /// Максимальный и минимальный инветарные номера в партии.
    /// </summary>
    [Field (910)]
    public object? Inventory { get; set; }

    /// <summary>
    /// Тираж каталожных карточек. Поле 20.
    /// </summary>
    [Field (20)]
    public object? CardCirculation { get; set; }

    /// <summary>
    /// Имя базы сбора данных.
    /// </summary>
    [Field (21)]
    public string? DatabaseName { get; set; }

    /// <summary>
    /// Код вида документа (имя рабочего листа). Поле 920.
    /// </summary>
    [Field (920)]
    [XmlElement ("worksheet")]
    [JsonPropertyName ("worksheet")]
    [Description ("Рабочий лист")]
    [DisplayName ("Рабочий лист")]
    public string? Worksheet { get; set; }

    /// <summary>
    /// Ассоциированная запись в базе данных.
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
    /// Применение данных к указанному библиографической записи <see cref="Record"/>.
    /// </summary>
    public Record ApplyTo
        (
            Record record
        )
    {
        Sure.NotNull (record);

        // TODO implement

        return record;
    }

    /// <summary>
    /// Разбор указанной библиографической записи.
    /// </summary>
    public static KsuRecord ParseRecord
        (
            Record record
        )
    {
        Sure.NotNull (record);

        return new KsuRecord()
        {
            Record = record
        };
    }

    /// <summary>
    /// Преобразование данных в библиографическую запись <see cref="Record"/>.
    /// </summary>
    public Record ToRecord()
    {
        // TODO implement

        return new Record();
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

        // TODO implement
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        // TODO implement
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<KsuRecord> (this, throwOnError);

        return verifier.Result;
    }

    #endregion
}
