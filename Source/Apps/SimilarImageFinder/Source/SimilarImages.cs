// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* SimilarImages.cs -- найденные похожие изображения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace SimilarImageFinder;

/// <summary>
/// Найденные похожие изображения.
/// </summary>
[PublicAPI]
public sealed class SimilarImages
{
    #region Properties

    /// <summary>
    /// Массив имен файлов схожих изображений.
    /// </summary>
    public string[]? FileNames { get; set; }

    #endregion
}
