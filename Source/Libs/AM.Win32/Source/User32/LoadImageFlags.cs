// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* LoadImageFlags.cs -- флаги для функции LoadImage
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32;

/// <summary>
/// Флаги для функции LoadImage.
/// </summary>
public enum LoadImageFlags
{
    /// <summary>
    /// Загрузка битового изображения.
    /// </summary>
    IMAGE_BITMAP = 0,

    /// <summary>
    /// Загрузка иконки.
    /// </summary>
    IMAGE_ICON = 1,

    /// <summary>
    /// Загрузка курсора.
    /// </summary>
    IMAGE_CURSOR = 2,

    /// <summary>
    /// Загрузка улучшенного метафайла.
    /// </summary>
    IMAGE_ENHMETAFILE = 3
}
