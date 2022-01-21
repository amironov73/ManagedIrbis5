// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* QuarterlyOrderInfo.cs -- поквартальные сведения о заказах
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Magazines;

/// <summary>
/// Поквартальные сведения о заказах, поле 938.
/// </summary>
[XmlRoot ("quarterly")]
public sealed class QuarterlyOrderInfo
    : IHandmadeSerializable,
        IVerifiable
{
    #region Constants

    /// <summary>
    /// Метка поля.
    /// </summary>
    public const int Tag = 938;

    /// <summary>
    /// Известные коды подполей.
    /// </summary>
    public const string KnownCodes = "abdenqvxy";

    #endregion

    #region Properties

    /// <summary>
    /// Период заказа. Подполе Q.
    /// </summary>
    [SubField ('q')]
    [XmlAttribute ("period")]
    [JsonPropertyName ("period")]
    [DisplayName ("Период заказа")]
    public string? Period { get; set; }

    /// <summary>
    /// Число номеров. Подполе N.
    /// </summary>
    [SubField ('n')]
    [XmlAttribute ("issues")]
    [JsonPropertyName ("issues")]
    [DisplayName ("Число номеров")]
    public string? NumberOfIssues { get; set; }

    /// <summary>
    /// Первый номер. Подполе A.
    /// </summary>
    [SubField ('a')]
    [XmlAttribute ("first")]
    [JsonPropertyName ("first")]
    [DisplayName ("Первый номер")]
    public string? FirstIssue { get; set; }

    /// <summary>
    /// Последний номер. Подполе B.
    /// </summary>
    [SubField ('b')]
    [XmlAttribute ("last")]
    [JsonPropertyName ("last")]
    [DisplayName ("Последний номер")]
    public string? LastIssue { get; set; }

    /// <summary>
    /// Цена заказа. Подполе Y.
    /// </summary>
    [SubField ('y')]
    [XmlAttribute ("totalPrice")]
    [JsonPropertyName ("totalPrice")]
    [DisplayName ("Цена заказа")]
    public string? TotalPrice { get; set; }

    /// <summary>
    /// Цена номера по комплектам. Подполе E.
    /// </summary>
    [SubField ('e')]
    [XmlAttribute ("issuePrice")]
    [JsonPropertyName ("issuePrice")]
    [DisplayName ("Цена номера по комплектам")]
    public string? IssuePrice { get; set; }

    /// <summary>
    /// Валюта. Подполе V.
    /// </summary>
    [SubField ('v')]
    [XmlAttribute ("currency")]
    [JsonPropertyName ("currency")]
    [DisplayName ("Валюта")]
    public string? Currency { get; set; }

    /// <summary>
    /// Периодичность (код). Подполе D.
    /// </summary>
    [SubField ('d')]
    [XmlAttribute ("code")]
    [JsonPropertyName ("code")]
    [DisplayName ("Периодичность (код)")]
    public string? PeriodicityCode { get; set; }

    /// <summary>
    /// Периодичность (число). Подполе X.
    /// </summary>
    [SubField ('x')]
    [XmlAttribute ("periodicity")]
    [JsonPropertyName ("periodicity")]
    [DisplayName ("Периодичность (число)")]
    public string? PeriodicityNumber { get; set; }

    /// <summary>
    /// Неизвестные подполя.
    /// </summary>
    [Browsable (false)]
    [XmlElement ("unknown")]
    [JsonPropertyName ("unknown")]
    public SubField[]? UnknownSubfields { get; set; }

    /// <summary>
    /// Связанное поле библиографической записи.
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
    public Field ApplyTo
        (
            Field field
        )
    {
        Sure.NotNull (field);

        return field
            .SetSubFieldValue ('q', Period)
            .SetSubFieldValue ('n', NumberOfIssues)
            .SetSubFieldValue ('a', FirstIssue)
            .SetSubFieldValue ('b', LastIssue)
            .SetSubFieldValue ('y', TotalPrice)
            .SetSubFieldValue ('e', IssuePrice)
            .SetSubFieldValue ('v', Currency)
            .SetSubFieldValue ('d', PeriodicityCode)
            .SetSubFieldValue ('x', PeriodicityNumber);
    }

    /// <summary>
    /// Разбор указанного поля библиографической записи.
    /// </summary>
    public static QuarterlyOrderInfo ParseField
        (
            Field field
        )
    {
        Sure.NotNull (field);

        var result = new QuarterlyOrderInfo
        {
            Period = field.GetFirstSubFieldValue ('q'),
            NumberOfIssues = field.GetFirstSubFieldValue ('n'),
            FirstIssue = field.GetFirstSubFieldValue ('a'),
            LastIssue = field.GetFirstSubFieldValue ('b'),
            TotalPrice = field.GetFirstSubFieldValue ('y'),
            IssuePrice = field.GetFirstSubFieldValue ('e'),
            Currency = field.GetFirstSubFieldValue ('v'),
            PeriodicityCode = field.GetFirstSubFieldValue ('d'),
            PeriodicityNumber = field.GetFirstSubFieldValue ('x'),
            UnknownSubfields = field.Subfields.GetUnknownSubFields (KnownCodes),
            Field = field
        };

        return result;
    }

    /// <summary>
    /// Разбор указанной библиографической записи.
    /// </summary>
    public static QuarterlyOrderInfo[] ParseRecord
        (
            Record record
        )
    {
        Sure.NotNull (record);

        var result = new List<QuarterlyOrderInfo>();
        foreach (var field in record.Fields)
        {
            if (field.Tag == Tag)
            {
                result.Add (ParseField (field));
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Нужно ли сериализовать свойство <see cref="UnknownSubfields"/>?
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool ShouldSerializeUnknownSubfields()
    {
        return !UnknownSubfields.IsNullOrEmpty();
    }

    /// <summary>
    /// Преобразование данных в поле библиографической записи.
    /// </summary>
    public Field ToField()
    {
        var result = new Field (Tag)
            .AddNonEmpty ('q', Period)
            .AddNonEmpty ('n', NumberOfIssues)
            .AddNonEmpty ('a', FirstIssue)
            .AddNonEmpty ('b', LastIssue)
            .AddNonEmpty ('y', TotalPrice)
            .AddNonEmpty ('e', IssuePrice)
            .AddNonEmpty ('v', Currency)
            .AddNonEmpty ('d', PeriodicityCode)
            .AddNonEmpty ('x', PeriodicityNumber)
            .AddRange (UnknownSubfields);

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

        Period = reader.ReadNullableString();
        NumberOfIssues = reader.ReadNullableString();
        FirstIssue = reader.ReadNullableString();
        LastIssue = reader.ReadNullableString();
        TotalPrice = reader.ReadNullableString();
        IssuePrice = reader.ReadNullableString();
        Currency = reader.ReadNullableString();
        PeriodicityCode = reader.ReadNullableString();
        PeriodicityNumber = reader.ReadNullableString();
        UnknownSubfields = reader.ReadNullableArray<SubField>();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer
            .WriteNullable (Period)
            .WriteNullable (NumberOfIssues)
            .WriteNullable (FirstIssue)
            .WriteNullable (LastIssue)
            .WriteNullable (TotalPrice)
            .WriteNullable (IssuePrice)
            .WriteNullable (Currency)
            .WriteNullable (PeriodicityCode)
            .WriteNullable (PeriodicityNumber)
            .WriteNullableArray (UnknownSubfields);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify" />
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<QuarterlyOrderInfo> (this, throwOnError);

        verifier
            .NotNullNorEmpty (Period)
            .NotNullNorEmpty (FirstIssue)
            .NotNullNorEmpty (LastIssue);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return Period.ToVisibleString();
    }

    #endregion
}
