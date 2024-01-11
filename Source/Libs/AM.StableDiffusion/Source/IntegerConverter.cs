// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* IntegerConverter.cs -- конвертер для целых чисел
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

#endregion

namespace AM.StableDiffusion;

/// <summary>
/// Конвертер для целых чисел.
/// </summary>
internal sealed class IntegerConverter
    : JsonConverter<int>
{
    #region JsonConverter members

    /// <inheritdoc cref="JsonConverter{T}.Read"/>
    public override int Read
        (
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
        => reader.GetString().SafeToInt32 ();

    /// <inheritdoc cref="JsonConverter{T}.Write"/>
    public override void Write
        (
            Utf8JsonWriter writer,
            int value,
            JsonSerializerOptions options
        )
        => writer.WriteNumberValue (value);

    #endregion
}
