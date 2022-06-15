// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* AthraSee.cs --
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
/// "СМ." в базе данных ATHRA.
/// Поле 410.
/// </summary>
/// <remarks>
/// Также "СМ. ТАКЖЕ" - поле 510.
/// </remarks>
[XmlRoot ("see")]
public sealed class AthraSee
    : IHandmadeSerializable,
        IVerifiable
{
    #region Constants

    /// <summary>
    /// Метка поля.
    /// </summary>
    public const int Tag = 410;

    /// <summary>
    /// Известные коды подполей.
    /// </summary>
    public const string KnownCodes = "0145789abcdfg<";

    #endregion

    #region Properties

    /// <summary>
    /// Начальный элемент ввода (фамилия или имя).
    /// Подполе a.
    /// </summary>
    [SubField ('a')]
    [XmlElement ("surname")]
    [JsonPropertyName ("surname")]
    [Description ("Начальный элемент ввода")]
    [DisplayName ("Начальный элемент ввода")]
    public string? Surname { get; set; }

    /// <summary>
    /// Инициалы.
    /// Подполе b.
    /// </summary>
    [SubField ('b')]
    [XmlElement ("initials")]
    [JsonPropertyName ("initials")]
    [Description ("Инициалы")]
    [DisplayName ("Инициалы")]
    public string? Initials { get; set; }

    /// <summary>
    /// Расширение инициалов.
    /// Подполе g.
    /// </summary>
    [SubField ('g')]
    [XmlElement ("extension")]
    [JsonPropertyName ("extension")]
    [Description ("Расширение инициалов")]
    [DisplayName ("Расширение инициалов")]
    public string? Extension { get; set; }

    /// <summary>
    /// Роль (инвертирование ФИО допустимо?).
    /// Подполе &lt;.
    /// </summary>
    [SubField ('<')]
    [XmlElement ("role")]
    [JsonPropertyName ("role")]
    [Description ("Инвертирование ФИО допустимо?")]
    [DisplayName ("Инвертирование ФИО допустимо?")]
    public string? Role { get; set; }

    /// <summary>
    /// Неотъемлемая часть имени (выводится в скобках).
    /// Подполе 1.
    /// </summary>
    [SubField ('1')]
    [XmlElement ("integral")]
    [JsonPropertyName ("integral")]
    [Description ("Неотъемлемая часть имени")]
    [DisplayName ("Неотъемлемая часть имени")]
    public string? IntegralPart { get; set; }

    /// <summary>
    /// Идентифицирующие признаки имени.
    /// Подполе c.
    /// </summary>
    [SubField ('c')]
    [XmlElement ("identifying")]
    [JsonPropertyName ("identifying")]
    [Description ("Идентифицирующие признаки имени")]
    [DisplayName ("Идентифицирующие признаки имени")]
    public string? IdentifyingSigns { get; set; }

    /// <summary>
    /// Римские цифры.
    /// Подполе d.
    /// </summary>
    [SubField ('d')]
    [XmlElement ("roman")]
    [JsonPropertyName ("roman")]
    [Description ("Римские цифры")]
    [DisplayName ("Римские цифры")]
    public string? RomanNumerals { get; set; }

    /// <summary>
    /// Даты.
    /// Подполе f.
    /// </summary>
    [SubField ('f')]
    [XmlElement ("dates")]
    [JsonPropertyName ("dates")]
    [Description ("Даты")]
    [DisplayName ("Даты")]
    public string? Dates { get; set; }

    /// <summary>
    /// Инструктивный текст.
    /// Подполе 0.
    /// </summary>
    [SubField ('0')]
    [XmlElement ("instruction")]
    [JsonPropertyName ("instruction")]
    [Description ("Инструктивный текст")]
    [DisplayName ("Инструктивный текст")]
    public string? Instruction { get; set; }

    /// <summary>
    /// Графика.
    /// Подполе 7.
    /// </summary>
    [SubField ('7')]
    [XmlElement ("graphics")]
    [JsonPropertyName ("graphics")]
    [Description ("Графика")]
    [DisplayName ("Графика")]
    public string? Graphics { get; set; }

    /// <summary>
    /// Язык заголовка.
    /// Подполе 8.
    /// </summary>
    [SubField ('8')]
    [XmlElement ("language")]
    [JsonPropertyName ("language")]
    [Description ("Язык заголовка")]
    [DisplayName ("Язык заголовка")]
    public string? Language { get; set; }

    /// <summary>
    /// Код формирования ссылки.
    /// Подполе 5.
    /// </summary>
    [SubField ('5')]
    [XmlElement ("link")]
    [JsonPropertyName ("link")]
    [Description ("Код формирования ссылки")]
    [DisplayName ("Код формирования ссылки")]
    public string? Link { get; set; }

    /// <summary>
    /// Признак ввода имени лица.
    /// Подполе 9.
    /// </summary>
    [SubField ('9')]
    [XmlElement ("mark")]
    [JsonPropertyName ("mark")]
    [Description ("Признак ввода имени лица")]
    [DisplayName ("Признак ввода имени лица")]
    public string? Mark { get; set; }

    /// <summary>
    /// Код отношения.
    /// Подполе 4.
    /// </summary>
    [SubField ('4')]
    [XmlElement ("relation")]
    [JsonPropertyName ("relation")]
    [Description ("Код отношения")]
    [DisplayName ("Код отношения")]
    public string? RelationCode { get; set; }

    /// <summary>
    /// Ассоциированное поле библиографической записи <see cref="Field"/>.
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
    /// Применение данных к полю библиографической записи <see cref="Field"/>.
    /// </summary>
    public Field ApplyTo (Field field) => field
        .SetSubFieldValue ('a', Surname)
        .SetSubFieldValue ('b', Initials)
        .SetSubFieldValue ('g', Extension)
        .SetSubFieldValue ('<', Role)
        .SetSubFieldValue ('1', IntegralPart)
        .SetSubFieldValue ('c', IdentifyingSigns)
        .SetSubFieldValue ('d', RomanNumerals)
        .SetSubFieldValue ('f', Dates)
        .SetSubFieldValue ('0', Instruction)
        .SetSubFieldValue ('7', Graphics)
        .SetSubFieldValue ('8', Language)
        .SetSubFieldValue ('5', Link)
        .SetSubFieldValue ('9', Mark)
        .SetSubFieldValue ('4', RelationCode);

    /// <summary>
    /// Разбор поля библиографической записи <see cref="Field"/>.
    /// </summary>
    public static AthraSee ParseField
        (
            Field field
        )
    {
        Sure.NotNull (field);

        return new ()
        {
            Surname = field.GetFirstSubFieldValue ('a'),
            Initials = field.GetFirstSubFieldValue ('b'),
            Extension = field.GetFirstSubFieldValue ('g'),
            Role = field.GetFirstSubFieldValue ('<'),
            IntegralPart = field.GetFirstSubFieldValue ('1'),
            IdentifyingSigns = field.GetFirstSubFieldValue ('c'),
            RomanNumerals = field.GetFirstSubFieldValue ('d'),
            Dates = field.GetFirstSubFieldValue ('f'),
            Instruction = field.GetFirstSubFieldValue ('0'),
            Graphics = field.GetFirstSubFieldValue ('7'),
            Language = field.GetFirstSubFieldValue ('8'),
            Link = field.GetFirstSubFieldValue ('5'),
            Mark = field.GetFirstSubFieldValue ('9'),
            RelationCode = field.GetFirstSubFieldValue ('4'),
            Field = field
        };
    }

    /// <summary>
    /// Преобразование в поле библиографической записи <see cref="Field"/>.
    /// </summary>
    public Field ToField()
    {
        return new Field { Tag = 410 }
            .AddNonEmpty ('a', Surname)
            .AddNonEmpty ('b', Initials)
            .AddNonEmpty ('g', Extension)
            .AddNonEmpty ('<', Role)
            .AddNonEmpty ('1', IntegralPart)
            .AddNonEmpty ('c', IdentifyingSigns)
            .AddNonEmpty ('d', RomanNumerals)
            .AddNonEmpty ('f', Dates)
            .AddNonEmpty ('0', Instruction)
            .AddNonEmpty ('7', Graphics)
            .AddNonEmpty ('8', Language)
            .AddNonEmpty ('5', Link)
            .AddNonEmpty ('9', Mark)
            .AddNonEmpty ('4', RelationCode);
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

        Surname = reader.ReadNullableString();
        Initials = reader.ReadNullableString();
        Extension = reader.ReadNullableString();
        Role = reader.ReadNullableString();
        IntegralPart = reader.ReadNullableString();
        IdentifyingSigns = reader.ReadNullableString();
        RomanNumerals = reader.ReadNullableString();
        Dates = reader.ReadNullableString();
        Instruction = reader.ReadNullableString();
        Graphics = reader.ReadNullableString();
        Language = reader.ReadNullableString();
        Link = reader.ReadNullableString();
        Mark = reader.ReadNullableString();
        RelationCode = reader.ReadNullableString();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer
            .WriteNullable (Surname)
            .WriteNullable (Initials)
            .WriteNullable (Extension)
            .WriteNullable (Role)
            .WriteNullable (IntegralPart)
            .WriteNullable (IdentifyingSigns)
            .WriteNullable (RomanNumerals)
            .WriteNullable (Dates)
            .WriteNullable (Instruction)
            .WriteNullable (Graphics)
            .WriteNullable (Language)
            .WriteNullable (Link)
            .WriteNullable (Mark)
            .WriteNullable (RelationCode);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<AthraSee> (this, throwOnError);

        verifier
            .NotNullNorEmpty (Surname);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return Surname.ToVisibleString();
    }

    #endregion
}
