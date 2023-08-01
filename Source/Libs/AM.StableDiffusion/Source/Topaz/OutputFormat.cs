// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* OutputFormat.cs -- формат, в котором сохраняется результат
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Topaz;

/// <summary>
/// Формат, в котором сохраняется результат.
/// </summary>
public static class OutputFormat
{
    #region Constants

    /// <summary>
    /// JPEG с трехбуквенным расширением.
    /// </summary>
    public const string Jpg = "jpg";

    /// <summary>
    /// JPEG с четырехбуквенным расширением.
    /// </summary>
    public const string Jpeg = "jpeg";

    /// <summary>
    /// PNG.
    /// </summary>
    public const string Png = "png";

    /// <summary>
    /// TIFF с трехбуквенным расширением.
    /// </summary>
    public const string Tif = "tif";

    /// <summary>
    /// TIFF с четырехбуквенным расширением.
    /// </summary>
    public const string Tiff = "tiff";

    /// <summary>
    /// DNG.
    /// </summary>
    public const string Dng = "dng";

    /// <summary>
    /// Использовать формат исходного файла.
    /// Этот вариант используется по умолчанию.
    /// </summary>
    public const string Preserve = "preserve";

    #endregion
}
