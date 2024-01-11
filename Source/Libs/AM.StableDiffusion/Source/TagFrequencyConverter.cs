// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* TagFrequencyConverter.cs -- конвертер для частот тегов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

#endregion

namespace AM.StableDiffusion;

/// <summary>
/// Конвертер для частот тегов.
/// </summary>
internal sealed class TagFrequencyConverter
    : JsonConverter<Dictionary<string, Dictionary<string, int>>>
{
    #region JsonConverter members

    /// <inheritdoc cref="JsonConverter{T}.Read"/>
    public override Dictionary<string, Dictionary<string, int>>? Read
        (
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
    {
        var json = reader.GetString();
        var dictionary = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, int>>> (json!);

        return dictionary;
    }

    /// <inheritdoc cref="JsonConverter{T}.Write"/>
    public override void Write
        (
            Utf8JsonWriter writer,
            Dictionary<string, Dictionary<string, int>> value,
            JsonSerializerOptions options
        )
    {
        writer.WriteStartObject();
        foreach (var pair1 in value)
        {
            writer.WriteStartObject (pair1.Key);
            foreach (var pair2 in pair1.Value)
            {
                writer.WriteStartObject();
                writer.WriteNumber (pair2.Key, pair2.Value);
                writer.WriteEndObject();
            }
            writer.WriteEndObject();
        }

        writer.WriteEndObject();
    }

    #endregion
}
