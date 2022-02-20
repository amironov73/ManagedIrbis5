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

/* ReceiptInfo.cs -- сведения о поступлении книг в библиотеку, поле 88.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

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
/// Сведения о поступлении книг в библиотеку.
/// Поле 88.
/// </summary>
public sealed class ReceiptInfo
    : IHandmadeSerializable,
    IVerifiable
{
    #region Constants

    /// <summary>
    /// Метка поля.
    /// </summary>
    public const int Tag = 88;

    /// <summary>
    /// Известные коды подполей.
    /// </summary>
    public const string KnownCodes = "abcdefghijlmnpy9";

    #endregion

    #region Properties

    /// <summary>
    /// Год и номер записи в КСУ.
    /// </summary>
    [SubField ('a')]
    [XmlElement ("number")]
    [JsonPropertyName ("number")]
    [Description ("Номер записи")]
    [DisplayName ("Номер записи")]
    public string? Number { get; set; }

    /// <summary>
    /// Номер акта индивидуального учета.
    /// </summary>
    [SubField ('y')]
    [XmlElement ("act")]
    [JsonPropertyName ("act")]
    [Description ("Номер акта")]
    [DisplayName ("Номер акта")]
    public string? Act { get; set; }

    /// <summary>
    /// Дата Alt-Д.
    /// </summary>
    [SubField ('b')]
    [XmlElement ("date")]
    [JsonPropertyName ("date")]
    [Description ("Дата")]
    [DisplayName ("Дата")]
    public string? Date { get; set; }

    /// <summary>
    /// Номер сопроводительного документа.
    /// </summary>
    [SubField ('c')]
    [XmlElement ("accompanying")]
    [JsonPropertyName ("accompanying")]
    [Description ("Номер сопроводительного документа")]
    [DisplayName ("Номер сопроводительного документа")]
    public string? Accompanying { get; set; }

    /// <summary>
    /// Источник комплектования (код).
    /// </summary>
    [SubField ('d')]
    [XmlElement ("source")]
    [JsonPropertyName ("source")]
    [Description ("Источник комплектования")]
    [DisplayName ("Источник комплектования")]
    public string? Source { get; set; }

    /// <summary>
    /// Номер заказа.
    /// </summary>
    [SubField ('9')]
    [XmlElement ("order")]
    [JsonPropertyName ("order")]
    [Description ("Номер заказа")]
    [DisplayName ("Номер заказа")]
    public string? Order { get; set; }

    /// <summary>
    /// Число наименований.
    /// </summary>
    [SubField ('e')]
    [XmlElement ("titles")]
    [JsonPropertyName ("titles")]
    [Description ("Число наименований")]
    [DisplayName ("Число наименований")]
    public string? Titles { get; set; }

    /// <summary>
    /// Число экземпляров.
    /// </summary>
    [SubField ('f')]
    [XmlElement ("exemplars")]
    [JsonPropertyName ("exemplars")]
    [Description ("Число экземпляров")]
    [DisplayName ("Число экземпляров")]
    public string? Exemplars { get; set; }

    /// <summary>
    /// На сумму.
    /// </summary>
    [SubField ('g')]
    [XmlElement ("summa")]
    [JsonPropertyName ("summa")]
    [Description ("На сумму")]
    [DisplayName ("На сумму")]
    public string? Summa { get; set; }

    /// <summary>
    /// Платно-бесплатно.
    /// </summary>
    [SubField ('h')]
    [XmlElement ("paid")]
    [JsonPropertyName ("paid")]
    [Description ("Платно-бесплатно")]
    [DisplayName ("Платно-бесплатно")]
    public string? Paid { get; set; }

    /// <summary>
    /// В том числе НДС.
    /// </summary>
    [SubField ('n')]
    [XmlElement ("vat")]
    [JsonPropertyName ("vat")]
    [Description ("НДС")]
    [DisplayName ("НДС")]
    public string? Vat { get; set; }

    /// <summary>
    /// Почтовые расходы.
    /// </summary>
    [SubField ('p')]
    [XmlElement ("postage")]
    [JsonPropertyName ("postage")]
    [Description ("Почтовые расходы")]
    [DisplayName ("Почтовые расходы")]
    public string? Postage { get; set; }

    /// <summary>
    /// Номер счета.
    /// </summary>
    [SubField ('j')]
    [XmlElement ("invoice-number")]
    [JsonPropertyName ("invoice-number")]
    [Description ("Номер счета")]
    [DisplayName ("Номер счета")]
    public string? InvoiceNumber { get; set; }

    /// <summary>
    /// Дата счета.
    /// </summary>
    [SubField ('i')]
    [XmlElement ("invoice-date")]
    [JsonPropertyName ("invoice-date")]
    [Description ("Дата счета")]
    [DisplayName ("Дата счета")]
    public string? InvoiceDate { get; set; }

    /// <summary>
    /// Источник финансирования.
    /// </summary>
    [SubField ('l')]
    [XmlElement ("funding")]
    [JsonPropertyName ("funding")]
    [Description ("Источник финансирования")]
    [DisplayName ("Источник финансирования")]
    public string? Funding { get; set; }

    /// <summary>
    /// Оплачено?
    /// </summary>
    [SubField ('m')]
    [XmlElement ("completed")]
    [JsonPropertyName ("completed")]
    [Description ("Оплачено")]
    [DisplayName ("Оплачено")]
    public string? Completed { get; set; }

    /// <summary>
    /// Неизвестные подполя.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public SubField[]? UnknownSubFields { get; set; }

    /// <summary>
    /// Связанное поле в нераскодированном виде.
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
    /// Применение данных к указанному полю библиографической записи <see cref="Field"/>.
    /// </summary>
    public Field ApplyTo
        (
            Field field
        )
    {
        Sure.NotNull (field);

        return field
            .SetSubFieldValue ('a', Number)
            .SetSubFieldValue ('y', Act)
            .SetSubFieldValue ('b', Date)
            .SetSubFieldValue ('c', Accompanying)
            .SetSubFieldValue ('d', Source)
            .SetSubFieldValue ('9', Order)
            .SetSubFieldValue ('e', Titles)
            .SetSubFieldValue ('f', Exemplars)
            .SetSubFieldValue ('g', Summa)
            .SetSubFieldValue ('h', Paid)
            .SetSubFieldValue ('n', Vat)
            .SetSubFieldValue ('p', Postage)
            .SetSubFieldValue ('j', InvoiceNumber)
            .SetSubFieldValue ('i', InvoiceDate)
            .SetSubFieldValue ('l', Funding)
            .SetSubFieldValue ('m', Completed);
    }

    /// <summary>
    /// Разбор указанного поля библиографической записи.
    /// </summary>
    public static ReceiptInfo ParseField
        (
            Field field
        )
    {
        Sure.NotNull (field);

        return new ReceiptInfo()
        {
            Number = field.GetFirstSubFieldValue ('a'),
            Act = field.GetFirstSubFieldValue ('y'),
            Date = field.GetFirstSubFieldValue ('b'),
            Accompanying = field.GetFirstSubFieldValue ('c'),
            Source = field.GetFirstSubFieldValue ('d'),
            Order = field.GetFirstSubFieldValue ('9'),
            Titles = field.GetFirstSubFieldValue ('e'),
            Exemplars = field.GetFirstSubFieldValue ('f'),
            Summa = field.GetFirstSubFieldValue ('g'),
            Paid = field.GetFirstSubFieldValue ('h'),
            Vat = field.GetFirstSubFieldValue ('n'),
            Postage = field.GetFirstSubFieldValue ('p'),
            InvoiceNumber = field.GetFirstSubFieldValue ('j'),
            InvoiceDate = field.GetFirstSubFieldValue ('i'),
            Funding = field.GetFirstSubFieldValue ('l'),
            Completed = field.GetFirstSubFieldValue ('m'),
            UnknownSubFields = field.Subfields.GetUnknownSubFields (KnownCodes),
            Field = field
        };
    }

    /// <summary>
    /// Преобразование данных в поле библиографической записи <see cref="Field"/>.
    /// </summary>
    public Field ToField()
    {
        return new Field (Tag)
            .AddNonEmpty ('a', Number)
            .AddNonEmpty ('y', Act)
            .AddNonEmpty ('b', Date)
            .AddNonEmpty ('c', Accompanying)
            .AddNonEmpty ('d', Source)
            .AddNonEmpty ('9', Order)
            .AddNonEmpty ('e', Titles)
            .AddNonEmpty ('f', Exemplars)
            .AddNonEmpty ('g', Summa)
            .AddNonEmpty ('h', Paid)
            .AddNonEmpty ('n', Vat)
            .AddNonEmpty ('p', Postage)
            .AddNonEmpty ('j', InvoiceNumber)
            .AddNonEmpty ('i', InvoiceDate)
            .AddNonEmpty ('l', Funding)
            .AddNonEmpty ('m', Completed)
            .AddRange (UnknownSubFields);
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

        Number = reader.ReadNullableString();
        Act = reader.ReadNullableString();
        Date = reader.ReadNullableString();
        Accompanying = reader.ReadNullableString();
        Source = reader.ReadNullableString();
        Order = reader.ReadNullableString();
        Titles = reader.ReadNullableString();
        Exemplars = reader.ReadNullableString();
        Summa = reader.ReadNullableString();
        Paid = reader.ReadNullableString();
        Vat = reader.ReadNullableString();
        Postage = reader.ReadNullableString();
        InvoiceNumber = reader.ReadNullableString();
        InvoiceDate = reader.ReadNullableString();
        Funding = reader.ReadNullableString();
        Completed = reader.ReadNullableString();
        UnknownSubFields = reader.ReadNullableArray<SubField>();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer
            .WriteNullable (Number)
            .WriteNullable (Act)
            .WriteNullable (Date)
            .WriteNullable (Accompanying)
            .WriteNullable (Source)
            .WriteNullable (Order)
            .WriteNullable (Titles)
            .WriteNullable (Exemplars)
            .WriteNullable (Summa)
            .WriteNullable (Paid)
            .WriteNullable (Vat)
            .WriteNullable (Postage)
            .WriteNullable (InvoiceNumber)
            .WriteNullable (InvoiceDate)
            .WriteNullable (Funding)
            .WriteNullable (Completed)
            .WriteNullableArray (UnknownSubFields);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<ReceiptInfo> (this, throwOnError);

        verifier
            .NotNullNorEmpty (Number);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"{Number.ToVisibleString()} {Date.ToVisibleString()}";
    }

    #endregion
}
