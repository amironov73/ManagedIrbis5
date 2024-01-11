// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* DatasetDirectoryConverter.cs -- конвертер для DatasetDirectory
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
/// Конвертер для <see cref="DatasetDirectory"/>.
/// </summary>
internal sealed class DatasetDirectoryConverter
    : JsonConverter<DatasetDirectory[]>
{
    #region JsonConverter members

    /// <inheritdoc cref="JsonConverter{T}.Read"/>
    public override DatasetDirectory[] Read
        (
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
    {
        var json = reader.GetString();
        var dictionary = JsonSerializer.Deserialize<Dictionary<string, DatasetDirectory>> (json!);
        var result = new List<DatasetDirectory>();
        foreach (var pair in dictionary!)
        {
            var directory = pair.Value;
            directory.Name = pair.Key;
            result.Add (directory);
        }

        return result.ToArray();
    }

    /// <inheritdoc cref="JsonConverter{T}.Write"/>
    public override void Write
        (
            Utf8JsonWriter writer,
            DatasetDirectory[] value,
            JsonSerializerOptions options
        )
    {
        writer.WriteStartArray();
        foreach (var directory in value)
        {
            writer.WriteStartObject();
            writer.WriteString ("name", directory.Name);
            writer.WriteNumber ("repeats", directory.Repeats);
            writer.WriteNumber ("image-count", directory.ImageCount);
            writer.WriteEndObject();
        }
        writer.WriteEndArray();
    }

    #endregion
}
