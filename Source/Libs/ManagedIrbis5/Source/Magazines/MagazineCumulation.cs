// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* MagazineCumulation.cs -- данные о кумуляции номеров журнала
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Magazines;

/// <summary>
/// Данные о кумуляции номеров журнала. Поле 909.
/// </summary>
[XmlRoot ("cumulation")]
public sealed class MagazineCumulation
    : IHandmadeSerializable,
    IVerifiable
{
    #region Constants

    /// <summary>
    /// Метка поля по умолчанию.
    /// </summary>
    public const int Tag = 909;

    /// <summary>
    /// Коды известных полей.
    /// </summary>
    public const string KnownCodes = "dfhkq";

    #endregion

    #region Properties

    /// <summary>
    /// Год. Подполе Q.
    /// </summary>
    [DisplayName ("Год")]
    [XmlAttribute ("year")]
    [JsonPropertyName ("year")]
    public string? Year { get; set; }

    /// <summary>
    /// Том. Подполе F.
    /// </summary>
    [DisplayName ("Том")]
    [XmlAttribute ("volume")]
    [JsonPropertyName ("volume")]
    public string? Volume { get; set; }

    /// <summary>
    /// Место хранения. Подполе D.
    /// </summary>
    [DisplayName ("Место хранения")]
    [XmlAttribute ("place")]
    [JsonPropertyName ("place")]
    public string? Place { get; set; }

    /// <summary>
    /// Кумулированные номера. Подполе H.
    /// </summary>
    [DisplayName ("Кумулированные номера")]
    [XmlAttribute ("numbers")]
    [JsonPropertyName ("numbers")]
    public string? Numbers { get; set; }

    /// <summary>
    /// Номер комплекта. Подполе K.
    /// </summary>
    [DisplayName ("Номер комплекта")]
    [XmlAttribute ("set")]
    [JsonPropertyName ("set")]
    public string? ComplectNumber { get; set; }

    /// <summary>
    /// Неопознанные подполя.
    /// </summary>
    [Browsable (false)]
    [XmlElement ("unknown")]
    [JsonPropertyName ("unknown")]
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
    /// Применение кумуляции к полю записи <see cref="Field"/>.
    /// </summary>
    public Field ApplyTo
        (
            Field field
        )
    {
        Sure.NotNull (field);

        return field
            .SetSubFieldValue ('q', Year)
            .SetSubFieldValue ('f', Volume)
            .SetSubFieldValue ('d', Place)
            .SetSubFieldValue ('h', Numbers)
            .SetSubFieldValue ('k', ComplectNumber);
    }

    /// <summary>
    /// Разбор поля.
    /// </summary>
    public static MagazineCumulation ParseField
        (
            Field field
        )
    {
        Sure.VerifyNotNull (field);

        // TODO: реализовать эффективно

        var result = new MagazineCumulation
        {
            Year = field.GetFirstSubFieldValue ('q'),
            Volume = field.GetFirstSubFieldValue ('f'),
            Place = field.GetFirstSubFieldValue ('d'),
            Numbers = field.GetFirstSubFieldValue ('h'),
            ComplectNumber = field.GetFirstSubFieldValue ('k'),
            UnknownSubFields = field.Subfields.GetUnknownSubFields (KnownCodes),
            Field = field
        };

        return result;
    }

    /// <summary>
    /// Разбор записи.
    /// </summary>
    public static MagazineCumulation[] ParseRecord
        (
            Record record,
            int tag = Tag
        )
    {
        Sure.VerifyNotNull (record);
        Sure.Positive (tag);

        return record.Fields
            .GetField (tag)
            .Select (ParseField)
            .ToArray();
    }

    /// <summary>
    /// Should serialize the <see cref="UnknownSubFields"/> array?
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool ShouldSerializeUnknownSubFields()
    {
        return UnknownSubFields is { Length: not 0 };
    }

    /// <summary>
    /// Преобразование в поле записи <see cref="Field"/>.
    /// </summary>
    public Field ToField()
    {
        return new Field (Tag)
            .AddNonEmpty ('q', Year)
            .AddNonEmpty ('f', Volume)
            .AddNonEmpty ('d', Place)
            .AddNonEmpty ('h', Numbers)
            .AddNonEmpty ('k', ComplectNumber)
            .AddRange (UnknownSubFields);
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

        Year = reader.ReadNullableString();
        Volume = reader.ReadNullableString();
        Place = reader.ReadNullableString();
        Numbers = reader.ReadNullableString();
        ComplectNumber = reader.ReadNullableString();
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
            .WriteNullable (Year)
            .WriteNullable (Volume)
            .WriteNullable (Place)
            .WriteNullable (Numbers)
            .WriteNullable (ComplectNumber)
            .WriteNullableArray (UnknownSubFields);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify" />
    [Pure]
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<MagazineCumulation> (this, throwOnError);

        verifier
            .NotNullNorEmpty (Year)
            .NotNullNorEmpty (Numbers);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return Utility.JoinNonEmpty (" | ", Year, Volume, Place, ComplectNumber, Numbers);
    }

    #endregion
}
