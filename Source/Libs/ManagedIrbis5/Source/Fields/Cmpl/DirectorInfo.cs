// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* DirectorInfo.cs -- директор (организации), поле 14.
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
/// Директор (организации) и главный бухгалтер - для подписей.
/// База CMPL, поле 14.
/// </summary>
[XmlRoot ("director")]
public sealed class DirectorInfo
    : IHandmadeSerializable,
    IVerifiable
{
    #region Constants

    /// <summary>
    /// Метка поля.
    /// </summary>
    public const int Tag = 14;

    /// <summary>
    /// Известные коды подполей.
    /// </summary>
    public const string KnownCodes = "abcdefghij";

    #endregion

    #region Properties

    /// <summary>
    /// Директор библиотеки (текст - полное название).
    /// </summary>
    [SubField ('a')]
    [XmlElement ("title")]
    [JsonPropertyName ("title")]
    [Description ("Полное название")]
    [DisplayName ("Полное название")]
    public string? Title { get; set; }

    /// <summary>
    /// Директор библиотеки (текст - аббревиатура).
    /// </summary>
    [SubField ('d')]
    [XmlElement ("abbreviation")]
    [JsonPropertyName ("abbreviation")]
    [Description ("Аббревиатура")]
    [DisplayName ("Аббревиатура")]
    public string? Abbreviation { get; set; }

    /// <summary>
    /// ФИО директора.
    /// </summary>
    [SubField ('b')]
    [XmlElement ("director-name")]
    [JsonPropertyName ("directorName")]
    [Description ("ФИО директора")]
    [DisplayName ("ФИО директора")]
    public string? DirectorName { get; set; }

    /// <summary>
    /// ФИО главного бухгалтера.
    /// </summary>
    [SubField ('c')]
    [XmlElement ("accountant")]
    [JsonPropertyName ("accountant")]
    [Description ("ФИО главного бухгалтера")]
    [DisplayName ("ФИО главного бухгалтера")]
    public string? AccountantName { get; set; }

    /// <summary>
    /// Контактное лицо - ФИО.
    /// </summary>
    [SubField ('f')]
    [XmlElement ("contact-name")]
    [JsonPropertyName ("contactName")]
    [Description ("Контактное лицо - ФИО")]
    [DisplayName ("Контактное лицо - ФИО")]
    public string? ContactName { get; set; }

    /// <summary>
    /// Контактное лицо - телефон.
    /// </summary>
    [SubField ('e')]
    [XmlElement ("contact-phone")]
    [JsonPropertyName ("conctactPhone")]
    [Description ("Контактное лицо - телефон")]
    [DisplayName ("Контактное лицо - телефон")]
    public string? ContactPhone { get; set; }

    /// <summary>
    /// Руководитель структурного подразделения.
    /// </summary>
    [SubField ('g')]
    [XmlElement ("head-of-structural-unit")]
    [JsonPropertyName ("headOfStructuralUnit")]
    [Description ("Руководитель структурного подразделения")]
    [DisplayName ("Руководитель структурного подразделения")]
    public string? HeadOfStructuralUnit { get; set; }

    /// <summary>
    /// Текст для Реестра (1-я строка).
    /// </summary>
    [SubField ('h')]
    [XmlElement ("registry1")]
    [JsonPropertyName ("registry1")]
    [Description ("Текст для Реестра (1-я строка)")]
    [DisplayName ("Текст для Реестра (1-я строка)")]
    public string? Registry1 { get; set; }

    /// <summary>
    /// Текст для Реестра (2-я строка).
    /// </summary>
    [SubField ('i')]
    [XmlElement ("registry2")]
    [JsonPropertyName ("registry2")]
    [Description ("Текст для Реестра (2-я строка)")]
    [DisplayName ("Текст для Реестра (2-я строка)")]
    public string? Registry2 { get; set; }

    /// <summary>
    /// Текст для Реестра (3-я строка).
    /// </summary>
    [SubField ('j')]
    [XmlElement ("registry3")]
    [JsonPropertyName ("registry3")]
    [Description ("Текст для Реестра (3-я строка)")]
    [DisplayName ("Текст для Реестра (3-я строка)")]
    public string? Registry3 { get; set; }

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
            .SetSubFieldValue ('a', Title)
            .SetSubFieldValue ('d', Abbreviation)
            .SetSubFieldValue ('b', DirectorName)
            .SetSubFieldValue ('c', AccountantName)
            .SetSubFieldValue ('f', ContactName)
            .SetSubFieldValue ('e', ContactPhone)
            .SetSubFieldValue ('g', HeadOfStructuralUnit)
            .SetSubFieldValue ('h', Registry1)
            .SetSubFieldValue ('i', Registry2)
            .SetSubFieldValue ('j', Registry3);
    }

    /// <summary>
    /// Разбор указанного поля библиографической записи.
    /// </summary>
    public static DirectorInfo ParseField
        (
            Field field
        )
    {
        Sure.NotNull (field);

        return new DirectorInfo()
        {
            Title = field.GetFirstSubFieldValue ('a'),
            Abbreviation = field.GetFirstSubFieldValue ('d'),
            DirectorName = field.GetFirstSubFieldValue ('b'),
            AccountantName = field.GetFirstSubFieldValue ('c'),
            ContactName = field.GetFirstSubFieldValue ('f'),
            ContactPhone = field.GetFirstSubFieldValue ('e'),
            HeadOfStructuralUnit = field.GetFirstSubFieldValue ('g'),
            Registry1 = field.GetFirstSubFieldValue ('h'),
            Registry2 = field.GetFirstSubFieldValue ('i'),
            Registry3 = field.GetFirstSubFieldValue ('j'),
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
            .AddNonEmpty ('a', Title)
            .AddNonEmpty ('d', Abbreviation)
            .AddNonEmpty ('b', DirectorName)
            .AddNonEmpty ('c', AccountantName)
            .AddNonEmpty ('f', ContactName)
            .AddNonEmpty ('e', ContactPhone)
            .AddNonEmpty ('g', HeadOfStructuralUnit)
            .AddNonEmpty ('h', Registry1)
            .AddNonEmpty ('i', Registry2)
            .AddNonEmpty ('j', Registry3)
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

        Title = reader.ReadNullableString();
        Abbreviation = reader.ReadNullableString();
        DirectorName = reader.ReadNullableString();
        AccountantName = reader.ReadNullableString();
        ContactName = reader.ReadNullableString();
        ContactPhone = reader.ReadNullableString();
        HeadOfStructuralUnit = reader.ReadNullableString();
        Registry1 = reader.ReadNullableString();
        Registry2 = reader.ReadNullableString();
        Registry3 = reader.ReadNullableString();
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
            .WriteNullable (Title)
            .WriteNullable (Abbreviation)
            .WriteNullable (DirectorName)
            .WriteNullable (AccountantName)
            .WriteNullable (ContactName)
            .WriteNullable (ContactPhone)
            .WriteNullable (HeadOfStructuralUnit)
            .WriteNullable (Registry1)
            .WriteNullable (Registry2)
            .WriteNullable (Registry3)
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
        var verifier = new Verifier<DirectorInfo> (this, throwOnError);

        verifier
            .AnyNotNullNorEmpty (Title, Abbreviation, DirectorName, AccountantName,
                ContactName, ContactPhone, HeadOfStructuralUnit, Registry1,
                Registry2, Registry3);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"{nameof (Title)}: {Title.ToVisibleString()}, {nameof (DirectorName)}: {DirectorName.ToVisibleString()}";
    }

    #endregion
}
