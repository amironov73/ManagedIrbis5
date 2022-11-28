// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* TEXTMETRIC.cs -- базовая информация о физическом шрифте
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32;

/// <summary>
/// Структура TEXTMETRIC содержит базовую информацию о физическом шрифте.
/// Все размеры указаны в логических единицах; то есть они зависят
/// от текущего режима отображения контекста отображения.
/// </summary>
[Serializable]
[StructLayout (LayoutKind.Sequential)]
public struct TEXTMETRIC
{
    /// <summary>
    /// Определяет высоту (восхождение + спуск) символов.
    /// </summary>
    public int tmHeight;

    /// <summary>
    /// Определяет восхождение (единицы над базовой линией) символов.
    /// </summary>
    public int tmAscent;

    /// <summary>
    /// Определяет спуск (единицы ниже базовой линии) символов.
    /// </summary>
    public int tmDescent;

    /// <summary>
    /// Определяет величину интерлиньяжа (пробела) внутри границ,
    /// установленных элементом tmHeight. В этой области могут
    /// встречаться знаки ударения и другие диакритические знаки.
    /// Разработчик может установить этот элемент равным нулю.
    /// </summary>
    public int tmInternalLeading;

    /// <summary>
    /// Указывает количество дополнительного интерлиньяжа (пробела),
    /// которое приложение добавляет между строками. Поскольку
    /// эта область находится за пределами шрифта, она не содержит
    /// меток и не изменяется вызовами вывода текста ни в режиме
    /// OPAQUE, ни в режиме TRANSPARENT. Разработчик может установить
    /// этот элемент равным нулю.
    /// </summary>
    public int tmExternalLeading;

    /// <summary>
    /// Указывает среднюю ширину символов шрифта (обычно определяется
    /// как ширина буквы x). Это значение не включает выступ,
    /// необходимый для жирного шрифта или курсива.
    /// </summary>
    public int tmAveCharWidth;

    /// <summary>
    /// Определяет ширину самого широкого символа в шрифте.
    /// </summary>
    public int tmMaxCharWidth;

    /// <summary>
    /// Определяет вес шрифта.
    /// </summary>
    public int tmWeight;

    /// <summary>
    /// Задает дополнительную ширину строки, которая может быть добавлена
    /// к некоторым синтезированным шрифтам. При синтезе некоторых
    /// атрибутов, таких как жирный шрифт или курсив, интерфейсу
    /// графического устройства (GDI) или устройству может потребоваться
    /// добавить ширину к строке как для каждого символа, так и для
    /// каждой строки. Например, GDI делает строку полужирной,
    /// увеличивая интервал между каждым символом и зачеркивая значение
    /// смещения; он выделяет шрифт курсивом, разрезая строку.
    /// В любом случае за основной струной остается выступ.
    /// Для строк, выделенных жирным шрифтом, выступ представляет
    /// собой расстояние, на которое смещается забастовка. Для курсивных
    /// строк выступ представляет собой величину, на которую верхняя
    /// часть шрифта сдвигается за нижнюю часть шрифта.
    /// </summary>
    public int tmOverhang;

    /// <summary>
    /// Определяет горизонтальную сторону устройства, для которого
    /// был разработан шрифт.
    /// </summary>
    public int tmDigitizedAspectX;

    /// <summary>
    /// Определяет вертикальный аспект устройства, для которого
    /// был разработан шрифт. Соотношение членов tmDigitizedAspectX
    /// и tmDigitizedAspectY — это соотношение сторон устройства,
    /// для которого был разработан шрифт.
    /// </summary>
    public int tmDigitizedAspectY;

    /// <summary>
    /// Указывает значение первого символа, определенного в шрифте.
    /// </summary>
    public char tmFirstChar;

    /// <summary>
    /// Задает значение символа, заменяемого символами,
    /// которых нет в шрифте.
    /// </summary>
    public char tmLastChar;

    /// <summary>
    /// Указывает значение символа, который будет использоваться
    /// для определения разрывов слов для выравнивания текста.
    /// </summary>
    public char tmDefaultChar;

    /// <summary>
    /// Указывает курсивный шрифт, если он не равен нулю.
    /// </summary>
    public char tmBreakChar;

    /// <summary>
    /// Указывает курсивный шрифт, если он не равен нулю.
    /// </summary>
    public byte tmItalic;

    /// <summary>
    /// Задает зачеркнутый шрифт, если он не равен нулю.
    /// </summary>
    public byte tmUnderlined;

    /// <summary>
    /// Задает зачеркнутый шрифт, если он не равен нулю.
    /// </summary>
    public byte tmStruckOut;

    /// <summary>
    /// Указывает информацию о шаге, технологии и семействе
    /// физического шрифта.
    /// </summary>
    public byte tmPitchAndFamily;

    /// <summary>
    /// Определяет набор символов шрифта. Набор символов может быть
    /// одним из значений <see cref="FontCharset"/>.
    /// </summary>
    public byte tmCharSet;
}
