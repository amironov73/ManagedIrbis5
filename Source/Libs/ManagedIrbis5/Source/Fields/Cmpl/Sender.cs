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

/* Sender.cs -- отправитель для письма-заказа на книги
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
/// Отправитель для письма-заказа на книги
/// </summary>
[XmlRoot ("sender")]
public sealed class Sender
    : IHandmadeSerializable,
    IVerifiable
{
    #region Constants

    /// <summary>
    /// Метка поля.
    /// </summary>
    public const int Tag = 15;

    /// <summary>
    /// Известные коды подполей.
    /// </summary>
    public const string KnownCodes = "abcdefgk";

    #endregion

    #region Properties

    /// <summary>
    /// Отправитель: первая строка на конверте.
    /// </summary>
    [SubField ('f')]
    [XmlElement ("firstLine")]
    [JsonPropertyName ("firstLine")]
    [Description ("Первая строка")]
    [DisplayName ("Первая строка")]
    public string? FirstLine { get; set; }

    /// <summary>
    /// Отправитель: вторая строка на конверте.
    /// </summary>
    [SubField ('g')]
    [XmlElement ("secondLine")]
    [JsonPropertyName ("secondLine")]
    [Description ("Вторая строка")]
    [DisplayName ("Вторая строка")]
    public string? SecondLine { get; set; }

    /// <summary>
    /// Улица.
    /// </summary>
    [SubField ('d')]
    [XmlElement ("street")]
    [JsonPropertyName ("street")]
    [Description ("Улица")]
    [DisplayName ("Улица")]
    public string? Street { get; set; }

    /// <summary>
    /// Номер дома.
    /// </summary>
    [SubField ('e')]
    [XmlElement ("building")]
    [JsonPropertyName ("building")]
    [Description ("Номер дома")]
    [DisplayName ("Номер дома")]
    public string? Building { get; set; }

    /// <summary>
    /// Город.
    /// </summary>
    [SubField ('c')]
    [XmlElement ("city")]
    [JsonPropertyName ("city")]
    [Description ("Город")]
    [DisplayName ("Город")]
    public string? City { get; set; }

    /// <summary>
    /// Страна.
    /// </summary>
    [SubField ('b')]
    [XmlElement ("country")]
    [JsonPropertyName ("country")]
    [Description ("Страна")]
    [DisplayName ("Страна")]
    public string? Country { get; set; }

    /// <summary>
    /// Почтовый индекс.
    /// </summary>
    [SubField ('a')]
    [XmlElement ("index")]
    [JsonPropertyName ("index")]
    [Description ("Индекс")]
    [DisplayName ("Индекс")]
    public string? Index { get; set; }

    /// <summary>
    /// Телефон.
    /// </summary>
    [SubField ('k')]
    [XmlElement ("phone")]
    [JsonPropertyName ("phone")]
    [Description ("Телефон")]
    [DisplayName ("Телефон")]
    public string? Phone { get; set; }

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
            .SetSubFieldValue ('f', FirstLine)
            .SetSubFieldValue ('g', SecondLine)
            .SetSubFieldValue ('d', Street)
            .SetSubFieldValue ('e', Building)
            .SetSubFieldValue ('c', City)
            .SetSubFieldValue ('b', Country)
            .SetSubFieldValue ('a', Index)
            .SetSubFieldValue ('k', Phone);
    }

    /// <summary>
    /// Разбор указанного поля библиографической записи.
    /// </summary>
    public static Sender ParseField
        (
            Field field
        )
    {
        Sure.NotNull (field);

        return new Sender()
        {
            FirstLine = field.GetFirstSubFieldValue ('f'),
            SecondLine = field.GetFirstSubFieldValue ('g'),
            Street = field.GetFirstSubFieldValue ('d'),
            Building = field.GetFirstSubFieldValue ('e'),
            City = field.GetFirstSubFieldValue ('c'),
            Country = field.GetFirstSubFieldValue ('b'),
            Index = field.GetFirstSubFieldValue ('a'),
            Phone = field.GetFirstSubFieldValue ('k'),
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
            .AddNonEmpty ('f', FirstLine)
            .AddNonEmpty ('g', SecondLine)
            .AddNonEmpty ('d', Street)
            .AddNonEmpty ('e', Building)
            .AddNonEmpty ('c', City)
            .AddNonEmpty ('b', Country)
            .AddNonEmpty ('a', Index)
            .AddNonEmpty ('k', Phone)
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

        FirstLine = reader.ReadNullableString();
        SecondLine = reader.ReadNullableString();
        Street = reader.ReadNullableString();
        Building = reader.ReadNullableString();
        City = reader.ReadNullableString();
        Country = reader.ReadNullableString();
        Index = reader.ReadNullableString();
        Phone = reader.ReadNullableString();
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
            .WriteNullable (FirstLine)
            .WriteNullable (SecondLine)
            .WriteNullable (Street)
            .WriteNullable (Building)
            .WriteNullable (City)
            .WriteNullable (Country)
            .WriteNullable (Index)
            .WriteNullable (Phone)
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
        var verifier = new Verifier<Sender> (this, throwOnError);

        verifier
            .NotNullNorEmpty (FirstLine)
            .NotNullNorEmpty (Index);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"{FirstLine.ToVisibleString()} {SecondLine.ToVisibleString()}";
    }

    #endregion
}
