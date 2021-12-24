// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* MagnaTypeConverter.cs -- конвертер для типа System.Type
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

#endregion

#nullable enable

namespace AM.Json;

/// <summary>
/// Конвертер для типа <c>System.Type</c>, чтобы
/// преодолеть ограничения Runtime.
/// </summary>
public class MagnaTypeConverter
    : JsonConverter<Type>
{
    #region JsonConverter members

    /// <inheritdoc cref="JsonConverter{T}.Read"/>
    public override Type? Read
        (
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
    {
        return Type.GetType (reader.GetString()!);
    }

    /// <inheritdoc cref="JsonConverter{T}.Write"/>
    public override void Write
        (
            Utf8JsonWriter writer,
            Type value,
            JsonSerializerOptions options
        )
    {
        writer.WriteStringValue (value.AssemblyQualifiedName);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Создание опций для сериализатора, включающих в себя
    /// нужный нам конвертер.
    /// </summary>
    public static JsonSerializerOptions CreateOptions()
    {
        return new ()
        {
            Converters = { new MagnaTypeConverter() }
        };
    }

    #endregion
}
