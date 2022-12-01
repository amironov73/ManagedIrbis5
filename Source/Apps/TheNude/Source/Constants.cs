// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

/* Constants.cs -- глобальные константы для приложения
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace TheNude;

/// <summary>
/// Глобальные константы для приложения.
/// </summary>
internal static class Constants
{
    #region Constants

    /// <summary>
    /// Имя приложения.
    /// </summary>
    public const string ApplicationName = "TheNude";

    /// <summary>
    /// Имя файла с сохраненными картинками.
    /// </summary>
    public const string ThumbnailsFileName = "thumbnails.json";

    #endregion
}
