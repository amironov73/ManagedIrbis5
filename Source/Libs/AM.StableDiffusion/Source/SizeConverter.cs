// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* SizeConverter.cs -- конвертер для размеров
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using SixLabors.ImageSharp;

#endregion

namespace AM.StableDiffusion;

/// <summary>
/// Коневертер для размеров.
/// </summary>
internal sealed class SizeConverter
    : JsonConverter<Size>
{
    #region JsonConverter

    /// <inheritdoc cref="JsonConverter{T}.Read"/>
    public override Size Read
        (
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
    {
        var text = reader.GetString()!.Trim ('(', ')');
        var parts = text.Split (',', StringSplitOptions.TrimEntries);
        var width = parts[0].SafeToInt32();
        var height = parts[1].SafeToInt32();

        return new Size (width, height);
    }

    /// <inheritdoc cref="JsonConverter{T}.Write"/>
    public override void Write
        (
            Utf8JsonWriter writer,
            Size value,
            JsonSerializerOptions options
        )
        => writer.WriteStringValue (value.ToString());

    #endregion
}
