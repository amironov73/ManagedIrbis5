// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable NonReadonlyMemberInGetHashCode

/* MxAlias.cs -- алиас
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Mx;

/// <summary>
/// Алиасы служат для создания псевдонимов команд и подключений
/// к серверу ИРБИС64.
/// </summary>
[XmlRoot("alias")]
public sealed class MxAlias
    : IHandmadeSerializable,
    IVerifiable,
    IEquatable<MxAlias>
{
    #region Properties

    /// <summary>
    /// Имя алиаса.
    /// </summary>
    [XmlAttribute ("name")]
    [JsonPropertyName ("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Значение алиаса.
    /// </summary>
    [XmlAttribute ("value")]
    [JsonPropertyName ("value")]
    public string? Value { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public MxAlias()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор со значениями.
    /// </summary>
    public MxAlias
        (
            string name,
            string value
        )
    {
        Sure.NotNullNorEmpty (name);
        Sure.NotNullNorEmpty (value);

        Name = name;
        Value = value;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор текстового представления алиаса.
    /// </summary>
    public static MxAlias Parse
        (
            string line
        )
    {
        Sure.NotNullNorEmpty (line);

        var navigator = new TextNavigator (line);
        var name = navigator.ReadUntil ('=');
        if (name.IsEmpty)
        {
            throw new IrbisException();
        }

        name = name.Trim();
        if (name.IsEmpty)
        {
            throw new IrbisException();
        }

        if (navigator.ReadChar() != '=')
        {
            throw new IrbisException();
        }

        var value = navigator.GetRemainingText();
        if (value.IsEmpty)
        {
            throw new IrbisException();
        }

        value = value.Trim();
        if (value.IsEmpty)
        {
            throw new IrbisException();
        }

        var result = new MxAlias (name.ToString(), value.ToString());

        return result;
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
            .WriteNullable (Name)
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
        var verifier = new Verifier<MxAlias>(this, throwOnError);

        verifier
            .NotNullNorEmpty (Name)
            .NotNullNorEmpty (Value);

        return verifier.Result;
    }

    #endregion

    #region IEquatable<T> members

    /// <inheritdoc cref="IEquatable{T}.Equals(T)" />
    public bool Equals
        (
            MxAlias? other
        )
    {
        return other is not null
               && Name.SameString (other.Name)
               && string.CompareOrdinal (Value, other.Value) == 0;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.Equals(object)" />
    public override bool Equals
        (
            object? obj
        )
    {
        return obj is MxAlias alias && Equals (alias);
    }

    /// <inheritdoc cref="object.GetHashCode" />
    public override int GetHashCode()
    {
        return HashCode.Combine (Name, Value);
    }

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return Name.ToVisibleString() + "=" + Value.ToVisibleString();
    }

    #endregion
}
