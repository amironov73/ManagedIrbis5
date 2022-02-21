// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* BibTexField.cs -- поле BibText-записи
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

#endregion

#nullable enable

namespace ManagedIrbis.BibTex;
//
// Каждая запись содержит некоторый список стандартных полей
// (можно вводить любые другие поля, которые просто игнорируются
// стандартными программами).
//

/// <summary>
/// Поле BibText-записи.
/// </summary>
[XmlRoot ("field")]
public sealed class BibTexField
    : IHandmadeSerializable,
    IVerifiable
{
    #region Properties

    /// <summary>
    /// Метка поля, см. <see cref="KnownTags"/>.
    /// </summary>
    [XmlAttribute ("tag")]
    [JsonPropertyName ("tag")]
    [Description ("Метка поля")]
    public string? Tag { get; set; }

    /// <summary>
    /// Значение поля.
    /// </summary>
    [XmlAttribute ("value")]
    [JsonPropertyName ("value")]
    [Description ("Значение поля")]
    public string? Value { get; set; }

    /// <summary>
    /// Произвольные пользовательские данные
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public object? UserData { get; set; }

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        Tag = reader.ReadNullableString();
        Value = reader.ReadNullableString();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
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

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<BibTexField> (this, throwOnError);

        verifier
            .NotNullNorEmpty (Tag)
            .IsOneOf (Tag, KnownTags.ListValues());

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"{Tag.ToVisibleString()}={Value.ToVisibleString()}";
    }

    #endregion
}
