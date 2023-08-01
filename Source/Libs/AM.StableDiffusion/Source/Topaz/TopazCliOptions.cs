// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* TopazCliOptions.cs -- опции командной строки для Topaz Photo AI
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

using AM.Text;

#endregion

#nullable enable

namespace AM.Topaz;

/*

> tpai.exe --help
QML debugging is enabled. Only use this in a safe environment.
Options:
    --output, -o: Output folder to save images to. If it doesn't exist the program will attempt to create it.
    --overwrite: Allow overwriting of files. THIS IS DESTRUCTIVE.
    --recursive, -r: If given a folder path, it will recurse into subdirectories instead of just grabbing top level files.
File Format Options:
    --format, -f: Set the output format. Accepts jpg, jpeg, png, tif, tiff, dng, or preserve. Default: preserve
        Note: Preserve will attempt to preserve the exact input extension, but RAW files will still be converted to DNG.
Format Specific Options:
    --quality, -q: JPEG quality for output. Must be between 0 and 100. Default: 95
    --compression, -c: PNG compression amount. Must be between 0 and 10. Default: 2
    --bit-depth, -d: TIFF bit depth. Must be either 8 or 16. Default: 16
    --tiff-compression: -tc: TIFF compression format. Must be "none", "lzw", or "zip".
        Note: lzw is not allowed on 16-bit output and will be converted to zip.
Debug Options:
    --showSettings: Shows the Autopilot settings for images before they are processed
    --skipProcessing: Skips processing the image (e.g., if you just want to know the settings)

Return values:
    0 - Success
    1 - Partial Success (e.g., some files failed)
    -1 (255) - No valid files passed.
    -2 (254) - Invalid log token. Open the app normally to login.
    -3 (253) - An invalid argument was found.

 */

/// <summary>
/// Опции командной строки для Topaz Photo AI.
/// </summary>
public sealed class TopazCliOptions
{
    #region Properties

    /// <summary>
    /// Папка, в которую предполагается поместить результат.
    /// Если не существует, Topaz попытается создаеть ее.
    /// </summary>
    public string? OutputPath { get; set; }

    /// <summary>
    /// Заменять файлы на новые версии без предупреждения.
    /// </summary>
    public bool Overwrite { get; set; }

    /// <summary>
    /// Рекурсивный обход директорий.
    /// </summary>
    public bool Recursive { get; set; }

    /// <summary>
    /// Формат для файлов с результатом обработки.
    /// См. <see cref="AM.Topaz.OutputFormat"/>.
    /// </summary>
    public string? OutputFormat { get; set; }

    /// <summary>
    /// Качество JPEG, между 0 и 100, по умолчанию 95.
    /// </summary>
    public int? JpegQuality { get; set; }

    /// <summary>
    /// Степень сжатия PNG, от 0 до 10, по умолчанию 2.
    /// </summary>
    public int? PngCompression { get; set; }

    /// <summary>
    /// Глубина цвета TIFF: 8 или 16, по умолчанию 16.
    /// </summary>
    public int? TiffBitDepth { get; set; }

    /// <summary>
    /// Метод сжатия TIFF: "none", "lzw" или "zip".
    /// </summary>
    public string? TiffCompression { get; set; }

    /// <summary>
    /// Отладочная опция: показ настроек автопилота перед их применением.
    /// </summary>
    public bool ShowSettings { get; set; }

    /// <summary>
    /// Отладочная опция: не выполнять обработку изображений,
    /// только показать настройки автопилота.
    /// </summary>
    public bool SkipProcessing { get; set; }

    #endregion

    #region Object methods

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        var builder = new StringBuilder();

        if (!string.IsNullOrEmpty (OutputPath))
        {
            builder.Append ($"--output \"{OutputPath}\"");
        }

        if (Overwrite)
        {
            builder.Append (" --overwrite");
        }

        if (Recursive)
        {
            builder.Append (" --recursive");
        }

        if (!string.IsNullOrEmpty (OutputFormat))
        {
            builder.Append ($" --format {OutputFormat}");
        }

        if (JpegQuality.HasValue)
        {
            builder.Append ($" --quality {JpegQuality.Value}");
        }

        if (PngCompression.HasValue)
        {
            builder.Append ($" --compression {PngCompression.Value}");
        }

        if (TiffBitDepth.HasValue)
        {
            builder.Append ($" --bit-depth {TiffBitDepth.Value}");
        }

        if (!string.IsNullOrEmpty (TiffCompression))
        {
            builder.Append ($" --tiff-compression {TiffCompression}");
        }

        if (ShowSettings)
        {
            builder.Append (" --showSettings");
        }

        if (SkipProcessing)
        {
            builder.Append (" --skipProcessing");
        }

        builder.Trim();

        return builder.ToString();
    }

    #endregion
}
