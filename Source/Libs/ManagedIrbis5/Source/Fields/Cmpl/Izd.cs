// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Izd.cs -- запись в базе CMPL об издающей, распространяющей или книготорговой организации
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Runtime;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Fields;

/// <summary>
/// Запись в базе CMPL об издающей, распространяющей или книготорговой организации.
/// </summary>
[XmlRoot ("izd")]
public sealed class Izd
    : IHandmadeSerializable,
    IVerifiable
{
    #region Properties

    /// <summary>
    /// Код, наименование, город. Поле 81.
    /// </summary>
    [Field (81)]
    [XmlElement ("code-name-city")]
    [JsonPropertyName ("codeNameCity")]
    [Description ("Код, наименование, город")]
    [DisplayName ("Код, наименование, город")]
    public object? CodeNameCity { get; set; }

    /// <summary>
    /// Код, аббревиатура, город. Поле 89.
    /// </summary>
    [Field (89)]
    public object? CodeAbbreviationCity { get; set; }

    /// <summary>
    /// ФИО руководителя, должность, телефон, телетайп, факс. Поле 82.
    /// </summary>
    [Field (82)]
    [XmlElement ("chief")]
    [JsonPropertyName ("chief")]
    [Description ("Руководитель")]
    [DisplayName ("Руководитель")]
    public object? Chief { get; set; }

    /// <summary>
    /// Полный почтовый адрес. Поле 83.
    /// </summary>
    [Field (83)]
    [XmlElement ("address")]
    [JsonPropertyName ("address")]
    [Description ("Полный почтовый адрес")]
    [DisplayName ("Полный почтовый адрес")]
    public object? Address { get; set; }

    /// <summary>
    /// Издательство: условия продажи. Поле 85.
    /// </summary>
    [Field (85)]
    [XmlElement ("publisher-conditions")]
    [JsonPropertyName ("publisherConditions")]
    [Description ("Издательство: условия продажи")]
    [DisplayName ("Издательство: условия продажи")]
    public string? PublisherConditions { get; set; }

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
    /// Первый заказ (перечислено-заказано/получено). Поле 41.
    /// </summary>
    [Field (41)]
    [XmlElement ("order1")]
    [JsonPropertyName ("order1")]
    [Description ("Первый заказ")]
    [DisplayName ("Первый заказ")]
    public object? Order1 { get; set; }

    /// <summary>
    /// Второй заказ (перечислено-заказано/получено). Поле 42.
    /// </summary>
    [Field (42)]
    [XmlElement ("order2")]
    [JsonPropertyName ("order2")]
    [Description ("Второй заказ")]
    [DisplayName ("Второй заказ")]
    public object? Order2 { get; set; }

    /// <summary>
    /// Третий заказ (перечислено-заказано/получено). Поле 43.
    /// </summary>
    [Field (43)]
    [XmlElement ("order3")]
    [JsonPropertyName ("order3")]
    [Description ("Третий заказ")]
    [DisplayName ("Третий заказ")]
    public object? Order3 { get; set; }

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
    public static Izd ParseRecord
        (
            Record record
        )
    {
        Sure.NotNull (record);

        return new Izd
        {
            CodeNameCity = null,
            CodeAbbreviationCity = null,
            Chief = null,
            Address = null,
            PublisherConditions = record.FM (85),
            Worksheet = record.FM (920),
            Order1 = null,
            Order2 = null,
            Order3 = null,
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
        var verifier = new Verifier<Izd> (this, throwOnError);

        // TODO implement

        return verifier.Result;
    }

    #endregion
}
