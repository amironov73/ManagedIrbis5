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

/* Credentials.cs --
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

namespace ManagedIrbis.Authentication;

/// <summary>
/// Credentials.
/// </summary>
[XmlRoot ("credentials")]
public sealed class IrbisCredentials
    : IHandmadeSerializable,
        IVerifiable
{
    #region Properties

    /// <summary>
    /// Hostname.
    /// </summary>
    [XmlAttribute ("hostname")]
    [JsonPropertyName ("hostname")]
    public string? Hostname { get; set; }

    /// <summary>
    /// Username.
    /// </summary>
    [XmlAttribute ("username")]
    [JsonPropertyName ("username")]
    public string? Username { get; set; }

    /// <summary>
    /// Password.
    /// </summary>
    [XmlAttribute ("password")]
    [JsonPropertyName ("password")]
    public string? Password { get; set; }

    /// <summary>
    /// Resource (e. g. database).
    /// </summary>
    [XmlAttribute ("resource")]
    [JsonPropertyName ("resource")]
    public string? Resource { get; set; }

    /// <summary>
    /// Role (e. g. workstation kind).
    /// </summary>
    [XmlAttribute ("role")]
    [JsonPropertyName ("role")]
    public string? Role { get; set; }

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        Hostname = reader.ReadNullableString();
        Username = reader.ReadNullableString();
        Password = reader.ReadNullableString();
        Resource = reader.ReadNullableString();
        Role = reader.ReadNullableString();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer
            .WriteNullable (Hostname)
            .WriteNullable (Username)
            .WriteNullable (Password)
            .WriteNullable (Resource)
            .WriteNullable (Role);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify" />
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<IrbisCredentials> (this, throwOnError);

        verifier
            .NotNullNorEmpty (Username);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return Username.ToVisibleString();
    }

    #endregion
}
