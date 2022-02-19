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

/* Signature.cs -- подпись для карточки-заказа книги
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
/// Подпись для карточки-заказа книги.
/// </summary>
[XmlRoot ("signature")]
public sealed class Signature
    : IHandmadeSerializable,
    IVerifiable
{
    #region Constants

    /// <summary>
    /// Метка поля.
    /// </summary>
    public const int Tag = 13;

    /// <summary>
    /// Известные коды подполей.
    /// </summary>
    public const string KnownCodes = "ab";

    #endregion

    #region Properties

    /// <summary>
    /// Должность.
    /// </summary>
    [SubField ('a')]
    [XmlElement ("title")]
    [JsonPropertyName ("title")]
    [Description ("Должность")]
    [DisplayName ("Должность")]
    public string? Title { get; set; }

    /// <summary>
    /// Подпись.
    /// </summary>
    [SubField ('b')]
    [XmlElement ("sign")]
    [JsonPropertyName ("sign")]
    [Description ("Подпись")]
    [DisplayName ("Подпись")]
    public string? Sign { get; set; }

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
            .SetSubFieldValue ('b', Sign);
    }

    /// <summary>
    /// Разбор указанного поля библиографической записи.
    /// </summary>
    public static Signature ParseField
        (
            Field field
        )
    {
        Sure.NotNull (field);

        return new Signature()
        {
            Title = field.GetFirstSubFieldValue ('a'),
            Sign = field.GetFirstSubFieldValue ('b'),
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
            .AddNonEmpty ('b', Sign)
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
        Sign = reader.ReadNullableString();
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
            .WriteNullable (Sign)
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
        var verifier = new Verifier<Signature> (this, throwOnError);

        verifier
            .NotNullNorEmpty (Title)
            .NotNull (Sign);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"{nameof (Title)}: {Title.ToVisibleString()}, {nameof (Sign)}: {Sign.ToVisibleString()}";
    }

    #endregion
}
