// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* ReaderRegistration.cs -- информация о регистрации/перерегистрации читателя
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Readers;

/// <summary>
/// Информация о регистрации/перерегистрации читателя.
/// </summary>
[Serializable]
[XmlRoot ("registration")]
public sealed class ReaderRegistration
    : IHandmadeSerializable
{
    #region Constants

    /// <summary>
    /// Поле регистрация.
    /// </summary>
    public const int RegistrationTag = 51;

    /// <summary>
    /// Поле "перерегистрация".
    /// </summary>
    public const int ReregistrationTag = 52;

    /// <summary>
    /// Коды известных подполей.
    /// </summary>
    public const string KnownCodes = "abc";

    #endregion

    #region Properties

    /// <summary>
    /// Дата. Подполе *.
    /// </summary>
    [XmlAttribute ("date")]
    [JsonPropertyName ("date")]
    [DisplayName ("Дата")]
    [Description ("Дата в формате ГГГГММДД")]
    public string? DateString { get; set; }

    /// <summary>
    /// Дата.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public DateTime Date
    {
        get => IrbisDate.ConvertStringToDate (DateString);
        set => DateString = IrbisDate.ConvertDateToString (value);
    }

    /// <summary>
    /// Место (кафедра обслуживания).
    /// Подполе c.
    /// </summary>
    [SubField ('c')]
    [XmlAttribute ("chair")]
    [JsonPropertyName ("chair")]
    [DisplayName ("Место")]
    [Description ("Место (кафедра обслуживания)")]
    public string? Chair { get; set; }

    /// <summary>
    /// Номер приказа. Подполе a.
    /// </summary>
    [SubField ('a')]
    [XmlAttribute ("order-number")]
    [JsonPropertyName ("order-number")]
    [DisplayName ("Приказ")]
    [Description ("Номер приказа")]
    public string? OrderNumber { get; set; }

    /// <summary>
    /// Причина. Подполе b.
    /// </summary>
    [SubField ('b')]
    [XmlAttribute ("reason")]
    [JsonPropertyName ("reason")]
    [DisplayName ("Причина")]
    [Description ("Причина")]
    public string? Reason { get; set; }

    /// <summary>
    /// Ссылка на зарегистрированного читателя.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public ReaderInfo? Reader { get; set; }

    /// <summary>
    /// Неизвестные подполя.
    /// </summary>
    [XmlElement ("unknown")]
    [JsonPropertyName ("unknown")]
    [Browsable (false)]
    public SubField[]? UnknownSubFields { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор поля библиографической записи,
    /// получение информации о регистрации.
    /// </summary>
    public static ReaderRegistration Parse
        (
            Field field
        )
    {
        Sure.NotNull (field);

        var result = new ReaderRegistration
        {
            DateString = field.Value,
            Chair = field.GetFirstSubFieldValue ('c'),
            OrderNumber = field.GetFirstSubFieldValue ('a'),
            Reason = field.GetFirstSubFieldValue ('b'),
            UnknownSubFields = field.Subfields.GetUnknownSubFields(KnownCodes)
        };

        return result;
    }

    /// <summary>
    /// Разбор полей библиографической записи с указанной меткой.
    /// </summary>
    public static ReaderRegistration[] Parse
        (
            Record record,
            int tag
        )
    {
        Sure.NotNull (record);
        Sure.Positive (tag);

        var result = record.Fields
            .GetField (tag)
            .Select (field => Parse (field))
            .ToArray();

        return result;
    }

    /// <summary>
    /// Преобразование регистрации в поле библиографической записи.
    /// </summary>
    public Field ToField()
    {
        var result = new Field (RegistrationTag, DateString)
            .AddNonEmpty ('c', Chair)
            .AddNonEmpty ('a', OrderNumber)
            .AddNonEmpty ('b', Reason)
            .AddRange (UnknownSubFields);

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

        DateString = reader.ReadNullableString();
        Chair = reader.ReadNullableString();
        OrderNumber = reader.ReadNullableString();
        Reason = reader.ReadNullableString();
        UnknownSubFields = reader.ReadNullableArray<SubField>();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer.WriteNullable (DateString);
        writer.WriteNullable (Chair);
        writer.WriteNullable (OrderNumber);
        writer.WriteNullable (Reason);
        writer.WriteNullableArray (UnknownSubFields);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return Utility.JoinNonEmpty
            (
                " - ",
                DateString,
                Chair
            );
    }

    #endregion
}
