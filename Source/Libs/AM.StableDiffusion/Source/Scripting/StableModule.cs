// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* StableModule.cs -- модуль для Барсика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

using AM.Scripting.Barsik;

using JetBrains.Annotations;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using static AM.Scripting.Barsik.Builtins;

#endregion

namespace AM.StableDiffusion.Scripting;

/// <summary>
/// Модуль для Барсика.
/// </summary>
[PublicAPI]
public sealed class StableModule
    : IBarsikModule
{
    #region Properties

    /// <summary>
    /// Реестр экспортированных функций.
    /// </summary>
    public static readonly Dictionary<string, FunctionDescriptor> Registry = new ()
    {
        { "check_prepared_image", new FunctionDescriptor ("check_prepared_image", CheckPreparedImage) },
        { "check_prepared_images", new FunctionDescriptor ("check_prepared_images", CheckPreparedImages) },
        { "decode_text_data", new FunctionDescriptor ("decode_text_data", DecodeTextData) },
        { "parse_data_blocks", new FunctionDescriptor ("parse_data_blocks", ParseSafetensorsDataBlocks) },
        { "read_all_metadata", new FunctionDescriptor ("read_all_metadata", ReadRawMetadata) },
        { "read_metadata", new FunctionDescriptor ("read_metadata", ReadSafetensorsMetadata) },
        { "retrieve_text_data", new FunctionDescriptor ("retrieve_text_data", RetrieveTextData) },
        { "slice_image", new FunctionDescriptor ("slice_image", SliceImage) },

    };

    #endregion

    #region Public methods

    /// <summary>
    /// Проверка изображения на соответствие требуемым параметрам.
    /// </summary>
    public static dynamic? CheckPreparedImage
        (
            Context context,
            dynamic?[] args
        )
    {
        var fileName = Compute (context, args, 0) as string;
        if (!string.IsNullOrEmpty (fileName))
        {
            return EmbeddingUtility.CheckPreparedImage (fileName);
        }

        return true;
    }

    /// <summary>
    /// Проверка изображения на соответствие требуемым параметрам.
    /// </summary>
    public static dynamic CheckPreparedImages
        (
            Context context,
            dynamic?[] args
        )
    {
        var directoryName = Compute (context, args, 0) as string;
        var result = new List<string>();
        if (!string.IsNullOrEmpty (directoryName))
        {
            foreach (var one in EmbeddingUtility.CheckPreparedImages (directoryName))
            {
                if (!string.IsNullOrEmpty (one))
                {
                    result.Add (one);
                }
            }

            return result;
        }

        return Array.Empty<string>();
    }

    /// <summary>
    /// Декодирование текстовых данных, извлеченных из изображения..
    /// </summary>
    public static dynamic? DecodeTextData
        (
            Context context,
            dynamic?[] args
        )
    {
        var textData = Compute (context, args, 0) as string;
        if (!string.IsNullOrEmpty (textData))
        {
            return ImageUtility.DecodeTextData (textData);
        }

        return null;
    }

    /// <summary>
    /// Чтение метаданных модели (чекпоинта или лоры).
    /// Метаданные никак не интерпретируются.
    /// </summary>
    public static dynamic? ReadRawMetadata
        (
            Context context,
            dynamic?[] args
        )
    {
        var fileName = Compute (context, args, 0) as string;
        if (!string.IsNullOrEmpty (fileName))
        {
            using var inputStream = File.OpenRead (fileName);
            var bytes = Safetensors.GetRawMetadata (inputStream);
            var json = Encoding.UTF8.GetString (bytes);
            var parsed = JObject.Parse (json);
            return parsed.ToString (Formatting.Indented);
        }

        return null;
    }

    /// <summary>
    /// Разбор блоков данных модели (чекпоинта или лоры).
    /// </summary>
    public static dynamic? ParseSafetensorsDataBlocks
        (
            Context context,
            dynamic?[] args
        )
    {
        var fileName = Compute (context, args, 0) as string;
        if (!string.IsNullOrEmpty (fileName))
        {
            using var inputStream = File.OpenRead (fileName);
            var bytes = Safetensors.GetRawMetadata (inputStream);
            var json = Encoding.UTF8.GetString (bytes);
            var document = JsonDocument.Parse (json);
            return Safetensors.ParseBlocks (document);
        }

        return null;
    }

    /// <summary>
    /// Чтение метаданных модели (чекпоинта или лоры).
    /// </summary>
    public static dynamic? ReadSafetensorsMetadata
        (
            Context context,
            dynamic?[] args
        )
    {
        var fileName = Compute (context, args, 0) as string;
        if (!string.IsNullOrEmpty (fileName))
        {
            return Safetensors.ReadMetadata (fileName);
        }

        return null;
    }

    /// <summary>
    /// Получение текстовых данных из указанного файла.
    /// </summary>
    public static dynamic? RetrieveTextData
        (
            Context context,
            dynamic?[] args
        )
    {
        var fileName = Compute (context, args, 0) as string;
        if (!string.IsNullOrEmpty (fileName))
        {
            return ImageUtility.RetrieveTextData (fileName);
        }

        return null;
    }

    /// <summary>
    /// Нарезка больного изображения на матрицу мелких..
    /// </summary>
    public static dynamic? SliceImage
        (
            Context context,
            dynamic?[] args
        )
    {
        if (args.Length != 8)
        {
            context.Error.WriteLine ("Wrong argument number");
            return null;
        }

        var originalImagePath = BarsikUtility.ToString (Compute (context, args, 0));
        if (string.IsNullOrEmpty (originalImagePath))
        {
            context.Error.WriteLine ("No original image file specified");
            return null;
        }

        if (!File.Exists (originalImagePath))
        {
            context.Error.WriteLine ($"File '{originalImagePath}' doesn't exist");
            return null;
        }

        var outputDirectory = BarsikUtility.ToString (Compute (context, args, 1));
        if (string.IsNullOrEmpty (outputDirectory))
        {
            context.Error.WriteLine ("No output directory specified");
            return null;
        }

        Directory.CreateDirectory (outputDirectory);

        var originX = BarsikUtility.ToInt32 (Compute (context, args, 2));
        var originY = BarsikUtility.ToInt32 (Compute (context, args, 3));
        var chunkWidth = BarsikUtility.ToInt32 (Compute (context, args, 4));
        var chunkHeight = BarsikUtility.ToInt32 (Compute (context, args, 5));
        var numberX = BarsikUtility.ToInt32 (Compute (context, args, 6));
        var numberY = BarsikUtility.ToInt32 (Compute (context, args, 7));

        var slicer = new ImageSlicer (context.Output);
        slicer.Slice (originalImagePath, outputDirectory, originX, originY,
                chunkWidth, chunkHeight, numberX, numberY);

        return null;
    }

    #endregion

    #region IBarsikModule members

    /// <inheritdoc cref="IBarsikModule.Description"/>
    public string Description => "StableDiffusion";

    /// <inheritdoc cref="IBarsikModule.Version"/>
    public Version Version { get; } = new (1, 0);

    /// <inheritdoc cref="IBarsikModule.AttachModule"/>
    public bool AttachModule
        (
            Interpreter interpreter
        )
    {
        Sure.NotNull (interpreter);

        var context = interpreter.Context.ThrowIfNull();
        foreach (var descriptor in Registry)
        {
            context.Functions[descriptor.Key] = descriptor.Value;
        }

        var assembly = typeof (StableModule).Assembly;
        StdLib.LoadAssembly (context, new dynamic?[] { assembly.GetName().Name });
        StdLib.Use (context, new dynamic?[] { "StableDiffusion" });

        return true;
    }

    /// <inheritdoc cref="IBarsikModule.DetachModule"/>
    public void DetachModule
        (
            Interpreter interpreter
        )
    {
        Sure.NotNull (interpreter);

        var context = interpreter.Context.ThrowIfNull();
        interpreter.ExternalCodeHandler = null;
        foreach (var descriptor in Registry)
        {
            context.Functions.Remove (descriptor.Key);
        }
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return Description;
    }

    #endregion
}
