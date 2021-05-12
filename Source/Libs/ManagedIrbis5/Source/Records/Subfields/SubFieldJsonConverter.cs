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

/* SubFieldJsonConverter.cs -- JSON-конвертер для типа SubField
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// JSON-конвертер для типа <see cref="SubField"/>,
    /// чтобы обойти ограничения <c>System.Text.Json.Serialization</c>.
    /// </summary>
    public sealed class SubFieldJsonConverter
        : JsonConverter<SubField>
    {
        /// <inheritdoc cref="JsonConverter{T}.Read"/>
        public override SubField Read
            (
                ref Utf8JsonReader reader,
                Type typeToConvert,
                JsonSerializerOptions options
            )
        {
            var result = new SubField();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return result;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException("Unexpected token");
                }

                var propertyName = reader.GetString();
                if (!reader.Read())
                {
                    throw new JsonException("Unexpected end");
                }

                var propertyValue = reader.GetString();

                switch (propertyName)
                {
                    case "code":
                        result.Code = propertyValue.FirstChar();
                        break;

                    case "value":
                        result.Value = propertyValue.AsMemory();
                        break;

                    default:
                        throw new JsonException("Unknown property name");
                }
            }

            throw new JsonException("Unexpected end");

        } // method Read

        /// <inheritdoc cref="JsonConverter{T}.Write"/>
        public override void Write
            (
                Utf8JsonWriter writer,
                SubField value,
                JsonSerializerOptions options
            )
        {
            writer.WriteStartObject();
            writer.WriteString("code", value.Code.ToString());
            writer.WriteString("value", value.Value.ToString());
            writer.WriteEndObject();

        } // method Write

    } // class SubFieldJsonConverter

} // namespace ManagedIrbis.Infrastructure
