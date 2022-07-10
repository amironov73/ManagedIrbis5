// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* AnimatedRectangleFlags.cs -- флаги для DrawAnimatedRects - тип анимации
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Флаги для метода <see cref="User32.DrawAnimatedRects"/>
/// - тип анимации.
/// </summary>
[Flags]
public enum AnimatedRectangleFlags
{
    /// <summary>
    /// Устаревшее значение (Windows 95/98/NT).
    /// </summary>
    IDANI_OPEN = 1,

    /// <summary>
    /// Единственно реально вопринимаемое значение.
    /// С типом анимации IDANI_CAPTION заголовок окна анимирует
    /// от позиции, указанной параметром lprcFrom до позиции,
    /// установленной в lprcTo. Результат подобен свертыванию
    /// или развертыванию окна.
    /// </summary>
    IDANI_CAPTION = 3
}
