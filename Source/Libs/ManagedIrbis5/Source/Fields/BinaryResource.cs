// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* BinaryResource.cs -- встроенный двоичный ресурс
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.IO;
using System.Linq;
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
/// Встроенный двоичный ресурс. Поле 953.
/// </summary>
[XmlRoot ("resource")]
public sealed class BinaryResource
    : IHandmadeSerializable,
        IVerifiable
{
    #region Constants

    /// <summary>
    /// Метка поля.
    /// </summary>
    public const int Tag = 953;

    /// <summary>
    /// Известные коды подполей.
    /// </summary>
    public const string KnownCodes = "abpt";

    #endregion

    #region Properties

    /// <summary>
    /// Тип двоичного ресурса, подполе A.
    /// </summary>
    /// <remarks>Например, "jpg".</remarks>
    [SubField ('a')]
    [XmlElement ("kind")]
    [JsonPropertyName ("kind")]
    [Description ("Тип двоичного ресурса")]
    [DisplayName ("Тип двоичного ресурса")]
    public string? Kind { get; set; }

    /// <summary>
    /// Собственно двоичный ресурс (закодированный).
    /// Подполе B.
    /// </summary>
    [SubField ('b')]
    [XmlElement ("resource")]
    [JsonPropertyName ("resource")]
    [Description ("Двоичный ресурс (закодированный)")]
    [DisplayName ("Двоичный ресурс (закодированный)")]
    public string? Resource { get; set; }

    /// <summary>
    /// Название двоичного ресурса. Подполе T.
    /// </summary>
    [SubField ('t')]
    [XmlElement ("title")]
    [JsonPropertyName ("title")]
    [Description ("Название двоичного ресурса")]
    [DisplayName ("Название двоичного ресурса")]
    public string? Title { get; set; }

    /// <summary>
    /// Характер просмотра. Подполе P.
    /// </summary>
    /// <remarks>
    /// См. <see cref="ResourceView"/>.
    /// </remarks>
    [SubField ('p')]
    [XmlElement ("view")]
    [JsonPropertyName ("view")]
    [Description ("Характер просмотра")]
    [DisplayName ("Характер просмотр")]
    public string? View { get; set; }

    /// <summary>
    /// Ассоциированное поле библиографической записи <see cref="Field"/>.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public Field? Field { get; private set; }

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
    /// Применение данные к полю библиографической записи <see cref="Field"/>.
    /// </summary>
    public Field ApplyToField
        (
            Field field
        )
    {
        Sure.NotNull (field);

        return field
            .SetSubFieldValue ('a', Kind)
            .SetSubFieldValue ('b', Resource)
            .SetSubFieldValue ('t', Title)
            .SetSubFieldValue ('p', View);
    }

    /// <summary>
    /// Декодирование ресурса.
    /// </summary>
    public byte[] Decode()
    {
        return IrbisUtility.DecodePercentString (Resource);
    }

    /// <summary>
    /// Кодирование ресурса в строковое представление.
    /// </summary>
    public string? Encode
        (
            byte[] array
        )
    {
        Sure.NotNull (array);

        return Resource = IrbisUtility.EncodePercentString (array);
    }

    /// <summary>
    /// Разбор поля библиографической записи.
    /// </summary>
    public static BinaryResource ParseField
        (
            Field field
        )
    {
        Sure.NotNull (field);

        var result = new BinaryResource
        {
            Kind = field.GetFirstSubFieldValue ('a'),
            Resource = field.GetFirstSubFieldValue ('b'),
            Title = field.GetFirstSubFieldValue ('t'),
            View = field.GetFirstSubFieldValue ('p'),
            Field = field
        };

        return result;
    }

    /// <summary>
    /// Разбор библиографической записи <see cref="Record"/>.
    /// </summary>
    public static BinaryResource[] ParseRecord
        (
            Record record,
            int tag = Tag
        )
    {
        Sure.NotNull (record);
        Sure.Positive (tag);

        return record.Fields
            .GetField (tag)
            .Select (ParseField)
            .ToArray();
    }

    /// <summary>
    /// Преобразование в поле библиографической записи <see cref="Field"/>.
    /// </summary>
    public Field ToField() => new Field (Tag)
        .AddNonEmpty ('a', Kind)
        .AddNonEmpty ('b', Resource)
        .AddNonEmpty ('t', Title)
        .AddNonEmpty ('p', View);

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        Kind = reader.ReadNullableString();
        Resource = reader.ReadNullableString();
        Title = reader.ReadNullableString();
        View = reader.ReadNullableString();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer
            .WriteNullable (Kind)
            .WriteNullable (Resource)
            .WriteNullable (Title)
            .WriteNullable (View);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify" />
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<BinaryResource> (this, throwOnError);

        verifier
            .NotNullNorEmpty (Resource);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return
            $"Kind: {Kind.ToVisibleString()}, Resource: {Resource.ToVisibleString()}, Title: {Title.ToVisibleString()}";
    }

    #endregion
}
