// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Alias.cs -- псевдоним для сервера или базы данных ИРБИС64
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

namespace ManagedIrbis.Client;

/// <summary>
/// Псевдоним для сервера или базы данных ИРБИС64.
/// </summary>
[XmlRoot("alias")]
public sealed class ConnectionAlias
    : IHandmadeSerializable,
    IVerifiable
{
    #region Properties

    /// <summary>
    /// Собственно псевдоним.
    /// </summary>
    [XmlAttribute ("name")]
    [JsonPropertyName ("name")]
    [Description ("Псевдоним")]
    public string? Name { get; set; }

    /// <summary>
    /// Значение псевдонима, например, строка подключения,
    /// в которую он раскрывается.
    /// </summary>
    [XmlAttribute ("value")]
    [JsonPropertyName ("value")]
    [Description ("Значение")]
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

        Name = reader.ReadNullableString();
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
            .WriteNullable(Name)
            .WriteNullable(Value);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify" />
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<ConnectionAlias> (this, throwOnError);

        verifier
            .NotNullNorEmpty (Name)
            .NotNullNorEmpty (Value);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return Name.ToVisibleString() + "=" + Value.ToVisibleString();
    }

    #endregion
}
