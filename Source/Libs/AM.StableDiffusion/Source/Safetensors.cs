// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Safetensors.cs -- возня с форматом safetensors
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

using JetBrains.Annotations;

#endregion

namespace AM.StableDiffusion;

/// <summary>
/// Возня с форматом safetensors.
/// </summary>
[PublicAPI]
public static class Safetensors
{
    #region Private members

    /// <summary>
    /// Получаем массив целых чисел, хранящийся в свойстве объекта.
    /// </summary>
    private static int[] GetIntegers
        (
            JsonElement element,
            string propertyName
        )
    {
        var array = element.GetProperty (propertyName);
        var result = new List<int> (array.GetArrayLength());
        foreach (var item in array.EnumerateArray())
        {
            result.Add (item.GetInt32());
        }

        return result.ToArray();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Получение формата файла в сыром виде.
    /// </summary>
    public static byte[] GetRawMetadata
        (
            Stream input
        )
    {
        Sure.NotNull (input);

        using var reader = new BinaryReader (input, encoding: Encoding.UTF8, leaveOpen: true);
        var metadataSize = reader.ReadInt32();
        // Console.WriteLine ($"Metadata size={metadataSize}");

        // пропускаем 4 байта с нулями
        reader.ReadByte();
        reader.ReadByte();
        reader.ReadByte();
        reader.ReadByte();

        var rawBytes = reader.ReadBytes (metadataSize);
        if (rawBytes.Length != metadataSize)
        {
            throw new ApplicationException ($"Bad safetensors file");
        }

        return rawBytes;
    }

    /// <summary>
    /// Декодирование метаданных в сырой JSON.
    /// </summary>
    public static JsonDocument DecodeMetadata
        (
            byte[] rawMetadata
        )
    {
        Sure.NotNull (rawMetadata);

        return JsonDocument.Parse (rawMetadata);
    }

    /// <summary>
    /// Разбор блоков данных.
    /// </summary>
    public static SafetensorsDataBlock[] ParseBlocks
        (
            JsonDocument document
        )
    {
        Sure.NotNull (document);

        var result = new List<SafetensorsDataBlock>();
        foreach (var property in document.RootElement.EnumerateObject())
        {
            var name = property.Name;
            if (name == "__metadata__")
            {
                continue;
            }

            var value = property.Value;
            var block = new SafetensorsDataBlock
            {
                Name = name,
                Type = value.GetProperty ("dtype").GetString(),
                Shape = GetIntegers (value, "shape"),
                Offsets = GetIntegers (value, "data_offsets ")
            };


            result.Add (block);
        }

        return result.ToArray();
    }

    /// <summary>
    /// Разбор метаданных по полям.
    /// </summary>
    public static SafetensorsMetadata? ParseMetadata
        (
            JsonDocument document
        )
    {
        Sure.NotNull (document);

        var element = document.RootElement.GetProperty ("__metadata__");

        return element.Deserialize<SafetensorsMetadata>();
    }

    /// <summary>
    /// Чтение метаданных из указанного файла.
    /// </summary>
    public static SafetensorsMetadata? ReadMetadata
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        using var inputStream = File.OpenRead (fileName);
        var rawMetadata = GetRawMetadata (inputStream);
        var document = DecodeMetadata (rawMetadata);
        var result = ParseMetadata (document);

        return result;
    }

    #endregion
}
