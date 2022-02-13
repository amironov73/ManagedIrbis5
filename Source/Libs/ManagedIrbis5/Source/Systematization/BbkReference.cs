// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* BbkReference.cs -- отсылка в ББК
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

namespace ManagedIrbis.Systematization;

/// <summary>
/// Отсылка в ББК.
/// </summary>
[XmlRoot ("reference")]
public sealed class BbkReference
    : IHandmadeSerializable,
        IVerifiable
{
    #region Constants

    /// <summary>
    /// Известные коды подполей.
    /// </summary>
    public const string KnownCodes = "ab";

    #endregion

    #region Properties

    /// <summary>
    /// Условие отсылки.
    /// Подполе a.
    /// </summary>
    [SubField ('a')]
    [XmlAttribute ("condition")]
    [JsonPropertyName ("condition")]
    [Description ("Условие отсылки")]
    public string? Condition { get; set; }

    /// <summary>
    /// Отсылочный код.
    /// Подполе b.
    /// </summary>
    [SubField ('b')]
    [XmlAttribute ("content")]
    [JsonPropertyName ("content")]
    [Description ("Отсылочный код")]
    public string? Content { get; set; }

    /// <summary>
    /// Неизвестные подполя.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public SubField[]? UnknownSubfields { get; set; }

    /// <summary>
    /// Ассоциированное поле библиографической записи.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public Field? Field { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Применение данных к указанному полю библиографической записи.
    /// </summary>
    public void ApplyTo
        (
            Field field
        )
    {
        Sure.NotNull (field);

        field
            .SetSubFieldValue ('a', Condition)
            .SetSubFieldValue ('b', Content);
    }

    /// <summary>
    /// Разбор указанного поля библиографической записи.
    /// </summary>
    public static BbkReference ParseField
        (
            Field field
        )
    {
        Sure.NotNull (field);

        var result = new BbkReference
        {
            Condition = field.GetFirstSubFieldValue ('a'),
            Content = field.GetFirstSubFieldValue ('b'),
            UnknownSubfields = field.Subfields.GetUnknownSubFields (KnownCodes),
            Field = field
        };

        return result;
    }

    /// <summary>
    /// Преобразование информации в поле библиографической записи.
    /// </summary>
    public Field ToField
        (
            int tag
        )
    {
        return new Field (tag)
            .AddNonEmpty ('a', Condition)
            .AddNonEmpty ('b', Content)
            .AddRange (UnknownSubfields);
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

        Condition = reader.ReadNullableString();
        Content = reader.ReadNullableString();
        UnknownSubfields = reader.ReadNullableArray<SubField>();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer
            .WriteNullable (Condition)
            .WriteNullable (Content)
            .WriteNullableArray (UnknownSubfields);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<BbkReference> (this, throwOnError);

        verifier
            .AnyNotNullNorEmpty (Condition, Content);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"{Condition.ToVisibleString()} : {Content.ToVisibleString()}";
    }

    #endregion
}
