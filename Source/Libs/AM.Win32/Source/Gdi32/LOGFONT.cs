// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* LOGFONT.cs -- определяет атрибуты шрифта
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Win32;

/// <summary>
/// Структура LOGFONT определяет атрибуты шрифта.
/// </summary>
[StructLayout (LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct LOGFONT
{
    /// <summary>
    /// <para>
    /// Высота в логических единицах символьной ячейки или символа шрифта.
    /// Значение высоты символа (также известное как высота em) представляет
    /// собой значение высоты символьной ячейки за вычетом внутреннего
    /// начального значения. Преобразователь шрифтов интерпретирует значение,
    /// указанное в lfHeight, следующим образом.
    /// </para>
    /// <para>
    /// &gt; 0 Преобразователь шрифтов преобразует это значение в единицы
    /// устройства и сопоставляет его с высотой ячейки доступных шрифтов.
    /// 0 Средство сопоставления шрифтов использует значение высоты
    /// по умолчанию при поиске соответствия. &lt; 0 Преобразователь шрифтов
    /// преобразует это значение в единицы устройства и сопоставляет
    /// его абсолютное значение с высотой символов доступных шрифтов.
    /// </para>
    /// <para>
    /// Для всех сравнений высоты средство сопоставления шрифтов ищет
    /// самый большой шрифт, который не превышает запрошенного размера.
    /// Это сопоставление происходит, когда шрифт используется в первый раз.
    /// </para>
    /// </summary>
    public int lfHeight;

    /// <summary>
    /// Средняя ширина символов шрифта в логических единицах.
    /// Если lfWidth равен нулю, соотношение сторон устройства сопоставляется
    /// с соотношением сторон оцифровки доступных шрифтов, чтобы найти
    /// наиболее близкое совпадение, определяемое абсолютным значением разницы.
    /// </summary>
    public int lfWidth;

    /// <summary>
    /// Угол в десятых долях градуса между вектором спуска и осью X устройства.
    /// Вектор спуска параллелен базовой линии строки текста. Когда
    /// графический режим установлен на GM_ADVANCED, вы можете указать
    /// угол спуска строки независимо от угла ориентации символов строки.
    /// Когда графический режим установлен на GM_COMPATIBLE, lfEscapement
    /// указывает и спуск, и ориентацию. Вы должны установить для lfEscapement
    /// и lfOrientation одно и то же значение.
    /// </summary>
    public int lfEscapement;

    /// <summary>
    /// Угол в десятых долях градуса между базовой линией каждого
    /// символа и осью X устройства.
    /// </summary>
    public int lfOrientation;

    /// <summary>
    /// Толщина шрифта в диапазоне от 0 до 1000. Например,
    /// 400 — обычный шрифт, а 700 — полужирный. Если это значение
    /// равно нулю, используется вес по умолчанию.
    /// </summary>
    public int lfWeight;

    /// <summary>
    /// Курсивный шрифт, если установлено значение TRUE.
    /// </summary>
    public byte lfItalic;

    /// <summary>
    /// Подчеркнутый шрифт, если установлено значение TRUE.
    /// </summary>
    public byte lfUnderline;

    /// <summary>
    /// Зачеркнутый шрифт, если установлено значение TRUE.
    /// </summary>
    public byte lfStrikeOut;

    /// <summary>
    /// Набор символов.
    /// </summary>
    public byte lfCharSet;

    /// <summary>
    /// Точность вывода. Точность вывода определяет, насколько точно
    /// вывод должен соответствовать высоте, ширине, ориентации символов,
    /// спуску, шагу и типу шрифта запрошенного шрифта.
    /// </summary>
    public byte lfOutPrecision;

    /// <summary>
    /// Точность отсечения. Точность отсечения определяет, как обрезать
    /// символы, частично находящиеся за пределами области отсечения.
    /// </summary>
    public byte lfClipPrecision;

    /// <summary>
    /// Качество вывода. Качество вывода определяет, насколько
    /// тщательно интерфейс графического устройства (GDI) должен
    /// пытаться сопоставить атрибуты логического шрифта с атрибутами
    /// реального физического шрифта.
    /// </summary>
    public byte lfQuality;

    /// <summary>
    /// Шаг и семейство шрифта.
    /// </summary>
    public byte lfPitchAndFamily;

    /// <summary>
    /// Строка с завершающим нулем, указывающая имя гарнитуры шрифта.
    /// Длина этой строки не должна превышать 32 значений TCHAR,
    /// включая завершающий NULL.
    /// </summary>
    [MarshalAs (UnmanagedType.ByValTStr, SizeConst = 32)]
    public string? lfFaceName;
}
