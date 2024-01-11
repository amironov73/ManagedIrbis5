// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* BooleanConverter.cs -- конвертер для логических значений
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

#endregion

namespace AM.StableDiffusion;

/// <summary>
/// Конвертер для логических значений.
/// </summary>
internal sealed class BooleanConverter
    : JsonConverter<bool>
{
    #region JsonConverter members

    /// <inheritdoc cref="JsonConverter{T}.Read"/>
    public override bool Read
        (
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
        => bool.Parse (reader.GetString() ?? "False");

    /// <inheritdoc cref="JsonConverter{T}.Write"/>
    public override void Write
        (
            Utf8JsonWriter writer,
            bool value,
            JsonSerializerOptions options
        )
        => writer.WriteBooleanValue (value);

    #endregion
}
