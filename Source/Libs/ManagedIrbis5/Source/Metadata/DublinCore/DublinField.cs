// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UseNameofExpression

/* DublinField.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Metadata.DublinCore;

/// <summary>
/// Field of the <see cref="DublinRecord"/>.
/// </summary>
public sealed class DublinField
    : IHandmadeSerializable,
    IVerifiable
{
    #region Properties

    /// <summary>
    /// Tag.
    /// </summary>
    [XmlAttribute ("tag")]
    [JsonPropertyName ("tag")]
    public string? Tag { get; set; }

    /// <summary>
    /// Value.
    /// </summary>
    [XmlAttribute ("value")]
    [JsonPropertyName ("value")]
    public string? Value { get; set; }

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        Tag = reader.ReadNullableString();
        Value = reader.ReadNullableString();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer
            .WriteNullable (Tag)
            .WriteNullable (Value);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify" />
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<DublinField> (this, throwOnError);

        verifier
            .NotNullNorEmpty (Tag)
            .NotNullNorEmpty (Value);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return $"{Tag.ToVisibleString()}={Value.ToVisibleString()}";
    }

    #endregion
}
