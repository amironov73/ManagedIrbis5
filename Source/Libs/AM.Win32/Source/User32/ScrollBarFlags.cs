// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* ScrollBarFlags.cs -- флаги для функций GetScrollPos и SetScrollPos
   Ars Magna project, http://arsmagna.ru */

#region Using directives

#endregion

namespace AM.Win32;

/// <summary>
/// Флаги для функций <c>GetScrollPos</c> и <c>SetScrollPos</c>.
/// </summary>
public enum ScrollBarFlags
{
    /// <summary>
    /// Получает положение полосы прокрутки на стандартной
    /// горизонтальной полосе прокрутки окна.
    /// </summary>
    SB_HORZ = 0,

    /// <summary>
    /// Получает положение полосы прокрутки на стандартной
    /// вертикальной полосе прокрутки окна.
    /// </summary>
    SB_VERT = 1,

    /// <summary>
    /// Извлекает положение полосы прокрутки в элементе управления
    /// полосы прокрутки. Параметр <c>hWnd</c> должен быть
    /// дескриптором элемента управления полосы прокрутки.
    /// </summary>
    SB_CTL = 2,

    /// <summary>
    /// Получает положение полосы прокрутки на стандартных
    /// полосах прокрутки окна (вертикальной и горизонтальной).
    /// </summary>
    SB_BOTH = 3
}
