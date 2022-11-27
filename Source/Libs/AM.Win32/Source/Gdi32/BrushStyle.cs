// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* BrushStyle.cs -- задает стиль кисти
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32;

/// <summary>
/// Задает стиль кисти.
/// </summary>
public enum BrushStyle
{
    /// <summary>
    /// Сплошная (одноцветная кисть).
    /// </summary>
    BS_SOLID = 0,

    /// <summary>
    /// То же, что и BS_HOLLOW.
    /// </summary>
    BS_NULL = 1,

    /// <summary>
    /// Пустая кисть.
    /// </summary>
    BS_HOLLOW = 0,

    /// <summary>
    /// Штриховка.
    /// </summary>
    BS_HATCHED = 2,

    /// <summary>
    /// Узорчатая кисть, определяемая растровым изображением в памяти.
    /// </summary>
    BS_PATTERN = 3,

    /// <summary>
    /// Не поддерживается.
    /// </summary>
    BS_INDEXED = 4,

    /// <summary>
    /// <para>Узорчатая кисть, определяемая спецификацией
    /// аппаратно-независимого растрового изображения (DIB).
    /// Если lbStyle имеет значение BS_DIBPATTERN, элемент lbHatch
    /// содержит дескриптор упакованного DIB.</para>
    /// <para>Windows 95: создание кистей из растровых изображений
    /// или файлов DIB размером более 8 на 8 пикселей не поддерживается.
    /// Если указано большее растровое изображение, используется
    /// только часть растрового изображения.</para>
    /// </summary>
    BS_DIBPATTERN = 5,

    /// <summary>
    /// Узорчатая кисть, определяемая спецификацией аппаратно-независимого
    /// растрового изображения (DIB). Если lbStyle имеет значение
    /// BS_DIBPATTERNPT, элемент lbHatch содержит указатель
    /// на упакованный DIB.
    /// </summary>
    BS_DIBPATTERNPT = 6,

    /// <summary>
    /// То же, что и BS_PATTERN.
    /// </summary>
    BS_PATTERN8X8 = 7,

    /// <summary>
    /// То же, что и BS_DIBPATTERN.
    /// </summary>
    BS_DIBPATTERN8X8 = 8,

    /// <summary>
    /// Не поддерживается.
    /// </summary>
    BS_MONOPATTERN = 9
}
