// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* SCROLLINFO.cs -- параметры полосы прокрутки
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System.Runtime.InteropServices;

#endregion

namespace AM.Win32;

/// <summary>
/// Структура <c>SCROLLINFO</c> содержит параметры полосы прокрутки,
/// которые должны быть установлены функцией <c>SetScrollInfo</c>
/// (или сообщением <c>SBM_SETSCROLLINFO</c>) или извлечены функцией
/// <c>GetScrollInfo</c> (или сообщением <c>SBM_GETSCROLLINFO</c>).
/// </summary>
[StructLayout (LayoutKind.Sequential, Size = StructureSize)]
public struct SCROLLINFO
{
    /// <summary>
    /// Константа: размер структуры в байтах.
    /// </summary>
    public const int StructureSize = 28;

    /// <summary>
    /// Размер структуры в байтах, должен быть установлен
    /// вызывающей функцией.
    /// </summary>
    public int Size;

    /// <summary>
    /// Указывает параметры полосы прокрутки для установки
    /// или извлечения.
    /// </summary>
    public int Mask;

    /// <summary>
    /// Задает минимальную позицию прокрутки.
    /// </summary>
    public int Min;

    /// <summary>
    /// Задает максимальную позицию прокрутки.
    /// </summary>
    public int Max;

    /// <summary>
    /// Задает размер страницы. Полоса прокрутки использует
    /// это значение для определения соответствующего размера
    /// пропорционального поля прокрутки.
    /// </summary>
    public int Page;

    /// <summary>
    /// Определяет положение полосы прокрутки.
    /// </summary>
    public int Pos;

    /// <summary>
    /// Указывает непосредственное положение полосы прокрутки,
    /// которую перетаскивает пользователь. Приложение может
    /// получить это значение при обработке кода запроса
    /// <c>SB_THUMBTRACK</c>. Приложение не может установить
    /// позицию непосредственной прокрутки; функция <c>SetScrollInfo</c>
    /// игнорирует этот элемент.
    /// </summary>
    public int TrackPos;
}
