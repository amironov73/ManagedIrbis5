// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* AnyTypeConverter.cs -- JSON-конвертер для произвольного типа
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Json;

/// <summary>
/// JSON-конвертер для произвольного типа.
/// </summary>
[PublicAPI]
public sealed class AnyTypeConverter<TObject>
    : JsonConverter<TObject>
{
    #region JsonConverter members

    /// <inheritdoc cref="JsonConverter{T}.Read"/>
    public override TObject? Read
        (
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return default;
        }

        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException();
        }

        var typeName = reader.GetString().ThrowIfNullOrEmpty();
        var type = Type.GetType (typeName, true).ThrowIfNull();
        return (TObject) Activator.CreateInstance (type)
            .ThrowIfNull();
    }

    /// <inheritdoc cref="JsonConverter{T}.Write"/>
    public override void Write
        (
            Utf8JsonWriter writer,
            TObject value,
            JsonSerializerOptions options
        )
    {
        if (value is null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStringValue (value.GetType().AssemblyQualifiedName);
        }
    }

    #endregion
}
