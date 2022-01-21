// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* OrdererdExemplarInfo.cs -- сведения о заказанных экземплярах журнала
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

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

namespace ManagedIrbis.Magazines;

/// <summary>
/// Сведения о заказанных экземплярах журнала, поле 901.
/// </summary>
[XmlRoot ("ordered")]
public sealed class OrderedExemplarInfo
    : IHandmadeSerializable,
    IVerifiable
{
    #region Constants

    /// <summary>
    /// Метка поля.
    /// </summary>
    public const int Tag = 901;

    /// <summary>
    /// Известные коды подполей.
    /// </summary>
    public const string KnownCodes = "12345abdefjnqv+";

    #endregion

    #region Properties

    /// <summary>
    /// Год заказа.
    /// </summary>
    [SubField ('q')]
    [XmlAttribute ("year")]
    [JsonPropertyName ("year")]
    [Description ("Год заказа")]
    public string? Year { get; set; }

    /// <summary>
    /// Статус.
    /// </summary>
    [SubField ('a')]
    [XmlAttribute ("status")]
    [JsonPropertyName ("status")]
    [Description ("Статус")]
    public string? Status { get; set; }

    /// <summary>
    /// Номер комплекта.
    /// </summary>
    [SubField ('b')]
    [XmlAttribute ("complect")]
    [JsonPropertyName ("complect")]
    [Description ("Номер комплекта")]
    public string? Complect { get; set; }

    /// <summary>
    /// Начальный (первый) номер в году для изданий с продолжающейся нумерацией.
    /// </summary>
    [SubField ('j')]
    [XmlAttribute ("starting-number")]
    [JsonPropertyName ("startingNumber")]
    [Description ("Начальный (первый) номер в году для изданий с продолжающейся нумерацией")]
    public string? StartingNumber { get; set; }

    /// <summary>
    /// Место хранения экземпляра.
    /// </summary>
    [SubField ('d')]
    [XmlAttribute ("place")]
    [JsonPropertyName ("place")]
    [Description ("Место хранения экземпляра")]
    public string? Place { get; set; }

    /// <summary>
    /// Прожолжительность хранения.
    /// Число - количество лет.
    /// </summary>
    [SubField ('v')]
    [XmlAttribute ("duration")]
    [JsonPropertyName ("duration")]
    [Description ("Продолжительность хранения. Число - количество лет")]
    public string? StorageDuration { get; set; }

    /// <summary>
    /// Количество экземпляров для ЦБС/ВУЗа.
    /// </summary>
    [SubField ('n')]
    [XmlAttribute ("amount")]
    [JsonPropertyName ("amount")]
    [Description ("Количество экземпляров для ЦБС/ВУЗа")]
    public string? Amount { get; set; }

    /// <summary>
    /// Канал поступления.
    /// </summary>
    [SubField ('f')]
    [XmlAttribute ("channel")]
    [JsonPropertyName ("channel")]
    [Description ("Канал поступления")]
    public string? Channel { get; set; }

    /// <summary>
    /// Цена отдельного выпуска комплекта.
    /// </summary>
    [SubField ('e')]
    [XmlAttribute ("price")]
    [JsonPropertyName ("price")]
    [Description ("Цена выпуска комплекта")]
    public string? Price { get; set; }

    /// <summary>
    /// Пересчитывать цену объединенных выпусков?
    /// </summary>
    [SubField ('+')]
    [XmlAttribute ("recalc")]
    [JsonPropertyName ("recalc")]
    [Description ("Пересчитывать цену объединенных выпусков?")]
    public string? Recalc { get; set; }

    /// <summary>
    /// Первый пункт технологического пути.
    /// </summary>
    [SubField ('1')]
    [XmlAttribute ("point1")]
    [JsonPropertyName ("point1")]
    [Description ("Первый пункт технологического пути")]
    public string? FirstPoint { get; set; }

    /// <summary>
    /// Второй пункт технологического пути.
    /// </summary>
    [SubField ('2')]
    [XmlAttribute ("point2")]
    [JsonPropertyName ("point2")]
    [Description ("Второй пункт технологического пути")]
    public string? SecondPoint { get; set; }

    /// <summary>
    /// Третий пункт технологического пути.
    /// </summary>
    [SubField ('3')]
    [XmlAttribute ("point3")]
    [JsonPropertyName ("point3")]
    [Description ("Третий пункт технологического пути")]
    public string? ThirdPoint { get; set; }

    /// <summary>
    /// Четвертый пункт технологического пути.
    /// </summary>
    [SubField ('4')]
    [XmlAttribute ("point4")]
    [JsonPropertyName ("point4")]
    [Description ("Четвертый пункт технологического пути")]
    public string? FourthPoint { get; set; }

    /// <summary>
    /// Пятый пункт технологического пути.
    /// </summary>
    [SubField ('5')]
    [XmlAttribute ("point5")]
    [JsonPropertyName ("point5")]
    [Description ("Пятый пункт технологического пути")]
    public string? FifthPoint { get; set; }

    /// <summary>
    /// Неизвестные подполя.
    /// </summary>
    [Browsable (false)]
    [XmlElement ("unknown")]
    [JsonPropertyName ("unknown")]
    public SubField[]? UnknownSubfields { get; set; }

    /// <summary>
    /// Связанное поле записи.
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
        return field
            .SetSubFieldValue ('q', Year)
            .SetSubFieldValue ('a', Status)
            .SetSubFieldValue ('b', Complect)
            .SetSubFieldValue ('j', StartingNumber)
            .SetSubFieldValue ('d', Place)
            .SetSubFieldValue ('v', StorageDuration)
            .SetSubFieldValue ('n', Amount)
            .SetSubFieldValue ('f', Channel)
            .SetSubFieldValue ('e', Price)
            .SetSubFieldValue ('+', Recalc)
            .SetSubFieldValue ('1', FirstPoint)
            .SetSubFieldValue ('2', SecondPoint)
            .SetSubFieldValue ('3', ThirdPoint)
            .SetSubFieldValue ('4', FourthPoint)
            .SetSubFieldValue ('5', FifthPoint);
    }


    /// <summary>
    /// Разбор поля библиографической записи.
    /// </summary>
    public static OrderedExemplarInfo ParseField
        (
            Field field
        )
    {
        Sure.NotNull (field);

        return new OrderedExemplarInfo()
        {
            Year = field.GetFirstSubFieldValue ('q'),
            Status = field.GetFirstSubFieldValue ('a'),
            Complect = field.GetFirstSubFieldValue ('b'),
            StartingNumber = field.GetFirstSubFieldValue ('j'),
            Place = field.GetFirstSubFieldValue ('d'),
            StorageDuration = field.GetFirstSubFieldValue ('v'),
            Amount = field.GetFirstSubFieldValue ('n'),
            Channel = field.GetFirstSubFieldValue ('f'),
            Price = field.GetFirstSubFieldValue ('e'),
            Recalc = field.GetFirstSubFieldValue ('+'),
            FirstPoint = field.GetFirstSubFieldValue ('1'),
            SecondPoint = field.GetFirstSubFieldValue ('2'),
            ThirdPoint = field.GetFirstSubFieldValue ('3'),
            FourthPoint = field.GetFirstSubFieldValue ('4'),
            FifthPoint = field.GetFirstSubFieldValue ('5'),
            UnknownSubfields = field.Subfields.GetUnknownSubFields (KnownCodes),
            Field = field
        };
    }

    /// <summary>
    /// Разбор библиографической записи.
    /// </summary>
    public static OrderedExemplarInfo[] ParseRecord
        (
            Record record
        )
    {
        Sure.NotNull (record);

        var result = new List<OrderedExemplarInfo>();
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
    /// Создание поля библиографической записи по данным о заказанном экземпляре.
    /// </summary>
    /// <returns></returns>
    public Field ToField()
    {
        return new Field (Tag)
            .AddNonEmpty ('q', Year)
            .AddNonEmpty ('a', Status)
            .AddNonEmpty ('b', Complect)
            .AddNonEmpty ('j', StartingNumber)
            .AddNonEmpty ('d', Place)
            .AddNonEmpty ('v', StorageDuration)
            .AddNonEmpty ('n', Amount)
            .AddNonEmpty ('f', Channel)
            .AddNonEmpty ('e', Price)
            .AddNonEmpty ('+', Recalc)
            .AddNonEmpty ('1', FirstPoint)
            .AddNonEmpty ('2', SecondPoint)
            .AddNonEmpty ('3', ThirdPoint)
            .AddNonEmpty ('4', FourthPoint)
            .AddNonEmpty ('5', FifthPoint)
            .AddRange (UnknownSubfields);
    }

    #endregion

    #region IHandmadeSerializable

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        Year = reader.ReadNullableString();
        Status = reader.ReadNullableString();
        Complect = reader.ReadNullableString();
        StartingNumber = reader.ReadNullableString();
        Place = reader.ReadNullableString();
        StorageDuration = reader.ReadNullableString();
        Amount = reader.ReadNullableString();
        Channel = reader.ReadNullableString();
        Price = reader.ReadNullableString();
        Recalc = reader.ReadNullableString();
        FirstPoint = reader.ReadNullableString();
        SecondPoint = reader.ReadNullableString();
        ThirdPoint = reader.ReadNullableString();
        FourthPoint = reader.ReadNullableString();
        FifthPoint = reader.ReadNullableString();
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
            .WriteNullable (Year)
            .WriteNullable (Status)
            .WriteNullable (Complect)
            .WriteNullable (StartingNumber)
            .WriteNullable (Place)
            .WriteNullable (StorageDuration)
            .WriteNullable (Amount)
            .WriteNullable (Channel)
            .WriteNullable (Price)
            .WriteNullable (Recalc)
            .WriteNullable (FirstPoint)
            .WriteNullable (SecondPoint)
            .WriteNullable (ThirdPoint)
            .WriteNullable (FourthPoint)
            .WriteNullable (FifthPoint)
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
        var verifier = new Verifier<OrderedExemplarInfo> (this, throwOnError);

        verifier
            .NotNullNorEmpty (Year)
            .NotNullNorEmpty (Complect)
            .NotNullNorEmpty (Status);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return $"{Year.ToVisibleString()} {Complect.ToVisibleString()} {Status.ToVisibleString()}";
    }

    #endregion
}
