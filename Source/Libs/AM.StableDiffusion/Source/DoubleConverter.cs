// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* DoubleConverter.cs -- конвертер для чисел с плавающей точкой
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

#endregion

namespace AM.StableDiffusion;

/// <summary>
/// Конвертер для чисел с плавающей точкой.
/// </summary>
internal sealed class DoubleConverter
    : JsonConverter<double>
{
    #region JsonConverter members

    /// <inheritdoc cref="JsonConverter{T}.Read"/>
    public override double Read
        (
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
        => reader.GetString().SafeToDouble();

    /// <inheritdoc cref="JsonConverter{T}.Write"/>
    public override void Write
        (
            Utf8JsonWriter writer,
            double value,
            JsonSerializerOptions options
        )
        => writer.WriteNumberValue (value);

    #endregion
}
