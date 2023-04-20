// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* DirectiveConverter.cs -- JSON-конвертер для директив
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AM.Kotik.Barsik.Directives;

/// <summary>
/// JSON-конвертер для директив
/// </summary>
internal sealed class DirectiveConverter
    : JsonConverter<DirectiveBase>
{
    #region JSonConverter members

    /// <inheritdoc cref="JsonConverter{T}.Read"/>
    public override DirectiveBase Read
        (
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
    {
        var typeName = reader.GetString().ThrowIfNullOrEmpty();
        var type = Type.GetType (typeName, true).ThrowIfNull();
        return (DirectiveBase) Activator.CreateInstance (type)
            .ThrowIfNull();
    }

    /// <inheritdoc cref="JsonConverter{T}.Write"/>
    public override void Write
        (
            Utf8JsonWriter writer,
            DirectiveBase value,
            JsonSerializerOptions options
        )
    {
        writer.WriteStringValue (value.GetType().AssemblyQualifiedName);
    }

    #endregion
}
