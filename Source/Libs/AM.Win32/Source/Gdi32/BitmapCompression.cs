// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* BitmapCompression.cs -- указывает тип сжатия для сжатого растрового изображения
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32;

/// <summary>
/// Указывает тип сжатия для сжатого растрового изображения снизу вверх.
/// </summary>
public enum BitmapCompression
{
    /// <summary>
    /// Изображение не сжато.
    /// </summary>
    BI_RGB = 0,

    /// <summary>
    /// Формат кодирования длин серий (RLE) для растровых изображений
    /// с 8 битами на пиксель. Формат сжатия представляет собой
    /// 2-байтовый формат, состоящий из байта счетчика,
    /// за которым следует байт, содержащий индекс цвета.
    /// </summary>
    BI_RLE8 = 1,

    /// <summary>
    /// Формат RLE для растровых изображений с 4 битами на пиксель.
    /// Формат сжатия представляет собой 2-байтовый формат,
    /// состоящий из байта счетчика, за которым следуют два
    /// индекса цвета длиной в слово.
    /// </summary>
    BI_RLE4 = 2,

    /// <summary>
    /// Указывает, что растровое изображение не сжато и что таблица
    /// цветов состоит из трех цветовых масок типа DWORD,
    /// определяющих красный, зеленый и синий компоненты каждого
    /// пикселя соответственно. Этот формат можно применять
    /// при использовании растровых изображений с разрешением
    /// 16 и 32 бита на пиксель.
    /// </summary>
    BI_BITFIELDS = 3,

    /// <summary>
    /// Windows 98/Me, Windows 2000/XP: указывает, что изображение
    /// является изображением в формате JPEG.
    /// </summary>
    BI_JPEG = 4,

    /// <summary>
    /// Windows 98/Me, Windows 2000/XP: указывает, что изображение
    /// представляет собой изображение в формате PNG.
    /// </summary>
    BI_PNG = 5
}
