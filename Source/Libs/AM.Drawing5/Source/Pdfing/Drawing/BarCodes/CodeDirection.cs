// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* CodeDirection.cs -- направление отрисовки штрих-кода
 * Ars Magna project, http://arsmagna.ru
 */

namespace PdfSharpCore.Drawing.BarCodes;

/// <summary>
/// Задает направление отрисовки штрих-кода.
/// </summary>
public enum CodeDirection
{
    /// <summary>
    /// Слева направо без каких-либо трансформаций.
    /// </summary>
    LeftToRight,

    /// <summary>
    /// Снизу вверх.
    /// </summary>
    BottomToTop,

    /// <summary>
    /// Справа налево.
    /// </summary>
    RightToLeft,

    /// <summary>
    /// Сверху вниз.
    /// </summary>
    TopToBottom
}
