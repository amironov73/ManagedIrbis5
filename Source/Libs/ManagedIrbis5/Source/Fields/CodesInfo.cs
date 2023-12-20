// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local
// ReSharper disable UseStringInterpolation

/* CodesInfo.cs -- коды (поле 900)
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
/// Коды (поле 900).
/// </summary>
[XmlRoot ("codes")]
public sealed class CodesInfo
    : IHandmadeSerializable,
        IVerifiable
{
    #region Constants

    /// <summary>
    /// Метка поля.
    /// </summary>
    public const int Tag = 900;

    /// <summary>
    /// Известные коды подполей.
    /// </summary>
    public const string KnownCodes = "bctxyz234569";

    #endregion

    #region Properties

    /// <summary>
    /// Тип документа. Подполе T.
    /// </summary>
    [SubField ('t')]
    [XmlAttribute ("type")]
    [JsonPropertyName ("type")]
    [Description ("Тип документа")]
    [DisplayName ("Тип документа")]
    public string? DocumentType { get; set; }

    /// <summary>
    /// Вид документа. Подполе B.
    /// </summary>
    [SubField ('b')]
    [XmlAttribute ("kind")]
    [JsonPropertyName ("kind")]
    [Description ("Вид документа")]
    [DisplayName ("Вид документа")]
    public string? DocumentKind { get; set; }

    /// <summary>
    /// Характер документа. Подполе C.
    /// </summary>
    [SubField ('c')]
    [XmlAttribute ("character1")]
    [JsonPropertyName ("character1")]
    [Description ("Характер документа")]
    [DisplayName ("Характер документа (1)")]
    public string? DocumentCharacter1 { get; set; }

    /// <summary>
    /// Характер документа. Подполе 2.
    /// </summary>
    [SubField ('2')]
    [XmlAttribute ("character2")]
    [JsonPropertyName ("character2")]
    [Description ("Характер документа")]
    [DisplayName ("Характер документа (2)")]
    public string? DocumentCharacter2 { get; set; }

    /// <summary>
    /// Характер документа. Подполе 3.
    /// </summary>
    [SubField ('3')]
    [XmlAttribute ("character3")]
    [JsonPropertyName ("character3")]
    [Description ("Характер документа")]
    [DisplayName ("Характер документа (3)")]
    public string? DocumentCharacter3 { get; set; }

    /// <summary>
    /// Характер документа. Подполе 4.
    /// </summary>
    [SubField ('4')]
    [XmlAttribute ("character4")]
    [JsonPropertyName ("character4")]
    [Description ("Характер документа")]
    [DisplayName ("Характер документа (4)")]
    public string? DocumentCharacter4 { get; set; }

    /// <summary>
    /// Характер документа. Подполе 5.
    /// </summary>
    [SubField ('5')]
    [XmlAttribute ("character5")]
    [JsonPropertyName ("character5")]
    [Description ("Характер документа")]
    [DisplayName ("Характер документа (5)")]
    public string? DocumentCharacter5 { get; set; }

    /// <summary>
    /// Характер документа. Подполе 6.
    /// </summary>
    [SubField ('6')]
    [XmlAttribute ("character6")]
    [JsonPropertyName ("character6")]
    [Description ("Характер документа")]
    [DisplayName ("Характер документа (6)")]
    public string? DocumentCharacter6 { get; set; }

    /// <summary>
    /// Код целевого назначения. Подполе X.
    /// </summary>
    [SubField ('x')]
    [XmlAttribute ("purpose1")]
    [JsonPropertyName ("purpose1")]
    [Description ("Код целевого назначения")]
    [DisplayName ("Код целевого назначения (1)")]
    public string? PurposeCode1 { get; set; }

    /// <summary>
    /// Код целевого назначения. Подполе Y.
    /// </summary>
    [SubField ('y')]
    [XmlAttribute ("purpose2")]
    [JsonPropertyName ("purpose2")]
    [Description ("Код целевого назначения")]
    [DisplayName ("Код целевого назначения (2)")]
    public string? PurposeCode2 { get; set; }

    /// <summary>
    /// Код целевого назначения. Подполе 9.
    /// </summary>
    [SubField ('9')]
    [XmlAttribute ("purpose3")]
    [JsonPropertyName ("purpose3")]
    [Description ("Код целевого назначения")]
    [DisplayName ("Код целевого назначения (3)")]
    public string? PurposeCode3 { get; set; }

    /// <summary>
    /// Возрастные ограничения. Подполе Z.
    /// </summary>
    [SubField ('z')]
    [XmlAttribute ("age")]
    [JsonPropertyName ("age")]
    [Description ("Возрастные ограничения")]
    [DisplayName ("Возрастные ограничения")]
    public string? AgeRestrictions { get; set; }

    /// <summary>
    /// Неизвестные подполя.
    /// </summary>
    [XmlElement ("unknown")]
    [JsonPropertyName ("unknown")]
    [Browsable (false)]
    public SubField[]? UnknownSubFields { get; set; }

    /// <summary>
    /// Ассоциированное поле библиографической записи.
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
    /// Применение данных к полю библиографической записи.
    /// </summary>
    public Field ApplyToField (Field field) => field
        .ThrowIfNull ()
        .SetSubFieldValue ('t', DocumentType)
        .SetSubFieldValue ('b', DocumentKind)
        .SetSubFieldValue ('c', DocumentCharacter1)
        .SetSubFieldValue ('2', DocumentCharacter2)
        .SetSubFieldValue ('3', DocumentCharacter3)
        .SetSubFieldValue ('4', DocumentCharacter4)
        .SetSubFieldValue ('5', DocumentCharacter5)
        .SetSubFieldValue ('6', DocumentCharacter6)
        .SetSubFieldValue ('x', PurposeCode1)
        .SetSubFieldValue ('y', PurposeCode2)
        .SetSubFieldValue ('9', PurposeCode3)
        .SetSubFieldValue ('z', AgeRestrictions);

    /// <summary>
    /// Проверка, является ли документ электронным (компьютерным).
    /// </summary>
    public bool IsElectronic()
    {
        var code = DocumentType;

        if (string.IsNullOrEmpty (code))
        {
            return false;
        }

        return code[0] is 'l' or 'L' or 'b' or 'B';
    }

    /// <summary>
    /// Проверка, является ли документ текстовым.
    /// </summary>
    public bool IsText()
    {
        var code = DocumentType;

        if (string.IsNullOrEmpty (code))
        {
            return false;
        }

        return code[0] is 'a' or 'A' or 'b' or 'B';
    }

    /// <summary>
    /// Разбор поля библиографической записи.
    /// </summary>
    public static CodesInfo Parse
        (
            Field field
        )
    {
        Sure.NotNull (field);

        return new CodesInfo
        {
            DocumentType = field.GetFirstSubFieldValue ('t'),
            DocumentKind = field.GetFirstSubFieldValue ('b'),
            DocumentCharacter1 = field.GetFirstSubFieldValue ('c'),
            DocumentCharacter2 = field.GetFirstSubFieldValue ('2'),
            DocumentCharacter3 = field.GetFirstSubFieldValue ('3'),
            DocumentCharacter4 = field.GetFirstSubFieldValue ('4'),
            DocumentCharacter5 = field.GetFirstSubFieldValue ('5'),
            DocumentCharacter6 = field.GetFirstSubFieldValue ('6'),
            PurposeCode1 = field.GetFirstSubFieldValue ('x'),
            PurposeCode2 = field.GetFirstSubFieldValue ('y'),
            PurposeCode3 = field.GetFirstSubFieldValue ('9'),
            AgeRestrictions = field.GetFirstSubFieldValue ('z'),
            UnknownSubFields = field.Subfields.GetUnknownSubFields (KnownCodes),
            Field = field
        };
    }

    /// <summary>
    /// Преобразование в поле библиографической записи.
    /// </summary>
    public Field ToField() => new Field (Tag)
        .AddNonEmpty ('t', DocumentType)
        .AddNonEmpty ('b', DocumentKind)
        .AddNonEmpty ('c', DocumentCharacter1)
        .AddNonEmpty ('2', DocumentCharacter2)
        .AddNonEmpty ('3', DocumentCharacter3)
        .AddNonEmpty ('4', DocumentCharacter4)
        .AddNonEmpty ('5', DocumentCharacter5)
        .AddNonEmpty ('6', DocumentCharacter6)
        .AddNonEmpty ('x', PurposeCode1)
        .AddNonEmpty ('y', PurposeCode2)
        .AddNonEmpty ('9', PurposeCode3)
        .AddNonEmpty ('z', AgeRestrictions)
        .AddRange (UnknownSubFields);

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        DocumentType = reader.ReadNullableString();
        DocumentKind = reader.ReadNullableString();
        DocumentCharacter1 = reader.ReadNullableString();
        DocumentCharacter2 = reader.ReadNullableString();
        DocumentCharacter3 = reader.ReadNullableString();
        DocumentCharacter4 = reader.ReadNullableString();
        DocumentCharacter5 = reader.ReadNullableString();
        DocumentCharacter6 = reader.ReadNullableString();
        PurposeCode1 = reader.ReadNullableString();
        PurposeCode2 = reader.ReadNullableString();
        PurposeCode3 = reader.ReadNullableString();
        AgeRestrictions = reader.ReadNullableString();
        UnknownSubFields = reader.ReadNullableArray<SubField>();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer
            .WriteNullable (DocumentType)
            .WriteNullable (DocumentKind)
            .WriteNullable (DocumentCharacter1)
            .WriteNullable (DocumentCharacter2)
            .WriteNullable (DocumentCharacter3)
            .WriteNullable (DocumentCharacter4)
            .WriteNullable (DocumentCharacter5)
            .WriteNullable (DocumentCharacter6)
            .WriteNullable (PurposeCode1)
            .WriteNullable (PurposeCode2)
            .WriteNullable (PurposeCode3)
            .WriteNullable (AgeRestrictions)
            .WriteNullableArray (UnknownSubFields);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify" />
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<CodesInfo> (this, throwOnError);

        verifier
            .NotNullNorEmpty (DocumentType);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString() => string.Format
        (
            "DocumentType: {0}, DocumentKind: {1}, DocumentCharacter1: {2}",
            DocumentType.ToVisibleString(),
            DocumentKind.ToVisibleString(),
            DocumentCharacter1.ToVisibleString()
        );

    #endregion
}
